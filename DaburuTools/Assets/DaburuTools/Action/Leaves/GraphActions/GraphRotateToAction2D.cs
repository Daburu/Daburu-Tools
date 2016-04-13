using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class GraphRotateToAction2D : Action
		{
			Transform mTransform;
			float mfInitialZEulerAngle;
			float mfDesiredZEulerAngle;
			float mfActionDuration;
			float mfElaspedDuration;
			Graph mGraph;

			public GraphRotateToAction2D(Transform _transform, Graph _graph)
			{
				mTransform = _transform;
				mGraph = _graph;
				SetupAction();
			}
			public GraphRotateToAction2D(Transform _transform, Graph _graph, float _desiredZEulerAngle, float _actionDuration)
			{
				mTransform = _transform;
				mGraph = _graph;
				SetupAction();
				SetAction(_desiredZEulerAngle, _actionDuration);
			}
			public void SetAction(float _desiredZEulerAngle, float _actionDuration)
			{
				mfDesiredZEulerAngle = _desiredZEulerAngle;
				mfActionDuration = _actionDuration;
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			private void SetupAction()
			{
				mfInitialZEulerAngle = mTransform.eulerAngles.z;
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

				float t = mGraph.Read(mfElaspedDuration / mfActionDuration);
				mTransform.eulerAngles = new Vector3(
					mTransform.eulerAngles.x,
					mTransform.eulerAngles.y,
					Mathf.LerpUnclamped(mfInitialZEulerAngle, mfDesiredZEulerAngle, t)
				);

				// Remove self after action is finished.
				if (mfElaspedDuration > mfActionDuration)
				{
					mTransform.eulerAngles = new Vector3(
						mTransform.eulerAngles.x,
						mTransform.eulerAngles.y,
						mfDesiredZEulerAngle
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
		}
	}
}
