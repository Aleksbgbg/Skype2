# Skype v2.0
Discord-based Skype reboot.

A desktop chat client, based on the successes of Discord, with the look and feel of Skype.

Skype v2.0 will have the following:
- Desktop client written using the WPF framework (C# and XAML)
- REST server endpoint using ASP.NET WebAPI (C#), allowing messages to be POST-ed and retrieved (GET)
- TCP socket server endpoint (C#), notifying clients of new messages
- PostgreDB database service, for storing messages