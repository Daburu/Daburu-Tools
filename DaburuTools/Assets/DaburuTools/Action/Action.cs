using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class Action
		{
			public Action mParent = null;
			public bool mbIsRunning = false;
			public bool mbIsResettable = false;

			// Optional.
			public virtual void OnActionBegin() { mbIsRunning = true; }
			public virtual void OnActionEnd() 	{ mbIsRunning = false; }

			// All must implement.
			public virtual void RunAction() 	{ if (!mbIsRunning) OnActionBegin(); }
			public virtual void MakeResettable(bool _bIsResettable)	{ mbIsResettable = _bIsResettable; }
			public virtual void Reset()	{}

			// Leaves do not need to override these functions.
			public virtual bool Add(Action _Action) 	{ return false; }
			public virtual bool Remove(Action _Action) 	{ return false; }
			public virtual LinkedListNode<Action> GetListHead() { return null; }
			public virtual bool IsComposite() 	{ return false; }
		}
	}

}
