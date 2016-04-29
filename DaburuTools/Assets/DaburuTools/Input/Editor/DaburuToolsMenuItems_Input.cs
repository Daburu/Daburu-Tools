using UnityEngine;
using UnityEditor;
using DaburuTools.Input;

public class DaburuToolsMenuItems_Input
{
	[MenuItem("DaburuTools/Input/Make Selected Camera Gyro Controlled", false, 1)]
	private static void DT_Input_MakeCameraGyroControlled()
	{
		GameObject cameraGameObject = Selection.activeGameObject;

		if (cameraGameObject.GetComponent<GyroControl>() != null)
		{
			Debug.LogWarning("Camera is already gyro controlled.");
			return;
		}

		Undo.AddComponent(cameraGameObject, typeof(GyroControl));
		Debug.Log("\"" +cameraGameObject.name + "\" is now gyro controlled.");
	}

	[MenuItem("DaburuTools/Input/Make Main Camera Gyro Controlled", false, 2)]
	private static void DT_Input_MakeMainCameraGyroControlled()
	{
		if (Camera.main == null)
		{
			Debug.LogWarning("No main camera found.");
			return;
		}

		if (Camera.main.gameObject.GetComponent<GyroControl>() != null)
		{
			Debug.LogWarning("Main camera is already gyro controlled");
			EditorGUIUtility.PingObject(Camera.main.gameObject);
			Selection.activeObject = Camera.main.gameObject;
			return;
		}

		EditorGUIUtility.PingObject(Camera.main.gameObject);
		Selection.activeObject = Camera.main.gameObject;
		Undo.AddComponent(Camera.main.gameObject, typeof(GyroControl));
		Debug.Log("Main Camera: \"" + Camera.main.gameObject.name + "\" is now gyro controlled.");
	}

	[MenuItem("DaburuTools/Input/Remove Gyro Controls from Selected Camera", false, 51)]
	private static void DT_Input_RemoveGyroControlFromCamera()
	{
		GameObject cameraGameObject = Selection.activeGameObject;
		GyroControl gyroCamScript = cameraGameObject.GetComponent<GyroControl>();

		if (gyroCamScript == null)
		{
			Debug.LogWarning("Camera is was not gyro controlled.");
			return;
		}

		Undo.DestroyObjectImmediate(cameraGameObject.GetComponent<GyroControl>());
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
