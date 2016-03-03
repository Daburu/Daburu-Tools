namespace DaburuTools
{
    // ParametricGraph(): A delegate to represent a method with a graph formula
    //                    Where all ParametricGraph() type will always have a parameter x
    //                    and a return float of f(x)
    public delegate float ParametricGraph(float _x);

    // Graph: A data structure to store graph equations
    public struct Graph
    {
        // Private Variables
            private ParametricGraph m_ParametricGraph;

        // Constructors
            /// <summary>
            /// Create a parametric graph instance
            /// </summary>
            /// <param name="_parametricGraph"> The delegate that contains a parametric graph equation </param>
            public Graph(ParametricGraph _parametricGraph)
            {
                m_ParametricGraph = _parametricGraph;
            }

        // Public Functions
            /// <summary>
            /// Read the f(x) value of the graph
            /// </summary>
            /// <param name="_x"> The x value of the graph </param>
            /// <returns> Returns the f(x) value of the graph </returns>
            public float Read(float _x)
            {
                return m_ParametricGraph(KeepInRange(_x));
            }

            /// <summary>
            /// Read the f(x) value of the graph.
            /// Note: This function does not clamp between 0.0f and 1.0f and that is out of the scope of the purpose of Graph.cs. Use at your own risk!
            /// </summary>
            /// <param name="_x"> The x value of the graph </param>
            /// <returns> Returns the f(x) value of the graph </returns>
            public float ReadUnclamped(float _x)
            {
                return m_ParametricGraph(_x);
            }

            /// <summary>
            /// Read the gradient of the graph at x
            /// </summary>
            /// <param name="_x"> The x value of the graph </param>
            /// <returns> Returns the gradient at x on the graph </returns>
            public float ReadGradient(float _x)
            {
                return m_ParametricGraph(_x) / _x;
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
                _x = KeepInRange(_x);
                _fBInfluence = KeepInRange(_fBInfluence);

                return _graphA.ReadUnclamped(_x) * (1f - _fBInfluence) + _fBInfluence * _graphB.ReadUnclamped(_x);
            }

            /// <summary>
            /// Creates a f(x) function of parametric graph-A with a certain percentage influenced from parametric graph-B and
            /// reads the f(x) value of the graph. This function does not clamp between 0.0f and 1.0f and that is out of the 
            /// scope of the purpose of Graph.cs. Use at your own risk!
            /// </summary>
            /// <param name="_graphA"> The initial graph A </param>
            /// <param name="_graphB"> The influencing graph B </param>
            /// <param name="_fBInfluence"> The percentage of influenced from the influencing graph B </param>
            /// <param name="_x"> The x value of the graph </param>
            /// <returns> Returns the f(x) value of the graph </returns>
            public static float MixUnclamped(Graph _graphA, Graph _graphB, float _fBInfluence, float _x)
            {
                _fBInfluence = KeepInRange(_fBInfluence);

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

        // Private Static Functions
            // KeepInRange(): Positivize all negatives x values. Mod all x that is more than 1
            private static float KeepInRange(float _x)
            {
                if (_x < 0f) _x = -_x;
                if (_x > 1f) _x %= 1f;
                return _x;
            }

        // (Private Static) Template Graphs Equations
            private static float LinearEquation(float _x) { return _x; }
            private static float InverseLinearEquation(float _x) { return 1f - _x; }
            private static float ExponentialEquation(float _x) { return _x * _x; }
            private static float InverseExponentialEquation(float _x) { return 1f - (ExponentialEquation(1f - _x)); }
            private static float SmoothStepEquation(float _x) { return _x * _x * (3f - 2f * _x); }

        // Static Initializers
            /// <summary> { f(x) = x } Creates a linear graph equation, this is used to represent the 'default' of all Graph.cs types </summary>
            public static Graph Linear { get { return new Graph(LinearEquation); } }
            /// <summary> { f(x) = 1 - x } Creates an inverse linear. The bigger the x, the smaller f(x) will be </summary>
            public static Graph InverseLinear { get { return new Graph(InverseLinearEquation); } }
            /// <summary> { f(x) = x^2 } Creates a exponential graph. Smooth start; Steep stop </summary>
            public static Graph Exponential { get { return new Graph(ExponentialEquation); } }
            /// <summary> { f(x) = 1-((1-x)^2) } Creates an inverse exponential graph. Steep Start; Smooth stop </summary>
            public static Graph InverseExponential { get { return new Graph(InverseExponentialEquation); } }
            /// <summary> { f(x) = 2x^3 - 2x^3 } Creates a slow start and end, with a steep body. Smooth Start; Smooth stop </summary>
            public static Graph SmoothStep { get { return new Graph(SmoothStepEquation); } }
    }
}
