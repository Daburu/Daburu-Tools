using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class ScaleToAction : Action
		{
			Transform mTransform;
			Graph mGraph;
			Vector3 mvecDesiredScale;
			float mfActionDuration;

			Vector3 mvecInitialScale;
			float mfElapsedDuration;

			public ScaleToAction(Transform _transform, Graph _graph, Vector3 _desiredScale, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(_graph);
				SetDesiredScale(_desiredScale);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public ScaleToAction(Transform _transform, Vector3 _desiredScale, float _actionDuration)
			{
				mTransform = _transform;
				SetGraph(Graph.Linear);
				SetDesiredScale(_desiredScale);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			public void SetDesiredScale(Vector3 _newDesiredScale)
			{
				mvecDesiredScale = _newDesiredScale;
			}
			public void SetActionDuration(float _newActionDuration)
			{
				mfActionDuration = _newActionDuration;
			}
			private void SetupAction()
			{
				mvecInitialScale = mTransform.localScale;
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
				mTransform.localScale = Vector3.LerpUnclamped(mvecInitialScale, mvecDesiredScale, t);

				// Remove self after action is finished.
				if (mfElapsedDuration >= mfActionDuration)
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
					mTransform.localScale = mvecDesiredScale;	// Force it to be the exact position that it wants.
				}

				OnActionEnd();
				mParent.Remove(this);
			}
		}
	}

}
