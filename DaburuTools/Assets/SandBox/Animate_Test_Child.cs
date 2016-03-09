using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;
using DaburuTools.Action;

public class Animate_Test_Child : MonoBehaviour {

	ActionSequence mActionSequence;

	void Awake()
	{
		
	}

	void Start()
	{
		/*	LocalMoveToAction Demonstration	*/
//		LocalMoveToAction[] localMoveToActions = {
//			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f)
//		};
//
//		ActionHandler.RunAction(new ActionSequence(localMoveToActions));

//		LocalRotateToAction localRotateToAction = new LocalRotateToAction(this.transform, new Vector3(180.0f, 0.0f, 15.0f), 10.0f);
//		ActionHandler.RunAction(localRotateToAction);


//		LocalMoveByAction localMB1 = new LocalMoveByAction(this.transform, new Vector3(0.0f, 3.0f, 2.0f), 4.0f);
//		LocalMoveByAction localMB2 = new LocalMoveByAction(this.transform, new Vector3(5.0f, 2.0f, -3.0f), 4.0f);
//		ActionHandler.RunAction(localMB1);
//		ActionHandler.RunAction(localMB2);

//		LocalRotateByAction localRB1 = new LocalRotateByAction(this.transform, new Vector3(0.0f, 1080.0f, 0.0f), 10.0f);
//		ActionHandler.RunAction(localRB1);

		LocalMoveToAction[] localMoveToActions = {
			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f)
		};
		ActionRepeat repeatedAction = new ActionRepeat(new ActionSequence(localMoveToActions), 2);
		ActionRepeat repeatingTheRepeated = new ActionRepeat(repeatedAction, 3);

		ActionHandler.RunAction(repeatedAction);
	}

	void Update()
	{
		
	}
}
