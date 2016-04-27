using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GyrotationTest : MonoBehaviour 
{
	public Transform m_TextTransform;
	public Transform m_GyroTransform;

	private Gyrotation m_gyrotation;
	private Text m_Text;

	// Use this for initialization
	void Start () 
	{
		m_Text = m_TextTransform.GetComponent<Text>();
		m_gyrotation = m_GyroTransform.GetComponent<Gyrotation>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Quaternion q = m_gyrotation.Rotation();
		m_Text.text = "(" + Mathf.RoundToInt(q.eulerAngles.x) + "f, " + Mathf.RoundToInt(q.eulerAngles.y) + "f, " + Mathf.RoundToInt(q.eulerAngles.z) + "f)";
	}
}
