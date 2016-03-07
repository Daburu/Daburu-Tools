using UnityEngine;
using System.Collections;
using DaburuTools.Action;

public class AnimateTest_2D : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
//		ActionSequence sequence = new ActionSequence();
//
//		RotateToAction2D rotAct = new RotateToAction2D(this.transform, 360.0f, 3.0f);
//		sequence.Add(rotAct);
//
//		RotateToAction2D rotAct2 = new RotateToAction2D(this.transform, -45.0f, 3.0f);
//		sequence.Add(rotAct2);
//
//		ActionHandler.RunAction(sequence);


		RotateByAction2D rotAct = new RotateByAction2D(this.transform, 720.0f, 3.0f);
		RotateByAction2D rotAct2 = new RotateByAction2D(this.transform, -180.0f, 3.0f);
		ActionHandler.RunAction(rotAct);
		ActionHandler.RunAction(rotAct2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
