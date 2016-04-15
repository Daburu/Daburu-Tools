using UnityEngine;
using System.Collections;
using DaburuTools;
using DaburuTools.Action;

public class ShakeTest_2D : MonoBehaviour
{
	void Start()
	{
		Graph attenGraph = new Graph((float _x) =>
			{
				if (_x < 0.5f)
					return _x * 2.0f;
				else
					return _x * -2.0f + 2.0f;
			});
		ShakeAction2D shake = new ShakeAction2D(transform, 20, 1.0f, attenGraph);
		//		shake.SetShakeFrequency(50.0f);
		shake.SetShakeByDuration(10.0f, 1000);
		shake.SetShakeIntensity(1.5f);
		ActionHandler.RunAction(shake);
	}
	
	void Update()
	{
	
	}
}
