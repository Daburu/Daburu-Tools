using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;
using DaburuTools;
using DaburuTools.Action;

public class Animate_Test : MonoBehaviour {

	void Awake()
	{
		
	}

	void Start()
	{
		/*	RotateToAction Demonstration	*/
//		RotateToAction rotateToAction = new RotateToAction(this.transform, new Vector3(45.0f, 45.0f, 45.0f), 10.0f);
//		ActionHandler.RunAction(rotateToAction);

//		ScaleToAction scaleAct = new ScaleToAction(this.transform, new Vector3(2.0f, 0.5f, 7.0f), 4.0f);
//		RotateToAction rotateAct = new RotateToAction(this.transform, new Vector3(45.0f, -45.0f, 180.0f), 3.5f);
//		MoveToAction moveAct = new MoveToAction(this.transform, new Vector3(-2.0f, 3.0f, 1.0f), 4.2f);
//		Action[] actions = { scaleAct, rotateAct, moveAct };
//
//		ActionSequence sequence = new ActionSequence(actions);
//		ActionHandler.RunAction(sequence);



//		ActionSequence sequence = new ActionSequence();
//
//		MoveByAction moveBy1 = new MoveByAction(this.transform, new Vector3(1.0f, 2.0f, 0.0f), 2.5f);
//		sequence.Add(moveBy1);
//
//		MoveByAction moveBy2 = new MoveByAction(this.transform, new Vector3(2.0f, 1.0f, 0.0f), 2.5f);
//		sequence.Add(moveBy2);
//
//		ActionHandler.RunAction(sequence);


//		MoveByAction moveBy1 = new MoveByAction(this.transform, new Vector3(1.0f, 2.0f, 4.0f), 2.5f);
//		MoveByAction moveBy2 = new MoveByAction(this.transform, new Vector3(-2.0f, -3.0f, -5.0f), 2.5f);
//
//		ActionHandler.RunAction(moveBy1);
//		ActionHandler.RunAction(moveBy2);


//		ScaleByAction scaleBy1 = new ScaleByAction(this.transform, new Vector3(1.0f, 0.3f, 0.4f), 2.0f);
//		ScaleByAction scaleBy2 = new ScaleByAction(this.transform, new Vector3(1.2f, 0.4f, 0.3f), 5.0f);
//		ScaleByAction scaleBy3 = new ScaleByAction(this.transform, new Vector3(1.0f, 10.0f, 10.0f), 1.5f);
//		ActionHandler.RunAction(scaleBy1);
//		ActionHandler.RunAction(scaleBy2);
//		ActionHandler.RunAction(scaleBy3);


//		RotateByAction rotBy1 = new RotateByAction(this.transform, new Vector3(720.0f, 0.0f, 0.0f), 5.0f);
//		RotateByAction rotBy2 = new RotateByAction(this.transform, new Vector3(0.0f, 720.0f, 0.0f), 5.0f);
//		RotateByAction rotBy3 = new RotateByAction(this.transform, new Vector3(0.0f, 0.0f, 720.0f), 5.0f);
//		Action[] actions = {rotBy1, rotBy2, rotBy3};
//		ActionSequence sequence = new ActionSequence(actions);
//		ActionHandler.RunAction(sequence);

//		RotateByAction rotByZ = new RotateByAction(this.transform, new Vector3(0.0f, 0.0f, 180.0f), 5.0f);
//		ActionHandler.RunAction(rotByZ);

//		GraphScaleToAction graphScale1 = new GraphScaleToAction(transform, Graph.SmoothStep, new Vector3(3.0f, 3.0f, 3.0f), 3.0f);
//		GraphScaleToAction graphScale2 = new GraphScaleToAction(transform, Graph.InverseExponential, new Vector3(1.0f, 1.0f, 1.0f), 3.0f);
//		GraphMoveToAction graphMove1 = new GraphMoveToAction(transform, Graph.SmoothStep, new Vector3(-1.5f, 0.0f, 3.0f), 2.0f);
//		GraphMoveToAction graphMove2 = new GraphMoveToAction(transform, Graph.Bobber, new Vector3(0.0f, 0.0f, 0.0f), 3.0f);
//		GraphRotateToAction graphRotate = new GraphRotateToAction(transform, Graph.SmoothStep, new Vector3(50.0f, 90.0f, 100.0f), 3.0f);
//		DelayAction delay1 = new DelayAction(1.0f);
//		DelayAction delay2 = new DelayAction(1.0f);
////		ActionSequence sequence = new ActionSequence(graphScale1, delay1, graphScale2, delay2);
////		ActionSequence sequence = new ActionSequence(graphMove1, delay1, graphMove2, delay2);
//
////		ActionRepeatForever repeatedAciton = new ActionRepeatForever(sequence);
////		ActionAfterDelay delayedAction = new ActionAfterDelay(repeatedAciton, 1.0f);
////		ActionHandler.RunAction(delayedAction);
//
//		ActionHandler.RunAction(graphRotate);

//		GraphRotateByAction rotAct = new GraphRotateByAction(transform, Graph.SmoothStep, new Vector3(0.0f, 30.0f, 0.0f), 2.0f);
//		ActionHandler.RunAction(new ActionRepeatForever(rotAct));

//		GraphMoveByAction moveBy = new GraphMoveByAction(transform, Graph.SmoothStep, new Vector3(3.0f, 0.0f, 3.0f), 2.0f);
//		GraphMoveByAction moveBy2 = new GraphMoveByAction(transform, Graph.SmoothStep, new Vector3(0.0f, -3.0f, 0.0f), 4.0f);
//		ActionHandler.RunActions(moveBy, moveBy2);

//		GraphScaleByAction scaleAct = new GraphScaleByAction(transform, Graph.SmoothStep, new Vector3(1.5f, 1.5f, 1.5f), 4.0f);
//		ActionHandler.RunAction(new ActionRepeatForever(scaleAct));
//		GraphScaleByAction scaleAct = new GraphScaleByAction(transform, Graph.SmoothStep, new Vector3(2.0f, 2.0f, 2.0f), 2.0f);
//		ActionHandler.RunAction(new ActionRepeatForever(scaleAct));
//
//		GraphScaleByAction scaleAct2 = new GraphScaleByAction(transform, Graph.Exponential, new Vector3(0.5f, 1.0f, 1.0f), 2.0f);
//		ActionHandler.RunAction(new ActionRepeatForever(scaleAct2));

//		PulseAction pulseAct = new PulseAction(transform, 3, Graph.SmoothStep, 2.0f, transform.localScale, new Vector3(5.0f, 5.0f, 5.0f));
//		PulseAction pulseAct2 = new PulseAction(transform, 1, Graph.Dipper, Graph.Dipper, 2.0f, 2.0f, transform.localScale, new Vector3(5.0f, 5.0f, 5.0f));
//		ActionHandler.RunAction(new ActionRepeatForever(pulseAct2));
//		ActionHandler.RunAction(pulseAct);

//		IdleRotateAction idleRot = new IdleRotateAction(transform, 1, Graph.Linear, 1.0f, 1.0f, 200.0f, 360.0f);
//		ActionHandler.RunAction(new ActionRepeatForever(idleRot));

		MoveToAction unscaledTest = new MoveToAction(this.transform, Vector3.forward * 10.0f, 40.0f);
		unscaledTest.SetUnscaledDeltaTime(true);
//		unscaledTest.SetUnscaledDeltaTime(false);
		ActionHandler.RunAction(unscaledTest);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
//			Destroy(transform.GetChild(0).gameObject);
			if (Time.timeScale > 0.0f)
				Time.timeScale = 0.0f;
			else
				Time.timeScale = 1.0f;
		}
	}
}
