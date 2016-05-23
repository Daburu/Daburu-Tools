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
			Graph mGraph;

			public LocalRotateToAction2D(Transform _transform, Graph _graph, float _desiredLocalZEulerAngle, float _actionDuration)
			{
				mTransform = _transform;
				mGraph = _graph;
				SetupAction();
				SetAction(_desiredLocalZEulerAngle, _actionDuration);
			}
			public LocalRotateToAction2D(Transform _transform, float _desiredLocalZEulerAngle, float _actionDuration)
			{
				mTransform = _transform;
				mGraph = Graph.Linear;
				SetupAction();
				SetAction(_desiredLocalZEulerAngle, _actionDuration);
			}
			public void SetAction(float _desiredLocalZEulerAngle, float _actionDuration)
			{
				mfDesiredLocalZEulerAngle = _desiredLocalZEulerAngle;
				mfActionDuration = _actionDuration;
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			private void SetupAction()
			{
				mfInitialLocalZEulerAngle = mTransform.localEulerAngles.z;
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

				mfElaspedDuration += ActionDeltaTime(mbIsUnscaledDeltaTime);

				float t = mGraph.Read(mfElaspedDuration / mfActionDuration);
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
				mfElaspedDuration += mfActionDuration;

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