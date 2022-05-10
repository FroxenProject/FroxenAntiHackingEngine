# FroxenAntiHackingEngine
Advanced Anti-Exploitation Library made in C# and C++ that's supposed to protect Froxen Messenger.

### IPC Protocol
Contains an Anonymous-Pipe-Based Inter-process communication Protocol implemented in C# that enables Froxen Parent to communicate with it's childs with an In-Out Pipe Direction.

### Sandboxing
The main security feature that Froxen depends on, this library provides both managed AppDomain Sandbox for C# and Token Integrity Lowering and Removing Permissions for native code which runs Outside-CLR, that sends stuff to parent using IPC, like messages.
