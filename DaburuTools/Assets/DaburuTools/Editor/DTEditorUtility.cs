using UnityEngine;
using UnityEditor;
using System.Collections;

namespace DaburuTools
{
	public class DTEditorUtility
	{
		/// <summary>
		/// Shrinks the passed in Rect by one unit on all sides. Maintains its position.
		/// Please pass in the Rect by ref.
		/// i.e. ShrinkRectByOne(ref myRect);
		/// </summary>
		public static void ShrinkRectByOne(ref Rect _rect)
		{
			_rect.min += Vector2.one;
			_rect.max -= Vector2.one;
		}

		public static void DestroyImmediateAndAllChildren(GameObject _gameObject)
		{
			while (_gameObject.transform.childCount > 0)
			{
				// Call this method on its children.
				DestroyImmediateAndAllChildren(_gameObject.transform.GetChild(0).gameObject);
			}

			// Finally Destroy itself.
			GameObject.DestroyImmediate(_gameObject);
			return;
		}
	}
}
