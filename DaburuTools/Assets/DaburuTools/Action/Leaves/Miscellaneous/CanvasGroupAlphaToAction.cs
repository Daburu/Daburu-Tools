using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace DaburuTools
{
	namespace Action
	{
		public class CanvasGroupAlphaToAction : Action
		{
			CanvasGroup mCanvasGroup;
			float mfDesiredAlpha;
			float mfActionDuration;
			Graph mGraph;

			float mfOriginalAlpha;
			float mfElaspedDuration;

			public CanvasGroupAlphaToAction(CanvasGroup _canvasGroup, Graph _graph, float _desiredAlpha, float _actionDuration)
			{
				mCanvasGroup = _canvasGroup;
				SetGraph(_graph);
				SetDesiredAlpha(_desiredAlpha);
				SetActionDuration(_actionDuration);
				SetupAction();
			}
			public CanvasGroupAlphaToAction(CanvasGroup _canvasGroup, float _desiredAlpha, float _actionDuration)
			{
				mCanvasGroup = _canvasGroup;
				SetGraph(Graph.Linear);
				SetDesiredAlpha(_desiredAlpha);
				SetActionDuration(_actionDuration);
				SetupAction();
			}
			public void SetDesiredAlpha(float _newDesiredAlpha)
			{
				mfDesiredAlpha = _newDesiredAlpha;
			}
			public void SetActionDuration(float _newActionDuration)
			{
				mfActionDuration = _newActionDuration;
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			private void SetupAction()
			{
				mfOriginalAlpha = mCanvasGroup.alpha;
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

				mfElaspedDuration += ActionDeltaTime(mbIsUnscaledDeltaTime);

				float t = mGraph.Read(mfElaspedDuration / mfActionDuration);
				mCanvasGroup.alpha = mGraph.Read(Mathf.Lerp(mfOriginalAlpha, mfDesiredAlpha, t));

				// Remove self after action is finished.
				if (mfElaspedDuration >= mfActionDuration)
				{
					// Snap to desired alpha.
					mCanvasGroup.alpha = mfDesiredAlpha;

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
				mfElaspedDuration = mfActionDuration;

				if (_bSnapToDesired)
				{
					// Snap to desired alpha.
					mCanvasGroup.alpha = mfDesiredAlpha;
				}

				OnActionEnd();
				mParent.Remove(this);
			}
		}
	}
}
