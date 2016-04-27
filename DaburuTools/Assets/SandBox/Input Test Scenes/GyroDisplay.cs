using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GyroDisplay : MonoBehaviour
{
	public Transform m_GyroTransform;
	public Transform m_TextTransform;

	private Text m_Text;
	private Gyrotation m_gyro;

	void Start()
	{
		m_gyro = m_GyroTransform.GetComponent<Gyrotation>();
		m_Text = m_TextTransform.GetComponent<Text>();
	}

	void Update()
	{
		Quaternion q = m_gyro.Rotation;
		m_Text.text = "(" + Mathf.RoundToInt(q.eulerAngles.x) + "f, " + Mathf.RoundToInt(q.eulerAngles.y) + "f, " + Mathf.RoundToInt(q.eulerAngles.z) + "f)";
	}
}
