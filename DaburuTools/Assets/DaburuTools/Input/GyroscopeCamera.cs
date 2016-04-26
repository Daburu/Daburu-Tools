using UnityEngine;
using System.Collections;

namespace DaburuTools
{
	namespace Input
	{
		public class GyroscopeCamera : MonoBehaviour
		{
			// Perspective
			public enum CameraPerspective { FirstPerson, ThirdPerson };
			public CameraPerspective mCameraPerspective = CameraPerspective.FirstPerson;
			public float mfThirdPersonPivotOffset = 0.0f;

			// Editor
			public float mouseSensitivityX = 3.5f;
			public float mouseSensitivityY = 3.5f;
			public bool mbLockNHideCursor = false;

			// Mobile
			private GameObject camParent, camYRotator;
			public GameObject camPivot;
			private Gyroscope gyro;
			private float yAngleOffset = 0.0f;

			// Component/Variable Cache
			private Transform cameraTransform;
			private float cameraRotationX, cameraRotationY;

			private void Awake()
			{
				// Editor
				cameraTransform = Camera.main.transform;
				cameraRotationX = cameraTransform.localEulerAngles.x + 90.0f;
				cameraRotationY = cameraTransform.localEulerAngles.y;
				if (mbLockNHideCursor)
				{
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;
				}

				// Mobile
				gyro = UnityEngine.Input.gyro;
				gyro.enabled = true;

				// Common
				switch (mCameraPerspective)
				{
				case CameraPerspective.FirstPerson:
					camPivot = new GameObject("CamPivot");
					camPivot.transform.position = transform.position;
					camPivot.transform.parent = transform.parent;
					transform.parent = camPivot.transform;
					break;
				case CameraPerspective.ThirdPerson:
					if (camPivot == null)
						Debug.LogError("Missing CamPivot GameObject. Did you move it away? Try toggling the Camera Perspective field to re-create it at the right spot.");
					else if (camPivot != transform.parent.gameObject)
						Debug.LogError("Wrong CamPivot gameObject. Did you move it away? Try toggling the Camera Perspective field to re-create it at the right spot.");
					break;
				}

				camParent = new GameObject ("CamParent");
				camParent.transform.position = camPivot.transform.position;
				camParent.transform.parent = camPivot.transform.parent;
				camPivot.transform.parent = camParent.transform;

				// Rotate the parent object by 90 degrees around the x axis
				camParent.transform.Rotate(Vector3.right, 90);

				// Set up the parent object of camParent to handle y rotation.
				camYRotator = new GameObject("CamYRotator");
				camYRotator.transform.position = transform.position;
				camYRotator.transform.parent = camParent.transform.parent;
				camParent.transform.parent = camYRotator.transform;
			}

			private void Update ()
			{
				// Editor
				#if UNITY_EDITOR

				cameraRotationX += UnityEngine.Input.GetAxis("Mouse Y") * mouseSensitivityY;
				cameraRotationY += UnityEngine.Input.GetAxis("Mouse X") * mouseSensitivityX;

				switch (mCameraPerspective)
				{
				case CameraPerspective.FirstPerson:
					transform.localEulerAngles = new Vector3(-cameraRotationX, 0, 0);
					camYRotator.transform.localEulerAngles = new Vector3(0, cameraRotationY, 0);
					break;
				case CameraPerspective.ThirdPerson:
					camPivot.transform.localEulerAngles = new Vector3(-cameraRotationX, 0, 0);
					camYRotator.transform.localEulerAngles = new Vector3(0, cameraRotationY, 0);
					break;
				}

				//Mobile
				#else
				// Invert the z and w of the gyro attitude
				Quaternion rotFix = new Quaternion(gyro.attitude.x, gyro.attitude.y, -gyro.attitude.z, -gyro.attitude.w);

				// Now set the local rotation of the child camera object
				switch (mCameraPerspective)
				{
				case CameraPerspective.FirstPerson:
					transform.localRotation = rotFix;
				case CameraPerspective.ThirdPerson:
					camPivot.transform.localRotation = rotFix;
				}
				#endif
			}

			public void CenterCamera()
			{
				camYRotator.transform.Rotate(Vector3.up, -yAngleOffset);
				yAngleOffset = -transform.rotation.eulerAngles.y;
				camYRotator.transform.Rotate(Vector3.up, yAngleOffset);
			}
		}
	}
}
