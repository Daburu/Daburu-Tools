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
				SetupAction();
			}
			public LocalRotateToAction(Transform _transform, Vector3 _desiredLocalRotation, float _actionDuration)
			{
				mTransform = _transform;
				SetupAction();
				SetAction(_desiredLocalRotation, _actionDuration);
			}
			public void SetAction(Vector3 _desiredLocalRotation, float _actionDuration)
			{
				mvecDesiredLocalRotation = _desiredLocalRotation;
				mfActionDuration = _actionDuration;
			}
			private void SetupAction()
			{
				mvecInitialLocalRotation = mTransform.localEulerAngles;
				mfElaspedDuration = 0f;
			}
			protected override void OnActionBegin()
			{
				base.OnActionBegin();

				SetupAction(); 
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
