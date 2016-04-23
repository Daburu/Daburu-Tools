namespace DaburuTools
{
	namespace Action
	{
		public class ActionRepeat : Action
		{
			public override void SetUnscaledDeltaTime(bool _bIsUnscaledDeltaTime)
			{
				base.SetUnscaledDeltaTime(_bIsUnscaledDeltaTime);

				// Set the same for children actions.
				mRepeatedAction.SetUnscaledDeltaTime(_bIsUnscaledDeltaTime);
			}

			private Action mRepeatedAction;
			private int mnNumRepeats;
			private int mnCurrentRepeats;
			private bool mbReadyToReset;

			public ActionRepeat(Action _Action, int _numRepeats)
			{
				_Action.mParent = this;
				mRepeatedAction = _Action;
				mnNumRepeats = _numRepeats;
				mnCurrentRepeats = 0;

				_Action.MakeResettable(true);
			}



			public override void RunAction()
			{
				base.RunAction();

				if (mRepeatedAction != null)
				{
					if (mbIsResettable && mbReadyToReset)
					{
						OnActionEnd();

						mParent.Remove(this);
					}
					else
					{
						mRepeatedAction.RunAction();
					}
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

				mbReadyToReset = false;
			}
			public override void Reset()
			{
				mnCurrentRepeats = 0;
				mbReadyToReset = false;
				mRepeatedAction.Reset();
			}
			public override void StopAction(bool _bSnapToDesired)
			{
				if (!mbIsRunning)
					return;

				// Prevent it from Resetting.
				MakeResettable(false);

				// Simulate the action has ended. Does not really matter by how much.
				mnCurrentRepeats = mnNumRepeats;

				if (mRepeatedAction.mbIsRunning == false)
					mRepeatedAction.RunAction();
				mRepeatedAction.StopAction(_bSnapToDesired);

				OnActionEnd();
				mParent.Remove(this);
			}



			// Doesn't make sense to add. Don't need to override Add.
			public override bool Remove(Action _Action)
			{
				mnCurrentRepeats++;
				if (mnCurrentRepeats < mnNumRepeats)
				{
					mRepeatedAction.Reset();
					return true;
				}

				// Simply de-reference to let GC collect.
				if (!mbIsResettable)
					mRepeatedAction = null;
				else
					mbReadyToReset = true;

				return true;
			}
			// No LinkedList to return. Don't need to override GetListHead.
			public override bool IsComposite() { return true; }
		}
	}

}
