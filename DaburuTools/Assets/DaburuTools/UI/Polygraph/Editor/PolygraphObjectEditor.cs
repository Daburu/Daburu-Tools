using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(PolygraphObject))]
public class PolygraphObjectEditor : Editor
{
	PolygraphObject myPolygraphObject;

	public override void OnInspectorGUI ()
	{
		myPolygraphObject = (PolygraphObject)target;

		Initialize ();

		if (GUILayout.Button ("Generate Graph")) {
			myPolygraphObject.Draw ();
		}

		if (GUI.GetNameOfFocusedControl ().Equals ("NumberOfAttributes")) {
			if (Event.current.type == EventType.keyUp && Event.current.keyCode == KeyCode.Return) {
				
			}
		}

		if (GUI.changed) {
			ValidateValues ();
		}
	}

	void OnValidate ()
	{
		myPolygraphObject = (PolygraphObject)target;
		ValidateValues ();
	}

	void Initialize ()
	{
		InitializeAttributesDataInput ();
		InitializeAnimationRelatedInput ();
		ValidateValues ();
	}


	// Instead of passing directly to the polygraph object, create a setter function get the polygraph object to validate values for me
	void InitializeAttributesDataInput ()
	{
		GUI.SetNextControlName ("NumberOfAttributes");
		myPolygraphObject.numberOfAttributes = 
			EditorGUILayout.IntField (new GUIContent ("Number of Attributes", "Number of attribute/stats the polygraph will show."), myPolygraphObject.numberOfAttributes);

		HandleArraySizeChanges ();

		for (int i = 0; i < myPolygraphObject.numberOfAttributes; i++) {
			myPolygraphObject.attributeLevelsArray [i] = 
				EditorGUILayout.Slider (new GUIContent ("Value " + i.ToString (), "Values of attributes."), myPolygraphObject.attributeLevelsArray [i], 0f, 1f);
		}

		myPolygraphObject.polygraphMaxRadius = 
			EditorGUILayout.FloatField (new GUIContent ("Polygraph Max Radius", "Distance between furthest node from centre."), myPolygraphObject.polygraphMaxRadius);
	}

	void HandleArraySizeChanges ()
	{
		if (myPolygraphObject.attributeLevelsArray == null) {
			myPolygraphObject.attributeLevelsArray = new float[myPolygraphObject.numberOfAttributes];
			for (int i = 0; i < myPolygraphObject.numberOfAttributes; i++)
				myPolygraphObject.attributeLevelsArray [i] = 1f;
		}

		if (myPolygraphObject.attributeLevelsArray.Length == myPolygraphObject.numberOfAttributes)
			return;
		else if (myPolygraphObject.attributeLevelsArray.Length < myPolygraphObject.numberOfAttributes) {
			float[] tempArray = myPolygraphObject.attributeLevelsArray;
			myPolygraphObject.attributeLevelsArray = new float[myPolygraphObject.numberOfAttributes];
			for (int i = 0; i < myPolygraphObject.numberOfAttributes; i++)
				myPolygraphObject.attributeLevelsArray [i] = 1f;
			for (int i = 0; i < tempArray.Length; i++) {
				myPolygraphObject.attributeLevelsArray [i] = tempArray [i];
			}
			tempArray = null;
		} else if (myPolygraphObject.attributeLevelsArray.Length > myPolygraphObject.numberOfAttributes) {
			float[] tempArray = myPolygraphObject.attributeLevelsArray;
			myPolygraphObject.attributeLevelsArray = new float[myPolygraphObject.numberOfAttributes];
			for (int i = 0; i < myPolygraphObject.numberOfAttributes; i++)
				myPolygraphObject.attributeLevelsArray [i] = 1f;
			for (int i = 0; i < myPolygraphObject.attributeLevelsArray.Length; i++) {
				myPolygraphObject.attributeLevelsArray [i] = tempArray [i];
			}
			tempArray = null;
		}
	}

	void ValidateValues ()
	{
		if (myPolygraphObject.numberOfAttributes < 3)
			myPolygraphObject.numberOfAttributes = 3;
		if (myPolygraphObject.animationSpeed < 0)
			myPolygraphObject.animationSpeed = 1;
	}

	void InitializeAnimationRelatedInput ()
	{
		myPolygraphObject.animateLevelChanges = 
			EditorGUILayout.Toggle (new GUIContent ("Animate the Level Changes", "Does the graph animate to show level changes?"), myPolygraphObject.animateLevelChanges);

		if (myPolygraphObject.animateLevelChanges) {
			myPolygraphObject.animationSpeed = 
				EditorGUILayout.FloatField (new GUIContent ("Animation Speed", "Speed at which the morphing animation plays."), myPolygraphObject.animationSpeed);
		}
	}
}
	
