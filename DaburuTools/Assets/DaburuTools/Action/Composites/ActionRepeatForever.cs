namespace DaburuTools
{
	namespace Action
	{
		public class ActionRepeatForever : Action
		{
			private Action mRepeatedAction;

			public ActionRepeatForever(Action _Action)
			{
				_Action.mParent = this;
				mRepeatedAction = _Action;

				_Action.MakeResettable(true);
			}



			public override void RunAction()
			{
				base.RunAction();

				if (mRepeatedAction != null)
					mRepeatedAction.RunAction();
			}
			public override void MakeResettable(bool _bIsResettable)
			{
				UnityEngine.Debug.LogWarning("ActionRepeatForever cannot be resetted");
			}
			public override void Reset()
			{
				UnityEngine.Debug.LogWarning("ActionRepeatForever cannot be resetted");
			}



			// Doesn't make sense to add. Don't need to override Add.
			public override bool Remove(Action _Action)
			{
				mRepeatedAction.Reset();
				return true;
			}
			// No LinkedList to return. Don't need to override GetListHead.
			public override bool IsComposite() { return true; }
		}
	}

}
