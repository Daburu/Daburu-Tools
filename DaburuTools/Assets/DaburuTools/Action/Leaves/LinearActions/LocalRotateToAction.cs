using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class LocalRotateToAction : Action
		{
			Transform mTransform;
			Vector3 mvecInitialLocalRotation;
			Vector3 mvecDesiredLocalRotation;
			float mfActionDuration;
			float mfElaspedDuration;

			public LocalRotateToAction(Transform _transform)
			{
				mTransform = _transform;
				SetupLocalRotateToAction();
			}

			public LocalRotateToAction(Transform _transform, Vector3 _desiredLocalRotation, float _actionDuration)
			{
				mTransform = _transform;
				SetupLocalRotateToAction();
				SetLocalRotateToAction(_desiredLocalRotation, _actionDuration);
			}

			public void SetLocalRotateToAction(Vector3 _desiredLocalRotation, float _actionDuration)
			{
				mvecDesiredLocalRotation = _desiredLocalRotation;
				mfActionDuration = _actionDuration;
			}

			private void SetupLocalRotateToAction()
			{
				mvecInitialLocalRotation = mTransform.localEulerAngles;
				mfElaspedDuration = 0f;
			}

			public override void OnActionBegin()
			{
				base.OnActionBegin();

				SetupLocalRotateToAction(); 
			}

			public override void RunAction()
			{
				base.RunAction();

				mfElaspedDuration += Time.deltaTime;

				float t = mfElaspedDuration / mfActionDuration;
				mTransform.localEulerAngles = Vector3.LerpUnclamped(mvecInitialLocalRotation, mvecDesiredLocalRotation, t);

				// Remove self after action is finished.
				if (mfElaspedDuration > mfActionDuration)
				{
					mTransform.localEulerAngles =  mvecDesiredLocalRotation;	// Force it to be the exact local rotation that it wants.
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
