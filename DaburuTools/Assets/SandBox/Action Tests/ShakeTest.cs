using UnityEngine;
using System.Collections;
using DaburuTools.Action;
using DaburuTools;

public class ShakeTest : MonoBehaviour
{
	void Start()
	{
//		Graph attenGraph = new Graph((float _x) =>
//			{
//				if (_x < 0.5f)
//					return _x * 2.0f;
//				else
//					return _x * -2.0f + 2.0f;
//			});
		ShakeAction shake = new ShakeAction(transform, 100, 1.5f, Graph.InverseLinear);
		shake.SetShakeByDuration(2.0f, 100);
		ActionHandler.RunAction(shake);
	}

	float ElaspedDuration;
	Vector3 offset;
	float inensityValue;
	
	void Update()
	{
		
	}
}
