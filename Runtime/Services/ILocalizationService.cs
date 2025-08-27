using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Localization;
#if CRACKED_LOCALIZATION_UNITASK_SUPPORT
using Cysharp.Threading.Tasks;
#endif

namespace CrackedSmile.Localization.Services
{
	public interface ILocalizationService
	{
		void SetLocale(string localeIdentifier);
		Locale GetCurrentLocale();
		List<Locale> GetAllLocales();
#if CRACKED_LOCALIZATION_UNITASK_SUPPORT
		UniTask<string> GetLocalizedStringAsUniTask(string tableName, string entryName, params object[] args);
#endif
		Task<string> GetLocalizedStringAsTask(string tableName, string entryName, params object[] args);
		void GetLocalizedStringAsync(string tableName, string entryName, Action<string> onComplete, params object[] args);
		string GetLocalizedString(string tableName, string entryName, params object[] args);
		IDisposable RegisterLocaleChangeCallback(Action onLocaleChanged);
	}
}