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
		mAnimate.ExpandContract(2.0f, 10, 2.0f);
	}

	void Update()
	{
		if (mAnimate.IsExpandContract == false)
			mAnimate.ExpandContract(2.0f, 10, 2.0f);
	}
}
