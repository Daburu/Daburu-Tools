using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class PulseAction : Action
		{
			Transform mTransform;
			Vector3 mvecMinScale;
			Vector3 mvecMaxScale;
			int mnNumCycles;
			Graph mExpandGraph, mShrinkGraph;
			float mfExpandDuration, mfShrinkDuration, mfCycleDuration;

			float mfElaspedDuration;
			int mnCurrentCycle;

			public PulseAction(
				Transform _transform, int _numCycles,
				Graph _expandGraph, Graph _shrinkGraph,
				float _expandDuration, float _shrinkDuration,
				Vector3 _minScale, Vector3 _maxScale)
			{
				mTransform = _transform;
				SetNumCycles(_numCycles);
				SetExpandShrinkGraphs(_expandGraph, _shrinkGraph);
				SetExpandShrinkDuration(_expandDuration, _shrinkDuration);
				SetMinMaxScale(_minScale, _maxScale);
			}
			public PulseAction(Transform _transform, int _numCycles, Graph _expandShrinkGraph, float _cycleDuration,
				Vector3 _minScale, Vector3 _maxScale)
			{
				mTransform = _transform;
				SetNumCycles(_numCycles);
				SetExpandShrinkGraphs(_expandShrinkGraph, _expandShrinkGraph);
				SetExpandShrinkDuration(_cycleDuration/2.0f, _cycleDuration/2.0f);
				SetMinMaxScale(_minScale, _maxScale);
			}

			public void SetNumCycles(int _newNumCycles)
			{
				mnNumCycles = _newNumCycles;
			}
			public void SetExpandShrinkGraphs(Graph _newExpandGraph, Graph _newShrinkGraph)
			{
				mExpandGraph = _newExpandGraph;
				mShrinkGraph = _newShrinkGraph;
			}
			public void SetExpandShrinkDuration(float _newExpandDuration, float _newShrinkDuration)
			{
				mfExpandDuration = _newExpandDuration;
				mfShrinkDuration = _newShrinkDuration;
				mfCycleDuration = mfExpandDuration + mfShrinkDuration;
			}
			public void SetMinMaxScale(Vector3 _newMinScale, Vector3 _newMaxScale)
			{
				mvecMinScale = _newMinScale;
				mvecMaxScale = _newMaxScale;
			}
			private void SetupAction()
			{
				mfElaspedDuration = 0f;
				mnCurrentCycle = 0;
			}
			protected override void OnActionBegin()
			{
				base.OnActionBegin();

				SetupAction(); 
			}


			// Currently only expands then shrinks. Ending with shrink.
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
				float mfCycleElasped = mfElaspedDuration - mfCycleDuration * mnCurrentCycle;
				if (mfCycleElasped < mfExpandDuration) // Expand
				{
					float t = mExpandGraph.Read(mfCycleElasped / mfExpandDuration);
					mTransform.localScale = Vector3.LerpUnclamped(mvecMinScale, mvecMaxScale, t);
				}
				else if (mfCycleElasped < mfCycleDuration) // Shrink
				{
					float t = mShrinkGraph.Read((mfCycleElasped - mfExpandDuration) / mfShrinkDuration);
					mTransform.localScale = Vector3.LerpUnclamped(mvecMaxScale, mvecMinScale, t);
				}
				else
				{
					mnCurrentCycle++;
					// Remove self after action is finished.
					if (mnCurrentCycle >= mnNumCycles)
					{
						mTransform.localScale = mvecMinScale;	// Force it to be the exact scale that it wants.
						OnActionEnd();
						mParent.Remove(this);
					}
					else
					{
						// Do the interpolation for the beginning of the next cycle.
						float t = mExpandGraph.Read((mfCycleElasped - mfCycleDuration) / mfExpandDuration);
						mTransform.localScale = Vector3.LerpUnclamped(mvecMinScale, mvecMaxScale, t);
					}
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
				mnCurrentCycle = mnNumCycles;

				if (_bSnapToDesired)
				{
					mTransform.localScale = mvecMinScale;	// Force it to be the exact position that it wants.
				}

				OnActionEnd();
				mParent.Remove(this);
			}
		}
	}

}
