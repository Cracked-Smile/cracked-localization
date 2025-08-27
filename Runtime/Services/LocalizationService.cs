using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrackedSmile.Localization.Services.Utility;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
#if CRACKED_LOCALIZATION_UNITASK_SUPPORT
using Cysharp.Threading.Tasks;
#endif

namespace CrackedSmile.Localization.Services
{
	public class LocalizationService : ILocalizationService, IDisposable
	{
		private readonly HashSet<AsyncOperationHandle> _pending = new();
		
		public void SetLocale(string localeIdentifier)
		{
			Locale locale = LocalizationSettings.AvailableLocales.GetLocale(localeIdentifier);
			
			if (locale != null)
			{
				LocalizationSettings.SelectedLocale = locale;
			}
			else
			{
				Debug.LogError($"Locale with identifier '{localeIdentifier}' not found.");
			}
		}

		public void Dispose()
		{
			CleanupPending();
		}

		public Locale GetCurrentLocale()
		{
			return LocalizationSettings.SelectedLocale;
		}
        
		public List<Locale> GetAllLocales()
		{
			return LocalizationSettings.AvailableLocales.Locales.ToList();
		}
			
#if CRACKED_LOCALIZATION_UNITASK_SUPPORT
		public async UniTask<string> GetLocalizedStringAsUniTask(string tableName, string entryName, params object[] args)
		{
			AsyncOperationHandle<string> handle = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(tableName, entryName, args);
			
			try
			{
				return await handle;
			}
			finally
			{
				if (handle.IsValid())
					Addressables.Release(handle);
			}
		}
#endif
		
		public async Task<string> GetLocalizedStringAsTask(string tableName, string entryName, params object[] args)
		{
			string localizedString = await LocalizationSettings.StringDatabase.GetLocalizedStringAsync(tableName, entryName, args).Task;
			return localizedString;
		}
		
		public void GetLocalizedStringAsync(string tableName, string entryName, Action<string> onComplete, params object[] args)
		{
			AsyncOperationHandle<string> pendingHandle = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(tableName, entryName, args);
			_pending.Add(pendingHandle);
			
			pendingHandle.Completed += handle =>
			{
				try
				{
					if (handle.Status == AsyncOperationStatus.Succeeded)
					{
						onComplete?.Invoke(handle.Result);
					}
					else
					{
						Debug.LogError($"Failed to load localized string for table: {tableName}, entry: {entryName}");
						onComplete?.Invoke(null);
					}
				}
				finally
				{
					SafeRelease(handle);
				}
			};
		}
		
		public string GetLocalizedString(string tableName, string entryName, params object[] args)
		{
			string localizedString = LocalizationSettings.StringDatabase.GetLocalizedString(tableName, entryName, args);
			return localizedString;
		}

		public IDisposable RegisterLocaleChangeCallback(Action onLocaleChanged)
		{
			LocalizationSettings.SelectedLocaleChanged += Handler;

			return new ActionDisposable(() => LocalizationSettings.SelectedLocaleChanged -= Handler);

			void Handler(Locale _)
			{
				onLocaleChanged?.Invoke();
			}
		}

		private void CleanupPending()
		{
			foreach (AsyncOperationHandle handle in _pending)
			{
				SafeRelease(handle);
			}
			
			_pending.Clear();
		}

		private void SafeRelease(AsyncOperationHandle handle)
		{
			if (_pending.Contains(handle))
				_pending.Remove(handle);
			
			if (handle.IsValid())
				Addressables.Release(handle);
		}
	}
}