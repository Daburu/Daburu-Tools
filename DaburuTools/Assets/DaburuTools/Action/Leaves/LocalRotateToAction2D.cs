using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class LocalRotateToAction2D : Action
		{
			Transform mTransform;
			float mfInitialLocalZEulerAngle;
			float mfDesiredLocalZEulerAngle;
			float mfActionDuration;
			float mfElaspedDuration;

			public LocalRotateToAction2D(Transform _transform)
			{
				mTransform = _transform;
				SetupRotateToAction();
			}

			public LocalRotateToAction2D(Transform _transform, float _desiredLocalZEulerAngle, float _actionDuration)
			{
				mTransform = _transform;
				SetupRotateToAction();
				SetRotateToAction(_desiredLocalZEulerAngle, _actionDuration);
			}

			public void SetRotateToAction(float _desiredLocalZEulerAngle, float _actionDuration)
			{
				mfDesiredLocalZEulerAngle = _desiredLocalZEulerAngle;
				mfActionDuration = _actionDuration;
			}

			private void SetupRotateToAction()
			{
				mfInitialLocalZEulerAngle = mTransform.localEulerAngles.z;
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
				mTransform.localEulerAngles = new Vector3(
					mTransform.localEulerAngles.x,
					mTransform.localEulerAngles.y,
					Mathf.LerpUnclamped(mfInitialLocalZEulerAngle, mfDesiredLocalZEulerAngle, t)
				);

				// Remove self after action is finished.
				if (mfElaspedDuration > mfActionDuration)
				{
					mTransform.localEulerAngles = new Vector3(
						mTransform.localEulerAngles.x,
						mTransform.localEulerAngles.y,
						mfDesiredLocalZEulerAngle
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
