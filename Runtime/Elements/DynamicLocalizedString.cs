using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace CrackedSmile.Localization.Elements
{
	[ExecuteAlways]
	public class DynamicLocalizedString : MonoBehaviour
	{
		[SerializeField] private LocalizedString localizedString;
		[SerializeField] private TMP_Text textField;
		
		[Tooltip("Behavior to apply when the localized string contains argument placeholders but no arguments have been provided.")]
		[SerializeField] private MissingArgumentsBehavior missingArgumentsBehavior = MissingArgumentsBehavior.EraseArguments;

		readonly string _argumentPattern = @"\{[^{}]*\}";
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
			{
				if (string.IsNullOrEmpty(value))
				{
					textField.text = string.Empty;
					return;
				}
				
				string finalString = value;
				
				switch (missingArgumentsBehavior)
				{
					case MissingArgumentsBehavior.None:
						break;
					
					case MissingArgumentsBehavior.EraseArguments:
						finalString = Regex.Replace(value, _argumentPattern, string.Empty);
						break;
					
					case MissingArgumentsBehavior.EraseFullString:
						if (Regex.IsMatch(value, _argumentPattern))
							finalString = string.Empty;
						break;
					
					default:
						throw new ArgumentOutOfRangeException();
				}
				
				textField.text = finalString;
			}
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