using UnityEngine;
using System.Collections;
using DaburuTools;
using DaburuTools.Action;
using UnityEngine.UI;

public class UIActionTests : MonoBehaviour
{
	private Image mImage;

	void Awake()
	{
		mImage = transform.FindChild("ImageTest").GetComponent<Image>();

		ImageAlphaToAction fadeImage = new ImageAlphaToAction(mImage, Graph.InverseExponential, 0.0f, 5.0f);
		ActionHandler.RunAction(fadeImage);
	}
}
