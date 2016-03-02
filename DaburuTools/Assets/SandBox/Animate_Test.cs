using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using DaburuTools.Action;

public class Animate_Test : MonoBehaviour {

	ActionSequence mActionSequence;

	void Awake()
	{
//		mActionSequence = new ActionSequence();
//		mActionSequence.Add(new ScaleToAction(this.transform, new Vector3(1.5f, 1.5f, 1.5f), 2.0f));
//		mActionSequence.Add(new ScaleToAction(this.transform, new Vector3(1.0f, 1.0f, 1.0f), 5.0f));
//		mActionSequence.Add(new ScaleToAction(this.transform, new Vector3(1.5f, 0.5f, 2.0f), 1.0f));
	}

	void Start()
	{
//		ScaleToAction[] scaleActions = {
//			new ScaleToAction(this.transform, new Vector3(1.5f, 1.5f, 1.5f), 2.0f),
//			new ScaleToAction(this.transform, new Vector3(1.0f, 1.0f, 1.0f), 1.0f)
//		};
//		MoveToAction[] moveActions = {
//			new MoveToAction(this.transform, new Vector3(5.0f, 5.0f, 5.0f), 2.0f),
//			new MoveToAction(this.transform, new Vector3(-5.0f, -5.0f, -5.0f), 10.0f)
//		};
//
//		ActionSequence scaleActionSequence = new ActionSequence(scaleActions);
//		ActionSequence moveActionSequence = new ActionSequence(moveActions);
//		ActionParallel actionParallel = new ActionParallel();
//		actionParallel.Add(scaleActionSequence);
//		actionParallel.Add(moveActionSequence);
//
//		ActionHandler.RunAction(actionParallel);

		RotateToAction rotateToAction = new RotateToAction(this.transform, new Vector3(45.0f, 45.0f, 45.0f), 10.0f);
		ActionHandler.RunAction(rotateToAction);
	}

	void Update()
	{
//		mActionSequence.RunAction();
	}
}
