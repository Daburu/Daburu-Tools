using UnityEngine;
//using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class ActionHandler : MonoBehaviour
		{
			private static ActionHandler sInstance = null;
			public static ActionHandler Instance { get { return sInstance; } }

			private MasterActionParallel mMasterActionParallel;

			private void SetUpActionHandler()
			{
				mMasterActionParallel = new MasterActionParallel();
			}

			void Awake()
			{
				if (sInstance == null)
					sInstance = this;
				else
					Destroy(this.gameObject);

				SetUpActionHandler();
			}

			void Update()
			{
				mMasterActionParallel.RunAction();
			}

			void OnDestroy()
			{
				sInstance = null;
			}

			#region Client Functions
			public static void RunAction(params Action[] _Actions)
			{
				sInstance.mMasterActionParallel.Add(_Actions);
			}
			#endregion

			#region Nested Special ActionParallelClass
			private sealed class MasterActionParallel : ActionParallel
			{
				public override void StopAction (bool _bSnapToDesired)
				{
					return;
				}
			}
			#endregion
		}
	}
}
