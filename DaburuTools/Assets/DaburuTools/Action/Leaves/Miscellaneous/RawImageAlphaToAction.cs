using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace DaburuTools
{
	namespace Action
	{
		public class RawImageAlphaToAction : Action
		{
			// 1: Declare your other variables such as Transforms, Graphs, etc.
			RawImage mRawImage;
			Graph mGraph;
			float mfDesiredAlpha;
			float mfActionDuration;

			float mfOriginalAlpha;
			float mfElapsedDuration;

			public RawImageAlphaToAction(RawImage _rawImage, Graph _graph, float _desiredAlpha, float _actionDuration)
			{
				mRawImage = _rawImage;
				SetGraph(_graph);
				SetDesiredAlpha(_desiredAlpha);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public RawImageAlphaToAction(RawImage _rawImage, float _desiredAlpha, float _actionDuration)
			{
				mRawImage = _rawImage;
				SetGraph(Graph.Linear);
				SetDesiredAlpha(_desiredAlpha);
				SetActionDuration(_actionDuration);

				SetupAction();
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			public void SetDesiredAlpha(float _newDesiredAlpha)
			{
				mfDesiredAlpha = _newDesiredAlpha;
			}
			public void SetActionDuration(float _newActionDuration)
			{
				mfActionDuration = _newActionDuration;
			}
			private void SetupAction()
			{
				mfOriginalAlpha = mRawImage.color.a;
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

				mfElapsedDuration += ActionDeltaTime(mbIsUnscaledDeltaTime);

				float t = mGraph.Read(mfElapsedDuration / mfActionDuration);
				Color newCol = mRawImage.color;
				newCol.a = mGraph.Read(Mathf.Lerp(mfOriginalAlpha, mfDesiredAlpha, t));
				mRawImage.color = newCol;

				// Remove self after action is finished.
				if (mfElapsedDuration >= mfActionDuration)
				{
					// Snap to desired alpha.
					Color finalCol = mRawImage.color;
					finalCol.a = mfDesiredAlpha;
					mRawImage.color = finalCol;

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
				mfElapsedDuration = mfActionDuration;

				if (_bSnapToDesired)
				{
					// Snap to desired alpha.
					Color finalCol = mRawImage.color;
					finalCol.a = mfDesiredAlpha;
					mRawImage.color = finalCol;
				}

				OnActionEnd();
				mParent.Remove(this);
			}
		}
	}
}
