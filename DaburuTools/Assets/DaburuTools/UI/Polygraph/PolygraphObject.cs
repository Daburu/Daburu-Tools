using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(Mesh))]
[RequireComponent (typeof(MeshRenderer))]
public class PolygraphObject : MonoBehaviour
{
	public int numberOfAttributes;
	public float[] attributeLevelsArray;
	public bool animateLevelChanges;
	public float animationSpeed;
	public float polygraphMaxRadius;

	private MeshFilter meshFilter;
	private Mesh mesh;
	private Vector3[] vertices;
	private int[] triangles;

	void GetReferencesToComponents ()
	{
		meshFilter = GetComponent<MeshFilter> ();
		mesh = meshFilter.mesh;
	}

	float GetAngleBetweenAttributeStruts ()
	{
		return 360f / numberOfAttributes;
	}

	/// <summary>
	/// Sets the value of the attribute of the specified index.
	/// </summary>
	public void SetAttributeValueAtIndex (float value, int index)
	{
		if (index < 0 || index >= attributeLevelsArray.Length) {
			Debug.LogError ("Can't set value: Index out of range.");
			return;
		}

		if (value < 0 || value > 1) {
			Debug.LogError ("Can't set value: Attribute value out of range.");
			return;
		}

		attributeLevelsArray [index] = value;
	}

	public void Draw ()
	{
		Debug.Log ("Draw");
	}

	void Start ()
	{
		GetReferencesToComponents ();
		GenerateVertices ();
		GenerateTriangles ();

//		string output = "";
//		for (int i = 0; i < triangles.Length; i++) {
//			output += triangles[i] + " ";
//		}
//		Debug.Log(output);
	}

	void Update ()
	{
		GenerateVertices ();
		GenerateTriangles ();
		SetUpMesh ();
	}

	void GenerateVertices ()
	{
		vertices = new Vector3[numberOfAttributes + 1];
		vertices [0] = Vector3.zero;
		for (int i = 1; i < vertices.Length; i++) {
			vertices [i] = Vector3.zero;
			vertices [i].y = attributeLevelsArray [i - 1] * polygraphMaxRadius;
			vertices [i] = RotateVertexByAngleAboutForward (vertices [i], (i - 1) * GetAngleBetweenAttributeStruts ());
			//vertices [i] = RotateToMainTransform (vertices [i]);
			//vertices [i] = ScaleToMainTransform (vertices [i]);
		}
	}

	void GenerateTriangles ()
	{
		triangles = new int[numberOfAttributes * 3];
		int numOfTriangles = 0;
		for (int i = 0; i < triangles.Length; i++) {
			if (i % 3 == 0) {
				triangles [i] = 0;
				triangles [i + 2] = (numOfTriangles == numberOfAttributes - 1) ? 1 : numOfTriangles + 2;
				triangles [i + 1] = numOfTriangles + 1;
				numOfTriangles++;
			}
		}
	}

	void SetUpMesh ()
	{
		mesh.Clear ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.Optimize ();
		mesh.RecalculateNormals ();
	}

	Vector3 RotateVertexByAngleAboutForward (Vector3 vertexToRotate, float angle)
	{
		return Quaternion.Euler (0, 0, angle) * vertexToRotate;
	}

	Vector3 RotateToMainTransform (Vector3 vertexToRotate)
	{
		return Quaternion.Euler (transform.rotation.eulerAngles) * vertexToRotate;
	}

	Vector3 ScaleToMainTransform (Vector3 vertexToRotate)
	{
		Vector3 result = new Vector3 (
			                 transform.lossyScale.x * vertexToRotate.x,
			                 transform.lossyScale.y * vertexToRotate.y,
			                 transform.lossyScale.z * vertexToRotate.z
		                 );

		return result;
	}

	//	void OnDrawGizmos ()
	//	{
	//		Gizmos.color = Color.green;
	//		GenerateVertices ();
	//		for (int i = 0; i < vertices.Length; i++) {
	//			Gizmos.DrawSphere (vertices [i], .1f);
	//		}
	//		GenerateTriangles ();
	//		for (int i = 0; i < triangles.Length; i++) {
	//			if (i % 3 == 0) {
	//				Gizmos.DrawLine (vertices [triangles [i]], vertices [triangles [i + 1]]);
	//				Gizmos.DrawLine (vertices [triangles [i + 1]], vertices [triangles [i + 2]]);
	//				Gizmos.DrawLine (vertices [triangles [i + 1]], vertices [triangles [i]]);
	//			}
	//		}
	//	}
}
