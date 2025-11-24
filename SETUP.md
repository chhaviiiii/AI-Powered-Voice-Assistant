# Quick Setup Guide

## For macOS Users

### Step 1: Install .NET SDK

You have two options:

**Option A: Using Homebrew (Recommended)**
```bash
brew install --cask dotnet-sdk
```

**Option B: Manual Installation**
1. Visit https://dotnet.microsoft.com/download
2. Download .NET 8.0 SDK for macOS
3. Run the installer

### Step 2: Verify Installation

```bash
dotnet --version
```

You should see something like `8.0.xxx`

### Step 3: Run the Application

```bash
cd /Users/chhavinayyar/AI-Powered-Voice-Assistant
dotnet restore
dotnet build
dotnet run
```

### Step 4: Use the Assistant

- Type commands in the console (e.g., "what time is it", "tell me a joke")
- The assistant will respond with text-to-speech on macOS
- Type `help` to see all available commands
- Type `exit` to quit

## Troubleshooting

**Issue: "command not found: dotnet"**
- Solution: Make sure .NET SDK is installed and your PATH is set correctly
- Try: `export PATH="$PATH:/usr/local/share/dotnet"` or restart your terminal

**Issue: Build errors**
- Solution: Make sure you're using .NET 8.0 SDK
- Check: `dotnet --version` should show 8.0.x

**Issue: Text-to-speech not working**
- Solution: Make sure macOS has speech permissions enabled
- Check: System Preferences > Security & Privacy > Privacy > Speech Recognition

## Notes

- The macOS version uses console input (type commands) instead of voice recognition
- Text-to-speech uses macOS native `say` command
- For full voice recognition on macOS, you would need to integrate AVFoundation (requires additional setup)

