﻿using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class RotateToAction : Action
		{
			Transform mTransform;
			Graph mGraph;
			Vector3 mvecDesiredRotation;
			float mfActionDuration;

			Vector3 mvecInitialRotation;
			float mfElapsedDuration;

			public RotateToAction(Transform _transform, Graph _graph, Vector3 _desiredRotation, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(_graph);
				SetDesiredRotation(_desiredRotation);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public RotateToAction(Transform _transform, Vector3 _desiredRotation, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(Graph.Linear);
				SetDesiredRotation(_desiredRotation);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			public void SetDesiredRotation(Vector3 _newDesiredRotation)
			{
				mvecDesiredRotation = _newDesiredRotation;
			}
			public void SetActionDuration(float _newActionDuration)
			{
				mfActionDuration = _newActionDuration;
			}
			private void SetupAction()
			{
				mvecInitialRotation = mTransform.eulerAngles;
				mfElapsedDuration = 0f;
			}
			protected override void OnActionBegin()
			{
				base.OnActionBegin();

				SetupAction(); 
			}



			public override void RunAction()
			{
				base.RunAction();

				if (mTransform == null)
				{
					// Debug.LogWarning("DaburuTools.Action: mTransform Deleted prematurely");
					mParent.Remove(this);
					return;
				}

				mfElapsedDuration += ActionDeltaTime(mbIsUnscaledDeltaTime);

				float t = mGraph.Read(mfElapsedDuration / mfActionDuration);
				mTransform.eulerAngles = Vector3.LerpUnclamped(mvecInitialRotation, mvecDesiredRotation, t);

				// Remove self after action is finished.
				if (mfElapsedDuration >= mfActionDuration)
				{
					mTransform.eulerAngles =  mvecDesiredRotation;	// Force it to be the exact rotation that it wants.
					OnActionEnd();
					mParent.Remove(this);
				}
			}
			public override void MakeResettable(bool _bIsResettable)
			{
				base.MakeResettable(_bIsResettable);
			}
			public override void Reset()
			{
				SetupAction();
			}
			public override void StopAction(bool _bSnapToDesired)
			{
				if (!mbIsRunning)
					return;

				// Prevent it from Resetting.
				MakeResettable(false);

				// Simulate the action has ended. Does not really matter by how much.
				mfElapsedDuration += mfActionDuration;

				if (_bSnapToDesired)
				{
					mTransform.eulerAngles =  mvecDesiredRotation;	// Force it to be the exact position that it wants.
				}

				OnActionEnd();
				mParent.Remove(this);
			}
		}
	}
}
