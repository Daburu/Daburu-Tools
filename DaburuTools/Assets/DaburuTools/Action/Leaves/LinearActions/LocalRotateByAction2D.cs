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
			Vector3 mvecDeltaPerSecond;
			float mfActionDuration;
			float mfElaspedDuration;

			public LocalRotateByAction2D(Transform _transform)
			{
				mTransform = _transform;
				SetupAction();
			}
			public LocalRotateByAction2D(Transform _transform, float _desiredZEulerAngle, float _actionDuration)
			{
				mTransform = _transform;
				SetupAction();
				SetAction(_desiredZEulerAngle, _actionDuration);
			}
			public void SetAction(float _desiredZEulerAngle, float _actionDuration)
			{
				mfDesiredTotalZEulerAngle = _desiredZEulerAngle;
				mfActionDuration = _actionDuration;
				mvecDeltaPerSecond = Vector3.forward * _desiredZEulerAngle / mfActionDuration;	// Cache so don't need to calcualte every RunAction.
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
				mfElaspedDuration += Time.deltaTime;

				Vector3 delta = mvecDeltaPerSecond * Time.deltaTime;
				mTransform.Rotate(delta, Space.Self);
				mfAccumulatedZEulerAngle += delta.z;

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
		}
	}
}
