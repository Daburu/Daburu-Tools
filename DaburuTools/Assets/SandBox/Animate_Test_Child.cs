using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using DaburuTools.Action;

public class Animate_Test_Child : MonoBehaviour {

	ActionSequence mActionSequence;

	void Awake()
	{
		
	}

	void Start()
	{
		LocalMoveToAction[] localMoveToActions = {
			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f)
		};

		ActionHandler.RunAction(new ActionSequence(localMoveToActions));
	}

	void Update()
	{
		
	}
}
