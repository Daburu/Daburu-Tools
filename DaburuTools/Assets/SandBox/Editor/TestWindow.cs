using UnityEngine;
using System.Collections;
using UnityEditor;

public class TestWindow : EditorWindow
{
	Rect windowRect = new Rect (100 + 100, 100, 100, 100);
	Rect windowRect2 = new Rect (100, 100, 100, 100);


	[MenuItem ("Window/Test Window", false, 5000)]
	static void Init () {
		EditorWindow.GetWindow (typeof (TestWindow));
	}

	private void OnGUI()
	{
		Handles.BeginGUI();
		Handles.DrawBezier(windowRect.center, windowRect2.center, new Vector2(windowRect.xMax + 50f,windowRect.center.y), new Vector2(windowRect2.xMin - 50f,windowRect2.center.y),Color.red,null,5f);
		Handles.EndGUI();

		BeginWindows();
		windowRect = GUI.Window (0, windowRect, WindowFunction, "Box1");
		windowRect2 = GUI.Window (1, windowRect2, WindowFunction, "Box2");

		EndWindows();

	}
	void WindowFunction (int windowID) 
	{
		GUI.DragWindow();
	}
}
