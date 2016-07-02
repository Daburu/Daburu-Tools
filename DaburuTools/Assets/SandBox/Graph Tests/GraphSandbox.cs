using UnityEngine;
using System.Collections;
using DaburuTools;

public class GraphSandbox : MonoBehaviour 
{
	Graph m_graph = Graph.Quadratic;
	Graph m_graph2 = Graph.InverseExponential;

	// Private Functions
	void Start()
	{
		//for (int i = 0; i < 10; i++)
		//{
		//	float x = (float)i / 10f;
		//	Debug.Log(m_graph.Read(x) + 3 + " vs. " + (m_graph + 3).Read(x));
		//}

		Graph graph = new Graph((float x) => { return 1f / x; });
		Debug.Log(graph.Read(0f));
	}
}
