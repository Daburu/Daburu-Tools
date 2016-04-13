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
				SetupAction();
			}
			public MoveToAction(Transform _transform, Vector3 _desiredPosition, float _actionDuration)
			{
				mTransform = _transform;
				SetupAction();
				SetAction(_desiredPosition, _actionDuration);
			}
			public void SetAction(Vector3 _desiredPosition, float _actionDuration)
			{
				mvecDesiredPos = _desiredPosition;
				mfActionDuration = _actionDuration;
			}
			private void SetupAction()
			{
				mvecInitialPos = mTransform.position;
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
				mTransform.position = Vector3.LerpUnclamped(mvecInitialPos, mvecDesiredPos, t);

				// Remove self after action is finished.
				if (mfElaspedDuration > mfActionDuration)
				{
					mTransform.position = mvecDesiredPos;	// Force it to be the exact position that it wants.
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
