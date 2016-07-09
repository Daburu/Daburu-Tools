using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class AxisLocalRotateByAction : Action
		{
			Transform mTransform;
			Graph mGraph;
			Vector3 mvecAxis;
			float mfDesiredAngleDelta;
			float mfActionDuration;

			float mfAccumulatedAngleDelta;
			float mfElapsedDuration;

			public AxisLocalRotateByAction(Transform _transform, Graph _graph, Vector3 _axis, float _desiredAngleDelta, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(_graph);
				SetAxis(_axis);
				SetDesiredAngleDelta(_desiredAngleDelta);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public AxisLocalRotateByAction(Transform _transform, Vector3 _axis, float _desiredAngleDelta, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(Graph.Linear);
				SetAxis(_axis);
				SetDesiredAngleDelta(_desiredAngleDelta);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			public void SetAxis(Vector3 _newAxis)
			{
				mvecAxis = _newAxis;
			}
			public void SetDesiredAngleDelta(float _newDesiredAngleDelta)
			{
				mfDesiredAngleDelta = _newDesiredAngleDelta;
			}
			public void SetActionDuration(float _newActionDuration)
			{
				mfActionDuration = _newActionDuration;
			}
			private void SetupAction()
			{
				mfAccumulatedAngleDelta = 0;
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

				mTransform.Rotate(mvecAxis, -mfAccumulatedAngleDelta, Space.Self);	// Reverse the previous frame's rotation.

				float t = mGraph.Read(mfElapsedDuration / mfActionDuration);
				mfAccumulatedAngleDelta = Mathf.LerpUnclamped(0.0f, mfDesiredAngleDelta, t);

				mTransform.Rotate(mvecAxis, mfAccumulatedAngleDelta, Space.Self);	// Apply the new delta rotation.

				// Remove self after action is finished.
				if (mfElapsedDuration >= mfActionDuration)
				{
					float imperfection = mfDesiredAngleDelta - mfAccumulatedAngleDelta;
					mTransform.Rotate(mvecAxis, imperfection, Space.Self);	// Force to exact delta displacement.

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
					float imperfection = mfDesiredAngleDelta - mfAccumulatedAngleDelta;
					mTransform.Rotate(mvecAxis, imperfection, Space.Self);	// Force to exact delta displacement.
				}

				OnActionEnd();
				mParent.Remove(this);
			}
		}
	}
}
