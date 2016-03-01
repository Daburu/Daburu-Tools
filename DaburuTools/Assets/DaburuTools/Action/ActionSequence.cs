using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class ActionSequence : Action
		{
			private LinkedList<Action> mActionLinkedList;

			public ActionSequence()
			{
				mActionLinkedList = new LinkedList<Action>();
			}

			public override void RunAction()
			{
				base.RunAction();

				if (mActionLinkedList.Count > 0)
					mActionLinkedList.First.Value.RunAction();
			}

			public override bool Add(Action _Action)
			{
				if (!IsComposite()) { return false; }

				_Action.mParent = this;
				mActionLinkedList.AddLast(_Action);
				return true;
			}
			public override bool Remove(Action _Action)
			{
				if (!IsComposite()) { return false; }

				if (GetListHead() == null) { return false; }

				return mActionLinkedList.Remove(_Action);
			}
			public override LinkedListNode<Action> GetListHead() { return mActionLinkedList.First; }
			public override bool IsComposite() { return true; }
		}
	}

}
