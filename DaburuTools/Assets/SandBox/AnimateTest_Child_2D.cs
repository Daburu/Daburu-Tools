using UnityEngine;
using System.Collections;
using DaburuTools.Action;

public class AnimateTest_Child_2D : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ActionSequence sequence = new ActionSequence();

		LocalRotateToAction2D rotAct = new LocalRotateToAction2D(this.transform, -359.0f, 3.0f);
		sequence.Add(rotAct);

		LocalRotateToAction2D rotAct2 = new LocalRotateToAction2D(this.transform, 145.0f, 3.0f);
		sequence.Add(rotAct2);

		ActionHandler.RunAction(sequence);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
