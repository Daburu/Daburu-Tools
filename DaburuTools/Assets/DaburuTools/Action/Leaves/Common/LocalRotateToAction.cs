using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class LocalRotateToAction : Action
		{
			Transform mTransform;
			Vector3 mvecInitialLocalRotation;
			Vector3 mvecDesiredLocalRotation;
			float mfActionDuration;
			float mfElaspedDuration;
			Graph mGraph;

			public LocalRotateToAction(Transform _transform, Graph _graph, Vector3 _desiredLocalRotation, float _actionDuration)
			{
				mTransform = _transform;
				mGraph = _graph;
				SetupAction();
				SetAction(_desiredLocalRotation, _actionDuration);
			}
			public LocalRotateToAction(Transform _transform, Vector3 _desiredLocalRotation, float _actionDuration)
			{
				mTransform = _transform;
				mGraph = Graph.Linear;
				SetupAction();
				SetAction(_desiredLocalRotation, _actionDuration);
			}
			public void SetAction(Vector3 _desiredLocalRotation, float _actionDuration)
			{
				mvecDesiredLocalRotation = _desiredLocalRotation;
				mfActionDuration = _actionDuration;
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			private void SetupAction()
			{
				mvecInitialLocalRotation = mTransform.localEulerAngles;
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
				mTransform.localEulerAngles = Vector3.LerpUnclamped(mvecInitialLocalRotation, mvecDesiredLocalRotation, t);

				// Remove self after action is finished.
				if (mfElaspedDuration > mfActionDuration)
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
				mfElaspedDuration += mfActionDuration;

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
