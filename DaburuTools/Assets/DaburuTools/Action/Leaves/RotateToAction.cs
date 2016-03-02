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

			public RotateToAction(Transform _transform)
			{
				mTransform = _transform;
				SetupRotateToAction();
			}

			public RotateToAction(Transform _transform, Vector3 _desiredRotationition, float _actionDuration)
			{
				mTransform = _transform;
				SetupRotateToAction();
				SetRotateToAction(_desiredRotationition, _actionDuration);
			}

			public void SetRotateToAction(Vector3 _desiredRotationition, float _actionDuration)
			{
				mvecDesiredRotation = _desiredRotationition;
				mfActionDuration = _actionDuration;
			}

			private void SetupRotateToAction()
			{
				mvecInitialRotation = mTransform.eulerAngles;
				mfElaspedDuration = 0f;
			}

			public override void OnActionBegin()
			{
				base.OnActionBegin();

				SetupRotateToAction(); 
			}

			public override void RunAction()
			{
				base.RunAction();

				mfElaspedDuration += Time.deltaTime;

				float t = mfElaspedDuration / mfActionDuration;
				mTransform.eulerAngles = Vector3.LerpUnclamped(mvecInitialRotation, mvecDesiredRotation, t);

				// Remove self after action is finished.
				if (mfElaspedDuration > mfActionDuration)
				{
					mTransform.eulerAngles =  mvecDesiredRotation;	// Force it to be the exact rotation that it wants.
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
