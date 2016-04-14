using UnityEngine;
using System.Collections;
using DaburuTools.Action;
using DaburuTools;

public class AnimateTest_Child_2D : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		ActionSequence sequence = new ActionSequence();
//
//		LocalRotateToAction2D rotAct = new LocalRotateToAction2D(this.transform, -359.0f, 3.0f);
//		sequence.Add(rotAct);
//
//		LocalRotateToAction2D rotAct2 = new LocalRotateToAction2D(this.transform, 145.0f, 3.0f);
//		sequence.Add(rotAct2);
//
//		ActionHandler.RunAction(sequence);


//		LocalRotateByAction2D rotAct = new LocalRotateByAction2D(this.transform, -720.0f, 3.0f);
//		LocalRotateByAction2D rotAct2 = new LocalRotateByAction2D(this.transform, 180.0f, 3.0f);
//		ActionHandler.RunAction(rotAct);
//		ActionHandler.RunAction(rotAct2);

//		GraphLocalRotateToAction2D rotAct = new GraphLocalRotateToAction2D(this.transform, DaburuTools.Graph.Linear, -720.0f, 3.0f);
//		GraphLocalRotateToAction2D rotAct2 = new GraphLocalRotateToAction2D(this.transform, DaburuTools.Graph.SmoothStep, -720.0f, 3.0f);
//		ActionHandler.RunAction(rotAct);
//		ActionHandler.RunAction(new ActionAfterDelay(rotAct2, 3.0f));

//		GraphLocalRotateByAction2D rotAct = new GraphLocalRotateByAction2D(transform, DaburuTools.Graph.SmoothStep, -120.0f, 2.0f);
//		ActionHandler.RunAction(new ActionRepeatForever(rotAct));

		OrbitAction2D orbit2D = new OrbitAction2D(transform, transform.parent, false, 3, Graph.Linear, 5.0f, false);
		ActionHandler.RunAction(orbit2D);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
