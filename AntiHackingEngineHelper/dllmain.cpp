#include "pch.h"
#include "Hooks.h"
#include <wincrypt.h>
#define DllExport extern "C" __declspec(dllexport)

DllExport void PreventCreatingServices()
{
    HookCreateService();
}

DllExport void PreventOutsideProcessAccess()
{
    HookOpenThread();
    HookOpenProcess();
    HookCreateToolhelp32Snapshot();
    HookK32EnumProcesses();
}

DllExport void PreventResumingThreads()
{
    HookResumeThread();
}

DllExport void PreventGettingScreenshots()
{
    HookCreateCompatibleBitmap();
}

DllExport void PreventCreatingFileHandles()
{
    HookCreateFile();
}

DWORD WINAPI ElevationWatcher(LPVOID mParam)
{
    while (true)
    {
        Sleep(1000);
        HANDLE hToken = NULL;
        if (OpenProcessToken(GetCurrentProcess(), TOKEN_QUERY, &hToken))
        {
            DWORD dwSize;
            TOKEN_ELEVATION IsElevated;
            if (GetTokenInformation(hToken, TokenElevation, &IsElevated, sizeof(IsElevated), &dwSize))
            {
                if (IsElevated.TokenIsElevated)
                {
                    ExitProcess(0);
                }
            }
        }
    }
}

DllExport void PreventPrivilegeOfEscalation()
{
    HANDLE ElevationWatcherThread = CreateThread(NULL, 0, ElevationWatcher, NULL, 0, 0);
    CloseHandle(ElevationWatcherThread);
}

DllExport BOOL ConfigureASLR(DWORD ASLROptions)
{
    PROCESS_MITIGATION_ASLR_POLICY ASLR = { 0 };
    if (ASLROptions & 1)
    {
        ASLR.EnableForceRelocateImages = true;
    }

    if (ASLROptions & 2)
    {
        ASLR.DisallowStrippedImages = true;
    }

    if (ASLROptions & 3)
    {
        ASLR.EnableBottomUpRandomization = true;
    }

    if (ASLROptions & 4)
    {
        ASLR.EnableHighEntropy = true;
    }
    return SetProcessMitigationPolicy(ProcessASLRPolicy, &ASLR, sizeof(ASLR));
}

DllExport BOOL IsCFGEnabled()
{
    PROCESS_MITIGATION_CONTROL_FLOW_GUARD_POLICY CFG = {0};
    GetProcessMitigationPolicy(GetCurrentProcess(), ProcessControlFlowGuardPolicy, &CFG, sizeof(CFG));
    return CFG.EnableControlFlowGuard;
}

DllExport BOOL StrictHandleChecksMitigationPolicy()
{
    PROCESS_MITIGATION_STRICT_HANDLE_CHECK_POLICY StrictHandleCheck = { 0 };
    StrictHandleCheck.HandleExceptionsPermanentlyEnabled = true;
    StrictHandleCheck.RaiseExceptionOnInvalidHandleReference = true;
    return SetProcessMitigationPolicy(ProcessStrictHandleCheckPolicy, &StrictHandleCheck, sizeof(StrictHandleCheck));
}

DllExport BOOL ImageLoadMitigationPolicy_DisableLoadingLowMandatoryImages()
{
    PROCESS_MITIGATION_IMAGE_LOAD_POLICY ImageLoadPolicy = { 0 };
    GetProcessMitigationPolicy(GetCurrentProcess(), ProcessImageLoadPolicy, &ImageLoadPolicy, sizeof(ImageLoadPolicy));
    ImageLoadPolicy.NoLowMandatoryLabelImages = true;
    return SetProcessMitigationPolicy(ProcessImageLoadPolicy, &ImageLoadPolicy, sizeof(ImageLoadPolicy));
}

DllExport void SetIntegrity(int PID, int Integrity)
{
    SID integrityLevelSid{};
    integrityLevelSid.Revision = 1;
    integrityLevelSid.SubAuthorityCount = 1;
    integrityLevelSid.IdentifierAuthority.Value[5] = 16;
    if (Integrity == 1)
    {
        integrityLevelSid.SubAuthority[0] = SECURITY_MANDATORY_UNTRUSTED_RID;
    }
    else
    {
        integrityLevelSid.SubAuthority[0] = SECURITY_MANDATORY_LOW_RID;
    }
    TOKEN_MANDATORY_LABEL tokenIntegrityLevel = {};
    tokenIntegrityLevel.Label.Attributes = SE_GROUP_INTEGRITY;
    tokenIntegrityLevel.Label.Sid = &integrityLevelSid;
    HANDLE hToken = NULL;
    OpenProcessToken(OpenProcess(PROCESS_ALL_ACCESS, false, PID), TOKEN_ALL_ACCESS, &hToken);
    SetTokenInformation(hToken, TokenIntegrityLevel, &tokenIntegrityLevel, sizeof(TOKEN_MANDATORY_LABEL) + GetLengthSid(&integrityLevelSid));
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
    return TRUE;
}