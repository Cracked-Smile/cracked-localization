# CrackedSmile.Localization
Ultra-simple wrapper around Unity Localization that makes it actually easy to use.

## Requires
* Addressables
* TextMeshPro
* (Optional) [UniTask](https://github.com/Cysharp/UniTask/tree/master)
> Supports async string load with UniTasks.  
> Just add `CRACKED_LOCALIZATION_UNITASK_SUPPORT` define.

## How To Use
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

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
