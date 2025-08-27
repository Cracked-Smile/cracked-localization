# CrackedSmile.Localization Documentation

## How To Use
### Create the Localization Settings
The Project's Localization Settings is an Asset. To create this Asset, go to **Edit > Project Settings > Localization** and click **Create**.  
<img width="891" height="811" alt="image" src="https://github.com/user-attachments/assets/02c2ca9f-93d2-4cda-9739-1289c528061c" />

### Create locales
A [locale](https://docs.unity3d.com/Packages/com.unity.localization@1.5/manual/Locale.html) represents a language and an optional region; they can also contain additional information, such as currency, calendar, and user-added custom data.

To open the Locale Generator window, navigate to the Localization Settings (menu: **Edit > Project Settings > Localization**) and select **Locale Generator**.  
<img width="891" height="811" alt="image" src="https://github.com/user-attachments/assets/3951e8c0-6767-41a5-9785-698426fcaba7" />
1. Select the checkbox next to the Locales you want to add.
2. Select **Generate Locales**.
3. Select where you want to save the Assets to. Unity creates an asset file for each locale you select.
   <img width="502" height="670" alt="image" src="https://github.com/user-attachments/assets/1136d981-3572-4823-b0fe-bd47b7fcd617" />

### Choose a default Locale
Use the Locale Selectors to determine which Locale your application uses by default if it is not English(en).
<img width="891" height="811" alt="image" src="https://github.com/user-attachments/assets/cff76909-b6a2-492e-9ff4-741572b148b0" />

### Setup localized strings
Strings are one of the most common areas to localize. Strings can either be static, in that the contents never change, or dynamic such as text that changes based on the current game status. The localization system uses [String Tables](https://docs.unity3d.com/Packages/com.unity.localization@1.5/manual/StringTables.html).
To localize strings, follow these steps:

1. Create the String Table Collection.
2. Select `Localization Talbe` and then clock `Open in Table Editor` button in inspector.
3. Add new entry by clicking `New Entry` button.
4. Add localized text for all languages (make sure `Key` is unique).

### Dynamic Localized String
`DynamicLocalizedString` is a MonoBehaviour that will likely cover 99% of your needs.
It automatically refreshes the string based on the current locale and works in both **Edit Mode** and **Play Mode**.
Simply select the string you want this text to use, and reference the `TMP_Text` component where the localized text should be applied.
<img width="864" height="119" alt="image" src="https://github.com/user-attachments/assets/f553e2e3-c56f-48d7-b786-3de42952335c" />

It is also dynamic, which means it can include dynamic arguments.  
Arguments in a localized string should use the following format: `{0}`, `{1}`, `{2}`, and so on.

Arguments can be set as follows:
```cs
int score = 123;
int timeSpent = 60;
_scoreLocalizedText.SetArguments(score, timeSpent); // Result: "Score: 123, Time spent: 60"
```

### Missing arguments bahaviour
Allows to select what should happen when the arguments were not provided. Works in both **EditMode** and **PlayMode**.
![Aug-27-2025 13-23-31](https://github.com/user-attachments/assets/a9411f91-44a5-40f0-9108-05742d75ca04)

### LocalizationService
`LocalizaitonService` is a layer between you and Unity's `LocalizationSettings`.  
You can still use `LocalizationSettings` directly if you prefer.  
LocalizationService was built for Dependency Injection, but works standalone too.  
If you are on the [YOLO](https://en.wikipedia.org/wiki/YOLO_(aphorism)) side you can wrap it in singleton yourself.  
With `LocalizationService` you can:
1. Set/Get the current locale.
2. Get all existing locales.
3. Get localized strings (sync and async).
4. Register a callback for locale changes.  
   
More features coming soon.
