using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class IdleRotateAction : Action
		{
			Transform mTransform;
			Vector3 mvecRandRotAxis;
			int mnNumCycles;
			Graph mGraph;
			float mfMinInterval, mfMaxInterval;
			float mfMinSpeed, mfMaxSpeed;

			float mfElaspedIntervalDuration;
			int mnCurrentCycle;
			float mfCurCycleInterval, mfCurCycleSpeed;
			float mfCurCycleTotalRot;

			public IdleRotateAction(Transform _transform, int _numCycles, Graph _graph,
				float _minInterval, float _maxInterval,
				float _minSpeed, float _maxSpeed)
			{
				mTransform = _transform;
				SetupAction();
				SetNumCycles(_numCycles);
				SetGraph(_graph);
				SetMinMaxInterval(_minInterval, _maxInterval);
				SetMinMaxSpeed(_minSpeed, _maxSpeed);
			}
			public IdleRotateAction(Transform _transform, int _numCycles, Graph _graph,
				float _interval, float _speed)
			{
				mTransform = _transform;
				SetupAction();
				SetNumCycles(_numCycles);
				SetGraph(_graph);
				SetMinMaxInterval(_interval, _interval);
				SetMinMaxSpeed(_speed, _speed);
			}
			public void SetNumCycles(int _newNumCycles)
			{
				mnNumCycles = _newNumCycles;
			}
			public void SetGraph(Graph _newGraph)
			{
				mGraph = _newGraph;
			}
			public void SetMinMaxInterval(float _newMinInterval, float _newMaxInterval)
			{
				mfMinInterval = _newMinInterval;
				mfMaxInterval = _newMaxInterval;
			}
			public void SetMinMaxSpeed(float _newMinSpeed, float _newMaxSpeed)
			{
				mfMinSpeed = _newMinSpeed;
				mfMaxSpeed = _newMaxSpeed;
			}
			private void SetupAction()
			{
				mfElaspedIntervalDuration = 0f;
				mnCurrentCycle = 0;
				SetNewCycle();
			}
			protected override void OnActionBegin()
			{
				base.OnActionBegin();

				SetupAction(); 
			}



			public override void RunAction()
			{
				base.RunAction();

				float tOld = mGraph.Read(mfElaspedIntervalDuration / mfCurCycleInterval);
				mfElaspedIntervalDuration += Time.deltaTime;
				float tNew = mGraph.Read(mfElaspedIntervalDuration / mfCurCycleInterval);

				// Undo Rotation.
				float prevRotation = Mathf.LerpUnclamped(0.0f, mfCurCycleTotalRot, tOld);
				mTransform.RotateAround(mTransform.position, mvecRandRotAxis, -prevRotation);

				if (mfElaspedIntervalDuration > mfCurCycleInterval)
				{
					// Apply End Rotation.
					mTransform.RotateAround(mTransform.position, mvecRandRotAxis, mfCurCycleTotalRot);

					mnCurrentCycle++;

					// Remove self after action is finished.
					if (mnCurrentCycle >= mnNumCycles)
					{
						OnActionEnd();
						mParent.Remove(this);
					}
					else
					{
						SetNewCycle();
						mfElaspedIntervalDuration = 0f;
					}
				}
				else
				{
					// Apply new Rotation.
					float newRotation = Mathf.LerpUnclamped(0.0f, mfCurCycleTotalRot, tNew);
					mTransform.RotateAround(mTransform.position, mvecRandRotAxis, newRotation);
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

			private void SetNewCycle()
			{
				mvecRandRotAxis = Random.insideUnitSphere.normalized;
				mfCurCycleInterval = Random.Range(mfMinInterval, mfMaxInterval);
				mfCurCycleSpeed = Random.Range(mfMinSpeed, mfMaxSpeed);
				mfCurCycleTotalRot = mfCurCycleInterval * mfCurCycleSpeed;
			}
		}
	}
}
