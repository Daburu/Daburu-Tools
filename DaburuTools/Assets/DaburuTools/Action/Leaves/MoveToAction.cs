using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class MoveToAction : Action
		{
			Transform mTransform;
			Vector3 mvecInitialPos;
			Vector3 mvecDesiredPos;
			float mfActionDuration;
			float mfElaspedDuration;

			public MoveToAction(Transform _transform)
			{
				mTransform = _transform;
				SetupMoveToAction();
			}

			public MoveToAction(Transform _transform, Vector3 _desiredPosition, float _actionDuration)
			{
				mTransform = _transform;
				SetupMoveToAction();
				SetMoveToAction(_desiredPosition, _actionDuration);
			}

			public void SetMoveToAction(Vector3 _desiredPosition, float _actionDuration)
			{
				mvecDesiredPos = _desiredPosition;
				mfActionDuration = _actionDuration;
			}

			private void SetupMoveToAction()
			{
				mvecInitialPos = mTransform.position;
				mfElaspedDuration = 0f;
			}

			public override void OnActionBegin()
			{
				base.OnActionBegin();

				SetupMoveToAction(); 
			}

			public override void RunAction()
			{
				base.RunAction();

				mfElaspedDuration += Time.deltaTime;

				float t = mfElaspedDuration / mfActionDuration;
				mTransform.position = Vector3.LerpUnclamped(mvecInitialPos, mvecDesiredPos, t);

				// Remove self after action is finished.
				if (mfElaspedDuration > mfActionDuration)
				{
					mTransform.position = mvecDesiredPos;	// Force it to be the exact scale that it wants.
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
