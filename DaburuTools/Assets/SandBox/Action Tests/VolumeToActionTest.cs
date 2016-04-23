using UnityEngine;
using System.Collections;
using DaburuTools.Action;
using DaburuTools;

public class VolumeToActionTest : MonoBehaviour
{
	void Start()
	{
		volDown = new VolumeToAction(gameObject.GetComponent<AudioSource>(), 0.0f, 5.0f);
		volDown.SetGraph(Graph.Exponential);
		VolumeToAction volUp = new VolumeToAction(gameObject.GetComponent<AudioSource>(), 1.0f, 5.0f);
		volUp.SetGraph(Graph.InverseExponential);
		sequence = new ActionSequence(volDown, volUp);
		forever = new ActionRepeatForever(sequence);
		ActionHandler.RunAction(forever);
	}

	VolumeToAction volDown;
	ActionSequence sequence;
	ActionRepeatForever forever;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
//			volDown.StopActionRecursive(true);
//			sequence.StopActionRecursive(true);
			forever.StopActionRecursive(true);
	}
}
