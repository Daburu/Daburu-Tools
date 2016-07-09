using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class LocalRotateByAction2D : Action
		{
			Transform mTransform;
			Graph mGraph;
			float mfDesiredTotalZEulerAngle;
			float mfActionDuration;

			float mfAccumulatedZEulerAngle;
			float mfElapsedDuration;

			public LocalRotateByAction2D(Transform _transform, Graph _graph, float _desiredZEulerAngle, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(_graph);
				SetDesiredZEulerAngle(_desiredZEulerAngle);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public LocalRotateByAction2D(Transform _transform, float _desiredZEulerAngle, float _actionDuration)
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
				mfDesiredTotalZEulerAngle = _newDesiredZEulerAngle;
			}
			public void SetActionDuration(float _newActionDuration)
			{
				mfActionDuration = _newActionDuration;
			}
			private void SetupAction()
			{
				mfAccumulatedZEulerAngle = 0f;
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

				Vector3 previousDeltaRot = new Vector3(
					0.0f,
					0.0f,
					mfAccumulatedZEulerAngle);
				mTransform.Rotate(-previousDeltaRot, Space.Self);	// Reverse the previous frame's rotation.

				float t = mGraph.Read(mfElapsedDuration / mfActionDuration);
				mfAccumulatedZEulerAngle = Mathf.LerpUnclamped(0.0f, mfDesiredTotalZEulerAngle, t);

				Vector3 newDeltaRot = new Vector3(
					0.0f,
					0.0f,
					mfAccumulatedZEulerAngle);
				mTransform.Rotate(newDeltaRot, Space.Self);	// Apply the new delta rotation.

				// Remove self after action is finished.
				if (mfElapsedDuration >= mfActionDuration)
				{
					Vector3 imperfection = Vector3.forward * (mfDesiredTotalZEulerAngle - mfAccumulatedZEulerAngle);
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
					Vector3 imperfection = Vector3.forward * (mfDesiredTotalZEulerAngle - mfAccumulatedZEulerAngle);
					mTransform.Rotate(imperfection, Space.Self);	// Force it to be the exact position that it wants.
				}

				OnActionEnd();
				mParent.Remove(this);
			}
		}
	}
}
