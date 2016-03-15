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

//		LocalMoveToAction[] localMoveToActions = {
//			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f)
//		};
//		LocalMoveToAction[] localMoveToActions2 = {
//			new LocalMoveToAction(this.transform, new Vector3(1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, -1.5f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(-1.5f, 0.0f, 0.0f), 0.25f),
//			new LocalMoveToAction(this.transform, new Vector3(0.0f, 0.0f, 1.5f), 0.25f)
//		};
//
//		ActionRepeat repeatedAction = new ActionRepeat(new ActionSequence(localMoveToActions), 2);
//		ActionSequence sequenceOfRepeats = new ActionSequence();
//		sequenceOfRepeats.Add(repeatedAction);
//		ActionRepeat repeatedAction2 = new ActionRepeat(new ActionSequence(localMoveToActions2), 2);
//		sequenceOfRepeats.Add(repeatedAction2);
//		ActionRepeat repeatingTheRepeated = new ActionRepeat(sequenceOfRepeats, 2);
//
//		ActionHandler.RunAction(repeatingTheRepeated);

//		LocalMoveByAction[] localMoveByActions = {
//			new LocalMoveByAction(this.transform, new Vector3(1.5f, 1.5f, 1.5f), 0.25f),
//			new LocalMoveByAction(this.transform, new Vector3(-1.0f, -1.0f, -1.0f), 0.25f)
//		};
//
//		ActionRepeat repeatedAction = new ActionRepeat(new ActionSequence(localMoveByActions), 5);
//		ActionHandler.RunAction(repeatedAction);

		ScaleByAction[] scaleByActions = {
			new ScaleByAction(this.transform, new Vector3(1.0f, 3.0f, 1.0f), 0.5f),
			new ScaleByAction(this.transform, new Vector3(4.0f, 1.0f, 1.0f), 1.0f)
		};
		ActionParallel actionParallel = new ActionParallel(scaleByActions);
		actionParallel.OnActionFinish = () => { Debug.Log("Scaled Up"); };
		ActionSequence sequence = new ActionSequence(actionParallel, new DelayAction(1.0f));
		ActionRepeat repeatAction = new ActionRepeat(sequence, 3);

		ScaleByAction[] scaleByActions2 = {
			new ScaleByAction(this.transform, new Vector3(1.0f, 0.33f, 1.0f), 0.5f),
			new ScaleByAction(this.transform, new Vector3(0.25f, 1.0f, 1.0f), 1.0f)
		};
		ActionParallel actionParallel2 = new ActionParallel(scaleByActions2);
		ActionRepeat repeatAction2 = new ActionRepeat(actionParallel2, 3);

		Action[] actions = {repeatAction, new ActionAfterDelay(repeatAction2, 0.5f)};
		ActionSequence sequenceScaleUpDown = new ActionSequence(actions);
		sequenceScaleUpDown.OnActionFinish = () => { Debug.Log("Finished cycle of scale up and down"); };
//		sequenceScaleUpDown.Add(repeatAction);
//		sequenceScaleUpDown.Add(new ActionAfterDelay(repeatAction2, 2.5f));
//		sequenceScaleUpDown.Add(repeatAction, new ActionAfterDelay(repeatAction2, 2.5f));

//		ActionRepeat finalRepeat = new ActionRepeat(sequenceScaleUpDown, 2);
//		ActionHandler.RunAction(finalRepeat);
		ActionRepeatForever foreverAction = new ActionRepeatForever(sequenceScaleUpDown);
		ActionAfterDelay delayedAction = new ActionAfterDelay(foreverAction, 5.0f);
		foreverAction.OnActionStart += () => { Debug.Log("Delegate Test"); };
		foreverAction.OnActionStart += foo;
		ActionHandler.RunAction(delayedAction);
	}

	void foo()
	{
		Debug.Log("Hello");
	}

	void Update()
	{
		
	}
}
