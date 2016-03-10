using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class ActionSequence : Action
		{
			private LinkedList<Action> mActionLinkedList;
			private LinkedList<Action> mStorageLinkedList;	// Used for resetting.

			public ActionSequence()
			{
				mActionLinkedList = new LinkedList<Action>();
			}
			public ActionSequence(params Action[] _Actions)
			{
				mActionLinkedList = new LinkedList<Action>();
				for (int i = 0; i < _Actions.Length; i++)
				{
					if (_Actions[i] == null) continue;
					Add(_Actions[i]);
				}
			}



			public override void RunAction()
			{
				base.RunAction();

				if (mActionLinkedList.Count > 0)
				{
					mActionLinkedList.First.Value.RunAction();
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

				for (LinkedListNode<Action> node = mActionLinkedList.First; node != null; node = node.Next)
					node.Value.MakeResettable(_bIsResettable);

				if (_bIsResettable)
					mStorageLinkedList = new LinkedList<Action>();
				else
					mStorageLinkedList = null;
			}
			public override void Reset()
			{
				for (LinkedListNode<Action> node = mStorageLinkedList.First; node != null; node = node.Next)
				{
					node.Value.Reset();
					mActionLinkedList.AddFirst(node.Value);
				}

				mStorageLinkedList.Clear();
				mbIsRunning = false;
			}



			public override bool Add(Action _Action)
			{
				_Action.mParent = this;
				mActionLinkedList.AddLast(_Action);
				return true;
			}
			public bool Add(params Action[] _Actions)
			{
				for (int i = 0; i < _Actions.Length; i++)
				{
					_Actions[i].mParent = this;
					mActionLinkedList.AddLast(_Actions[i]);
				}

				return true;
			}
			public override bool Remove(Action _Action)
			{
				if (GetListHead() == null) { return false; }

				if (mbIsResettable)
				{
					mStorageLinkedList.AddFirst(mActionLinkedList.First.Value);
				}
				return mActionLinkedList.Remove(_Action);
			}
			public override LinkedListNode<Action> GetListHead() { return mActionLinkedList.First; }
			public override bool IsComposite() { return true; }
		}
	}

}
