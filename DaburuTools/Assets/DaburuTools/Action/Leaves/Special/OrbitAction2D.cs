﻿using UnityEngine;
using System.Collections;

namespace DaburuTools
{
	namespace Action
	{
		public class OrbitAction2D : Action
		{
			Transform mTransform;
			Transform mOrbitPointTransform;
			Vector3 mOrbitAxisDir;
			bool mbIsClockwise;
			public bool IsClockwise
			{
				get { return mbIsClockwise; }
				set
				{
					mbIsClockwise = value;
					if (mbIsClockwise)
						mOrbitAxisDir = -Vector3.forward;
					else
						mOrbitAxisDir = Vector3.forward;
				}
			}
			int mnNumCycles;
			Graph mRevolutionGraph;
			float mfCycleDuration;
			bool mbPreventOwnAxisRotation;
			public bool PreventOwnAxisRotation
			{
				get { return mbPreventOwnAxisRotation; }
				set { mbPreventOwnAxisRotation = value; }
			}

			float mfElaspedDuration;
			int mnCurrentCycle;

			public OrbitAction2D(
				Transform _transform, Transform mOrbitPointTransform,
				bool _isClockwise,
				int _numCycles, Graph _revolutionGraph,
				float _cycleDuration,
				bool _preventOwnAxisRotation = true)
			{
				mTransform = _transform;
				SetOrbitPointTransform(mOrbitPointTransform);
				IsClockwise = _isClockwise;
				SetNumCycles(_numCycles);
				SetRevolutionGraph(_revolutionGraph);
				SetCycleDuration(_cycleDuration);
				mbPreventOwnAxisRotation = _preventOwnAxisRotation;
			}

			public void SetOrbitPointTransform(Transform _newOrbitPointTransform)
			{
				mOrbitPointTransform = _newOrbitPointTransform;
			}
			public void SetNumCycles(int _newNumCycles)
			{
				mnNumCycles = _newNumCycles;
			}
			public void SetRevolutionGraph(Graph _newRevolutionGraph)
			{
				mRevolutionGraph = _newRevolutionGraph;
			}
			public void SetCycleDuration(float _newCycleDuration)
			{
				mfCycleDuration = _newCycleDuration;
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

				// Undo previous frame's rotation.
				float mfCycleElaspedOld = mfElaspedDuration - mfCycleDuration * mnCurrentCycle;
				float tOld = mRevolutionGraph.Read(mfCycleElaspedOld / mfCycleDuration);
				mTransform.RotateAround(mOrbitPointTransform.position, mOrbitAxisDir, -360.0f * tOld);
				// Offset Rotation so that the orbit action does not affect the object's rotation.
				if (PreventOwnAxisRotation)
					mTransform.Rotate(mOrbitAxisDir, 360.0f * tOld);

				mfElaspedDuration += Time.deltaTime;
				float mfCycleElasped = mfElaspedDuration - mfCycleDuration * mnCurrentCycle;
				if (mfCycleElasped < mfCycleDuration)
				{
					float t = mRevolutionGraph.Read(mfCycleElasped / mfCycleDuration);
					mTransform.RotateAround(mOrbitPointTransform.position, mOrbitAxisDir, 360.0f * t);

					// Offset Rotation so that the orbit action does not affect the object's rotation.
					if (mbPreventOwnAxisRotation)
						mTransform.Rotate(mOrbitAxisDir, 360.0f * -t);
				}
				else
				{
					mnCurrentCycle++;
					// Remove self after action is finished.
					if (mnCurrentCycle >= mnNumCycles)
					{
						// Force it to be the end position of the cycle.
						mTransform.RotateAround(mOrbitPointTransform.position, mOrbitAxisDir, 360.0f);
						// Offset Rotation so that the orbit action does not affect the object's rotation.
						if (mbPreventOwnAxisRotation)
							mTransform.Rotate(mOrbitAxisDir, -360.0f);
						OnActionEnd();
						mParent.Remove(this);
					}
					else
					{
						// Do the interpolation for the beginning of the next cycle.
						float t = mRevolutionGraph.Read((mfCycleElasped - mfCycleDuration) / mfCycleDuration);
						mTransform.RotateAround(mOrbitPointTransform.position, mOrbitAxisDir, 360.0f * t);

						// Offset Rotation so that the orbit action does not affect the object's rotation.
						if (mbPreventOwnAxisRotation)
							mTransform.Rotate(mOrbitAxisDir, 360.0f * -t);
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
		}
	}

}
