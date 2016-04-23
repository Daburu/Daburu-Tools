using System.Collections.Generic;

namespace DaburuTools
{
	namespace Action
	{
		public class Action
		{
			// Unscaled Delta Time Settings
			protected bool mbIsUnscaledDeltaTime = false;
			public virtual void SetUnscaledDeltaTime(bool _bIsUnscaledDeltaTime)	{ mbIsUnscaledDeltaTime = _bIsUnscaledDeltaTime; }
			protected float ActionDeltaTime(bool _bIsUnscaledDeltaTime)
			{
				if (_bIsUnscaledDeltaTime)
					return UnityEngine.Time.unscaledDeltaTime;
				else
					return UnityEngine.Time.deltaTime;
			}

			public Action mParent = null;
			public bool mbIsRunning = false;
			public bool mbIsResettable = false;
			public OnActionBeginDelegate OnActionStart = EmptyFunc;
			public OnActionEndDelegate OnActionFinish = EmptyFunc;

			// Optional.
			public delegate void OnActionBeginDelegate();
			public delegate void OnActionEndDelegate();
			protected virtual void OnActionBegin()	{ mbIsRunning = true; OnActionStart(); }
			protected virtual void OnActionEnd() 	{ mbIsRunning = false; OnActionFinish(); }
			private static void EmptyFunc() {}

			// All must implement.
			public virtual void RunAction() 	{ if (!mbIsRunning) OnActionBegin(); }
			public virtual void MakeResettable(bool _bIsResettable)	{ mbIsResettable = _bIsResettable; }
			public virtual void Reset()	{}
			public virtual void StopAction(bool _bSnapToDesired = false)	{}
			// Do not override ActionRecurisve.
			public void StopActionRecursive(bool _bSnapToDesired = false)
			{
				if (!mbIsRunning)
					return;

				// Stop itself.
				StopAction(_bSnapToDesired);

				if (mParent != null)
					mParent.StopActionRecursive(_bSnapToDesired);
			}

			// Leaves do not need to override these functions.
			public virtual bool Add(Action _Action) 	{ return false; }
			public virtual bool Remove(Action _Action) 	{ return false; }
			public virtual LinkedListNode<Action> GetListHead() { return null; }
			public virtual bool IsComposite() 	{ return false; }
		}
	}

}
