using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class GraphLocalMoveByAction : Action
		{
			Transform mTransform;
			Vector3 mvecAccumulatedLocalDelta;
			Vector3 mvecDesiredTotalLocalDelta;
			float mfActionDuration;
			float mfElaspedDuration;
			Graph mGraph;

			public GraphLocalMoveByAction(Transform _transform, Graph _graph)
			{
				mTransform = _transform;
				mGraph = _graph;
				SetupAction();
			}
			public GraphLocalMoveByAction(Transform _transform, Graph _graph, Vector3 _desiredLocalDelta, float _actionDuration)
			{
				mTransform = _transform;
				mGraph = _graph;
				SetupAction();
				SetAction(_desiredLocalDelta, _actionDuration);
			}
			public void SetAction(Vector3 _desiredLocalDelta, float _actionDuration)
			{
				mvecDesiredTotalLocalDelta = _desiredLocalDelta;
				mfActionDuration = _actionDuration;
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			private void SetupAction()
			{
				mvecAccumulatedLocalDelta = Vector3.zero;
				mfElaspedDuration = 0f;
			}



			public override void RunAction()
			{
				base.RunAction();

				// It is less tricky to track the action by elasped time.
				// Otherwise, we need to check the sqrDist of both vec3s
				// for when we need to terminate the action.
				mfElaspedDuration += Time.deltaTime;

				mTransform.localPosition -= mvecAccumulatedLocalDelta;	// Reverse the previous frame's rotation.

				float t = mGraph.Read(mfElaspedDuration / mfActionDuration);
				mvecAccumulatedLocalDelta = Vector3.LerpUnclamped(Vector3.zero, mvecDesiredTotalLocalDelta, t);

				mTransform.localPosition += mvecAccumulatedLocalDelta;	// Apply the new delta rotation.

				// Remove self after action is finished.
				if (mfElaspedDuration > mfActionDuration)
				{
					Vector3 imperfection = mvecDesiredTotalLocalDelta - mvecAccumulatedLocalDelta;
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
