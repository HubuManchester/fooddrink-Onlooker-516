# FoodPicker - 今天吃什么 🍲

A cross-platform mobile app built with **.NET MAUI** that helps users decide what to eat.  
Theme: **Food and Drink**.

---

## Features

### 📱 Four Main Tabs

| Tab | Description |
|-----|-------------|
| 🎲 **随机推荐** | Shake your phone or tap a button to get a random food recommendation |
| 🧭 **美食指南** | Compass that recommends regional cuisine based on which direction you're facing |
| 📷 **美食相册** | Take photos of your meals and browse them in a grid |
| ⭐ **我的收藏** | Save favorite recommendations and add personal notes |

### 🔧 Mobile Hardware Used (7 features)

| # | Hardware | Feature |
|---|----------|---------|
| 1 | Accelerometer | Shake to get a new food suggestion |
| 2 | Compass / Magnetometer | Food recommendations based on heading direction |
| 3 | Geolocation + Geocoding | Get user location and display address |
| 4 | Camera | Take photos of meals |
| 5 | Text-to-Speech | Voice announcement of recommended foods |
| 6 | Audio | Custom "rattle" sound effect (procedurally generated WAV) |
| 7 | Haptic Feedback | Vibration on button taps and shake events |

### ✅ Other Features

- Dark mode support (auto-adapts to system theme)
- Input validation with user-friendly error messages
- Accessibility labels for screen readers
- MVVM architecture
- Cross-platform: Android & Windows

---

## Tech Stack

- **.NET MAUI** (.NET 9)
- **XAML** for UI
- **C#** for logic
- **MVVM** pattern
- **Plugin.Maui.Audio** for sound playback

---

## Getting Started

### Prerequisites

- .NET 9 SDK
- MAUI workload (`dotnet workload install maui`)
- Visual Studio 2022 (recommended) or VS Code

### Run on Android

```bash
dotnet build -f net9.0-android
```

Then deploy via Visual Studio Android Emulator or physical device.

### Run on Windows

```bash
dotnet build -f net9.0-windows10.0.19041.0
dotnet run -f net9.0-windows10.0.19041.0
```

---

## Project Structure

```
FoodPicker/
├── Models/          # FoodItem, FavoriteItem
├── ViewModels/      # RandomViewModel, CompassViewModel, etc.
├── Views/           # XAML pages (RandomPage, CompassPage, ...)
├── Services/        # FoodDataService, FavoritesService
├── Helpers/         # SoundHelper, Converters
├── Resources/       # Fonts, Images, Styles (Colors.xaml, Styles.xaml)
└── Platforms/       # Android, iOS, Windows platform-specific code
```

---

## Screencast

A 10-15 minute screencast demonstrates all features against the marking criteria:
- UI/UX Design & Accessibility (30%)
- Mobile Hardware (20%)
- Functionality (20%)
- Validation & Error Handling (10%)
- Code Quality (10%)
- Deployment (5%)
- GitHub Usage (5%)

---

## Author

Rong Xiao — Mobile Computing Coursework, 2026
