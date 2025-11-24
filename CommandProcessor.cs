using System;
using System.Diagnostics;
using System.Linq;

namespace VoiceAssistant
{
    public class CommandProcessor
    {
        public string ProcessCommand(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
                return "I didn't catch that. Could you repeat?";

            string lowerCommand = command.ToLower();

            // Time command
            if (ContainsKeywords(lowerCommand, new[] { "time", "what time", "tell me the time" }))
            {
                return GetTimeResponse();
            }

            // Date command
            if (ContainsKeywords(lowerCommand, new[] { "date", "what date", "what's the date", "today's date" }))
            {
                return GetDateResponse();
            }

            // Joke command
            if (ContainsKeywords(lowerCommand, new[] { "joke", "tell me a joke", "make me laugh", "funny" }))
            {
                return GetJoke();
            }

            // YouTube command
            if (ContainsKeywords(lowerCommand, new[] { "open youtube", "youtube", "open yt" }))
            {
                OpenYouTube();
                return "Opening YouTube for you!";
            }

            // Google command
            if (ContainsKeywords(lowerCommand, new[] { "open google", "google" }))
            {
                OpenGoogle();
                return "Opening Google for you!";
            }

            // Calculator command
            if (ContainsKeywords(lowerCommand, new[] { "calculator", "open calculator", "calc" }))
            {
                OpenCalculator();
                return "Opening calculator for you!";
            }

            // Notepad command
            if (ContainsKeywords(lowerCommand, new[] { "notepad", "open notepad", "text editor" }))
            {
                OpenNotepad();
                return "Opening Notepad for you!";
            }

            // Greeting
            if (ContainsKeywords(lowerCommand, new[] { "hello", "hi", "hey", "good morning", "good afternoon", "good evening" }))
            {
                return GetGreeting();
            }

            // Help command
            if (ContainsKeywords(lowerCommand, new[] { "help", "what can you do", "commands", "capabilities" }))
            {
                return GetHelpMessage();
            }

            // Weather (placeholder - can be extended with API)
            if (ContainsKeywords(lowerCommand, new[] { "weather", "temperature", "how's the weather" }))
            {
                return "I'm sorry, weather information is not available yet. This feature can be added with a weather API integration.";
            }

            // Default response
            return $"I heard you say: \"{command}\". I'm still learning, but I can help you with time, date, jokes, opening apps like YouTube, Google, Calculator, and Notepad. Try saying 'help' for more options!";
        }

        private bool ContainsKeywords(string text, string[] keywords)
        {
            return keywords.Any(keyword => text.Contains(keyword));
        }

        private string GetTimeResponse()
        {
            DateTime now = DateTime.Now;
            string timeString = now.ToString("h:mm tt");
            return $"The current time is {timeString}.";
        }

        private string GetDateResponse()
        {
            DateTime now = DateTime.Now;
            string dateString = now.ToString("dddd, MMMM dd, yyyy");
            return $"Today is {dateString}.";
        }

        private string GetJoke()
        {
            string[] jokes = {
                "Why don't scientists trust atoms? Because they make up everything!",
                "Why did the scarecrow win an award? He was outstanding in his field!",
                "Why don't eggs tell jokes? They'd crack each other up!",
                "What do you call a fake noodle? An impasta!",
                "Why did the math book look so sad? Because it had too many problems!",
                "What's the best thing about Switzerland? I don't know, but the flag is a big plus!",
                "Why don't skeletons fight each other? They don't have the guts!",
                "What do you call a bear with no teeth? A gummy bear!",
                "Why did the coffee file a police report? It got mugged!",
                "What's orange and sounds like a parrot? A carrot!"
            };

            Random random = new Random();
            return jokes[random.Next(jokes.Length)];
        }

        private void OpenYouTube()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://www.youtube.com",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening YouTube: {ex.Message}");
            }
        }

        private void OpenGoogle()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://www.google.com",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening Google: {ex.Message}");
            }
        }

        private void OpenCalculator()
        {
            try
            {
                if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                {
                    Process.Start("calc.exe");
                }
                else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
                {
                    Process.Start("open", "-a Calculator");
                }
                else
                {
                    Process.Start("gnome-calculator");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening Calculator: {ex.Message}");
            }
        }

        private void OpenNotepad()
        {
            try
            {
                if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                {
                    Process.Start("notepad.exe");
                }
                else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
                {
                    Process.Start("open", "-a TextEdit");
                }
                else
                {
                    Process.Start("gedit");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening Text Editor: {ex.Message}");
            }
        }

        private string GetGreeting()
        {
            int hour = DateTime.Now.Hour;
            string greeting = hour < 12 ? "Good morning" : hour < 18 ? "Good afternoon" : "Good evening";
            return $"{greeting}! How can I help you today?";
        }

        private string GetHelpMessage()
        {
            return "I can help you with several things:\n" +
                   "• Tell you the current time\n" +
                   "• Tell you today's date\n" +
                   "• Tell you a joke\n" +
                   "• Open YouTube, Google, Calculator, or Notepad\n" +
                   "• Have a conversation\n" +
                   "Just speak naturally and I'll try to understand!";
        }
    }
}

