using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using DaburuTools.Action;

public class Animate_Test : MonoBehaviour {

	ActionSequence mActionSequence;

	void Awake()
	{
		mActionSequence = new ActionSequence();
		mActionSequence.Add(new ScaleAction(this.transform, new Vector3(1.5f, 1.5f, 1.5f), 2.0f));
		mActionSequence.Add(new ScaleAction(this.transform, new Vector3(1.0f, 1.0f, 1.0f), 5.0f));
		mActionSequence.Add(new ScaleAction(this.transform, new Vector3(1.5f, 0.5f, 2.0f), 1.0f));
	}

	void Update()
	{
		mActionSequence.RunAction();
	}
}
