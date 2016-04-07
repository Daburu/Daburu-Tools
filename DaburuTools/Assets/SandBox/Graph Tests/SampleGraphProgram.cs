using UnityEngine;
using DaburuTools;

public class SampleGraphProgram : MonoBehaviour 
{
	public Transform m_Node; // <- Please put the preFab named 'Node' into this variable in the inspector for graph to work!
	private Graph m_Graph = Graph.Linear;

	// This SampleGraphProgram will try to touch on all features of Graph.cs
	// and hope that you have a full understanding of how it all works!
	// The entire tutorial will be covered in the Start() function
	void Start () 
	{
		// How the tutorial works
			/* Every section of the tutorial will have a header, a wall of text - like this one, 
			 * and commented examples below this. The header to will give the general idea of
			 * what the current section is teaching and this wall of text will give in greater
			 * detail. To see how each of the section work, all you have to do is to uncomment
			 * the following (not-asterisk comment type) comments to see how the code actually
			 * works. Uncomment the following line to see how this tutorial works!
			 */

			//Debug.Log("Yay! I completed the tutorial!");

		// Definition of a Graph
			/* Similar to other structs, a Graph must be defined in order for it to work. So
			 * we can define a graph by the following line.
			 * Note: The graph is seen best in the viewport
			 */

			//m_Graph = Graph.Linear;

			/* The above shown is a static initializer. Of course, there are many other static
			 * initialisers, try some of the static initializer below!
			 */

			//m_Graph = Graph.InverseLinear;
			//m_Graph = Graph.Exponential;
			//m_Graph = Graph.InverseExponential;
			//m_Graph = Graph.SmoothStep;

		// Reading the graph
			/* What is the purpose of the graph when you can read the output? Simply call the
			 * the function Read() and pass in a x value to read the f(x) value of the equation!
			 * 
			 * However, the design of Graph.cs is that hopefully all x values goes between 0.0f 
			 * and 1.0f. So the graph equation handles by doing the following (in order!)
			 *  1. Positivize all negative x values
			 *  2. Remove the integers and leave only the decimal (floating) point of the value 
			 *     (other than 0 and 1!)
			 * 
			 * Of course, we also included the unclamped version of Read() - ReadUnClamped()!
			 * But reading x values outside of 0.0f to 1.0f range is unadvised (unless you've
			 * created your own equation wink* wink*) as things might not go as planned.
			 * 
			 * Try the examples below!
			 */

			//Debug.Log("Clamped x on an inverse-linear -> f(x) = " + Graph.InverseLinear.Read(0.3f));
			//Debug.Log("Clamped x on an linear -> f(x) = " + Graph.Linear.Read(-0.4f));
			//Debug.Log("Clamped x on an linear -> f(x) = " + Graph.Linear.Read(3.1f));
			//Debug.Log("Unclamped x on an inverse-linear -> f(x) = " + Graph.InverseLinear.ReadUnclamped(6.9f));

		// Creating your own graph
			/* Yes! You can create and define your own graph - with your own equation!
			 * To create your own graph, simply define the equation of the graph in a method -
			 * like the following example. Once the method is defined, simply pass the method
			 * name into the constructor of Graph.cs - simple!
			 * 
			 * However, there are some things to take note when creating your own graph!
			 * 1. The constructor of the graph takes in a ParametricGraph delegate
			 * 1b. To simply put a ParametricGraph is any method with one FLOAT parameter and a
			 *     FLOAT return type. Where the parameter will be x and the return will be f(x)
			 * 2. The method doesn't have to contain a formula. As long as the graph can return
			 *    a f(x) for all values of x
			 */
			
			//// ExampleCubic have an equation of f(x) = 9x^3 - 8x^2 + x + 0.5;
			//m_Graph = new Graph(ExampleCubic);

		// Stacking of graph
			/* You can also stack multiple graphs together to get more graphs! Try some of the 
			 * equations below!
			 */

			//// ExampleStack have an equation of f(g(h(x))) where f(x) = InverseLinear, g(x) =
			//// SmoothStep, h(x) = InverseExpoenential
			//m_Graph = new Graph(ExampleStack);

		PlotGraph(m_Graph);
	}

	// Note: ExampleCubic() have one float parameter and one float return type
	public float ExampleCubic(float _x)
	{
		return 9f * (_x * _x * _x) - 8f * (_x * _x) + _x + 0.5f;
	}

	public float ExampleStack(float _x)
	{
		return Graph.InverseLinear.Read(Graph.SmoothStep.Read(Graph.InverseExponential.Read(_x)));
	}

	// PlotGraph(): A simple method that helps to plot a graph
	void PlotGraph(Graph _graph)
	{
		for (int i = 0; i < 100; i++)
		{
			Instantiate(m_Node, new Vector3((float)i / 10f, _graph.Read((float)i / 100f) * 10f, 0f), Quaternion.identity);
		}
	}
}
