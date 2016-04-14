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

			private ActionParallel mMasterActionParallel;

			private void SetUpActionHandler()
			{
				mMasterActionParallel = new ActionParallel();
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
			public static void RunAction(Action _Action)
			{
				sInstance.mMasterActionParallel.Add(_Action);
			}

			public static void RunActions(params Action[] _Actions)
			{
				sInstance.mMasterActionParallel.Add(_Actions);
			}
			#endregion
		}
	}
}
