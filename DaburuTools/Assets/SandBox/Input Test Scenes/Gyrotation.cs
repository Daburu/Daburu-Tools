using UnityEngine;
using System.Collections;

public class Gyrotation : MonoBehaviour 
{
	// Editable Variables
	[Tooltip("The pivot of gyroscope rotation. If nothing is assigned , the pivot will be the center of the current transform")]
	[SerializeField] private Transform m_rotationPivot = this.transform;

	// Un-Editable Variables
	private Gyroscope m_gyroscope;
	private Vector3 m_vectorFromPivot;

	// Private Functions
	// Awake(): is called at the start of the program
	void Awake () 
	{
		// Gyroscope Initialisation
		m_gyroscope = UnityEngine.Input.gyro;
		m_gyroscope.enabled = true;

		// Initialsation
		m_vectorFromPivot = this.transform.position - m_rotationPivot.transform.position; 

		// if: There is no rotation pivot assigned, the current gameObject will be assigned instead
		if (m_rotationPivot == null)
			m_rotationPivot = this.transform;
	}
		
	void Update()
	{
		Quaternion finalRotation = new Quaternion(m_gyroscope.attitude.x, m_gyroscope.attitude.y, -m_gyroscope.attitude.z, -m_gyroscope.attitude.w);

		m_rotationPivot.transform.position = finalRotation * m_vectorFromPivot;
		m_rotationPivot.transform.rotation = finalRotation;
	}
}
