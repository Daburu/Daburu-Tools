using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Meshedit))]
public class MesheditEditor : Editor 
{
	// Serialised Properties
	private SerializedProperty mSPropList_mesheditStep = null;

	// Private Variables
	private ReorderableList mROList_mesheditStep = null;

	void OnEnable()
	{
		mSPropList_mesheditStep = serializedObject.FindProperty ("mList_mesheditStep");

		mROList_mesheditStep = new ReorderableList (serializedObject, mSPropList_mesheditStep, true, true, true, true);

		mROList_mesheditStep.drawHeaderCallback = (Rect _rectROListHeader) => 
		{
			EditorGUI.LabelField(_rectROListHeader, new GUIContent("Order of Actions", "The order of action to be executed in sequence"));
		};
		mROList_mesheditStep.drawElementCallback = (Rect _rectROList, int _nIndex, bool _bIsActive, bool _bIsFocused) => 
		{
			SerializedProperty sPropElement = mROList_mesheditStep.serializedProperty.GetArrayElementAtIndex(_nIndex);
			EditorGUI.LabelField(
				new Rect(_rectROList.x, _rectROList.y, _rectROList.width * 0.1f, EditorGUIUtility.singleLineHeight), 
				(_nIndex + 1) + ": ");
			_rectROList.x += _rectROList.width * 0.1f;
			EditorGUI.PropertyField(
				new Rect(_rectROList.x, _rectROList.y, _rectROList.width * 0.9f, EditorGUIUtility.singleLineHeight),
				sPropElement, 
				GUIContent.none);
		};
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();

		EditorGUILayout.Space ();
		mROList_mesheditStep.DoLayoutList ();

		serializedObject.ApplyModifiedProperties();
	}

	// Getter-Setter Functions
	private GameObject RefGameObject { get { return ((Meshedit)serializedObject.targetObject).gameObject; } }
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
