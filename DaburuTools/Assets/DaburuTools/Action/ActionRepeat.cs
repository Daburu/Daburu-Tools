using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class ActionRepeat : Action
		{
			private Action mRepeatedAction;
			private int mnNumRepeats;
			private int mnCurrentRepeats;

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
					mRepeatedAction.RunAction();

				if (mParent != null && mRepeatedAction == null)
					mParent.Remove(this);
			}
			public override void MakeResettable(bool _bIsResettable)
			{
				base.MakeResettable(_bIsResettable);
			}
			public override void Reset()
			{
				mnCurrentRepeats = 0;
				mRepeatedAction.Reset();
			}



			// Can't add. Don't need to override, will return false based on Action.cs
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
				{
					mRepeatedAction = null;
				}

				return true;
			}
			// No LinkedList to return. Don't need to override GetListHead.
			public override bool IsComposite() { return true; }
		}
	}

}
