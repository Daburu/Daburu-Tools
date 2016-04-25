namespace DaburuTools
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
			private ExplicitFunction m_ExplicitFunction;    // m_ExplicitFunction: The delegate to store the graph function

		// Constructors
			public Graph(ExplicitFunction _explicitFunction, GraphCycle _graphCycle)
			{
				m_ExplicitFunction = _explicitFunction;
				enum_graphCycle = _graphCycle;
			}
			
			/// <summary>
			/// Create a parametric graph instance
			/// </summary>
			/// <param name="_explicitFunction"> The delegate that contains a parametric graph equation </param>
			public Graph(ExplicitFunction _explicitFunction)
			{
				m_ExplicitFunction = _explicitFunction;
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
						return m_ExplicitFunction(_x);

					case GraphCycle.Repeat:
						if (_x > 1f)
							return m_ExplicitFunction(_x % 1f);
						else if (_x < 0f)
							return m_ExplicitFunction(1f + (_x % 1f));
						else
							return m_ExplicitFunction(_x);

					case GraphCycle.Constant:
						if (_x < 0f)
							return m_ExplicitFunction(0f);
						else if (_x > 1f)
							return m_ExplicitFunction(1f);
						else
							return m_ExplicitFunction(_x);

					case GraphCycle.Continuous:
						if (_x > 1f)
							return m_ExplicitFunction(1f) * (float)((int)_x) + m_ExplicitFunction(_x % 1f);
						else if (_x < 0f)
							return m_ExplicitFunction(1f) * ((float)((int)_x) - 1f) + m_ExplicitFunction(1f + (_x % 1f));
						else
							return m_ExplicitFunction(_x);

					default:
#if UNITY_EDITOR
						UnityEngine.Debug.LogWarning("Graph.cs: Read() function is unable to detect GraphCycle type");
#endif
						return 0f;
				}
            }

            /// <summary>
            /// Read the f(x) value of the graph.
            /// Note: This function does not clamp between 0.0f and 1.0f and that is out of the scope of the purpose of Graph.cs. Use at your own risk!
            /// </summary>
            /// <param name="_x"> The x value of the graph </param>
            /// <returns> Returns the f(x) value of the graph </returns>
            public float ReadUnclamped(float _x)
            {
                return m_ExplicitFunction(_x);
            }

            /// <summary>
            /// Read the gradient of the graph at x
            /// </summary>
            /// <param name="_x"> The x value of the graph </param>
            /// <returns> Returns the gradient at x on the graph </returns>
            public float ReadGradient(float _x)
            {
                return m_ExplicitFunction(_x) / _x;
            }

        // Public Static Functions
            /// <summary>
            /// Creates a f(x) function of parametric graph-A with a certain percentage influenced from parametric graph-B and
            /// reads the f(x) value of the graph
            /// </summary>
            /// <param name="_graphA"> The initial graph A </param>
            /// <param name="_graphB"> The influencing graph B </param>
            /// <param name="_fBInfluence"> The percentage of influenced from the influencing graph B </param>
            /// <param name="_x"> The x value of the graph </param>
            /// <returns> Returns the f(x) value of the graph </returns>
            public static float Mix(Graph _graphA, Graph _graphB, float _fBInfluence, float _x)
            {
				if (_fBInfluence < 0f)
					_fBInfluence = 0f;
				else if (_fBInfluence > 1f)
					_fBInfluence = 1f;

                return _graphA.ReadUnclamped(_x) * (1f - _fBInfluence) + _fBInfluence * _graphB.ReadUnclamped(_x);
            }

            /// <summary>
            /// Returns the result of the average of two graphs at x
            /// </summary>
            /// <param name="_graphA"> The first graph </param>
            /// <param name="_graphB"> The second graph </param>
            /// <param name="_x"> The x value of the graph </param>
            /// <returns> Returns the average of f(x) and g(x) </returns>
            public static float Average(Graph _graphA, Graph _graphB, float _x)
            {
                return (_graphA.ReadUnclamped(_x) + _graphB.ReadUnclamped(_x)) / 2f;
            }
            
            /// <summary>
            /// Return the product of two graphs. Despite this function does not keep within range of 0 to 1, if both f(x)
            /// and g(x) does not exceed 1, the product would stay within 1
            /// </summary>
            /// <param name="_graphA"> The first graph </param>
            /// <param name="_graphB"> The second graph </param>
            /// <param name="_x"> The x value of the graph </param>
            /// <returns> Return the product of both graphs </returns>
            public static float Multiply(Graph _graphA, Graph _graphB, float _x)
            {
                return _graphA.ReadUnclamped(_x) * _graphB.ReadUnclamped(_x);
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
    }
}
