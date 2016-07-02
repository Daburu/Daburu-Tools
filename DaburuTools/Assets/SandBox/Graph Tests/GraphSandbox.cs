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
		for (int i = 0; i < 10; i++)
		{
 			Debug.Log(Graph.Multiply(m_graph, m_graph2).Read((float)i / 10f) + " vs. " + Graph.MultiplyAsFloat(m_graph, m_graph2, (float)i / 10f));
		}
	}
}
