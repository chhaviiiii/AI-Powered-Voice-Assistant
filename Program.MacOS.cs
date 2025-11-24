using System;
using System.Threading.Tasks;

namespace VoiceAssistant
{
    // macOS Console version
    public class ProgramMacOS
    {
        private static VoiceAssistantServiceMacOS? _assistant;

        public static void Run(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("🎤 AI Voice Assistant (macOS Console Version)");
            Console.WriteLine("=============================================\n");

            _assistant = new VoiceAssistantServiceMacOS();
            _assistant.OnCommandProcessed += (input, response) =>
            {
                Console.WriteLine($"\nYou: {input}");
                Console.WriteLine($"Assistant: {response}\n");
            };
            _assistant.OnStatusChanged += (status) =>
            {
                Console.WriteLine($"Status: {status}");
            };

            Console.WriteLine("Type 'help' for available commands, or 'exit' to quit.\n");
            Console.WriteLine("Note: For full speech recognition, consider using a macOS app with AVFoundation.\n");

            RunConsoleLoop();
        }

        private static void RunConsoleLoop()
        {
            while (true)
            {
                Console.Write("🎤 > ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrEmpty(input))
                    continue;

                if (input.ToLower() == "exit" || input.ToLower() == "quit")
                {
                    Console.WriteLine("Goodbye! 👋");
                    _assistant?.Dispose();
                    break;
                }

                _assistant?.ProcessTextInput(input);
            }
        }
    }
}

