using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using Braveo.QOL;

#if UNITY_EDITOR

namespace Braveo.QOL {

	[CustomPropertyDrawer(typeof(DualVar<,>))]
	public class DualVarPropertyDrawer : PropertyDrawer {

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var v1Property = property.FindPropertyRelative("v1");
			return EditorGUI.GetPropertyHeight(v1Property);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			var v1Property = property.FindPropertyRelative("v1");
			var v2Property = property.FindPropertyRelative("v2");

			position.width -= 125;
			EditorGUI.PropertyField(position, v1Property, label, true);

			position.x += position.width + 125;
			position.width = EditorGUI.GetPropertyHeight(v2Property) + 101;
			position.x -= position.width;
			EditorGUI.PropertyField(position, v2Property, GUIContent.none);
		}

	}

}

#endif