using UnityEngine;
using System.Collections;
using DaburuTools;

public class GraphSandbox : MonoBehaviour 
{
	Graph m_graph = Graph.Linear;
	Graph m_graphAdd;
	Graph m_graphSubtract;
	Graph m_graphMultiply;
	Graph m_graphDivide;

	// Private Functions
	void Start()
	{
		m_graphAdd = m_graph + 1f;
		m_graphSubtract = m_graph - 1f;
		m_graphMultiply = m_graph * 2f;
		m_graphDivide = m_graph / 2f;

		for (int i = 0; i < 10; i++)
		{
			float x = (float)i / 10f;

			Debug.Log("At " + i + ", Default: " + m_graph.Read(x) + ", Add: " + m_graphAdd.Read(x) + ", Subtract: " + m_graphSubtract.Read(x) + ", Multiply: " + m_graphMultiply.Read(x) + ", Divide: " + m_graphDivide.Read(x));
		}
	}
}
