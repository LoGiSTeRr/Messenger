using ServerMessenger;

ManualResetEvent manualResetEvent = new ManualResetEvent(false);

HostServer hostServer = new HostServer();
hostServer.Launch();

manualResetEvent.WaitOne();