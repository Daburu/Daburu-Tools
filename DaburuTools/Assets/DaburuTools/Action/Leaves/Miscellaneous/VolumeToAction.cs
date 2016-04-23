using UnityEngine;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class VolumeToAction : Action
		{
			AudioSource mAudioSource;
			float mfDesiredVolume;
			float mfActionDuration;
			Graph mGraph;

			float mfOriginalVolume;
			float mfElaspedDuration;

			public VolumeToAction(AudioSource _audioSource, float _desiredVolume, float _actionDuration)
			{
				mAudioSource = _audioSource;
				SetDesiredVolume(_desiredVolume);
				SetActionDuration(_actionDuration);
				SetGraph(Graph.Linear);
				SetupAction();
			}

			public void SetDesiredVolume(float _newDesiredVolume)
			{
				mfDesiredVolume = _newDesiredVolume;
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
				mfOriginalVolume = mAudioSource.volume;
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
				mAudioSource.volume = Mathf.Lerp(mfOriginalVolume, mfDesiredVolume, t);

				// Remove self after action is finished.
				if (mfElaspedDuration > mfActionDuration)
				{
					// Snap volume to desired volume.
					mAudioSource.volume = mfDesiredVolume;

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
					// Snap volume to desired volume.
					mAudioSource.volume = mfDesiredVolume;
				}

				OnActionEnd();
				mParent.Remove(this);
			}
		}
	}
}
