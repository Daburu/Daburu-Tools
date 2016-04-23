using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class ActionParallel : Action
		{
			public override void SetUnscaledDeltaTime(bool _bIsUnscaledDeltaTime)
			{
				base.SetUnscaledDeltaTime(_bIsUnscaledDeltaTime);

				// Set the same for children actions.
				for (LinkedListNode<Action> node = mActionLinkedList.First; node != null; node = node.Next)
					node.Value.SetUnscaledDeltaTime(_bIsUnscaledDeltaTime);
			}

			private LinkedList<Action> mActionLinkedList;
			private LinkedList<Action> mStorageLinkedList;	// Used for resetting.

			public ActionParallel()
			{
				mActionLinkedList = new LinkedList<Action>();
			}
			public ActionParallel(params Action[] _Actions)
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
					for (LinkedListNode<Action> node = mActionLinkedList.First; node != null; node = node.Next)
						node.Value.RunAction();
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
			public override void StopAction(bool _bSnapToDesired)
			{
				if (!mbIsRunning)
					return;

				// Prevent it from Resetting.
				MakeResettable(false);

				// Use an array because cannot remove node from linkedlist while traversing.
				Action[] actionList = new Action[mActionLinkedList.Count];
				int numActions = 0;

				for (LinkedListNode<Action> node = mActionLinkedList.First; node != null; node = node.Next)
				{
					// Ensure they are all running so that the StopAction can work properly.
					if (node.Value.mbIsRunning == false)
						node.Value.RunAction();

					// Add to array to be used later.
					actionList[numActions] = node.Value;
					numActions++;
				}

				for (int i = 0; i < actionList.Length; i++)
				{
					actionList[i].StopAction(_bSnapToDesired);
				}

				OnActionEnd();
				mParent.Remove(this);
			}



			public override bool Add(Action _Action)
			{
				_Action.mParent = this;
				mActionLinkedList.AddFirst(_Action);
				return true;
			}
			public bool Add(params Action[] _Actions)
			{
				for (int i = 0; i < _Actions.Length; i++)
				{
					_Actions[i].mParent = this;
					mActionLinkedList.AddFirst(_Actions[i]);
				}

				return true;
			}
			public override bool Remove(Action _Action)
			{
				if (GetListHead() == null) { return false; }

				if (mbIsResettable)
				{
					mStorageLinkedList.AddFirst(mActionLinkedList.Find(_Action).Value);
				}
				return mActionLinkedList.Remove(_Action);
			}
			public override LinkedListNode<Action> GetListHead() { return mActionLinkedList.First; }
			public override bool IsComposite() { return true; }
		}
	}

}
