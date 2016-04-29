using UnityEngine;
using System.Collections;

namespace DaburuTools
{
	namespace Input
	{
		public class GyroControl : MonoBehaviour 
		{
			// Editor
			public float mouseSensitivityX = 3.5f;
			public float mouseSensitivityY = 3.5f;
			public bool mbLockNHideCursor = false;
			private float cameraRotationX, cameraRotationY;

			private enum SnapTo { WorldAxis, InitialRotation };

			// Editable Variables
			[SerializeField] private Transform m_RotationPivot = null;
			[SerializeField] private bool bIsPivotRotating = false;

			[SerializeField] private SnapTo enum_snapTo = SnapTo.InitialRotation;

			// Un-Editable Variables
			private Gyroscope m_Gyroscope;				// m_Gyroscope: A reference to the gyroscope
			private Quaternion m_gyroscopeRotation;		// m_gyroscopeRotation: The proper axis-defined rotation of the gyroscope
			private Quaternion m_unityWorldRotation;	// m_unityWorldRotation: The current rotation that goes along with the world-axis

			private Vector3 m_vectorFromPivot;						// m_vectorFromPivot: The distance between the this object and the pivot
			private Quaternion m_initialRotationOnAwake;			// m_initialRotationOnAwake: The initial rotation of this object when this script is awake
			private Quaternion m_inverseInitialRotationOnAwake;		// m_inverseInitialRotationOnAwake: The inverse of the initial rotation of this object
			private float mf_snapToPointOffsetRotation = 0f;		// mf_snapToPointOffsetRotation: The 'center-to-screen' y-axis offset

			// Private Functions
			// Awake(): is called at the start of the program
			void Awake () 
			{
				// Gyroscope Initialisation
				m_Gyroscope = UnityEngine.Input.gyro;
				m_Gyroscope.enabled = true;

				// if: There is no rotation pivot assigned, the current gameObject will be assigned instead
				if (m_RotationPivot == null)
					m_RotationPivot = this.transform;
				
				// Initialisation
				m_vectorFromPivot = this.transform.position - m_RotationPivot.transform.position;
				m_initialRotationOnAwake = transform.rotation;
				m_inverseInitialRotationOnAwake = Quaternion.Inverse(m_initialRotationOnAwake);
				UpdateGyroscopeRotation();

				// This line fixed the x-axis problem
				transform.Rotate(Vector3.right, -m_initialRotationOnAwake.eulerAngles.x * 2f);

				// Editor
				cameraRotationX = transform.localEulerAngles.x;
				cameraRotationY = transform.localEulerAngles.y;
				if (mbLockNHideCursor)
				{
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;
				}
			}
			
			// Update(): is called every frame
			void Update()
			{
				UpdateGyroscopeRotation();

				// Editor - Mouse Emulation
#if UNITY_EDITOR
				cameraRotationX += UnityEngine.Input.GetAxis("Mouse Y") * mouseSensitivityY;
				cameraRotationY += UnityEngine.Input.GetAxis("Mouse X") * mouseSensitivityX;

				m_gyroscopeRotation = Quaternion.Euler(-cameraRotationX, cameraRotationY, 0.0f);
#endif
				// Converts gyroscope rotation to unity world rotation
				m_unityWorldRotation = Quaternion.Euler(0f, mf_snapToPointOffsetRotation, 0f) * m_gyroscopeRotation;
				transform.rotation = m_unityWorldRotation;

				// if: The current rotation pivot is not this object a.k.a Third Person Mode
				if (m_RotationPivot != this.transform)
				{
					if (bIsPivotRotating)
						m_RotationPivot.rotation = m_unityWorldRotation;

					transform.position = (m_unityWorldRotation * m_inverseInitialRotationOnAwake * m_vectorFromPivot) + m_RotationPivot.position;
				}
			}

			// UpdateGyroscopeRotation(): Recalculates the gyroscope rotation and updates it into m_gyroscopeRotation;
			private void UpdateGyroscopeRotation()
			{
				m_gyroscopeRotation = Quaternion.Euler(90f, 0f, 0f) * new Quaternion(m_Gyroscope.attitude.x, m_Gyroscope.attitude.y, -m_Gyroscope.attitude.z, -m_Gyroscope.attitude.w);
			}

			// Public Functions
			/// <summary>
			/// Sets the current rotation of the gyroscope to be the initial roation of the object
			/// </summary>
			public void SnapToPoint()
			{
				switch(enum_snapTo)
				{
					case SnapTo.InitialRotation:
						mf_snapToPointOffsetRotation = m_initialRotationOnAwake.eulerAngles.y - m_gyroscopeRotation.eulerAngles.y;
						break;
					case SnapTo.WorldAxis:
						mf_snapToPointOffsetRotation = -m_gyroscopeRotation.eulerAngles.y;
						break;
				}
			}

			// Getter-Setter Functions
			/// <summary>
			/// Returns the current rotation of gyrotation
			/// </summary>
			public Quaternion GyroscopeRotation { get { return m_gyroscopeRotation; } }

			/// <summary>
			/// Returns the rotation in relating to world axis
			/// </summary>
			public Quaternion WorldRotation { get { return m_unityWorldRotation; } }
		}
	}
}
