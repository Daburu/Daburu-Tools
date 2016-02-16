using UnityEngine;
using System.Collections;
using DaburuTools.Animate;

public class Animate_Test : MonoBehaviour {

	Animate mAnimate;

	void Awake()
	{
		mAnimate = new Animate(this.transform);
	}
		
	void Start()
	{
		mAnimate.ExpandContract(2.0f, 10, 2.0f, false, true, 0.0f);
		mAnimate.Idle(2.5f, 5.0f);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			mAnimate.StopIdle(true);
		}
	}
}
