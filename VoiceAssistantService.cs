using System;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading.Tasks;

namespace VoiceAssistant
{
    public class VoiceAssistantService : IDisposable
    {
        private SpeechRecognitionEngine _recognizer;
        private SpeechSynthesizer _synthesizer;
        private CommandProcessor _commandProcessor;
        private bool _isDisposed = false;

        public event Action<string, string> OnCommandProcessed;
        public event Action<string> OnStatusChanged;

        public VoiceAssistantService()
        {
            InitializeSpeechRecognition();
            InitializeSpeechSynthesis();
            _commandProcessor = new CommandProcessor();
        }

        private void InitializeSpeechRecognition()
        {
            try
            {
                _recognizer = new SpeechRecognitionEngine();
                
                // Create grammar with common commands
                var grammarBuilder = new GrammarBuilder();
                grammarBuilder.AppendDictation();
                
                var grammar = new Grammar(grammarBuilder);
                _recognizer.LoadGrammar(grammar);

                _recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
                _recognizer.SpeechRecognitionRejected += Recognizer_SpeechRecognitionRejected;
                _recognizer.SetInputToDefaultAudioDevice();
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"Error initializing speech recognition: {ex.Message}");
            }
        }

        private void InitializeSpeechSynthesis()
        {
            try
            {
                _synthesizer = new SpeechSynthesizer();
                _synthesizer.SetOutputToDefaultAudioDevice();
                _synthesizer.Rate = 0; // Normal speed
                _synthesizer.Volume = 100;
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"Error initializing speech synthesis: {ex.Message}");
            }
        }

        public void StartListening()
        {
            try
            {
                if (_recognizer != null && !_isDisposed)
                {
                    OnStatusChanged?.Invoke("🎤 Listening...");
                    _recognizer.RecognizeAsync(RecognizeMode.Multiple);
                }
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"Error starting recognition: {ex.Message}");
            }
        }

        public void StopListening()
        {
            try
            {
                if (_recognizer != null && !_isDisposed)
                {
                    _recognizer.RecognizeAsyncStop();
                    OnStatusChanged?.Invoke("Ready to listen...");
                }
            }
            catch (Exception ex)
            {
                OnStatusChanged?.Invoke($"Error stopping recognition: {ex.Message}");
            }
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string recognizedText = e.Result.Text;
            OnStatusChanged?.Invoke("Processing command...");
            
            Task.Run(() =>
            {
                string response = _commandProcessor.ProcessCommand(recognizedText);
                Speak(response);
                OnCommandProcessed?.Invoke(recognizedText, response);
                OnStatusChanged?.Invoke("Ready to listen...");
            });
        }

        private void Recognizer_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            OnStatusChanged?.Invoke("Could not understand. Please try again...");
        }

        public void Speak(string text)
        {
            try
            {
                if (_synthesizer != null && !_isDisposed)
                {
                    _synthesizer.SpeakAsync(text);
                }
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
                _recognizer?.RecognizeAsyncStop();
                _recognizer?.Dispose();
                _synthesizer?.Dispose();
            }
            catch { }

            _isDisposed = true;
        }
    }
}

