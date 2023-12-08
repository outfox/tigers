using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Tiger.Events.Editor
{
	[CustomEditor(typeof(DataChannel<>), true)]
	public class DataChannelEditor : UnityEditor.Editor
	{
		private bool _foldout = true;
		public override void OnInspectorGUI()
		{

			// Use reflection to get the generic type of the target
			DrawDefaultInspector();
			EditorGUILayout.Space();
			var targetType = target.GetType();
			GUILayout.Label(target.name, EditorStyles.whiteLabel);
			EditorGUILayout.LabelField($"Underlying Type", targetType.ToString());
			
			EditorGUI.BeginDisabledGroup(!Application.isPlaying || serializedObject.isEditingMultipleObjects);
			EditorGUILayout.Space();
			
			_foldout = EditorGUILayout.BeginFoldoutHeaderGroup(_foldout, "Emit Value (Runtime Only)");
			ListAvailableStates();
			EditorGUILayout.EndFoldoutHeaderGroup();
			
			EditorGUI.EndDisabledGroup();
		}


		private object ExtractCurrent()
		{
			var targetType = target.GetType();
			var valueField = targetType.GetField("_value", BindingFlags.Instance | BindingFlags.NonPublic);
			var value = valueField?.GetValue(target);
			return value;
		}

		private object ExtractDefault()
		{
			var targetType = target.GetType();
			var defaultValueField = targetType.GetField("defaultValue", BindingFlags.Instance | BindingFlags.NonPublic);
			var defaultValue = defaultValueField?.GetValue(target);
			return defaultValue;
		}

		private bool ExtractEnumerated(out string[] enumNames, out Array enumValues)
		{
			enumNames = null;
			enumValues = null;
			
			// Now we need to find the Emit method and the defaultValue field
			var targetType = target.GetType();
			var enumerateMethod = targetType.GetMethod("Enumerate");

			if (enumerateMethod == null) return false;

			// Invoke the methods if they exist
			object[] arguments = {null, null};

			//Nothing?
			var success = (bool) enumerateMethod.Invoke(target, arguments);

			// After invocation, the array contains the out parameter values.
			enumNames = (string[]) arguments[0];
			enumValues = (Array) arguments[1];
			return success;
		}
		
		private void ListAvailableStates()
		{
			if (!ExtractEnumerated(out var enumNames, out var enumValues)) return;
			var defaultValue = ExtractDefault();
			var currentValue = ExtractCurrent();
			
			var targetType = target.GetType();
			var emitMethod = targetType.GetMethod("Emit");
			if (emitMethod == null) return;
			
			var color = GUI.color;
			GUILayout.BeginHorizontal();
			for (var i = 0; i < enumNames.Length; i++)
			{
				var enumName = enumNames[i];
				var value = enumValues.GetValue(i);
				if (value.Equals(currentValue))
				{
					GUI.color = Color.green;
					enumName += " (Current)";
				}
				else
				if (value.Equals(defaultValue))
				{
					GUI.color = Color.white;
					enumName += " (Default)"; 
				}
				
				if (GUILayout.Button(enumName))
				{
					// Invoke the Emit method with the enum value
					emitMethod.Invoke(target, new[] {value, this});
				}
				
				GUI.color = color;
				
				if (i % 2 != 1) continue;
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
			}

			GUILayout.EndHorizontal();
		}
	}
}