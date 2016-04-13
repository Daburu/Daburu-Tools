using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class LocalMoveByAction : Action
		{
			Transform mTransform;
			Vector3 mvecAccumulatedDelta;
			Vector3 mvecDesiredTotalDelta;
			Vector3 mvecDeltaPerSecond;
			float mfActionDuration;
			float mfElaspedDuration;

			public LocalMoveByAction(Transform _transform)
			{
				mTransform = _transform;
				SetupAction();
			}
			public LocalMoveByAction(Transform _transform, Vector3 _desiredDelta, float _actionDuration)
			{
				mTransform = _transform;
				SetupAction();
				SetAction(_desiredDelta, _actionDuration);
			}
			public void SetAction(Vector3 _desiredDelta, float _actionDuration)
			{
				mvecDesiredTotalDelta = _desiredDelta;
				mfActionDuration = _actionDuration;
				mvecDeltaPerSecond = _desiredDelta / mfActionDuration;	// Cache so don't need to calcualte every RunAction.
			}
			private void SetupAction()
			{
				mvecAccumulatedDelta = Vector3.zero;
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
				mTransform.localPosition += delta;
				mvecAccumulatedDelta += delta;

				// Remove self after action is finished.
				if (mfElaspedDuration > mfActionDuration)
				{
					Vector3 imperfection = mvecDesiredTotalDelta - mvecAccumulatedDelta;
					mTransform.localPosition += imperfection;	// Force to exact delta displacement.

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
