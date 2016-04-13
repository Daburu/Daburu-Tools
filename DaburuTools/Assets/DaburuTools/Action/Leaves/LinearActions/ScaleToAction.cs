using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class ScaleToAction : Action
		{
			Transform mTransform;
			Vector3 mvecInitialScale;
			Vector3 mvecDesiredScale;
			float mfActionDuration;
			float mfElaspedDuration;

			public ScaleToAction(Transform _transform)
			{
				mTransform = _transform;
				SetupAction();
			}
			public ScaleToAction(Transform _transform, Vector3 _desiredScale, float _actionDuration)
			{
				mTransform = _transform;
				SetupAction();
				SetAction(_desiredScale, _actionDuration);
			}
			public void SetAction(Vector3 _desiredScale, float _actionDuration)
			{
				mvecDesiredScale = _desiredScale;
				mfActionDuration = _actionDuration;
			}
			private void SetupAction()
			{
				mvecInitialScale = mTransform.localScale;
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
				mTransform.localScale = Vector3.LerpUnclamped(mvecInitialScale, mvecDesiredScale, t);

				// Remove self after action is finished.
				if (mfElaspedDuration > mfActionDuration)
				{
					mTransform.localScale = mvecDesiredScale;	// Force it to be the exact scale that it wants.
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
