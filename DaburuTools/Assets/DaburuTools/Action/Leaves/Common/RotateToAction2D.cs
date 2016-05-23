using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class RotateToAction2D : Action
		{
			Transform mTransform;
			float mfInitialZEulerAngle;
			float mfDesiredZEulerAngle;
			float mfActionDuration;
			float mfElaspedDuration;
			Graph mGraph;

			public RotateToAction2D(Transform _transform, Graph _graph, float _desiredZEulerAngle, float _actionDuration)
			{
				mTransform = _transform;
				mGraph = _graph;
				SetupAction();
				SetAction(_desiredZEulerAngle, _actionDuration);
			}
			public RotateToAction2D(Transform _transform, float _desiredZEulerAngle, float _actionDuration)
			{
				mTransform = _transform;
				mGraph = Graph.Linear;
				SetupAction();
				SetAction(_desiredZEulerAngle, _actionDuration);
			}
			public void SetAction(float _desiredZEulerAngle, float _actionDuration)
			{
				mfDesiredZEulerAngle = _desiredZEulerAngle;
				mfActionDuration = _actionDuration;
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			private void SetupAction()
			{
				mfInitialZEulerAngle = mTransform.eulerAngles.z;
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
				mTransform.eulerAngles = new Vector3(
					mTransform.eulerAngles.x,
					mTransform.eulerAngles.y,
					Mathf.LerpUnclamped(mfInitialZEulerAngle, mfDesiredZEulerAngle, t)
				);

				// Remove self after action is finished.
				if (mfElaspedDuration > mfActionDuration)
				{
					mTransform.eulerAngles = new Vector3(
						mTransform.eulerAngles.x,
						mTransform.eulerAngles.y,
						mfDesiredZEulerAngle
					);	// Force it to be the exact rotation that it wants.
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
					mTransform.eulerAngles = new Vector3(
						mTransform.eulerAngles.x,
						mTransform.eulerAngles.y,
						mfDesiredZEulerAngle
					);	// Force it to be the exact position that it wants.
				}

				OnActionEnd();
				mParent.Remove(this);
			}
		}
	}
}
