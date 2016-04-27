using UnityEngine;
using System.Collections;

public class Gyrotation : MonoBehaviour 
{
	// Editable Variables
	[Header("Pivot Properties")]
	[Tooltip("The pivot of gyroscope rotation. If nothing is assigned , the pivot will be the center of the current transform")]
	[SerializeField] private Transform m_RotationPivot = null;

	// Un-Editable Variables
	private Gyroscope mGyroscope;
	private Quaternion mCurrentRotation;

	private Vector3 mVectorFromPivot;
	private Vector3 mInitialPosition;
	private Quaternion mIdentityRotation = Quaternion.identity;

	// Private Functions
	// Awake(): is called at the start of the program
	void Awake () 
	{
		// Gyroscope Initialisation
		mGyroscope = UnityEngine.Input.gyro;
		mGyroscope.enabled = true;

		// if: There is no rotation pivot assigned, the current gameObject will be assigned instead
		if (m_RotationPivot == null)
			m_RotationPivot = this.transform;
		
		// Initialsation
		mVectorFromPivot = this.transform.position - m_RotationPivot.transform.position;
		mInitialPosition = this.transform.position;
	}
	
	// Update(): is called every frame
	void Update()
	{
		Quaternion finalRotation = new Quaternion(mGyroscope.attitude.x, mGyroscope.attitude.y, -mGyroscope.attitude.z, -mGyroscope.attitude.w);
		mCurrentRotation = mIdentityRotation * Quaternion.Euler(90f, 0f, 0f) * finalRotation;

		m_RotationPivot.position = mCurrentRotation * mVectorFromPivot + mInitialPosition;
		m_RotationPivot.rotation = mCurrentRotation;
	}

	// Public Functions
	/// <summary>
	/// Sets the current rotation of the gyroscope to be the default
	/// </summary>
	public void SetIdentity()
	{
		mIdentityRotation = mCurrentRotation;
	}

	// Getter-Setter Functions
	/// <summary>
	/// Returns the current rotation of gyrotation
	/// </summary>
	public Quaternion Rotation { get { return mCurrentRotation; } }
}
