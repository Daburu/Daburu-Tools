using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class LocalMoveToAction : Action
		{
			Transform mTransform;
			Graph mGraph;
			Vector3 mvecDesiredLocalPos;
			float mfActionDuration;

			Vector3 mvecInitialLocalPos;
			float mfElapsedDuration;

			public LocalMoveToAction(Transform _transform, Graph _graph, Vector3 _desiredLocalPosition, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(_graph);
				SetDesiredLocalPosition(_desiredLocalPosition);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public LocalMoveToAction(Transform _transform, Vector3 _desiredLocalPosition, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(Graph.Linear);
				SetDesiredLocalPosition(_desiredLocalPosition);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			public void SetDesiredLocalPosition(Vector3 _newDesiredLocalPosition)
			{
				mvecDesiredLocalPos = _newDesiredLocalPosition;
			}
			public void SetActionDuration(float _newActionDuration)
			{
				mfActionDuration = _newActionDuration;
			}
			private void SetupAction()
			{
				mvecInitialLocalPos = mTransform.localPosition;
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
				mTransform.localPosition = Vector3.LerpUnclamped(mvecInitialLocalPos, mvecDesiredLocalPos, t);

				// Remove self after action is finished.
				if (mfElapsedDuration >= mfActionDuration)
				{
					mTransform.localPosition = mvecDesiredLocalPos;	// Force it to be the exact local position that it wants.
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
					mTransform.localPosition = mvecDesiredLocalPos;	// Force it to be the exact position that it wants.
				}

				OnActionEnd();
				mParent.Remove(this);
			}
		}
	}
}
