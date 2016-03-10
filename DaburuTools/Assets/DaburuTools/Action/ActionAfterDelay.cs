namespace DaburuTools
{
	namespace Action
	{
		public class ActionAfterDelay : Action
		{
			private Action mDelayedAction;
			private float mfTimeDelay;
			private float mfTimePassed;

			public ActionAfterDelay(Action _Action, float _fTimeDelay)
			{
				_Action.mParent = this;
				mDelayedAction = _Action;
				mfTimeDelay = _fTimeDelay;
				mfTimePassed = 0f;
			}



			public override void RunAction()
			{
				base.RunAction();

				// Delay the action till delay span has passed.
				if (mfTimePassed < mfTimeDelay)
				{
					mfTimePassed += UnityEngine.Time.deltaTime;
					return;
				}

				if (mDelayedAction != null)
				{
					mDelayedAction.RunAction();
				}
				else
				{
					OnActionEnd();

					if (mParent != null)
						mParent.Remove(this);
				}
			}
			public override void MakeResettable(bool _bIsResettable)
			{
				base.MakeResettable(_bIsResettable);

				mDelayedAction.MakeResettable(_bIsResettable);
			}
			public override void Reset()
			{
				mfTimePassed = 0f;
				mDelayedAction.Reset();
			}



			// Doesn't make sense to add. Don't need to override Add.
			public override bool Remove(Action _Action)
			{
				// Simply de-reference to let GC collect.
				if (!mbIsResettable)
					mDelayedAction = null;
				else
					mParent.Remove(this);

				return true;
			}
			// No LinkedList to return. Don't need to override GetListHead.
			public override bool IsComposite() { return true; }
		}
	}

}
