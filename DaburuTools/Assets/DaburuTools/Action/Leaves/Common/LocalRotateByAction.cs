using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class LocalRotateByAction : Action
		{
			Transform mTransform;
			Graph mGraph;
			Vector3 mvecDesiredTotalDelta;
			float mfActionDuration;

			Vector3 mvecAccumulatedDelta;
			float mfElapsedDuration;

			public LocalRotateByAction(Transform _transform, Graph _graph, Vector3 _desiredDelta, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(_graph);
				SetDesiredDelta(_desiredDelta);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public LocalRotateByAction(Transform _transform, Vector3 _desiredDelta, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(Graph.Linear);
				SetDesiredDelta(_desiredDelta);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			public void SetDesiredDelta(Vector3 _newDesiredDelta)
			{
				mvecDesiredTotalDelta = _newDesiredDelta;
			}
			public void SetActionDuration(float _newActionDuration)
			{
				mfActionDuration = _newActionDuration;
			}
			private void SetupAction()
			{
				mvecAccumulatedDelta = Vector3.zero;
				mfElapsedDuration = 0f;
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

				mTransform.Rotate(-mvecAccumulatedDelta, Space.Self);	// Reverse the previous frame's rotation.

				float t = mGraph.Read(mfElapsedDuration / mfActionDuration);
				mvecAccumulatedDelta = Vector3.LerpUnclamped(Vector3.zero, mvecDesiredTotalDelta, t);

				mTransform.Rotate(mvecAccumulatedDelta, Space.Self);	// Apply the new delta rotation.

				// Remove self after action is finished.
				if (mfElapsedDuration >= mfActionDuration)
				{
					Vector3 imperfection = mvecDesiredTotalDelta - mvecAccumulatedDelta;
					mTransform.Rotate(imperfection, Space.Self);	// Force to exact delta displacement.

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
					Vector3 imperfection = mvecDesiredTotalDelta - mvecAccumulatedDelta;
					mTransform.Rotate(imperfection, Space.Self);	// Force it to be the exact position that it wants.
				}

				OnActionEnd();
				mParent.Remove(this);
			}
		}
	}
}
