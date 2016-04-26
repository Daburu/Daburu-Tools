using UnityEngine;
using UnityEditor;
using DaburuTools.Input;

public class DaburuToolsMenuItems_Input
{
	[MenuItem("DaburuTools/Input/Make Selected Camera Gyro Controlled", false, 1)]
	private static void DT_Input_MakeCameraGyroControlled()
	{
		GameObject cameraGameObject = Selection.activeGameObject;

		if (cameraGameObject.GetComponent<GyroscopeCamera>() != null)
		{
			Debug.LogWarning("Camera is already gyro controlled.");
			return;
		}

		Undo.AddComponent(cameraGameObject, typeof(GyroscopeCamera));
		Debug.Log("\"" +cameraGameObject.name + "\" is now controlled by gyroscope.");
	}

	[MenuItem("DaburuTools/Input/Make Main Camera Gyro Controlled", false, 2)]
	private static void DT_Input_MakeMainCameraGyroControlled()
	{
		if (Camera.main == null)
		{
			Debug.LogWarning("No main camera found.");
			return;
		}

		if (Camera.main.gameObject.GetComponent<GyroscopeCamera>() != null)
		{
			Debug.LogWarning("Main camera is already gyro controlled");
			EditorGUIUtility.PingObject(Camera.main.gameObject);
			Selection.activeObject = Camera.main.gameObject;
			return;
		}

		Undo.AddComponent(Camera.main.gameObject, typeof(GyroscopeCamera));
		Debug.Log("Main Camera: \"" + Camera.main.gameObject.name + "\" is now controlled by gyroscope.");
	}

	[MenuItem("DaburuTools/Input/Remove Gyro Controls from Selected Camera", false, 51)]
	private static void DT_Input_RemoveGyroControlFromCamera()
	{
		GameObject cameraGameObject = Selection.activeGameObject;
		GyroscopeCamera gyroCamScript = cameraGameObject.GetComponent<GyroscopeCamera>();

		if (gyroCamScript == null)
		{
			Debug.LogWarning("Camera is already not controlled by gyroscope.");
			return;
		}

		if ((int)gyroCamScript.mCameraPerspective == 1)	// If Third Person
		{
			if (gyroCamScript.camPivot == null)
				Debug.LogWarning("CamPivot was not found. Could not complete cleanup. Did you move the third person cam pivot away?");
			else
			{
				Undo.SetTransformParent(gyroCamScript.transform, gyroCamScript.camPivot.transform.parent, "De-parent GyroCam from camPivot");
				Undo.DestroyObjectImmediate(gyroCamScript.camPivot);
			}
		}

		Undo.DestroyObjectImmediate(cameraGameObject.GetComponent<GyroscopeCamera>());
		Debug.Log("\"" + cameraGameObject.name + "\" has its gyroscope controls removed.");
	}

	#region Validation Functions
	[MenuItem("DaburuTools/Input/Make Selected Camera Gyro Controlled", true)]
	[MenuItem("DaburuTools/Input/Remove Gyro Controls from Selected Camera", true)]
	private static bool SelectedGameObjectIsCameraValidation()
	{
		if (Selection.activeGameObject == null)
			return false;

		// Returns true when the selected GameObject has a camera component.
		if (Selection.activeGameObject.GetComponent<Camera>() != null)
			return true;
		else
			return false;
	}
	#endregion
}
