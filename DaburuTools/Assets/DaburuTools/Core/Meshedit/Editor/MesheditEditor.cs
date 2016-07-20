using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

[CustomEditor(typeof(Meshedit))]
// MesheditEditor.cs: The editor of Mesh-Edit
public class MesheditEditor : Editor 
{
	// Serialised Properties
	private SerializedProperty mSPropList_mesheditStep = null;
	private SerializedProperty mSProp_enumPivotType = null;

	private SerializedProperty mSProp_strMeshName = null;
	//private SerializedProperty mSProp_strMeshPath = null;

	private SerializedProperty mSProp_bIsDisplayOutput = null;
	private SerializedProperty mSProp_bIsDisplayPivot = null;
	private SerializedProperty mSProp_bIsExportMesh = null;

	// GUIContent Variables
	private GUIContent mGUICont_bIsDisplayOutput
		= new GUIContent ("Display Preview?", "Determine if it should display output model in the scene");
	private GUIContent mGUICont_enumPivotType
		= new GUIContent("Display at", "Determine the position at which the output model will display at");

	private GUIContent mGUICont_strMeshName 
		= new GUIContent ("Mesh Name", "The name of the mesh");
	private GUIContent mGUICont_strmeshPath
		= new GUIContent ("Export To", "The directory of the saved mesh");

	private GUIContent mGUICont_bIsDisplayPivot
		= new GUIContent ("Display Pivot?", "Determine if it should display mesh pivot in the scene");
	private GUIContent mGUICont_bIsExportMesh
		= new GUIContent ("Export Mesh?", "Determine if the mesh should be exported on transformation");

	// Private Variables
	private ReorderableList mROList_mesheditStep = null;

	void OnEnable()
	{
		// Serialisation Initialisation
		mSPropList_mesheditStep = serializedObject.FindProperty ("mList_mesheditStep");
		mSProp_enumPivotType = serializedObject.FindProperty ("m_enumPivotType");

		mSProp_strMeshName = serializedObject.FindProperty ("m_strMeshName");
		//mSProp_strMeshPath = serializedObject.FindProperty ("m_strMeshPath");

		mSProp_bIsDisplayOutput = serializedObject.FindProperty ("m_bIsDisplayOutput");
		mSProp_bIsDisplayPivot = serializedObject.FindProperty ("m_bIsDisplayPivot");
		mSProp_bIsExportMesh = serializedObject.FindProperty ("m_bIsExportMesh");

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
			thisMeshedit.CalculateOutputAndUpdate ();
		};
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();
		EditorGUILayout.Space ();

		// Transformation Propeties
		EditorGUILayout.LabelField ("Transformation Properties", EditorStyles.boldLabel);
		EditorGUILayout.HelpBox ("Use the list below to transform the current mesh", MessageType.Info);
			mROList_mesheditStep.DoLayoutList ();

		EditorGUILayout.Space ();

		// Debugging Properties
		EditorGUILayout.LabelField ("Debugging Properties", EditorStyles.boldLabel);
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.PropertyField (mSProp_bIsDisplayOutput, mGUICont_bIsDisplayOutput);
		// if: There is a change detected in the boolean field
		if (EditorGUI.EndChangeCheck ())
			thisMeshedit.CalculateOutputAndUpdate ();	
		if (mSProp_bIsDisplayOutput.boolValue) 
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.HelpBox((mSProp_enumPivotType.enumValueIndex == 0 ? 
				"The output model will be previewed with the gameObject's pivot" : "The output model will the world's origin as the pivot.") 
				+ "\n\nNOTE: This will not affect the final output at all!", MessageType.Info);
				EditorGUILayout.PropertyField (mSProp_enumPivotType, mGUICont_enumPivotType);
			EditorGUILayout.PropertyField (mSProp_bIsDisplayPivot, mGUICont_bIsDisplayPivot);
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.Space ();

		// Exporting Properties
		EditorGUILayout.LabelField ("Exporting Properties", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField (mSProp_bIsExportMesh, mGUICont_bIsExportMesh);
		if (mSProp_bIsExportMesh.boolValue)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField (mSProp_strMeshName, mGUICont_strMeshName);
			//EditorGUILayout.LabelField (mGUICont_strmeshPath);
			GUILayout.BeginHorizontal ();
			EditorGUI.indentLevel++;
			//EditorGUILayout.LabelField ("Assets/", GUILayout.MaxWidth (80f));
			//EditorGUILayout.PropertyField (mSProp_strMeshPath, GUIContent.none);
			EditorGUI.indentLevel--;
			//EditorGUILayout.LabelField ("\\" + (mSProp_strMeshName.stringValue.Length == 0 ? "Mesh" : mSProp_strMeshName.stringValue));
			GUILayout.EndHorizontal ();
			EditorGUILayout.HelpBox ("The exported mesh will be saved as " 
				+ (mSProp_strMeshName.stringValue.Length == 0 ? "meshedit_exportmesh" : mSProp_strMeshName.stringValue) + ".obj"
				+ "\n\nExported mesh will be saved in the Assets main folder", MessageType.Info);
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.Space ();

		// Commission Properties
		EditorGUILayout.LabelField ("Commission Properties", EditorStyles.boldLabel);
		EditorGUILayout.HelpBox ("Click the button below to transform the gameObject. Once done, this script will be deleted", MessageType.Info);
		GUI.color = new Color (1f, 0.6f, 0.25f);
		if (mSProp_bIsExportMesh.boolValue)
			if (GUILayout.Button ("Export Only"))
			{
				thisMeshedit.Export ();
			}
		if (GUILayout.Button (mSProp_bIsExportMesh.boolValue ? "Transform & Export" : "Transform"))
		{
			thisMeshedit.Export ();
			thisMeshedit.Commit ();
			return;
		}
		GUI.color = Color.white;

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
		_serialisedProperty.FindPropertyRelative("m_fVariableX").floatValue = 
			EditorGUI.FloatField (rectDrawer, new GUIContent("X"), _serialisedProperty.FindPropertyRelative("m_fVariableX").floatValue);
		rectDrawer.x += rectDrawer.width;
		_serialisedProperty.FindPropertyRelative("m_fVariableY").floatValue = 
			EditorGUI.FloatField (rectDrawer, new GUIContent("Y"), _serialisedProperty.FindPropertyRelative("m_fVariableY").floatValue);
		rectDrawer.x += rectDrawer.width;
		_serialisedProperty.FindPropertyRelative("m_fVariableZ").floatValue = 
			EditorGUI.FloatField (rectDrawer, new GUIContent("Z"), _serialisedProperty.FindPropertyRelative("m_fVariableZ").floatValue);
		
		EditorGUI.EndProperty();
	}
}
