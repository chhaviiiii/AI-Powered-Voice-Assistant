using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace VoiceAssistant
{
    // macOS-compatible implementation using native commands
    public class VoiceAssistantServiceMacOS : IDisposable
    {
        private CommandProcessor _commandProcessor;
        private Process? _speechProcess;
        private bool _isDisposed = false;

        public event Action<string, string>? OnCommandProcessed;
        public event Action<string>? OnStatusChanged;

        public VoiceAssistantServiceMacOS()
        {
            _commandProcessor = new CommandProcessor();
        }

        public void StartListening()
        {
            OnStatusChanged?.Invoke("🎤 Please type your command (macOS speech recognition requires additional setup)...");
            // Note: For full speech recognition on macOS, you would need to use
            // AVFoundation or a third-party library. This version uses text input.
        }

        public void StopListening()
        {
            OnStatusChanged?.Invoke("Ready to listen...");
        }

        // Process text input (can be extended with actual speech recognition)
        public void ProcessTextInput(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            OnStatusChanged?.Invoke("Processing command...");
            
            Task.Run(() =>
            {
                string response = _commandProcessor.ProcessCommand(text);
                Speak(response);
                OnCommandProcessed?.Invoke(text, response);
                OnStatusChanged?.Invoke("Ready to listen...");
            });
        }

        public void Speak(string text)
        {
            try
            {
                if (_isDisposed) return;

                // Use macOS 'say' command for text-to-speech
                var startInfo = new ProcessStartInfo
                {
                    FileName = "/usr/bin/say",
                    Arguments = $"\"{text}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                _speechProcess = Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"Error speaking: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            try
            {
                _speechProcess?.Kill();
                _speechProcess?.Dispose();
            }
            catch { }

            _isDisposed = true;
        }
    }
}

