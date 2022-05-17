#include <Windows.h>
#include <string>
#include <vector>
#include <winternl.h>
#include <iostream>
#include <string>
#include <mmsystem.h>
#include <combaseapi.h>
#include <dshow.h>
#include <conio.h>
#include <strmif.h>
#include <tlhelp32.h>
#include <Psapi.h>

FARPROC OriginalOpenProcess = GetProcAddress(GetModuleHandle(L"kernelbase.dll"), "OpenProcess");
FARPROC OriginalResumeThread = GetProcAddress(GetModuleHandle(L"kernelbase.dll"), "ResumeThread");
FARPROC OriginalOpenThread = GetProcAddress(GetModuleHandle(L"kernelbase.dll"), "OpenThread");
FARPROC OriginalCreateCompatibleBitmap = GetProcAddress(GetModuleHandle(L"gdi32full.dll"), "CreateCompatibleBitmap");
BOOL AllowWritingToProtectedFunction = false;

bool Hook(void* src, void* dst, int len)
{
    DWORD MinLen = 14;

    if (len < MinLen) return NULL;

    BYTE stub[] = {
    0xFF, 0x25, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    };
    
    void* pTrampoline = VirtualAlloc(0, len + sizeof(stub), MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);
    DWORD dwOld = 0;
    VirtualProtect(src, len, PAGE_EXECUTE_READWRITE, &dwOld);
    DWORD64 retto = (DWORD64)src + len;
    memcpy(stub + 6, &retto, 8);
    memcpy((void*)((DWORD_PTR)pTrampoline), src, len);
    memcpy((void*)((DWORD_PTR)pTrampoline + len), stub, sizeof(stub));
    memcpy(stub + 6, &dst, 8);
    memcpy(src, stub, sizeof(stub));
    for (int i = MinLen; i < len; i++)
    {
        *(BYTE*)((DWORD_PTR)src + i) = 0x90;
    }
    VirtualProtect(src, len, dwOld, &dwOld);
    return (void*)((DWORD_PTR)pTrampoline);
}

HANDLE HookedOpenProcess(DWORD dwDesiredAccess, BOOL bInheritHandle, DWORD dwProcessId)
{
    if (dwProcessId != GetCurrentProcessId())
    {
        SetLastError(ERROR_INVALID_PARAMETER);
        return 0;
    }
    else
    {
        Hook(OpenProcess, OriginalOpenProcess, 21);
        HANDLE CurrentProcessHandle = OpenProcess(dwDesiredAccess, bInheritHandle, GetCurrentProcessId());
        Hook(OpenProcess, HookedOpenProcess, 21);
        return CurrentProcessHandle;
    }
}

void HookOpenProcess()
{
    Hook(OpenProcess, HookedOpenProcess, 21);
}

SC_HANDLE HookedCreateService()
{
    SetLastError(ERROR_PATH_NOT_FOUND);
    return 0;
}

void HookCreateService()
{
    Hook(CreateServiceA, HookedCreateService, 21);
    Hook(CreateServiceW, HookedCreateService, 21);
}

DWORD HookedResumeThread(HANDLE hThread)
{
    DWORD PID = GetProcessIdOfThread(hThread);
    if (PID == GetCurrentProcessId())
    {
        Hook(ResumeThread, OriginalResumeThread, 21);
        DWORD Result = ResumeThread(hThread);
        Hook(ResumeThread, HookedResumeThread, 21);
        return Result;
    }
    else
    {
        SetLastError(ERROR_THREAD_NOT_IN_PROCESS);
        return -1;
    }
}

void HookResumeThread()
{
    Hook(ResumeThread, HookedResumeThread, 21);
}

HANDLE HookedOpenThread(DWORD dwDesiredAccess, BOOL bInheritHandle, DWORD dwThreadId)
{
    Hook(OpenThread, OriginalOpenThread, 21);
    HANDLE ThreadHandle = OpenThread(PROCESS_QUERY_INFORMATION, false, dwThreadId);
    DWORD PIDOfThread = GetProcessIdOfThread(ThreadHandle);
    CloseHandle(ThreadHandle);
    Hook(OpenThread, HookedOpenThread, 21);
    if (PIDOfThread != GetCurrentProcessId())
    {
        SetLastError(ERROR_THREAD_NOT_IN_PROCESS);
        return 0;
    }
    else
    {
        Hook(OpenThread, OriginalOpenThread, 21);
        HANDLE ThreadHandle = OpenThread(dwDesiredAccess, bInheritHandle, dwThreadId);
        Hook(OpenThread, HookedOpenThread, 21);
        return ThreadHandle;
    }
}

void HookOpenThread()
{
    Hook(OpenThread, HookedOpenThread, 21);
}

HANDLE HookedCreateToolhelp32Snapshot(DWORD dwFlags, DWORD th32ProcessID)
{
    SetLastError(ERROR_INVALID_PARAMETER);
    return 0;
}

void HookCreateToolhelp32Snapshot()
{
    Hook(GetProcAddress(GetModuleHandle(L"kernel32.dll"), "CreateToolhelp32Snapshot"), HookedCreateToolhelp32Snapshot, 5);
}

BOOL HookedK32EnumProcesses(DWORD* lpidProcess, DWORD cb, LPDWORD lpcbNeeded)
{
    SetLastError(ERROR_INVALID_PARAMETER);
    return false;
}

void HookK32EnumProcesses()
{
    Hook(K32EnumProcesses, HookedK32EnumProcesses, 16);
}

HBITMAP HookedCreateCompatibleBitmap(HDC hdc, int cx, int cy)
{
    int Height = GetSystemMetrics(SM_CYSCREEN);
    int Width = GetSystemMetrics(SM_CXSCREEN);
    if (cx == Width || cy == Height)
    {
        SetLastError(ERROR_INVALID_PARAMETER);
        return 0;
    }
    else
    {
        Hook(CreateCompatibleBitmap, OriginalCreateCompatibleBitmap, 21);
        HBITMAP Bitmap = CreateCompatibleBitmap(hdc, cx, cy);
        Hook(CreateCompatibleBitmap, HookedCreateCompatibleBitmap, 21);
        return Bitmap;
    }
}

void HookCreateCompatibleBitmap()
{
    Hook(CreateCompatibleBitmap, HookedCreateCompatibleBitmap, 21);
}

HANDLE HookedCreateFileA(LPCSTR lpFileName, DWORD dwDesiredAccess, DWORD dwShareMode, LPSECURITY_ATTRIBUTES lpSecurityAttributes, DWORD dwCreationDisposition, DWORD dwFlagsAndAttributes, HANDLE hTemplateFile)
{
    SetLastError(ERROR_FILE_NOT_FOUND);
    return 0;
}

HANDLE HookedCreateFileW(LPCWSTR lpFileName, DWORD dwDesiredAccess, DWORD dwShareMode, LPSECURITY_ATTRIBUTES lpSecurityAttributes, DWORD dwCreationDisposition, DWORD dwFlagsAndAttributes, HANDLE hTemplateFile)
{
    SetLastError(ERROR_FILE_NOT_FOUND);
    return 0;
}

void HookCreateFile()
{
    Hook(CreateFileA, HookedCreateFileA, 21);
    Hook(CreateFileW, HookedCreateFileW, 21);
}