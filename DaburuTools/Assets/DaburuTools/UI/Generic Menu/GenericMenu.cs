using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;

namespace DaburuTools
{
	namespace UI
	{
		public class GenericMenu : MonoBehaviour
		{
			public Vector2 mVec2MenuSize = new Vector2(400.0f, 300.0f);
			public float mfMenuPaddingTop = 0.02f;
			public float mfMenuPaddingBottom = 0.02f;
			public float mfButtonHorzPercent = 0.6f;
			public float mfButtonVertMargin = 0.02f;

			public int mnButtonFontSize = 14;

			public string[] mArrStrButtonNames;
			public Button.ButtonClickedEvent[] mArrButtonEvents;
		}
	}
}
