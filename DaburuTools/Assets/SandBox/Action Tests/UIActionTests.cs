using UnityEngine;
using System.Collections;
using DaburuTools;
using DaburuTools.Action;
using UnityEngine.UI;

public class UIActionTests : MonoBehaviour
{
	private Image mImage;
	private RawImage mRawImage;

	void Awake()
	{
		mImage = transform.FindChild("ImageTest").GetComponent<Image>();
		mRawImage = transform.FindChild("RawImageTest").GetComponent<RawImage>();

		ImageAlphaToAction fadeImage = new ImageAlphaToAction(mImage, Graph.InverseExponential, 0.0f, 5.0f);
		ActionHandler.RunAction(fadeImage);

		RawImageAlphaToAction fadeRawImage = new RawImageAlphaToAction(mRawImage, Graph.Exponential, 0.0f, 5.0f);
		ActionHandler.RunAction(fadeRawImage);
	}
}
