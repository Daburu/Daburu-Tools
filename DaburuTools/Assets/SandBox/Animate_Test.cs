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

		ScaleToAction scaleAct = new ScaleToAction(this.transform, new Vector3(2.0f, 0.5f, 7.0f), 4.0f);
		RotateToAction rotateAct = new RotateToAction(this.transform, new Vector3(45.0f, -45.0f, 180.0f), 3.5f);
		MoveToAction moveAct = new MoveToAction(this.transform, new Vector3(-2.0f, 3.0f, 1.0f), 4.2f);
		Action[] actions = { scaleAct, rotateAct, moveAct };

		ActionSequence sequence = new ActionSequence(actions);
		ActionHandler.RunAction(sequence);
	}

	void Update()
	{
		
	}
}
