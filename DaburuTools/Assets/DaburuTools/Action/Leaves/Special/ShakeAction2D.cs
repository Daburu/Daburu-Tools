using UnityEngine;
using System.Collections;

namespace DaburuTools
{
	namespace Action
	{
		public class ShakeAction2D : Action
		{
			Transform mTransform;
			int mnNumShakes;
			float mfShakePeriod;
			float mfShakeIntensity;
			Graph mAttenuationGraph;

			Vector3 mVecDeltaPos;
			float mfElaspedDuration;
			int mnCurrentCycle;

			public ShakeAction2D(Transform _transform, int _numShakes, float _shakeIntensity)
			{
				mTransform = _transform;

				SetNumShakes(_numShakes);
				SetShakeIntensity(_shakeIntensity);
				SetAttenuationGraph(Graph.One);

				SetShakePeriod(0.05f);
			}
			public ShakeAction2D(Transform _transform, int _numShakes, float _shakeIntensity, Graph _attenuationGraph)
			{
				mTransform = _transform;

				SetNumShakes(_numShakes);
				SetShakeIntensity(_shakeIntensity);
				SetAttenuationGraph(_attenuationGraph);

				SetShakePeriod(0.05f);
			}

			public void SetNumShakes(int _newNumShakes)
			{
				mnNumShakes = _newNumShakes;
			}
			public void SetShakeIntensity(float _newShakeIntensity)
			{
				mfShakeIntensity = _newShakeIntensity;
			}
			public void SetAttenuationGraph(Graph _newAttenuationGraph)
			{
				mAttenuationGraph = _newAttenuationGraph;
			}
			public void SetShakeFrequency(float _newShakeFrequency)
			{
				SetShakePeriod(1.0f / _newShakeFrequency);
			}
			public void SetShakeByDuration(float _newShakeDuration, int _newNumShakes)
			{
				SetNumShakes(_newNumShakes);
				SetShakePeriod(_newShakeDuration / _newNumShakes);
			}
			private void SetShakePeriod(float _newShakePeriod)
			{
				mfShakePeriod = _newShakePeriod;
			}
			private void SetupAction()
			{
				mfElaspedDuration = 0f;
				mnCurrentCycle = 0;
				mVecDeltaPos = Vector3.zero;
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
				float mfCycleElasped = mfElaspedDuration - mfShakePeriod * mnCurrentCycle;
				if (mfCycleElasped > mfShakePeriod)
				{
					mnCurrentCycle = (int) (mfElaspedDuration / mfShakePeriod);

					// Remove self after action is finished.
					if (mnCurrentCycle >= mnNumShakes)
					{
						// Force it back to original position.
						mTransform.position -= mVecDeltaPos;

						OnActionEnd();
						mParent.Remove(this);
					}
					else
					{
						// Set back to original position.
						mTransform.position -= mVecDeltaPos;
						// Set new shake pos.
						float t = mAttenuationGraph.Read(mfElaspedDuration / (mfShakePeriod * mnNumShakes));
						mVecDeltaPos = Random.insideUnitCircle * mfShakeIntensity * t;
						mTransform.position += mVecDeltaPos;
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
				mnCurrentCycle = mnNumShakes;

				if (_bSnapToDesired)
				{
					mTransform.position -= mVecDeltaPos;	// Force it to be the exact position that it wants.
				}

				OnActionEnd();
				mParent.Remove(this);
			}
		}
	}

}
