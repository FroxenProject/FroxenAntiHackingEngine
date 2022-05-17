# Froxen Anti-Hacking Engine
Advanced Anti-Exploitation Library made in C# and C++ that's supposed to protect Froxen Messenger.

### IPC Protocol
Contains an Anonymous Pipe-Based Inter-process communication Protocol implemented in C# that enables Froxen Parent to communicate with it's childs with an In-Out Pipe Direction.

### Sandboxing
The main security feature that Froxen and many other software depends on, this library provides both managed AppDomain Sandbox for C# and Token Integrity Lowering and Removing Permissions for native code which runs Outside-CLR that sends stuff to parent using IPC, like encrypted messages, also there's a module in AntiHackingEngineHelper that prevents Privilege of escalation by checking the program token every 1 second.

### Hooks
Provides API Hooking to harden process even more, to make it a little struggle for attackers to do certain damages and it make exploitation a little bit expensive, it provides hooks that: Prevent Snapping Webcam and getting screenshots, Prevent Controlling Other Processes in the system, Prevent Creating File Handles, Prevent Creating Services, Prevent Resuming Threads which are not in the main process which provides additional layer of protection from creating new child processes.

### Windows Mitigations
Provides Windows Exploit Mitigations for the C# Process, like: ASLR Customization, Child Process Mitigation (prevents creating new child processes), Win32k Calls Mitigation Policy which prevents Win32k.sys privilege-of-escalation sandbox escapes attacks by blocking calls to Win32k.sys, Disable Non-System Fonts Mitigation Policy which prevents processing untrusted fonts and mitigate some kernel sandbox escapes, Control Flow Guard (makes it harder to execute arbitrary code through vulnerabilities such as buffer overflows), Binary Signature Mitigation Policy (only microsoft-signed images are allowed to load into the process), Termination On Heap Corruption, Image Load Mitigation Policy (prevents loading remotely located images to the process), Extension Point Disable Mitigation Policy (in short it prevents certain privilege-of-escalation attacks by disabling some extension points).

and contains other components like JobObjects, etc.
# Notice
Copyright (C) FroxenProject

THE "CREATOR" OF THIS PROGRAM ARE NOT RESPONSIBLE FOR ANY FORM OF MALICIOUS USE.

The Program are Licensed under GNU General Public License v3.0.
