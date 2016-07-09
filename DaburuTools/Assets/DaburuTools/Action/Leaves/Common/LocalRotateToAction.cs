using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class LocalRotateToAction : Action
		{
			Transform mTransform;
			Graph mGraph;
			Vector3 mvecDesiredLocalRotation;
			float mfActionDuration;

			Vector3 mvecInitialLocalRotation;
			float mfElapsedDuration;

			public LocalRotateToAction(Transform _transform, Graph _graph, Vector3 _desiredLocalRotation, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(_graph);
				SetDesiredLocalRotation(_desiredLocalRotation);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public LocalRotateToAction(Transform _transform, Vector3 _desiredLocalRotation, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(Graph.Linear);
				SetDesiredLocalRotation(_desiredLocalRotation);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			public void SetDesiredLocalRotation(Vector3 _newDesiredLocalRotation)
			{
				mvecDesiredLocalRotation = _newDesiredLocalRotation;
			}
			public void SetActionDuration(float _newActionDuration)
			{
				mfActionDuration = _newActionDuration;
			}
			private void SetupAction()
			{
				mvecInitialLocalRotation = mTransform.localEulerAngles;
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
				mTransform.localEulerAngles = Vector3.LerpUnclamped(mvecInitialLocalRotation, mvecDesiredLocalRotation, t);

				// Remove self after action is finished.
				if (mfElapsedDuration >= mfActionDuration)
				{
					mTransform.localEulerAngles =  mvecDesiredLocalRotation;	// Force it to be the exact local rotation that it wants.
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
					mTransform.localEulerAngles =  mvecDesiredLocalRotation;	// Force it to be the exact position that it wants.
				}

				OnActionEnd();
				mParent.Remove(this);
			}
		}
	}
}
