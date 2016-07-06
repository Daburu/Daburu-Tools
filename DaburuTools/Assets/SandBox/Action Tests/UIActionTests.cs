using UnityEngine;
using System.Collections;
using DaburuTools;
using DaburuTools.Action;
using UnityEngine.UI;

public class UIActionTests : MonoBehaviour
{
	private Image mImage;
	private RawImage mRawImage;
	private Text mText;
	private CanvasGroup mCanvasGroup;

	void Awake()
	{
		mImage = transform.FindChild("ImageTest").GetComponent<Image>();
		mRawImage = transform.FindChild("RawImageTest").GetComponent<RawImage>();
		mText = transform.FindChild("TextTest").GetComponent<Text>();
		mCanvasGroup = transform.FindChild("CanvasGroupTest").GetComponent<CanvasGroup>();

		ImageAlphaToAction fadeImage = new ImageAlphaToAction(mImage, Graph.InverseExponential, 0.0f, 5.0f);
		ActionHandler.RunAction(fadeImage);

		RawImageAlphaToAction fadeRawImage = new RawImageAlphaToAction(mRawImage, Graph.Exponential, 0.0f, 5.0f);
		ActionHandler.RunAction(fadeRawImage);

		TextAlphaToAction fadeText = new TextAlphaToAction(mText, 0.0f, 2.5f);
		ActionHandler.RunAction(fadeText);

		CanvasGroupAlphaToAction canvasGroupTest = new CanvasGroupAlphaToAction(mCanvasGroup, 0.0f, 1.0f);
		ActionHandler.RunAction(canvasGroupTest);
	}
}
