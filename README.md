# FoodPicker 🍲

A cross-platform mobile app built with **.NET MAUI** that helps users decide what to eat.  
Theme: **Food and Drink** — 53 dishes across 14 regional Chinese cuisines.

---

## Features

### 📱 Four Main Tabs

| Tab | Description |
|-----|-------------|
| 🎲 **Discover** | Shake your phone or tap to get a random food recommendation. Search for dishes, save favourites, share with friends. |
| 🧭 **Compass** | Real-time magnetic compass that recommends regional cuisine based on the direction you are facing. |
| 📷 **Album** | Take photos or import from gallery. Organise by category (Friends, Tasty & Cheap, Late Night, etc.) |
| ⭐ **Favourites** | View saved dishes with stats. Add personal notes. Pick a random favourite when you cannot decide. |

### 🔧 Mobile Hardware Used (5 features)

| # | Hardware | Feature |
|---|----------|---------|
| 1 | Accelerometer (Shake) | Shake phone to get a new food suggestion with animation |
| 2 | Compass / Magnetometer | Real-time direction-based food recommendations with smoothed rotation |
| 3 | Geolocation + Geocoding | GPS location with reverse geocoding and permission handling |
| 4 | Camera | Take food photos and save to local storage |
| 5 | Haptic Feedback | Tactile vibration on button taps and shake events |

### ✅ Other Highlights

- **Dark mode** — auto-adapts to system theme via `AppThemeBinding`
- **Input validation** — empty check and minimum length with friendly error messages
- **Accessibility** — `SemanticProperties` on all interactive elements, minimum 44px touch targets
- **53 dish database** — 14 regional categories with emoji, descriptions, and pairing suggestions
- **Food search** — 5-layer matching (name, description, category, character-level)
- **Share integration** — system share sheet for food recommendations
- **Custom sound effects** — procedurally generated WAV "rattle" sound
- **MVVM architecture** — clean separation of Views, ViewModels, Models, and Services

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
├── Models/          # FoodItem, FavoriteItem, PhotoItem
├── ViewModels/      # RandomViewModel, CompassViewModel, AlbumViewModel, FavoritesViewModel
├── Views/           # XAML pages (RandomPage, CompassPage, AlbumPage, FavoritesPage)
├── Services/        # FoodDataService (53 dishes), FavoritesService (singleton)
├── Helpers/         # SoundHelper (WAV generation), Converters
├── Resources/       # Fonts, Images, Styles (Colors.xaml, Styles.xaml)
└── Platforms/       # Android, iOS, Windows platform-specific code
```

---

## Screencast

A 10-15 minute screencast demonstrating all features against the marking criteria:
- UI/UX Design & Accessibility (30%)
- Mobile Hardware (20%)
- Functionality (20%)
- Validation & Error Handling (10%)
- Code Quality (10%)
- Deployment (5%)
- GitHub Usage (5%)

---

## Author

Yang Senyu — Mobile Computing Coursework, 2026
