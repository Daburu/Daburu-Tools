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
		ActionHandler.AddAction(new ScaleToAction(this.transform, new Vector3(1.5f, 1.5f, 1.5f), 2.0f));

		ScaleToAction scale_1 = new ScaleToAction(this.transform, new Vector3(1.5f, 1.5f, 1.5f), 2.0f);
		ScaleToAction scale_2 = new ScaleToAction(this.transform, new Vector3(1.0f, 1.0f, 1.0f), 1.0f);

		ActionSequence actionSequence = new ActionSequence();
		actionSequence.Add(scale_1);
		actionSequence.Add(scale_2);

		ActionHandler.AddAction(actionSequence);
	}

	void Update()
	{
//		mActionSequence.RunAction();
	}
}
