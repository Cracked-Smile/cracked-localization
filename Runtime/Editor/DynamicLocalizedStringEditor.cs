using CrackedSmile.Localization.Elements;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace CrackedSmile.Localization.cracked_localization.Runtime.Editor
{
#if UNITY_EDITOR
	[CanEditMultipleObjects]
	[CustomEditor(typeof(DynamicLocalizedString), true)]
	public class DynamicLocalizedStringEditor : UnityEditor.Editor
	{
		private SerializedProperty _localizedStringProp;
		private SerializedProperty _textFieldProp;
		private SerializedProperty _missingArgsBehaviorProp;
		private SerializedProperty _autoArgumentsProp;

		private void OnEnable()
		{
			_localizedStringProp = serializedObject.FindProperty("localizedString");
			_textFieldProp = serializedObject.FindProperty("textField");
			_missingArgsBehaviorProp = serializedObject.FindProperty("missingArgumentsBehavior");
			_autoArgumentsProp = serializedObject.FindProperty("autoArguments");

			Undo.undoRedoPerformed += OnUndoRedoPerformed;
		}

		private void OnDisable()
		{
			Undo.undoRedoPerformed -= OnUndoRedoPerformed;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(_localizedStringProp);
			EditorGUILayout.PropertyField(_textFieldProp);
			EditorGUILayout.PropertyField(_missingArgsBehaviorProp);

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(_autoArgumentsProp, includeChildren: true);
			bool autoArgsChanged = EditorGUI.EndChangeCheck();

			serializedObject.ApplyModifiedProperties();

			if (autoArgsChanged)
			{
				RefreshAllTargets();
			}
		}

		private void OnUndoRedoPerformed()
		{
			if (this == null)
				return;

			serializedObject.Update();
			serializedObject.ApplyModifiedProperties();
			RefreshAllTargets();
			Repaint();
		}

		private void RefreshAllTargets()
		{
			foreach (Object currentTarget in targets)
			{
				DynamicLocalizedString component = currentTarget as DynamicLocalizedString;
				
				if (component == null) 
					continue;

				EditorApplication.delayCall += () =>
				{
					if (component == null) 
						return;
					
					EditorUtility.SetDirty(component);
					
					try
					{
						component.Refresh();
					}
					catch (System.Exception e)
					{
						Debug.LogException(e, component);
					}

					if (!Application.isPlaying)
						EditorApplication.QueuePlayerLoopUpdate();
				};
			}
		}
	}
#endif
}