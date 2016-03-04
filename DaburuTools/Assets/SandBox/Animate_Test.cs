using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;
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


		MoveByAction moveBy1 = new MoveByAction(this.transform, new Vector3(1.0f, 2.0f, 4.0f), 2.5f);
		MoveByAction moveBy2 = new MoveByAction(this.transform, new Vector3(-2.0f, -3.0f, -5.0f), 2.5f);

		ActionHandler.RunAction(moveBy1);
		ActionHandler.RunAction(moveBy2);
	}

	void Update()
	{
		
	}
}
