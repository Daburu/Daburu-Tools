using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class ActionParallel : Action
		{
			private LinkedList<Action> mActionLinkedList;

			public ActionParallel()
			{
				mActionLinkedList = new LinkedList<Action>();
			}

			public ActionParallel(Action[] _Actions)
			{
				for (int i = 0; i < _Actions.Length; i++)
				{
					if (_Actions[i] == null) continue;
					mActionLinkedList.AddFirst(_Actions[i]);
				}
			}

			public override void RunAction()
			{
				base.RunAction();

				if (mParent != null && mActionLinkedList.Count == 0)
					mParent.Remove(this);

				for (LinkedListNode<Action> node = mActionLinkedList.First; node != null; node = node.Next)
				{
					node.Value.RunAction();
				}
			}

			public override bool Add(Action _Action)
			{
				if (!IsComposite()) { return false; }

				_Action.mParent = this;
				mActionLinkedList.AddFirst(_Action);
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
