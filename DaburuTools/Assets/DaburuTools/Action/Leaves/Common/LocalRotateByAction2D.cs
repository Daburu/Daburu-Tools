using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class LocalRotateByAction2D : Action
		{
			Transform mTransform;
			float mfAccumulatedZEulerAngle;
			float mfDesiredTotalZEulerAngle;
			float mfActionDuration;
			float mfElaspedDuration;
			Graph mGraph;

			public LocalRotateByAction2D(Transform _transform, Graph _graph, float _desiredZEulerAngle, float _actionDuration)
			{
				mTransform = _transform;
				mGraph = _graph;
				SetupAction();
				SetAction(_desiredZEulerAngle, _actionDuration);
			}
			public LocalRotateByAction2D(Transform _transform, float _desiredZEulerAngle, float _actionDuration)
			{
				mTransform = _transform;
				mGraph = Graph.Linear;
				SetupAction();
				SetAction(_desiredZEulerAngle, _actionDuration);
			}
			public void SetAction(float _desiredZEulerAngle, float _actionDuration)
			{
				mfDesiredTotalZEulerAngle = _desiredZEulerAngle;
				mfActionDuration = _actionDuration;
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			private void SetupAction()
			{
				mfAccumulatedZEulerAngle = 0f;
				mfElaspedDuration = 0f;
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

				// It is less tricky to track the action by elasped time.
				// Otherwise, we need to check the sqrDist of both vec3s
				// for when we need to terminate the action.
				mfElaspedDuration += ActionDeltaTime(mbIsUnscaledDeltaTime);

				Vector3 previousDeltaRot = new Vector3(
					0.0f,
					0.0f,
					mfAccumulatedZEulerAngle);
				mTransform.Rotate(-previousDeltaRot, Space.Self);	// Reverse the previous frame's rotation.

				float t = mGraph.Read(mfElaspedDuration / mfActionDuration);
				mfAccumulatedZEulerAngle = Mathf.LerpUnclamped(0.0f, mfDesiredTotalZEulerAngle, t);

				Vector3 newDeltaRot = new Vector3(
					0.0f,
					0.0f,
					mfAccumulatedZEulerAngle);
				mTransform.Rotate(newDeltaRot, Space.Self);	// Apply the new delta rotation.

				// Remove self after action is finished.
				if (mfElaspedDuration > mfActionDuration)
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
				mfElaspedDuration += mfActionDuration;

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
