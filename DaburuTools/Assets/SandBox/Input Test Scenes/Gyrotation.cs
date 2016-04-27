using UnityEngine;
using System.Collections;

public class Gyrotation : MonoBehaviour 
{
	// Editable Variables
	[Tooltip("The pivot of gyroscope rotation. If nothing is assigned , the pivot will be the center of the current transform")]
	[SerializeField] private Transform m_rotationPivot = null;

	// Un-Editable Variables
	private Gyroscope m_gyroscope;
	private Vector3 m_initialPosition;

	private Quaternion m_currentOrientation;
	private Vector3 m_vectorFromPivot;

	// Private Functions
	// Awake(): is called at the start of the program
	void Awake () 
	{
		// Gyroscope Initialisation
		m_gyroscope = UnityEngine.Input.gyro;
		m_gyroscope.enabled = true;

		// if: There is no rotation pivot assigned, the current gameObject will be assigned instead
		if (m_rotationPivot == null)
			m_rotationPivot = this.transform;
		
		// Initialsation
		m_vectorFromPivot = this.transform.position - m_rotationPivot.transform.position;
		m_initialPosition = this.transform.position;
	}
		
	void Update()
	{
		Quaternion finalRotation = new Quaternion(m_gyroscope.attitude.x, m_gyroscope.attitude.y, -m_gyroscope.attitude.z, -m_gyroscope.attitude.w);
		m_currentOrientation = Quaternion.Euler(90f, 0f, 0f) * finalRotation;

		m_rotationPivot.position = m_currentOrientation * m_vectorFromPivot + m_initialPosition;
		m_rotationPivot.rotation = m_currentOrientation;
	}

	public Quaternion Orientation { get { return m_currentOrientation; } }
}
