using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEditor;

namespace DaburuTools
{
	namespace UI
	{
		[CustomEditor(typeof(GenericMenu))]
		[CanEditMultipleObjects]
		public class GenericMenuInspector : Editor
		{
			private GameObject thisGO { get { return ((GenericMenu)serializedObject.targetObject).gameObject; } }
			private const string kStandardSpritePath           = "UI/Skin/UISprite.psd";
			private const string kBackgroundSpriteResourcePath = "UI/Skin/Background.psd";

			private bool mbNeedUpdateNumberOfButtons;
			private bool mbNeedUpdateButtonOrder;
			private bool mbNeedUpdateButtonTransforms;

			private bool mbNeedUpdateMenuSize;
			private bool mbNeedUpdateButtonFontSize;
			private bool mbNeedUpdateButtonNames;
			private bool mbNeedUpdateButtonEvents;

			private SerializedProperty mSP_MenuSize;
			private SerializedProperty mSP_MenuPaddingTop;
			private SerializedProperty mSP_MenuPaddingBottom;
			private SerializedProperty mSP_ButtonHorzPercent;
			private SerializedProperty mSP_ButtonVertMargin;

			private SerializedProperty mSP_ButtonFontSize;

			private SerializedProperty mSP_ButtonNames;
			private SerializedProperty mSP_ButtonEvents;

			private GUIContent menuSizeContent;
			private GUIContent menuPaddingTopContent;
			private GUIContent menuPaddingBottomContent;
			private GUIContent buttonHorzPercentContent;
			private GUIContent buttonVertMarginContent;
			private GUIContent buttonFontSizeContent;
			private GUIContent forceReconstructButtonContent;
			private GUIContent addButtonContent;
			private GUIContent buttonNameElementContent;
			private GUIContent moveButtonDownContent;
			private GUIContent deleteButtonContent;
			private GUIContent buttonEventElementContent;

			private GUIStyle MarginNMarginStyle;

			private void OnEnable()
			{
				mbNeedUpdateNumberOfButtons		= false;
				mbNeedUpdateButtonOrder			= false;
				mbNeedUpdateButtonTransforms	= false;

				mbNeedUpdateMenuSize			= false;
				mbNeedUpdateButtonFontSize		= false;
				mbNeedUpdateButtonNames			= false;
				mbNeedUpdateButtonEvents		= false;

				mSP_MenuSize 			= serializedObject.FindProperty("mVec2MenuSize");
				mSP_MenuPaddingTop 		= serializedObject.FindProperty("mfMenuPaddingTop");
				mSP_MenuPaddingBottom	= serializedObject.FindProperty("mfMenuPaddingBottom");
				mSP_ButtonHorzPercent 	= serializedObject.FindProperty("mfButtonHorzPercent");
				mSP_ButtonVertMargin	= serializedObject.FindProperty("mfButtonVertMargin");
				mSP_ButtonFontSize 		= serializedObject.FindProperty("mnButtonFontSize");
				mSP_ButtonNames 		= serializedObject.FindProperty("mArrStrButtonNames");
				mSP_ButtonEvents 		= serializedObject.FindProperty("mArrButtonEvents");

				menuSizeContent 			= new GUIContent("Menu Size", "Size of menu in pixels");
				menuPaddingTopContent		= new GUIContent("Top Padding (%)", "Padding for the top of the menu");
				menuPaddingBottomContent	= new GUIContent("Bottom Padding (%)", "Paddingfor the bottom of the menu");
				buttonHorzPercentContent 	= new GUIContent("Horizontal Size (%)", "The percentage size of the button compared to the menu size.");
				buttonVertMarginContent		= new GUIContent("Verticle Margin (%)", "The percentage size of the button verticle padding");
				buttonFontSizeContent 		= new GUIContent("Font Size", "Size of text on buttons");
				forceReconstructButtonContent 	= new GUIContent("Force Reconstruct", "Destroys the currently created menu and re-creates it.");
				addButtonContent 			= new GUIContent("Add Menu Button", "Adds one button to the menu.");
				buttonNameElementContent 	= new GUIContent("Button X", "The display text of the button");
				moveButtonDownContent 		= new GUIContent("\u21b4", "Move this button down.");
				deleteButtonContent 		= new GUIContent("-", "Delete this button.");
				buttonEventElementContent 	= new GUIContent("OnClick", "Functions to be executed when the menu button is clicked");

				MarginNMarginStyle = new GUIStyle();
				MarginNMarginStyle.margin = MarginNMarginStyle.padding = new RectOffset(0, 0, 7, 7);
			}

			public override void OnInspectorGUI()
			{
				serializedObject.Update();

				EditorGUILayout.Space();
				EditorGUILayout.LabelField("General Options", EditorStyles.boldLabel);

				// Menu Size Field.
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(mSP_MenuSize, menuSizeContent);
				if (EditorGUI.EndChangeCheck())
					mbNeedUpdateMenuSize = true;

				// Menu Top and Bottom Padding Fields
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.Slider(mSP_MenuPaddingTop, 0.0f, 1.0f, menuPaddingTopContent);
				EditorGUILayout.Slider(mSP_MenuPaddingBottom, 0.0f, 1.0f, menuPaddingBottomContent);

				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Button Layout Options", EditorStyles.boldLabel);

				// Button Horizontal Percentage Field.
				EditorGUILayout.Slider(mSP_ButtonHorzPercent, 0.0f, 1.0f, buttonHorzPercentContent);

				// Button Verticle Margin Field.
				EditorGUILayout.Slider(mSP_ButtonVertMargin, 0.0f, 0.1f, buttonVertMarginContent);
				if (EditorGUI.EndChangeCheck())
					mbNeedUpdateButtonTransforms = true;

				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Button Content Options", EditorStyles.boldLabel);

				// Button Font Size Field.
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(mSP_ButtonFontSize, buttonFontSizeContent);
				if (EditorGUI.EndChangeCheck())
					mbNeedUpdateButtonFontSize = true;

				// Menu Button order, name, and events.
				DrawMenuButtonProperties();

				EditorGUILayout.Space();

				// Danger Zone (Force Reconstruct, Deleting, etc.)
				Rect dangerZoneRect = EditorGUILayout.BeginVertical(MarginNMarginStyle);
				EditorGUI.DrawRect(dangerZoneRect, Color.grey);
				DTEditorUtility.ShrinkRectByOne(ref dangerZoneRect);
				EditorGUI.DrawRect(dangerZoneRect, Color.red);
				EditorGUILayout.LabelField("DANGER ZONE", EditorStyles.boldLabel);
				if (GUILayout.Button(forceReconstructButtonContent))
					ConstructCanvasFromScratch();
				GUILayout.EndVertical();

				serializedObject.ApplyModifiedProperties();

				// Once inspector is done drawing and applying modifications,
				// Ensure that a Canvas and EventSystem are created.
				// If there is no Canvas and/or EventSystem, EnsureMenuCanvas will create one.
				EnsureMenuCanvas();
				UpdateCanvas();	// Then Update the Canvas if needed.
			}

			private void DrawMenuButtonProperties()
			{
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Menu Button Properties:", EditorStyles.boldLabel);

				// Draw array of menu button names and events.
				for (int index = 0; index < mSP_ButtonNames.arraySize; index++)
				{
					// Div-like wrapper for each button data.
					Rect buttonDataRect = EditorGUILayout.BeginVertical(MarginNMarginStyle);
					EditorGUI.DrawRect(buttonDataRect, Color.grey);
					DTEditorUtility.ShrinkRectByOne(ref buttonDataRect);
					EditorGUI.DrawRect(buttonDataRect, Color.white);

					DrawButtonDataAtIndex(index);

					EditorGUILayout.EndVertical();
				}

				// Button to add menu buttons.
				if (GUILayout.Button(addButtonContent))
				{
					mSP_ButtonNames.InsertArrayElementAtIndex(mSP_ButtonNames.arraySize);
					mSP_ButtonEvents.InsertArrayElementAtIndex(mSP_ButtonEvents.arraySize);

					mbNeedUpdateNumberOfButtons = true;
				}
			}

			private void DrawButtonDataAtIndex(int _index)
			{
				bool bNeedDeleteIndex = false;

				EditorGUILayout.BeginHorizontal();

				// Menu Button Name Field
				EditorGUI.BeginChangeCheck();
				buttonNameElementContent.text = "Button " + _index;
				EditorGUILayout.PropertyField(mSP_ButtonNames.GetArrayElementAtIndex(_index), buttonNameElementContent);
				if (EditorGUI.EndChangeCheck())
					mbNeedUpdateButtonNames = true;

				// Button to move down menu buttons.
				if (GUILayout.Button(moveButtonDownContent, GUILayout.Width(20f), GUILayout.Height(14f)))
				{
					if ((_index + 1) < mSP_ButtonNames.arraySize)
					{
						mSP_ButtonNames.MoveArrayElement(_index, _index + 1);
						mSP_ButtonEvents.MoveArrayElement(_index, _index + 1);

						mbNeedUpdateButtonOrder = true;
					}
					else
					{
						Debug.LogWarning("Button is at bottom. Cannot move further down");
					}
				}

				// Button to delete menu buttons.
				if (GUILayout.Button(deleteButtonContent, GUILayout.Width(20f), GUILayout.Height(14f)))
				{
					bNeedDeleteIndex = true;
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();

				// OnClick Events Field
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(mSP_ButtonEvents.GetArrayElementAtIndex(_index), buttonEventElementContent);
				if (EditorGUI.EndChangeCheck())
					mbNeedUpdateButtonEvents = true;

				// Logic to handle deletion of buttons in both arrays.
				if (bNeedDeleteIndex)
				{
					// The double delete is because of the way unity handles deletion in this case.
					// If it references something, it will clear the reference, but not remove the element from the list.
					int nameOldSize = mSP_ButtonNames.arraySize;
					mSP_ButtonNames.DeleteArrayElementAtIndex(_index);
					if (mSP_ButtonNames.arraySize == nameOldSize)
						mSP_ButtonNames.DeleteArrayElementAtIndex(_index);

					int eventOldSize = mSP_ButtonEvents.arraySize;
					mSP_ButtonEvents.DeleteArrayElementAtIndex(_index);
					if (mSP_ButtonEvents.arraySize == eventOldSize)
						mSP_ButtonEvents.DeleteArrayElementAtIndex(_index);

					mbNeedUpdateNumberOfButtons = true;
				}
			}

			#region Construction and Destruction Methods
			private void EnsureEventSystem()
			{
				if (FindObjectOfType<EventSystem>() == null)
				{
//					Debug.Log("EVENT SYSTEM NOT FOUND");
					GameObject eventSystem = new GameObject("EventSystem", typeof(EventSystem));
					eventSystem.AddComponent<StandaloneInputModule>();
				}
				else
				{
//					Debug.Log("Event System Found");
				}
			}

			private void EnsureMenuCanvas()
			{
				Canvas[] canvas = thisGO.transform.GetComponentsInChildren<Canvas>();
				if (canvas.Length == 0) // No child Canvas
					ConstructCanvasFromScratch();

				EnsureEventSystem();
			}

			private void ConstructCanvasFromScratch()
			{
				// Destroy previous canvas.
				DestroyCanvas();

				// Child Canvas Object
				GameObject genericMenuCanvas = new GameObject("Generic Menu Canvas");
				genericMenuCanvas.transform.SetParent(thisGO.transform);

				genericMenuCanvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
				genericMenuCanvas.AddComponent<CanvasScaler>();
				genericMenuCanvas.AddComponent<GraphicRaycaster>();

				// Panel
				GameObject genericMenuPanel = new GameObject("Generic Menu Panel");
				genericMenuPanel.transform.SetParent(genericMenuCanvas.transform);

				Image genericMenuPanelImage = genericMenuPanel.AddComponent<Image>();
				genericMenuPanelImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpriteResourcePath);
				genericMenuPanelImage.type = Image.Type.Sliced;

				RectTransform genericMenuPanelRT = genericMenuPanel.GetComponent<RectTransform>();
				genericMenuPanelRT.anchoredPosition = Vector2.zero;
				genericMenuPanelRT.sizeDelta = mSP_MenuSize.vector2Value;

				// Buttons
				int numButtons = mSP_ButtonNames.arraySize;
				float buttonVertBlockPercentage = (1.0f - mSP_MenuPaddingTop.floatValue - mSP_MenuPaddingBottom.floatValue) / numButtons;
				for (int i = 0; i < numButtons; i++)
				{
					AddGenericMenuButton(genericMenuPanel.transform, i, numButtons, buttonVertBlockPercentage);
				}
			}

			private void DestroyCanvas()
			{
				while (thisGO.transform.childCount > 0)
				{
					DTEditorUtility.DestroyImmediateAndAllChildren(thisGO.transform.GetChild(0).gameObject);
				}
			}

			private void AddGenericMenuButton(Transform _parent, int _index, int _numButtons, float _buttonVertBlockPercentage)
			{
				// Button Game Object
				GameObject genericMenuButton = new GameObject("Generic Menu Button");
				genericMenuButton.transform.SetParent(_parent);

				Image genericMenuButtonImage = genericMenuButton.AddComponent<Image>();
				genericMenuButtonImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
				genericMenuButtonImage.type = Image.Type.Sliced;

				Button genericMenuButtonButton = genericMenuButton.AddComponent<Button>();
				ColorBlock colors = (genericMenuButtonButton as Selectable).colors;
				colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
				colors.pressedColor     = new Color(0.698f, 0.698f, 0.698f);
				colors.disabledColor    = new Color(0.521f, 0.521f, 0.521f);
				genericMenuButtonButton.onClick = thisGO.GetComponent<GenericMenu>().mArrButtonEvents[_index];

				RectTransform genericMenuButtonRT = genericMenuButton.GetComponent<RectTransform>();
				Vector2 anchorMin = Vector2.zero;
				Vector2 anchorMax = Vector2.one;
				// Settle anchor X minmax.
				anchorMin.x = (1.0f - mSP_ButtonHorzPercent.floatValue) / 2.0f;
				anchorMax.x = 1.0f - anchorMin.x;
				// Settle anchor y minmax.
				anchorMax.y = 1.0f - mSP_MenuPaddingTop.floatValue - mSP_ButtonVertMargin.floatValue - (_buttonVertBlockPercentage * _index);
				anchorMin.y = anchorMax.y - _buttonVertBlockPercentage + (mSP_ButtonVertMargin.floatValue * 2);
				genericMenuButtonRT.anchorMin = anchorMin;
				genericMenuButtonRT.anchorMax = anchorMax;
				genericMenuButtonRT.sizeDelta = Vector2.zero;
				genericMenuButtonRT.anchoredPosition = Vector2.zero;

				// Button Text Child Object
				GameObject childText = new GameObject("Text");
				GameObjectUtility.SetParentAndAlign(childText, genericMenuButton);

				Text text = childText.AddComponent<Text>();
				text.text = thisGO.GetComponent<GenericMenu>().mArrStrButtonNames[_index];
				text.color = Color.black;
				text.alignment = TextAnchor.MiddleCenter;

				text.rectTransform.anchorMin = Vector2.zero;
				text.rectTransform.anchorMax = Vector2.one;
				text.rectTransform.sizeDelta = Vector2.zero;
				text.rectTransform.anchoredPosition = Vector2.zero;
			}
			#endregion

			#region Update Functions
			private void UpdateCanvas()
			{
				if (mbNeedUpdateNumberOfButtons)
				{
					UpdateNumberOfButtons();
					mbNeedUpdateNumberOfButtons = false;
				}

				if (mbNeedUpdateButtonOrder)
				{
					UpdateButtonOrder();
					mbNeedUpdateButtonOrder = false;
				}

				if (mbNeedUpdateButtonTransforms)
				{
					UpdateButtonTransforms();
					mbNeedUpdateButtonTransforms = false;
				}



				if (mbNeedUpdateMenuSize)
				{
					UpdateMenuSize();
					mbNeedUpdateMenuSize = false;
				}

				if (mbNeedUpdateButtonFontSize)
				{
					UpdateButtonFontSize();
					mbNeedUpdateButtonFontSize = false;
				}

				if (mbNeedUpdateButtonNames)
				{
					UpdateButtonNames();
					mbNeedUpdateButtonNames = false;
				}

				if (mbNeedUpdateButtonEvents)
				{
					UpdateButtonEvents();
					mbNeedUpdateButtonEvents = false;
				}
			}

			private void UpdateNumberOfButtons()
			{
				if (thisGO.transform.GetChild(0).GetChild(0).childCount < thisGO.GetComponent<GenericMenu>().mArrStrButtonNames.Length)
				{
					// Add new Button
					int numButtons = mSP_ButtonNames.arraySize;
					float buttonVertBlockPercentage = (1.0f - mSP_MenuPaddingTop.floatValue - mSP_MenuPaddingBottom.floatValue) / numButtons;
					int index = numButtons - 1;
					Transform parent = thisGO.transform.GetChild(0).GetChild(0);
					AddGenericMenuButton(parent, index, numButtons, buttonVertBlockPercentage);
				}
				else if (thisGO.transform.GetChild(0).GetChild(0).childCount > thisGO.GetComponent<GenericMenu>().mArrStrButtonNames.Length)
				{
					// Remove Button
					Transform panelTransform = thisGO.transform.GetChild(0).GetChild(0);
					int numButtons = mSP_ButtonNames.arraySize;
					for (int i = 0; i < numButtons; i++)
					{
						Text genericMenuButtonText = panelTransform.GetChild(i).GetChild(0).GetComponent<Text>();
						if (genericMenuButtonText.text != thisGO.GetComponent<GenericMenu>().mArrStrButtonNames[i])
						{
							numButtons = -1;
							DTEditorUtility.DestroyImmediateAndAllChildren(panelTransform.GetChild(i).gameObject);
							break;
						}
					}

					// Its the last buton that should be deleted.
					if (numButtons > 0)
					{
						DTEditorUtility.DestroyImmediateAndAllChildren(panelTransform.GetChild(mSP_ButtonNames.arraySize).gameObject);
					}
				}
				else
				{
					Debug.LogWarning("No change in number of buttons");
					return;
				}

				// Update all the button positions.
				UpdateButtonTransforms();
			}

			private void UpdateButtonOrder()
			{
				Transform panelTransform = thisGO.transform.GetChild(0).GetChild(0);
				int numButtons = mSP_ButtonNames.arraySize - 1;	// Button to re-order only detected till second last index.
				for (int i = 0; i < numButtons; i++)
				{
					Text genericMenuButtonText = panelTransform.GetChild(i).GetChild(0).GetComponent<Text>();
					if (genericMenuButtonText.text != thisGO.GetComponent<GenericMenu>().mArrStrButtonNames[i])
					{
						// Swap with button in next index
						Transform topButton = panelTransform.GetChild(i);
						topButton.SetSiblingIndex(topButton.GetSiblingIndex() + 1);

						UpdateButtonTransforms();
						break;
					}
				}
			}

			private void UpdateButtonTransforms()
			{
				Transform panelTransform = thisGO.transform.GetChild(0).GetChild(0);
				int numButtons = mSP_ButtonNames.arraySize;
				float buttonVertBlockPercentage = (1.0f - mSP_MenuPaddingTop.floatValue - mSP_MenuPaddingBottom.floatValue) / numButtons;
				for (int i = 0; i < numButtons; i++)
				{
					RectTransform genericMenuButtonRT = panelTransform.GetChild(i).GetComponent<RectTransform>();
					Vector2 anchorMin = Vector2.zero;
					Vector2 anchorMax = Vector2.one;
					// Settle anchor X minmax.
					anchorMin.x = (1.0f - mSP_ButtonHorzPercent.floatValue) / 2.0f;
					anchorMax.x = 1.0f - anchorMin.x;
					// Settle anchor y minmax.
					anchorMax.y = 1.0f - mSP_MenuPaddingTop.floatValue - mSP_ButtonVertMargin.floatValue - (buttonVertBlockPercentage * i);
					anchorMin.y = anchorMax.y - buttonVertBlockPercentage + (mSP_ButtonVertMargin.floatValue * 2);
					genericMenuButtonRT.anchorMin = anchorMin;
					genericMenuButtonRT.anchorMax = anchorMax;
					genericMenuButtonRT.sizeDelta = Vector2.zero;
					genericMenuButtonRT.anchoredPosition = Vector2.zero;
				}
			}

			private void UpdateMenuSize()
			{
				// Check for existance of panel.
				if (thisGO.transform.GetChild(0) == null || 
					thisGO.transform.GetChild(0).GetChild(0) == null)
				{
					Debug.LogError("Generic Menu Panel not found.");
					return;
				}

				RectTransform panelSize = thisGO.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>();
				panelSize.sizeDelta = mSP_MenuSize.vector2Value;
			}

			private void UpdateButtonFontSize()
			{
				Transform panelTransform = thisGO.transform.GetChild(0).GetChild(0);
				int numButtons = mSP_ButtonNames.arraySize;
				for (int i = 0; i < numButtons; i++)
				{
					Text genericMenuButtonText = panelTransform.GetChild(i).GetChild(0).GetComponent<Text>();
					genericMenuButtonText.fontSize = thisGO.GetComponent<GenericMenu>().mnButtonFontSize;
				}
			}

			private void UpdateButtonNames()
			{
				Transform panelTransform = thisGO.transform.GetChild(0).GetChild(0);
				int numButtons = mSP_ButtonNames.arraySize;
				for (int i = 0; i < numButtons; i++)
				{
					Text genericMenuButtonText = panelTransform.GetChild(i).GetChild(0).GetComponent<Text>();
					genericMenuButtonText.text = thisGO.GetComponent<GenericMenu>().mArrStrButtonNames[i];
				}
			}

			private void UpdateButtonEvents()
			{
				Transform panelTransform = thisGO.transform.GetChild(0).GetChild(0);
				int numButtons = mSP_ButtonNames.arraySize;
				for (int i = 0; i < numButtons; i++)
				{
					Button genericMenuButtonButton = panelTransform.GetChild(i).GetComponent<Button>();
					genericMenuButtonButton.onClick = thisGO.GetComponent<GenericMenu>().mArrButtonEvents[i];
				}
			}
			#endregion
		}
	}
}
