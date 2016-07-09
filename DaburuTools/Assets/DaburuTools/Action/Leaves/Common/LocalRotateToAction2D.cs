using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class LocalRotateToAction2D : Action
		{
			Transform mTransform;
			Graph mGraph;
			float mfDesiredLocalZEulerAngle;
			float mfActionDuration;

			float mfInitialLocalZEulerAngle;
			float mfElapsedDuration;

			public LocalRotateToAction2D(Transform _transform, Graph _graph, float _desiredLocalZEulerAngle, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(_graph);
				SetDesiredLocalZEulerAngle(_desiredLocalZEulerAngle);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public LocalRotateToAction2D(Transform _transform, float _desiredLocalZEulerAngle, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(Graph.Linear);
				SetDesiredLocalZEulerAngle(_desiredLocalZEulerAngle);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			public void SetDesiredLocalZEulerAngle(float _newDesiredLocalZEulerAngle)
			{
				mfDesiredLocalZEulerAngle = _newDesiredLocalZEulerAngle;
			}
			public void SetActionDuration(float _newActionDuration)
			{
				mfActionDuration = _newActionDuration;
			}
			private void SetupAction()
			{
				mfInitialLocalZEulerAngle = mTransform.localEulerAngles.z;
				mfElapsedDuration = 0f;
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

				mfElapsedDuration += ActionDeltaTime(mbIsUnscaledDeltaTime);

				float t = mGraph.Read(mfElapsedDuration / mfActionDuration);
				mTransform.localEulerAngles = new Vector3(
					mTransform.localEulerAngles.x,
					mTransform.localEulerAngles.y,
					Mathf.LerpUnclamped(mfInitialLocalZEulerAngle, mfDesiredLocalZEulerAngle, t)
				);

				// Remove self after action is finished.
				if (mfElapsedDuration >= mfActionDuration)
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
			public override void MakeResettable(bool _bIsResettable)
			{
				base.MakeResettable(_bIsResettable);
			}
			public override void Reset()
			{
				SetupAction();
			}
			public override void StopAction(bool _bSnapToDesired)
			{
				if (!mbIsRunning)
					return;

				// Prevent it from Resetting.
				MakeResettable(false);

				// Simulate the action has ended. Does not really matter by how much.
				mfElapsedDuration += mfActionDuration;

				if (_bSnapToDesired)
				{
					mTransform.localEulerAngles = new Vector3(
						mTransform.localEulerAngles.x,
						mTransform.localEulerAngles.y,
						mfDesiredLocalZEulerAngle
					);	// Force it to be the exact position that it wants.
				}

				OnActionEnd();
				mParent.Remove(this);
			}
		}
	}
}