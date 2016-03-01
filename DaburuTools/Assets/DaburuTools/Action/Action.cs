using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class Action
		{
			public Action mParent = null;
			public bool mbIsRunning = false;

			public virtual void OnActionBegin() { mbIsRunning = true; }
			public virtual void OnActionEnd() 	{ mbIsRunning = false; }
			public virtual void RunAction() 	{ if (!mbIsRunning) OnActionBegin(); }

			public virtual bool Add(Action _Action) 	{ return false; }
			public virtual bool Remove(Action _Action) 	{ return false; }
			public virtual LinkedListNode<Action> GetListHead() { return null; }

			public virtual bool IsComposite() 	{ return false; }
		}
	}

}
