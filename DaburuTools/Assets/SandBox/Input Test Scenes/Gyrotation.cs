using UnityEngine;
using System.Collections;

public class Gyrotation : MonoBehaviour 
{
	// Editable Variables
	[Tooltip("The pivot of gyroscope rotation. If nothing is assigned , the pivot will be the center of the current transform")]
	[SerializeField] private Transform m_rotationPivot;

	// Un-Editable Variables
	private Gyroscope m_gyroscope;

	// Private Functions
	// Awake(): is called at the start of the program
	void Awake () 
	{
		m_gyroscope = UnityEngine.Input.gyro;
		m_gyroscope.enabled = true;

		// if: There is no rotation pivot assigned, the current gameObject will be assigned instead
		if (m_rotationPivot == null)
			m_rotationPivot = this.transform;
	}
		
	void Update()
	{
		Quaternion finalRotation = new Quaternion(m_gyroscope.attitude.x, m_gyroscope.attitude.y, -m_gyroscope.attitude.z, -m_gyroscope.attitude.w);

		m_RotationPivot = finalRotation
	}
}
