using System;
using System.Runtime.InteropServices;
using Avalonia;

namespace VoiceAssistant
{
    public class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            // Detect OS and run appropriate version
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || 
                RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // Run Avalonia UI on macOS/Linux
                BuildAvaloniaApp()
                    .StartWithClassicDesktopLifetime(args);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Try to run Windows WPF version, fallback to Avalonia if WPF not available
                try
                {
#if WINDOWS
                    var app = new System.Windows.Application();
                    app.InitializeComponent();
                    app.Run();
#else
                    throw new PlatformNotSupportedException("WPF not available");
#endif
                }
                catch
                {
                    // Fallback to Avalonia UI on Windows if WPF fails
                    BuildAvaloniaApp()
                        .StartWithClassicDesktopLifetime(args);
                }
            }
            else
            {
                // Fallback to Avalonia UI for unsupported OS
                BuildAvaloniaApp()
                    .StartWithClassicDesktopLifetime(args);
            }
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}

