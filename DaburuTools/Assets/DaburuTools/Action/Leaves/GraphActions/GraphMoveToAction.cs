using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class GraphMoveToAction : Action
		{
			Transform mTransform;
			Vector3 mvecInitialPos;
			Vector3 mvecDesiredPos;
			float mfActionDuration;
			float mfElaspedDuration;
			Graph mGraph;

			public GraphMoveToAction(Transform _transform, Graph _graph)
			{
				mTransform = _transform;
				mGraph = _graph;
				SetupAction();
			}
			public GraphMoveToAction(Transform _transform, Graph _graph, Vector3 _desiredPosition, float _actionDuration)
			{
				mTransform = _transform;
				mGraph = _graph;
				SetupAction();
				SetAction(_desiredPosition, _actionDuration);
			}
			public void SetAction(Vector3 _desiredPosition, float _actionDuration)
			{
				mvecDesiredPos = _desiredPosition;
				mfActionDuration = _actionDuration;
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
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

				mfElaspedDuration += Time.deltaTime;

				float t = mGraph.Read(mfElaspedDuration / mfActionDuration);
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
