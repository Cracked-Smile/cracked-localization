using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace CrackedLocalization
{
	[ExecuteAlways]
	public class DynamicLocalizedString : MonoBehaviour
	{
		[SerializeField] private LocalizedString localizedString;
		[SerializeField] private TMP_Text textField;

		private bool _stringSubscribed;

		public LocalizedString StringReference => localizedString;

		private void OnEnable()
		{
			Register();
			Refresh();
		}

		private void OnDisable()
		{
			Unregister();
		}

		protected virtual void OnDestroy()
		{
			Unregister();
		}

		private void OnValidate()
		{
			if (isActiveAndEnabled)
			{
				Unregister();
				Register();
				Refresh();
			}
		}

		private void Register()
		{
			if (localizedString == null || _stringSubscribed)
				return;

			localizedString.StringChanged += OnStringChanged;
			_stringSubscribed = true;
		}

		private void Unregister()
		{
			if (localizedString != null && _stringSubscribed)
				localizedString.StringChanged -= OnStringChanged;

			_stringSubscribed = false;
		}

		private void OnStringChanged(string value)
		{
			if (textField != null)
				textField.text = value ?? string.Empty;
		}

		private void Refresh()
		{
			if (localizedString == null || textField == null)
				return;

			localizedString.RefreshString();
		}

		public void SetArguments(params object[] args)
		{
			if (localizedString == null) return;
			localizedString.Arguments = args;
			Refresh();
		}
	}
}