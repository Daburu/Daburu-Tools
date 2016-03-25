using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class GraphScaleToAction : Action
		{
			Transform mTransform;
			Vector3 mvecInitialScale;
			Vector3 mvecDesiredScale;
			float mfActionDuration;
			float mfElaspedDuration;
			Graph mGraph;

			public GraphScaleToAction(Transform _transform, Graph _graph)
			{
				mTransform = _transform;
				mGraph = _graph;
				SetupAction();
			}
			public GraphScaleToAction(Transform _transform, Graph _graph, Vector3 _desiredScale, float _actionDuration)
			{
				mTransform = _transform;
				mGraph = _graph;
				SetupAction();
				SetAction(_desiredScale, _actionDuration);
			}
			public void SetAction(Vector3 _desiredScale, float _actionDuration)
			{
				mvecDesiredScale = _desiredScale;
				mfActionDuration = _actionDuration;
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
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

				mfElaspedDuration += Time.deltaTime;

				float t = mGraph.Read(mfElaspedDuration / mfActionDuration);
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
