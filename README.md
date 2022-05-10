# Froxen Anti-Hacking Engine
Advanced Anti-Exploitation Library made in C# and C++ that's supposed to protect Froxen Messenger.

### IPC Protocol
Contains an Anonymous-Pipe-Based Inter-process communication Protocol implemented in C# that enables Froxen Parent to communicate with it's childs with an In-Out Pipe Direction.

### Sandboxing
The main security feature that Froxen and many other software depends on, this library provides both managed AppDomain Sandbox for C# and Token Integrity Lowering and Removing Permissions for native code which runs Outside-CLR, that sends stuff to parent using IPC, like encrypted messages.

### Hooks
Provides API Hooking to harden process even more, to make it a little struggle for attackers to do certain damages and it make exploitation a little bit expensive, it provides hooks that: Prevent Snapping Webcam and getting screenshots, Prevent Controlling Other Processes in the system, Prevent Creating File Handles, Prevent Creating Services, Prevent Resuming Threads which are not in the main process and it provides additional layer of protection from creating new child processes.
