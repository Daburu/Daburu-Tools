using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class GraphLocalMoveByAction : Action
		{
			Transform mTransform;
			Vector3 mvecAccumulatedDelta;
			Vector3 mvecDesiredTotalDelta;
			float mfActionDuration;
			float mfElaspedDuration;
			Graph mGraph;

			public GraphLocalMoveByAction(Transform _transform, Graph _graph)
			{
				mTransform = _transform;
				mGraph = _graph;
				SetupAction();
			}
			public GraphLocalMoveByAction(Transform _transform, Graph _graph, Vector3 _desiredDelta, float _actionDuration)
			{
				mTransform = _transform;
				mGraph = _graph;
				SetupAction();
				SetAction(_desiredDelta, _actionDuration);
			}
			public void SetAction(Vector3 _desiredDelta, float _actionDuration)
			{
				mvecDesiredTotalDelta = _desiredDelta;
				mfActionDuration = _actionDuration;
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			private void SetupAction()
			{
				mvecAccumulatedDelta = Vector3.zero;
				mfElaspedDuration = 0f;
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

				// It is less tricky to track the action by elasped time.
				// Otherwise, we need to check the sqrDist of both vec3s
				// for when we need to terminate the action.
				mfElaspedDuration += Time.deltaTime;

				mTransform.localPosition -= mvecAccumulatedDelta;	// Reverse the previous frame's rotation.

				float t = mGraph.Read(mfElaspedDuration / mfActionDuration);
				mvecAccumulatedDelta = Vector3.LerpUnclamped(Vector3.zero, mvecDesiredTotalDelta, t);

				mTransform.localPosition += mvecAccumulatedDelta;	// Apply the new delta rotation.

				// Remove self after action is finished.
				if (mfElaspedDuration > mfActionDuration)
				{
					Vector3 imperfection = mvecDesiredTotalDelta - mvecAccumulatedDelta;
					mTransform.localPosition += imperfection;	// Force to exact delta displacement.

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
