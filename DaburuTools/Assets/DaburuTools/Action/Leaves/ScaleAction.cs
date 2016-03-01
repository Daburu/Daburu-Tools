using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class ScaleAction : Action
		{
			Transform mTransform;
			Vector3 mvecInitialScale;
			Vector3 mvecDesiredScale;
			float mfActionDuration;
			float mfElaspedDuration;

			public ScaleAction(Transform _transform)
			{
				mTransform = _transform;
				SetupScaleAction();
			}

			public ScaleAction(Transform _transform, Vector3 _desiredScale, float _actionDuration)
			{
				mTransform = _transform;
				SetupScaleAction();
				SetScaleAction(_desiredScale, _actionDuration);
			}

			public void SetScaleAction(Vector3 _desiredScale, float _actionDuration)
			{
				mvecDesiredScale = _desiredScale;
				mfActionDuration = _actionDuration;
			}

			private void SetupScaleAction()
			{
				mvecInitialScale = mTransform.localScale;
				mfElaspedDuration = 0f;
			}

			public override void OnActionBegin()
			{
				base.OnActionBegin();

				SetupScaleAction(); 
			}

			public override void RunAction()
			{
				base.RunAction();

				mfElaspedDuration += Time.deltaTime;

				float t = mfElaspedDuration / mfActionDuration;
				mTransform.localScale = Vector3.LerpUnclamped(mvecInitialScale, mvecDesiredScale, t);

				// Remove self after action is finished.
				if (mfElaspedDuration > mfActionDuration)
				{
					mTransform.localScale = mvecDesiredScale;	// Force it to be the exact scale that it wants.
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
