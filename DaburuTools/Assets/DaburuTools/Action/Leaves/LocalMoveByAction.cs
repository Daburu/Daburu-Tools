using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class LocalMoveByAction : Action
		{
			Transform mTransform;
			Vector3 mvecAccumulatedLocalDelta;
			Vector3 mvecDesiredTotalLocalDelta;
			Vector3 mvecLocalDeltaPerSecond;
			float mfActionDuration;
			float mfElaspedDuration;

			public LocalMoveByAction(Transform _transform)
			{
				mTransform = _transform;
				SetupMoveToAction();
			}

			public LocalMoveByAction(Transform _transform, Vector3 _desiredLocalDelta, float _actionDuration)
			{
				mTransform = _transform;
				SetupMoveToAction();
				SetMoveToAction(_desiredLocalDelta, _actionDuration);
			}

			public void SetMoveToAction(Vector3 _desiredLocalDelta, float _actionDuration)
			{
				mvecDesiredTotalLocalDelta = _desiredLocalDelta;
				mfActionDuration = _actionDuration;
				mvecLocalDeltaPerSecond = _desiredLocalDelta / mfActionDuration;	// Cache so don't need to calcualte every RunAction.
			}

			private void SetupMoveToAction()
			{
				mvecAccumulatedLocalDelta = Vector3.zero;
				mfElaspedDuration = 0f;
			}

			public override void RunAction()
			{
				base.RunAction();

				// It is less tricky to track the action by elasped time.
				// Otherwise, we need to check the sqrDist of both vec3s
				// for when we need to terminate the action.
				mfElaspedDuration += Time.deltaTime;

				Vector3 delta = mvecLocalDeltaPerSecond * Time.deltaTime;
				mTransform.localPosition += delta;
				mvecAccumulatedLocalDelta += delta;

				// Remove self after action is finished.
				if (mfElaspedDuration > mfActionDuration)
				{
					Vector3 imperfection = mvecDesiredTotalLocalDelta - mvecAccumulatedLocalDelta;
					mTransform.localPosition += imperfection;	// Force to exact delta displacement.

					OnActionEnd();
					mParent.Remove(this);
				}
			}

			public override bool Add(Action _Action)
			{
				return false;
			}
			public override bool Remove(Action _Action)
			{
				return false;
			}
			public override LinkedListNode<Action> GetListHead() { return null; }
		}
	}
}
