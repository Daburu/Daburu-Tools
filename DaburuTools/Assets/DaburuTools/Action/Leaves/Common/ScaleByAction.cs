using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class ScaleByAction : Action
		{
			Transform mTransform;
			Graph mGraph;
			Vector3 mvecDesiredScaleDelta;
			float mfActionDuration;

			Vector3 mvecAccumulatedScale;
			float mfElapsedDuration;

			public ScaleByAction(Transform _transform, Graph _graph, Vector3 _desiredDelta, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(_graph);
				SetDesiredDelta(_desiredDelta);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public ScaleByAction(Transform _transform, Vector3 _desiredDelta, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(Graph.Linear);
				SetDesiredDelta(_desiredDelta);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			public void SetDesiredDelta(Vector3 _newDesiredDelta)
			{
				mvecDesiredScaleDelta = _newDesiredDelta - Vector3.one;
			}
			public void SetActionDuration(float _newActionDuration)
			{
				mfActionDuration = _newActionDuration;
			}
			private void SetupAction()
			{
				mvecAccumulatedScale = Vector3.one;
				mfElapsedDuration = 0f;
			}
			private Vector3 CalcInverseAccumulatedScale()
			{
				Vector3 inverseAccumulatedScale = mTransform.localScale;
				inverseAccumulatedScale.x /= mvecAccumulatedScale.x;
				inverseAccumulatedScale.y /= mvecAccumulatedScale.y;
				inverseAccumulatedScale.z /= mvecAccumulatedScale.z;

				return inverseAccumulatedScale;
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
				Vector3 delta = Vector3.LerpUnclamped(Vector3.zero, mvecDesiredScaleDelta, t) + Vector3.one - mvecAccumulatedScale;

				mTransform.localScale = Vector3.Scale(CalcInverseAccumulatedScale(), mvecAccumulatedScale + delta);
				mvecAccumulatedScale += delta;


				// Remove self after action is finished.
				if (mfElapsedDuration >= mfActionDuration)
				{
					Vector3 finalScaleVec = CalcInverseAccumulatedScale();
					finalScaleVec = Vector3.Scale(finalScaleVec, mvecDesiredScaleDelta + Vector3.one);
					mTransform.localScale = finalScaleVec;	// Force it to be the exact scale that it wants.

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
					Vector3 finalScaleVec = CalcInverseAccumulatedScale();
					finalScaleVec = Vector3.Scale(finalScaleVec, mvecDesiredScaleDelta + Vector3.one);
					mTransform.localScale = finalScaleVec;	// Force it to be the exact position that it wants.
				}

				OnActionEnd();
				mParent.Remove(this);
			}
		}
	}

}
