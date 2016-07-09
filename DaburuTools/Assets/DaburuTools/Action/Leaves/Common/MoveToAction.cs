using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class MoveToAction : Action
		{
			Transform mTransform;
			Graph mGraph;
			Vector3 mvecDesiredPos;
			float mfActionDuration;

			Vector3 mvecInitialPos;
			float mfElapsedDuration;

			public MoveToAction(Transform _transform, Graph _graph, Vector3 _desiredPosition, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(_graph);
				SetDesiredPosition(_desiredPosition);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public MoveToAction(Transform _transform, Vector3 _desiredPosition, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(Graph.Linear);
				SetDesiredPosition(_desiredPosition);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			public void SetDesiredPosition(Vector3 _newDesiredPosition)
			{
				mvecDesiredPos = _newDesiredPosition;
			}
			public void SetActionDuration(float _newActionDuration)
			{
				mfActionDuration = _newActionDuration;
			}
			private void SetupAction()
			{
				mvecInitialPos = mTransform.position;
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
				mTransform.position = Vector3.LerpUnclamped(mvecInitialPos, mvecDesiredPos, t);

				// Remove self after action is finished.
				if (mfElapsedDuration >= mfActionDuration)
				{
					mTransform.position = mvecDesiredPos;	// Force it to be the exact position that it wants.
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
					mTransform.position = mvecDesiredPos;	// Force it to be the exact position that it wants.
				}

				OnActionEnd();
				mParent.Remove(this);
			}
		}
	}
}
