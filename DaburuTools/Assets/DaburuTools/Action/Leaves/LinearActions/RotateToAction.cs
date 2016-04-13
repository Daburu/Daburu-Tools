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
				SetupAction();
			}
			public RotateToAction(Transform _transform, Vector3 _desiredRotation, float _actionDuration)
			{
				mTransform = _transform;
				SetupAction();
				SetAction(_desiredRotation, _actionDuration);
			}
			public void SetAction(Vector3 _desiredRotationition, float _actionDuration)
			{
				mvecDesiredRotation = _desiredRotationition;
				mfActionDuration = _actionDuration;
			}
			private void SetupAction()
			{
				mvecInitialRotation = mTransform.eulerAngles;
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

				if (mTransform == null)
				{
					// Debug.LogWarning("DaburuTools.Action: mTransform Deleted prematurely");
					mParent.Remove(this);
					return;
				}

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
