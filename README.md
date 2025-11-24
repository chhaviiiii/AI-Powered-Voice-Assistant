# AI-Powered Voice Assistant 🎤

A modern voice assistant built with C# that listens to your commands and responds intelligently. Features a beautiful cross-platform GUI using Avalonia UI (works on Windows, macOS, and Linux).

## Features

- **Speech-to-Text**: 
  - Windows: Uses System.Speech for voice recognition
  - macOS/Linux: Text input mode (can be extended with native APIs)
- **Text-to-Speech**: 
  - Windows: Uses System.Speech synthesis
  - macOS: Uses native `say` command
- **Smart Commands**: Understands various commands including:
  - "What time is it?" - Get the current time
  - "What's the date?" - Get today's date
  - "Tell me a joke" - Hear a random joke
  - "Open YouTube" - Opens YouTube in your browser
  - "Open Google" - Opens Google search
  - "Open Calculator" - Launches Calculator (platform-specific)
  - "Open Notepad/TextEdit" - Opens text editor (platform-specific)
  - "Help" - See available commands
- **Modern UI**: Beautiful, dark-themed cross-platform interface with smooth animations (Avalonia UI)
- **Text Input**: Type commands directly in the UI input field
- **Real-time Conversation**: See your commands and responses in a chat-like interface

## Requirements

- .NET 8.0 SDK or later (or .NET 10.0 SDK)
- **Windows**: GUI with full speech recognition (WPF) or cross-platform GUI (Avalonia)
- **macOS/Linux**: Cross-platform GUI (Avalonia UI) with text input and text-to-speech
- Microphone for voice input (Windows)
- Speakers/headphones for audio output

## Installation

### macOS

1. **Install .NET SDK**:
   ```bash
   brew install --cask dotnet-sdk
   ```
   Or download from [Microsoft's website](https://dotnet.microsoft.com/download)

2. **Verify installation**:
   ```bash
   dotnet --version
   ```

### Windows

1. Download and install .NET 8.0 SDK from [Microsoft's website](https://dotnet.microsoft.com/download)
2. Or use Visual Studio 2022 which includes .NET SDK

## How to Run

### macOS/Linux (GUI Version)

1. **Restore and build**:
   ```bash
   dotnet restore
   dotnet build
   ```

2. **Run the application**:
   ```bash
   dotnet run
   ```

3. **Usage**:
   - A beautiful GUI window will open
   - Type your commands in the input field at the bottom
   - Press Enter or click Send to submit
   - The assistant will respond with text-to-speech (macOS) and display the response
   - Click the microphone button to start/stop listening mode

### Windows (GUI Version)

1. **Restore and build**:
   ```bash
   dotnet restore
   dotnet build
   ```

2. **Run the application**:
   ```bash
   dotnet run
   ```

3. **Usage**:
   - Click the microphone button to start listening
   - Speak your command clearly
   - The assistant will process your command and respond both visually and audibly
   - Click the microphone button again to stop listening

**Note**: On Windows, if WPF is not available, the application will automatically fall back to console mode.

## Project Structure

- `MainWindow.xaml` / `MainWindow.xaml.cs` - Main UI and window logic
- `VoiceAssistantService.cs` - Handles speech recognition and synthesis
- `CommandProcessor.cs` - Processes and responds to voice commands
- `App.xaml` / `App.xaml.cs` - Application entry point and resources

## Customization

You can easily extend the `CommandProcessor.cs` class to add more commands. Simply add new keyword checks and corresponding actions in the `ProcessCommand` method.

## Notes

- Make sure your microphone is enabled and working
- Speak clearly for best recognition results
- The application uses Windows Speech Recognition, which may require initial setup on first use