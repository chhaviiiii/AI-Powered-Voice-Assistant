using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;

namespace VoiceAssistant
{
    public partial class MainWindowAvalonia : Window
    {
        private VoiceAssistantServiceMacOS? _voiceAssistant;
        private bool _isListening = false;

        public MainWindowAvalonia()
        {
            InitializeComponent();
            
            // Initialize the voice assistant
            _voiceAssistant = new VoiceAssistantServiceMacOS();
            _voiceAssistant.OnCommandProcessed += HandleCommandResponse;
            _voiceAssistant.OnStatusChanged += UpdateStatus;
        }

        private void VoiceButton_Click(object? sender, RoutedEventArgs e)
        {
            if (!_isListening)
            {
                StartListening();
            }
            else
            {
                StopListening();
            }
        }

        private void StartListening()
        {
            _isListening = true;
            AnimateButton(true);
            _voiceAssistant?.StartListening();
            AddMessage("You", "🎤 Ready! Type your command in the input field above", true);
            
            // Focus the input field
            if (CommandInput != null)
            {
                CommandInput.Focus();
            }
        }
        
        private void SendButton_Click(object? sender, RoutedEventArgs e)
        {
            SendCommand();
        }
        
        private void CommandInput_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendCommand();
            }
        }
        
        private void SendCommand()
        {
            if (CommandInput != null && !string.IsNullOrWhiteSpace(CommandInput.Text))
            {
                string command = CommandInput.Text.Trim();
                CommandInput.Text = "";
                _voiceAssistant?.ProcessTextInput(command);
            }
        }

        private void StopListening()
        {
            _isListening = false;
            AnimateButton(false);
            _voiceAssistant?.StopListening();
        }

        private void AnimateButton(bool isActive)
        {
            // Change button color
            if (VoiceButton != null)
            {
                VoiceButton.Fill = new SolidColorBrush(
                    isActive ? Color.FromRgb(239, 68, 68) : Color.FromRgb(99, 102, 241)
                );
            }
        }

        private void HandleCommandResponse(string userInput, string response)
        {
            Dispatcher.UIThread.Post(() =>
            {
                AddMessage("You", userInput, true);
                AddMessage("Assistant", response, false);
            });
        }

        private void UpdateStatus(string status)
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (StatusText != null)
                {
                    StatusText.Text = status;
                }
            });
        }

        private void AddMessage(string sender, string message, bool isUser)
        {
            if (ConversationPanel == null) return;

            var border = new Border
            {
                Background = isUser 
                    ? new SolidColorBrush(Color.FromRgb(99, 102, 241)) 
                    : new SolidColorBrush(Color.FromRgb(30, 41, 59)),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(15),
                Margin = new Thickness(0, 0, 0, 10),
                HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                MaxWidth = 500
            };

            var textBlock = new TextBlock
            {
                Text = $"{sender}: {message}",
                Foreground = new SolidColorBrush(Colors.White),
                TextWrapping = TextWrapping.Wrap,
                FontSize = 14
            };

            border.Child = textBlock;
            ConversationPanel.Children.Add(border);

            // Scroll to bottom
            if (ConversationScroll != null)
            {
                Dispatcher.UIThread.Post(() =>
                {
                    ConversationScroll.ScrollToEnd();
                }, DispatcherPriority.Background);
            }
        }

        private void MinimizeButton_Click(object? sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object? sender, RoutedEventArgs e)
        {
            _voiceAssistant?.Dispose();
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            _voiceAssistant?.Dispose();
            base.OnClosed(e);
        }
    }
}

