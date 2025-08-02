namespace RR.App.Tests.E2E;

public static class AppiumServerHelper
{
    private static AppiumLocalService? _appiumLocalService;

    public static void StartAppiumLocalServer(string host, int port)
    {
        if (_appiumLocalService is not null && _appiumLocalService.IsRunning)
        {
            return;
        }

        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string appiumJSPath = Path.Combine(appDataPath, @"npm\node_modules\appium\build\lib\main.js");
        string nodePath = @"C:\Program Files\nodejs\node.exe"; // Update this path if necessary

        // Check if the paths exist
        if (!File.Exists(appiumJSPath))
        {
            throw new FileNotFoundException("Appium main.js file not found", appiumJSPath);
        }

        if (!File.Exists(nodePath))
        {
            nodePath = @"E:\node\node.exe";
            if (!File.Exists(nodePath))
                throw new FileNotFoundException("Node.js executable not found", nodePath);
        }

        var builder = new AppiumServiceBuilder()
            .WithIPAddress(host)
            .UsingPort(port)
            .WithAppiumJS(new FileInfo(appiumJSPath))
            .UsingDriverExecutable(new FileInfo(nodePath));

        // Start the server with the builder
        _appiumLocalService = builder.Build();
        _appiumLocalService.Start();
    }

    public static void DisposeAppiumLocalServer()
    {
        _appiumLocalService?.Dispose();
    }
}