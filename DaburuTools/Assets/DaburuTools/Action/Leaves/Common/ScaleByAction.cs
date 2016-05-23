using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class ScaleByAction : Action
		{
			Transform mTransform;
			Vector3 mvecAccumulatedScale;
			Vector3 mvecDesiredScaleDelta;
			float mfActionDuration;
			float mfElaspedDuration;
			Graph mGraph;

			public ScaleByAction(Transform _transform, Graph _graph, Vector3 _desiredDelta, float _actionDuration)
			{
				mTransform = _transform;
				mGraph = _graph;
				SetupAction();
				SetAction(_desiredDelta, _actionDuration);
			}
			public ScaleByAction(Transform _transform, Vector3 _desiredDelta, float _actionDuration)
			{
				mTransform = _transform;
				mGraph = Graph.Linear;
				SetupAction();
				SetAction(_desiredDelta, _actionDuration);
			}
			public void SetAction(Vector3 _desiredDelta, float _actionDuration)
			{
				mvecDesiredScaleDelta = _desiredDelta - Vector3.one;
				mfActionDuration = _actionDuration;
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			private void SetupAction()
			{
				mvecAccumulatedScale = Vector3.one;
				mfElaspedDuration = 0f;
			}
			private Vector3 CalcInverseAccumulatedScale()
			{
				Vector3 inverseAccumulatedScale = mTransform.localScale;
				inverseAccumulatedScale.x /= mvecAccumulatedScale.x;
				inverseAccumulatedScale.y /= mvecAccumulatedScale.y;
				inverseAccumulatedScale.z /= mvecAccumulatedScale.z;

				return inverseAccumulatedScale;
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
				Vector3 delta = Vector3.LerpUnclamped(Vector3.zero, mvecDesiredScaleDelta, t) + Vector3.one - mvecAccumulatedScale;

				mTransform.localScale = Vector3.Scale(CalcInverseAccumulatedScale(), mvecAccumulatedScale + delta);
				mvecAccumulatedScale += delta;


				// Remove self after action is finished.
				if (mfElaspedDuration > mfActionDuration)
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
				mfElaspedDuration += mfActionDuration;

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
