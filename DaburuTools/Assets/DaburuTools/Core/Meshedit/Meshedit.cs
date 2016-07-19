using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
// Meshedit.cs: Edit the mesh before going into play mode!
public class Meshedit : MonoBehaviour 
{
	// Serialised Variables
	public MesheditStep[] marr_mesheditStep = null;

	// Un-serialised Variables

	// Private Functions
	// Awake(): is called when the script is first initialised
	void Awake () 
	{
		
	}

	// Update(): is called once per frame
	void Update()
	{
		if (Application.isPlaying) 
		{
			Debug.Log (name + ".MeshEdit(Component): You left this script in play mode! This script is designed to be only executed in edit mode! Disabling itself!");
			this.enabled = false;
		}
	}
}

[System.Serializable]
// MesheditStep.cs: The data to show the steps to take 
public struct MesheditStep
{
	public enum EnumMesheditStep { Position, EulerRotation, Scale };

	public EnumMesheditStep m_enumMesheditStep;
	public float m_fVariableX;
	public float m_fVariableY;
	public float m_fVariableZ;

	public MesheditStep(EnumMesheditStep _enumMesheditStep, float _x, float _y, float _z)
	{
		m_enumMesheditStep = _enumMesheditStep;
		m_fVariableX = _x;
		m_fVariableY = _y;
		m_fVariableZ = _z;
	}
}