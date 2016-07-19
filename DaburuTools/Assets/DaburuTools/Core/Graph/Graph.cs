﻿namespace DaburuTools
{
	// GraphCycle: Determines the pattern of the graph at every integer unit on the x-axis
	public enum GraphCycle { None, Constant, Repeat, Continuous }

	// ExplicitFunction(): A delegate to represent a method with a graph formula
	//                    Where all ExplicitFunction() type will always have a parameter x
	//                    and a return float of f(x)
	public delegate float ExplicitFunction(float _x);

	// Graph: A data structure to store graph equations
	public struct Graph
	{
		// Private Variables
			private GraphCycle enum_graphCycle;
			private ExplicitFunction m_delEquation;    // m_delEquation: The delegate to store the graph function

		// Constructors
			public Graph(ExplicitFunction _explicitFunction, GraphCycle _graphCycle)
			{
				m_delEquation = _explicitFunction;
				enum_graphCycle = _graphCycle;
			}
			
			/// <summary>
			/// Create a parametric graph instance
			/// </summary>
			/// <param name="_explicitFunction"> The delegate that contains a parametric graph equation </param>
			public Graph(ExplicitFunction _explicitFunction)
			{
				m_delEquation = _explicitFunction;
				enum_graphCycle = GraphCycle.None;
			}

		// Public Functions
			/// <summary>
			/// Read the f(x) value of the graph
			/// </summary>
			/// <param name="_x"> The x value of the graph </param>
			/// <returns> Returns the f(x) value of the graph </returns>
            public float Read(float _x)
            {
				switch (enum_graphCycle)
				{
 					case GraphCycle.None:
						return m_delEquation(_x);

					case GraphCycle.Repeat:
						if (_x > 1f)
							return m_delEquation(_x % 1f);
						else if (_x < 0f)
							return m_delEquation(1f + (_x % 1f));
						else
							return m_delEquation(_x);

					case GraphCycle.Constant:
						if (_x < 0f)
							return m_delEquation(0f);
						else if (_x > 1f)
							return m_delEquation(1f);
						else
							return m_delEquation(_x);

					case GraphCycle.Continuous:
						if (_x > 1f)
							return m_delEquation(1f) * (float)((int)_x) + m_delEquation(_x % 1f);
						else if (_x < 0f)
							return m_delEquation(1f) * ((float)((int)_x) - 1f) + m_delEquation(1f + (_x % 1f));
						else
							return m_delEquation(_x);

					default:
#if UNITY_EDITOR
						UnityEngine.Debug.LogWarning("Graph.cs: Read() function is unable to detect GraphCycle type");
#endif
						return 0f;
				}
            }

			/// <summary>
			/// Read the f(x) value of the graph without any checks or GraphCycle loops - Faster handling
			/// </summary>
			/// <param name="_x">The x value of the graph </param>
			/// <returns> Return the f(x) value of the graph </returns>
			public float ReadRaw(float _x)
			{
				return m_delEquation(_x);
			}

            /// <summary>
            /// Read the gradient of the graph at x
            /// </summary>
            /// <param name="_x"> The x value of the graph </param>
            /// <returns> Returns the gradient at x on the graph </returns>
            public float ReadGradient(float _x)
            {
                return m_delEquation(_x) / _x;
            }

        // Public Static Functions
			/// <summary>
			/// Flip the graph along the f(x)-direction
			/// </summary>
			/// <param name="_graph"> The graph to be flipped </param>
			/// <returns> Returns the flipped graph </returns>
			public static Graph VerticalFlip(Graph _graph)
			{
				return VerticalFlip(_graph, 0.5f);
			}
			
			/// <summary>
			/// Flip the graph along the f(x)-direction
			/// </summary>
			/// <param name="_graph"> The graph to be flipped </param>
			/// <param name="_fFlip"> The value of f(x) at which the graph will be flipped along </param>
			/// <returns> Returns the flipped graph </returns>
			public static Graph VerticalFlip(Graph _graph, float _fFlip)
			{
				return new Graph((float x) =>
				{
					return VerticalFlipAsFloat(_graph, _fFlip, x);
				}, _graph.enum_graphCycle);
			}

			/// <summary>
			/// Flips the graph along the f(x)-direction and returns the x value of the graph
			/// </summary>
			/// <param name="_graph"> The graph to be flipped </param>
			/// <param name="_fFlip"> The value of f(x) at which the graph will be flipped along </param>
			/// <returns> Returns the value of f(x) on the flipped graph </returns>
			public static float VerticalFlipAsFloat(Graph _graph, float _x)
			{
				return VerticalFlipAsFloat(_graph, 0.5f, _x);
			}
			
			/// <summary>
			/// Flips the graph along the f(x)-direction and returns the x value of the graph
			/// </summary>
			/// <param name="_graph"> The graph to be flipped </param>
			/// <param name="_fFlip"> The value of f(x) at which the graph will be flipped along </param>
			/// <param name="_x"> The x value of the graph </param>
			/// <returns> Returns the value of f(x) on the flipped graph </returns>
			public static float VerticalFlipAsFloat(Graph _graph, float _fFlip, float _x)
			{
				return _fFlip * 2f - _graph.Read(_x);
			}

			/// <summary>
			/// Flip the graph along the x-direction
			/// </summary>
			/// <param name="_graph"> The graph to be flipped </param>
			/// <returns> Returns the flipped graph </returns>
			public static Graph HorizontalFlip(Graph _graph)
			{
				return HorizontalFlip(_graph, 0.5f);
			}

			/// <summary>
			/// Flip the graph along the x-direction
			/// </summary>
			/// <param name="_graph"> The graph to be flipped </param>
			/// <param name="_fFlip"> The value of x at which the graph will be flipped along </param>
			/// <returns> Returns the flipped graph </returns>
			public static Graph HorizontalFlip(Graph _graph, float _fFlip)
			{
				return new Graph((float x) =>
				{
					return HorizontalFlipAsFloat(_graph, _fFlip, x);
				}, _graph.enum_graphCycle);
			}

			/// <summary>
			/// Flips the graph along the x-direction and returns the x value of the graph
			/// </summary>
			/// <param name="_graph"> The graph to be flipped </param>
			/// <param name="_fFlip"> The value of f(x) at which the graph will be flipped along </param>
			/// <returns> Returns the value of x on the flipped graph </returns>
			public static float HorizontalFlipAsFloat(Graph _graph, float _x)
			{
				return HorizontalFlipAsFloat(_graph, 0.5f, _x);
			}

			/// <summary>
			/// Flips the graph along the x-direction and returns the x value of the graph
			/// </summary>
			/// <param name="_graph"> The graph to be flipped </param>
			/// <param name="_fFlip"> The value of f(x) at which the graph will be flipped along </param>
			/// <param name="_x"> The x value of the graph </param>
			/// <returns> Returns the value of x on the flipped graph </returns>
			public static float HorizontalFlipAsFloat(Graph _graph, float _fFlip, float _x)
			{
				return _graph.Read(_fFlip * 2f - _x);
			}

			/// <summary>
			/// Creates a graph of graph-A with a certain percentage influenced from graph-B
			/// </summary>
			/// <param name="_graphA"> The initial graph A </param>
			/// <param name="_graphB"> The influencing graph B </param>
			/// <param name="_fBInfluence"> The percentage of influenced from the influencing graph B </param>
			/// <returns> Returns a graph that mixed the two graphs together </returns>
			public static Graph Mix(Graph _graphA, Graph _graphB, float _fBInfluence)
			{
				return Mix(_graphA, _graphB, _fBInfluence, GraphCycle.None);
			}

			/// <summary>
			/// Creates a graph of graph-A with a certain percentage influenced from graph-B
			/// </summary>
			/// <param name="_graphA"> The initial graph A </param>
			/// <param name="_graphB"> The influencing graph B </param>
			/// <param name="_fBInfluence"> The percentage of influenced from the influencing graph B </param>
			/// <param name="_enumGraphCycle"> The graph cycle type of the graph </param>
			/// <returns> Returns a graph that mixed the two graphs together </returns>
			public static Graph Mix(Graph _graphA, Graph _graphB, float _fBInfluence, GraphCycle _enumGraphCycle)
			{
				return new Graph((float x) =>
				{
					return MixAsFloat(_graphA, _graphB, _fBInfluence, x);
				}, _enumGraphCycle);
			}

            /// <summary>
            /// Returns the result at x of graph-A with a certain percentage influenced from graph-B
            /// </summary>
            /// <param name="_graphA"> The initial graph A </param>
            /// <param name="_graphB"> The influencing graph B </param>
            /// <param name="_fBInfluence"> The percentage of influenced from the influencing graph B </param>
            /// <param name="_x"> The x value of the graph </param>
            /// <returns> Returns the f(x) value of the graph </returns>
            public static float MixAsFloat(Graph _graphA, Graph _graphB, float _fBInfluence, float _x)
            {
                return _graphA.Read(_x) * (1f - _fBInfluence) + _fBInfluence * _graphB.Read(_x);
            }

			/// <summary>
			/// Creates a graph from the average of graph-A and graph-B
			/// </summary>
			/// <param name="_graphA"> The first graph </param>
			/// <param name="_graphB"> The second graph </param>
			/// <returns> Returns the average of both graph </returns>
			public static Graph Average(Graph _graphA, Graph _graphB)
			{
				return Average(_graphA, _graphB, GraphCycle.None);
			}

			/// <summary>
			/// Creates a graph from the average of graph-A and graph-B
			/// </summary>
			/// <param name="_graphA"> The first graph </param>
			/// <param name="_graphB"> The second graph </param>
			/// <param name="_enumGraphCycle"> The graph cycle type of the graph </param>
			/// <returns> Returns the average of both graph </returns>
			public static Graph Average(Graph _graphA, Graph _graphB, GraphCycle _enumGraphCycle)
			{
				return new Graph((float x) =>
				{
					return AverageAsFloat(_graphA, _graphB, x);
				}, _enumGraphCycle);
			}

            /// <summary>
			/// Returns the average of graph-A and graph-B at x
            /// </summary>
            /// <param name="_graphA"> The first graph </param>
            /// <param name="_graphB"> The second graph </param>
            /// <param name="_x"> The x value of the graph </param>
            /// <returns> Returns the average of f(x) and g(x) </returns>
            public static float AverageAsFloat(Graph _graphA, Graph _graphB, float _x)
            {
                return (_graphA.Read(_x) + _graphB.Read(_x)) / 2f;
            }

			/// <summary>
			/// Returns the product of graph-A and graph-B. Alternatively, the operator _graphA * _graphB could also be use to represent the same thing
			/// </summary>
			/// <param name="_graphA"> The first graph </param>
			/// <param name="_graphB"> The second graph </param>
			/// <returns> Return the product of both graphs </returns>
			public static Graph Multiply(Graph _graphA, Graph _graphB)
			{
				return Multiply(_graphA, _graphB, GraphCycle.None);
			}

			/// <summary>
			/// Returns the product of graph-A and graph-B. Alternatively, the operator _graphA * _graphB could also be use to represent the same thing
			/// </summary>
			/// <param name="_graphA"> The first graph </param>
			/// <param name="_graphB"> The second graph </param>
			///	<param name="_enumGraphCycle"> The graph cycle type of the graph </param>
			/// <returns> Return the product of both graphs </returns>
			public static Graph Multiply(Graph _graphA, Graph _graphB, GraphCycle _enumgraphCycle)
			{
				return new Graph((float x) =>
				{
					return MultiplyAsFloat(_graphA, _graphB, x);
				}, _enumgraphCycle);
			}

            /// <summary>
			/// Returns the product of graph-A and graph-B at x. the operator (_graphA * _graphB).Read() could also be use to represent the same thing;
            /// </summary>
            /// <param name="_graphA"> The first graph </param>
            /// <param name="_graphB"> The second graph </param>
            /// <param name="_x"> The x value of the graph </param>
            /// <returns> Return the product of both graphs </returns>
            public static float MultiplyAsFloat(Graph _graphA, Graph _graphB, float _x)
            {
                return _graphA.Read(_x) * _graphB.Read(_x);
            }

		// (Public Static) Graph Overloaded Operators
			/// <summary>
			/// Returns a new graph with an offset of the float along the f(x)-axis
			/// </summary>
			public static Graph operator +(Graph _graph, float _x) 
			{
				return new Graph((float x) =>
				{
					return _graph.Read(x) + _x;
				}, _graph.enum_graphCycle);
			}

			/// <summary>
			/// Returns a new graph with an offset of the float along the f(x)-axis
			/// </summary>
			public static Graph operator -(Graph _graph, float _x)
			{
 				return new Graph((float x) =>
				{
					return _graph.Read(x) - _x;
				}, _graph.enum_graphCycle);
			}
			
			/// <summary>
			/// Returns a new graph that is scaled by the floating point
			/// </summary>
			public static Graph operator *(Graph _graph, float _x)
			{
				return new Graph((float x) =>
				{
					return _graph.Read(x) * _x;
				}, _graph.enum_graphCycle);
			}
			
			/// <summary>
			/// Returns a new graph that is scaled by the floating point
			/// </summary>
			public static Graph operator /(Graph _graph, float _x)
			{
				return new Graph((float x) =>
				{
					return _graph.Read(x) / _x;
				}, _graph.enum_graphCycle);
			}

			/// <summary>
			/// Returns a new graph which is the addition of two graphs. Note: The new graph's graphCycle defaults back to GraphCycle.None;
			/// </summary>
			public static Graph operator +(Graph _graphA, Graph _graphB)
			{
 				return new Graph((float x) =>
				{
					return _graphA.Read(x) + _graphB.Read(x);
				}, GraphCycle.None);
			}
			
			/// <summary>
			/// Returns a new graph which is the substraction of two graphs. Note: The new graph's graphCycle defaults back to GraphCycle.None;
			/// </summary>
			public static Graph operator -(Graph _graphA, Graph _graphB)
			{
				return new Graph((float x) =>
				{
					return _graphA.Read(x) - _graphB.Read(x);
				}, GraphCycle.None);
			}
			
			/// <summary>
			/// Return a new graph that is scaled by the other graph. Alternatively, the operator Graph.Multiply(_graphA, _graphB) could also be use to represent the same thing
			/// </summary>
			public static Graph operator *(Graph _graphA, Graph _graphB)
			{
				return new Graph((float x) =>
				{
					return _graphA.Read(x) * _graphB.Read(x);
				}, GraphCycle.None);
			}

			/// <summary>
			/// Return a new graph that is scaled by the other graph
			/// </summary>
			public static Graph operator /(Graph _graphA, Graph _graphB)
			{
				return new Graph((float x) =>
				{
					return _graphA.Read(x) / _graphB.Read(x);
				}, GraphCycle.None);
			}

        // (Private Static) Template Graphs Equations
            private static float LinearEquation(float _x) { return _x; }
            private static float InverseLinearEquation(float _x) { return 1f - _x; }
            private static float ExponentialEquation(float _x) { return _x * _x; }
            private static float InverseExponentialEquation(float _x) { return 1f - (ExponentialEquation(1f - _x)); }
            private static float SmoothStepEquation(float _x) { return _x * _x * (3f - 2f * _x); }
            private static float QuadraticEquation(float _x) { return 4f * _x * (_x - 1f) + 1f; }
            private static float InverseQuadraticEquation(float _x) { return 4f * _x * (1f - _x); }
            private static float DipperEquation(float _x) { return (3f * _x * _x * _x - _x) / 2f; }
            private static float BobberEquation(float _x) { return 1f - DipperEquation(1f - _x); }
			private static float OneEquation(float _x) { return 1f; }
			private static float ZeroEquation(float _x) { return 0f; }

        // Static Initializers
            /// <summary> { f(x) = x } Creates a linear graph equation, this is used to represent the 'default' of all Graph.cs types </summary>
            public static Graph Linear { get { return new Graph(LinearEquation); } }
            /// <summary> { f(x) = 1 - x } Creates an inverse linear. The bigger the x, the smaller f(x) will be </summary>
            public static Graph InverseLinear { get { return new Graph(InverseLinearEquation); } }
            /// <summary> { f(x) = x^2 } Creates a exponential graph. Smooth start; Steep stop </summary>
            public static Graph Exponential { get { return new Graph(ExponentialEquation); } }
            /// <summary> { f(x) = 1 - ((1 - x)^2) } Creates an inverse exponential graph. Steep Start; Smooth stop </summary>
            public static Graph InverseExponential { get { return new Graph(InverseExponentialEquation); } }
            /// <summary> { f(x) = 2x^3 - 2x^3 } Creates a slow start and end, with a steep body. Smooth Start; Smooth stop </summary>
            public static Graph SmoothStep { get { return new Graph(SmoothStepEquation); } }
            /// <summary> { f(x) = 4x^2 - 4x } Creates a happy curve (U-shaped) </summary>
            public static Graph Quadratic { get { return new Graph(QuadraticEquation); } }
            /// <summary> { f(x) = 4x - 4x^2 } Creates a sad curve (N-shaped) </summary>
            public static Graph InverseQuadratic { get { return new Graph(InverseQuadraticEquation); } }
            /// <summary> { f(x) = (3x^3 - x) / 2 } Creates a similar curve to exponential, except it dips into the negatives at the front </summary>
            public static Graph Dipper { get { return new Graph(DipperEquation); } }
            /// <summary> { f(x) = (1 - (3(1 - x)^3 + x - 2)) / 2 } Create a similar curve to inverse-exponential, excepts it bobs above the graph at the end </summary>
            public static Graph Bobber { get { return new Graph(BobberEquation); } }
			/// <summary> { f(x) = 1 } Creates a x = 1 graph equation. It will always return one </summary>
			public static Graph One { get { return new Graph(OneEquation); } }
			/// <summary> { f(x) = 0 } Creates a x = 0 graph equation. It will always return zero </summary>
			public static Graph Zero { get { return new Graph(ZeroEquation); } }

		// Getter-Setter Functions
			public GraphCycle Cycle { get { return enum_graphCycle; } set { enum_graphCycle = value; } }

		// Obselete Functions (Last Updated: Higher than v4.1)
			[System.Obsolete("Use Graph.Read() instead, which no longer clamp f(x) between 0f to 1f")]
			public float ReadUnclamped(float _x) { return 0f; }
    }
}
