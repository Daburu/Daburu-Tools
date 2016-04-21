using UnityEngine;
using System.Collections;
using DaburuTools.Action;
using DaburuTools;

public class VolumeToActionTest : MonoBehaviour
{
	void Start()
	{
		VolumeToAction volDown = new VolumeToAction(gameObject.GetComponent<AudioSource>(), 0.0f, 5.0f);
		volDown.SetGraph(Graph.Exponential);
		VolumeToAction volUp = new VolumeToAction(gameObject.GetComponent<AudioSource>(), 1.0f, 5.0f);
		volUp.SetGraph(Graph.InverseExponential);
		ActionSequence sequence = new ActionSequence(volDown, volUp);
		ActionHandler.RunAction(new ActionRepeatForever(sequence));
	}
}
