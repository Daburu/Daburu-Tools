using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class LocalMoveToAction : Action
		{
			Transform mTransform;
			Vector3 mvecInitialLocalPos;
			Vector3 mvecDesiredLocalPos;
			float mfActionDuration;
			float mfElaspedDuration;

			public LocalMoveToAction(Transform _transform)
			{
				mTransform = _transform;
				SetupLocalMoveToAction();
			}

			public LocalMoveToAction(Transform _transform, Vector3 _desiredLocalPosition, float _actionDuration)
			{
				mTransform = _transform;
				SetupLocalMoveToAction();
				SetLocalMoveToAction(_desiredLocalPosition, _actionDuration);
			}

			public void SetLocalMoveToAction(Vector3 _desiredLocalPosition, float _actionDuration)
			{
				mvecDesiredLocalPos = _desiredLocalPosition;
				mfActionDuration = _actionDuration;
			}

			private void SetupLocalMoveToAction()
			{
				mvecInitialLocalPos = mTransform.localPosition;
				mfElaspedDuration = 0f;
			}

			public override void OnActionBegin()
			{
				base.OnActionBegin();

				SetupLocalMoveToAction(); 
			}

			public override void RunAction()
			{
				base.RunAction();

				mfElaspedDuration += Time.deltaTime;

				float t = mfElaspedDuration / mfActionDuration;
				mTransform.localPosition = Vector3.LerpUnclamped(mvecInitialLocalPos, mvecDesiredLocalPos, t);

				// Remove self after action is finished.
				if (mfElaspedDuration > mfActionDuration)
				{
					mTransform.localPosition = mvecDesiredLocalPos;	// Force it to be the exact local position that it wants.
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
				// Forces OnActionBegin to run again.
				// Which runs SetupLocalMoveToAction. Effectiely resetting.
				mbIsRunning = false;
			}
		}
	}
}
