using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class RotateToAction2D : Action
		{
			Transform mTransform;
			Graph mGraph;
			float mfDesiredZEulerAngle;
			float mfActionDuration;

			float mfInitialZEulerAngle;
			float mfElapsedDuration;

			public RotateToAction2D(Transform _transform, Graph _graph, float _desiredZEulerAngle, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(_graph);
				SetDesiredZEulerAngle(_desiredZEulerAngle);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public RotateToAction2D(Transform _transform, float _desiredZEulerAngle, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(Graph.Linear);
				SetDesiredZEulerAngle(_desiredZEulerAngle);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			public void SetDesiredZEulerAngle(float _newDesiredZEulerAngle)
			{
				mfDesiredZEulerAngle = _newDesiredZEulerAngle;
			}
			public void SetActionDuration(float _newActionDuration)
			{
				mfActionDuration = _newActionDuration;
			}
			private void SetupAction()
			{
				mfInitialZEulerAngle = mTransform.eulerAngles.z;
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
				mTransform.eulerAngles = new Vector3(
					mTransform.eulerAngles.x,
					mTransform.eulerAngles.y,
					Mathf.LerpUnclamped(mfInitialZEulerAngle, mfDesiredZEulerAngle, t)
				);

				// Remove self after action is finished.
				if (mfElapsedDuration >= mfActionDuration)
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
				mfElapsedDuration += mfActionDuration;

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
