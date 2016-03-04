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

			public RotateToAction2D(Transform _transform)
			{
				mTransform = _transform;
				SetupRotateToAction();
			}

			public RotateToAction2D(Transform _transform, float _desiredZEulerAngle, float _actionDuration)
			{
				mTransform = _transform;
				SetupRotateToAction();
				SetRotateToAction(_desiredZEulerAngle, _actionDuration);
			}

			public void SetRotateToAction(float _desiredZEulerAngle, float _actionDuration)
			{
				mfDesiredZEulerAngle = _desiredZEulerAngle;
				mfActionDuration = _actionDuration;
			}

			private void SetupRotateToAction()
			{
				mfInitialZEulerAngle = mTransform.eulerAngles.z;
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
