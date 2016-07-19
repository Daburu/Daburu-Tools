using UnityEngine;
using UnityEditor;

public class MesheditEditor : Editor 
{
	// Serialised Properties

	void OnEnable()
	{
		
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();

		serializedObject.ApplyModifiedProperties();
	}
}

[CustomPropertyDrawer(typeof(MesheditStep))]
public class MesheditStepDrawer : PropertyDrawer
{
	public override void OnGUI (Rect _rectPosition, SerializedProperty _serialisedProperty, GUIContent _GUIContLabel)
	{
		_GUIContLabel = EditorGUI.BeginProperty(_rectPosition, _GUIContLabel, _serialisedProperty);
		Rect rectDrawer = EditorGUI.PrefixLabel (_rectPosition, GUIContent.none);
		float fInspectorLength = rectDrawer.width;

		rectDrawer.width *= 0.4f;
		EditorGUI.PropertyField(rectDrawer, _serialisedProperty.FindPropertyRelative("m_enumMesheditStep"), GUIContent.none);

		rectDrawer.x += rectDrawer.width;
		rectDrawer.width = fInspectorLength;
		rectDrawer.width *= 0.2f;
		EditorGUIUtility.labelWidth = 15f;
		EditorGUI.PropertyField(rectDrawer, _serialisedProperty.FindPropertyRelative("m_fVariableX"), new GUIContent("X"));
		rectDrawer.x += rectDrawer.width;
		EditorGUI.PropertyField(rectDrawer, _serialisedProperty.FindPropertyRelative("m_fVariableY"), new GUIContent("Y"));
		rectDrawer.x += rectDrawer.width;
		EditorGUI.PropertyField(rectDrawer, _serialisedProperty.FindPropertyRelative("m_fVariableZ"), new GUIContent("Z"));
		
		EditorGUI.EndProperty();
	}	
}
