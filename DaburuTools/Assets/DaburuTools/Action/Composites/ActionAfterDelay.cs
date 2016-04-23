namespace DaburuTools
{
	namespace Action
	{
		public class ActionAfterDelay : Action
		{
			public override void SetUnscaledDeltaTime(bool _bIsUnscaledDeltaTime)
			{
				base.SetUnscaledDeltaTime(_bIsUnscaledDeltaTime);

				// Set the same for children actions.
				mDelayedAction.SetUnscaledDeltaTime(_bIsUnscaledDeltaTime);
			}

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
					mfTimePassed += ActionDeltaTime(mbIsUnscaledDeltaTime);
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

				if (mDelayedAction != null)
					mDelayedAction.MakeResettable(_bIsResettable);
			}
			public override void Reset()
			{
				if (!mbIsResettable)
					return;

				mfTimePassed = 0f;
				mDelayedAction.Reset();
			}
			public override void StopAction(bool _bSnapToDesired)
			{
				if (!mbIsRunning)
					return;

				// Prevent it from Resetting.
				MakeResettable(false);

				if (mDelayedAction != null)
				{
					if (mDelayedAction.mbIsRunning == false)
						mDelayedAction.RunAction();
					// Need another null check, incase the delayed action is a sequence or parallel.
					// When they are going to run, they might have deleted themself because they are empty.
					if (mDelayedAction != null)
						mDelayedAction.StopAction(_bSnapToDesired);
				}

				OnActionEnd();
				mParent.Remove(this);
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
