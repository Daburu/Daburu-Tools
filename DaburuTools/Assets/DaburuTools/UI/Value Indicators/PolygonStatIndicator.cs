using UnityEngine;
using System.Collections;

public class PolygonStatIndicator : MonoBehaviour
{
	static Material lineMaterial;
	[SerializeField] private int NumStats = 5;
	[Range(0.02f, 1.0f)]
	[SerializeField] private float[] StatPercentage;
	[Tooltip("The Radius of the Polygon in unity units (world space)")]
	[SerializeField] private float PolygonRadius = 0.5f;
	[SerializeField] private Vector2 CenterPoint = new Vector2(0.5f, 0.5f);
	[SerializeField] private Color BGCenterColor = new Color(0.0f, 0.0f, 0.0f);
	[SerializeField] private Color BGColor = new Color(0.2f, 0.2f, 0.2f);
	[SerializeField] private Color StatColor = new Color(0.9f, 0.9f, 0.9f);

	void Awake()
	{
		if (gameObject.GetComponent<Camera>() == null)
			Debug.LogError("PolygonStatIndicator not attached to GameObject with <Camera> component.\nWill not render on runtime.");

		if (StatPercentage.Length != NumStats)
			Debug.LogError("Number of StatPercentages does not match number of Stats");
	}

	void OnPostRender()
	{
		CreateLineMaterial();
		GL.PushMatrix();
		lineMaterial.SetPass(0);
		GL.LoadOrtho();

		GL.Begin(GL.TRIANGLES);

		for (int i = 0; i < NumStats; i++)
		{
			GL.Color(BGCenterColor);
			GL.Vertex3(CenterPoint.x, CenterPoint.y, 0.0f);

			GL.Color(BGColor);
			float angleRad, x, y;
			int index = i;

			angleRad = (2.0f * Mathf.PI) / NumStats * index + (Mathf.PI / 2.0f);
			x = CenterPoint.x + (Mathf.Cos(angleRad) * PolygonRadius * 100.0f / Screen.width);
			y = CenterPoint.y + (Mathf.Sin(angleRad) * PolygonRadius * 100.0f / Screen.height);
			GL.Vertex3(x, y, 0);

			index += 1;
			if (index == NumStats)
				index = 0;

			angleRad = (2.0f * Mathf.PI) / NumStats * index + (Mathf.PI / 2.0f);
			x = CenterPoint.x + (Mathf.Cos(angleRad) * PolygonRadius * 100.0f / Screen.width);
			y = CenterPoint.y + (Mathf.Sin(angleRad) * PolygonRadius * 100.0f / Screen.height);
			GL.Vertex3(x, y, 0);
		}

		for (int i = 0; i < NumStats; i++)
		{
			GL.Color(StatColor);
			GL.Vertex3(CenterPoint.x, CenterPoint.y, 0.0f);
			float angleRad, x, y;

			int index = i;

			angleRad = (2.0f * Mathf.PI) / NumStats * index + (Mathf.PI / 2.0f);
			x = CenterPoint.x + (Mathf.Cos(angleRad) * PolygonRadius * 100.0f / Screen.width * StatPercentage[index]);
			y = CenterPoint.y + (Mathf.Sin(angleRad) * PolygonRadius * 100.0f / Screen.height * StatPercentage[index]);
			GL.Vertex3(x, y, 0);

			index += 1;
			if (index == NumStats)
				index = 0;

			angleRad = (2.0f * Mathf.PI) / NumStats * index + (Mathf.PI / 2.0f);
			x = CenterPoint.x + (Mathf.Cos(angleRad) * PolygonRadius * 100.0f / Screen.width * StatPercentage[index]);
			y = CenterPoint.y + (Mathf.Sin(angleRad) * PolygonRadius * 100.0f / Screen.height * StatPercentage[index]);
			GL.Vertex3(x, y, 0);
		}

		GL.End();

		GL.PopMatrix();
	}

	static void CreateLineMaterial()
	{
		if (!lineMaterial)
		{
			// Unity has a built-in shader that is useful for drawing
			// simple colored things.
			var shader = Shader.Find ("Hidden/Internal-Colored");
			lineMaterial = new Material (shader);
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			// Turn on alpha blending
			lineMaterial.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
			lineMaterial.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
			// Turn backface culling off
			lineMaterial.SetInt ("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
			// Turn off depth writes
			lineMaterial.SetInt ("_ZWrite", 0);
		}
	}

	public bool UpdateStat(int _index, float _percentage)
	{
		if (_index >= NumStats)
		{
			Debug.LogWarning("FAILURE: UpdateStat. Index out of range");
			return false;
		}

		StatPercentage[_index] = Mathf.Clamp(_percentage, 0.02f, 1.0f);
		return true;
	}
}
