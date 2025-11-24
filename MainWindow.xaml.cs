using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace VoiceAssistant
{
    public partial class MainWindow : Window
    {
        private readonly VoiceAssistantService _voiceAssistant;
        private bool _isListening = false;

        public MainWindow()
        {
            InitializeComponent();
            _voiceAssistant = new VoiceAssistantService();
            _voiceAssistant.OnCommandProcessed += HandleCommandResponse;
            _voiceAssistant.OnStatusChanged += UpdateStatus;
        }

        private void VoiceButton_Click(object sender, RoutedEventArgs e)
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
            _voiceAssistant.StartListening();
            AddMessage("You", "🎤 Listening...", true);
        }

        private void StopListening()
        {
            _isListening = false;
            AnimateButton(false);
            _voiceAssistant.StopListening();
        }

        private void AnimateButton(bool isActive)
        {
            var colorAnimation = new ColorAnimation
            {
                To = isActive ? Colors.FromRgb(239, 68, 68) : Colors.FromRgb(99, 102, 241),
                Duration = TimeSpan.FromMilliseconds(300)
            };

            var brush = new SolidColorBrush(Colors.FromRgb(99, 102, 241));
            VoiceButton.Fill = brush;
            brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }

        private void HandleCommandResponse(string userInput, string response)
        {
            Dispatcher.Invoke(() =>
            {
                AddMessage("You", userInput, true);
                AddMessage("Assistant", response, false);
            });
        }

        private void UpdateStatus(string status)
        {
            Dispatcher.Invoke(() =>
            {
                StatusText.Text = status;
            });
        }

        private void AddMessage(string sender, string message, bool isUser)
        {
            var border = new Border
            {
                Background = isUser 
                    ? new SolidColorBrush(Colors.FromRgb(99, 102, 241)) 
                    : new SolidColorBrush(Colors.FromRgb(30, 41, 59)),
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

            ConversationScroll.ScrollToEnd();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            _voiceAssistant.Dispose();
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            _voiceAssistant.Dispose();
            base.OnClosed(e);
        }
    }
}

