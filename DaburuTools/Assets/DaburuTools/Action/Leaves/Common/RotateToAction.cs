using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class RotateToAction : Action
		{
			Transform mTransform;
			Vector3 mvecInitialRotation;
			Vector3 mvecDesiredRotation;
			float mfActionDuration;
			float mfElaspedDuration;
			Graph mGraph;

			public RotateToAction(Transform _transform, Graph _graph, Vector3 _desiredRotation, float _actionDuration)
			{
				mTransform = _transform;
				mGraph = _graph;
				SetupAction();
				SetAction(_desiredRotation, _actionDuration);
			}
			public RotateToAction(Transform _transform, Vector3 _desiredRotation, float _actionDuration)
			{
				mTransform = _transform;
				mGraph = Graph.Linear;
				SetupAction();
				SetAction(_desiredRotation, _actionDuration);
			}
			public void SetAction(Vector3 _desiredRotationition, float _actionDuration)
			{
				mvecDesiredRotation = _desiredRotationition;
				mfActionDuration = _actionDuration;
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			private void SetupAction()
			{
				mvecInitialRotation = mTransform.eulerAngles;
				mfElaspedDuration = 0f;
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

				mfElaspedDuration += ActionDeltaTime(mbIsUnscaledDeltaTime);

				float t = mGraph.Read(mfElaspedDuration / mfActionDuration);
				mTransform.eulerAngles = Vector3.LerpUnclamped(mvecInitialRotation, mvecDesiredRotation, t);

				// Remove self after action is finished.
				if (mfElaspedDuration > mfActionDuration)
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
				mfElaspedDuration += mfActionDuration;

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
