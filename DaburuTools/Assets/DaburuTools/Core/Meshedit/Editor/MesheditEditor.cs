using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Meshedit))]
// MesheditEditor.cs: The editor of Mesh-Edit
public class MesheditEditor : Editor 
{
	// Serialised Properties
	private SerializedProperty mSPropList_mesheditStep = null;
	private SerializedProperty mSProp_enumPivotType = null;

	private SerializedProperty mSProp_bIsDisplayOutput = null;

	// GUIContent Variables
	private GUIContent mGUICont_bIsDisplayOutput
		= new GUIContent ("Display Output", "Determine if it should display mesh output in the scene");
	private GUIContent mGUICont_enumPivotType
		= new GUIContent("Display At", "Determine the position at which the output model will display at");

	// Private Variables
	private ReorderableList mROList_mesheditStep = null;

	void OnEnable()
	{
		// Serialisation Initialisation
		mSPropList_mesheditStep = serializedObject.FindProperty ("mList_mesheditStep");
		mSProp_enumPivotType = serializedObject.FindProperty ("m_enumPivotType");

		mSProp_bIsDisplayOutput = serializedObject.FindProperty ("m_bIsDisplayOutput");

		// Re-orderable List Settings
		mROList_mesheditStep = new ReorderableList (serializedObject, mSPropList_mesheditStep, true, true, true, true);
		mROList_mesheditStep.drawHeaderCallback = (Rect _rectROListHeader) => 
		{
			EditorGUI.LabelField(_rectROListHeader, new GUIContent("Order of Transformation", "The order of transformation to be executed in sequence"));
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

		// Transformation Propeties
		EditorGUILayout.LabelField ("Transformation Properties", EditorStyles.boldLabel);
		mROList_mesheditStep.DoLayoutList ();
		EditorGUILayout.HelpBox ("Use the list above to transform the current mesh", MessageType.Info);
		EditorGUILayout.Space ();

		// Debugging Properties
		EditorGUILayout.LabelField ("Debugging Properties", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField (mSProp_bIsDisplayOutput, mGUICont_bIsDisplayOutput);
		if (mSProp_bIsDisplayOutput.boolValue) 
		{
			EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField (mSProp_enumPivotType, mGUICont_enumPivotType);
				EditorGUILayout.HelpBox((mSProp_enumPivotType.enumValueIndex == 0 ? 
					"The output model will be previewed with the gameObject's pivot" : "The output model will the world's origin as the pivot.") 
					+ "\n\nNOTE: This will not affect the final output at all!", MessageType.Info);
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.Space ();

		// Commission Properties
		EditorGUILayout.LabelField ("Commission Properties", EditorStyles.boldLabel);
		GUI.color = new Color (1f, 0.25f, 0.25f);
		if (GUILayout.Button ("Commit", GUILayout.MinHeight (40f)))
		{
			thisMeshedit.Commit ();
			return;
		}
		GUI.color = Color.white;
		EditorGUILayout.HelpBox ("Click the button above to commit the changes to the gameObject. Once done, this script will be deleted", MessageType.Info);

		serializedObject.ApplyModifiedProperties();
	}

	// Getter-Setter Functions
	private Meshedit thisMeshedit { get { return (Meshedit)serializedObject.targetObject; } }
}

[CustomPropertyDrawer(typeof(MesheditStep))]
// MesheditDrawer.cs: The property drawer for Mesh-Edit Steps
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
