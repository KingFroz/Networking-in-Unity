
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Text))]
public class FPSCounter : MonoBehaviour {
    const float fpsMeasurePeriod = 0.5f;
    private int m_FpsAccumulator = 0;
    private float m_FpsNextPeriod = 0;
    private int m_CurrentFps;

    private Text m_Text;
    const string display = "{0} fps";

    void Start () {
        m_Text = GetComponent<Text>();
        m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
    }
	
	void Update () {
        countingFPS();
    }

    private void countingFPS()
    {
        //Measure average frames per second
        m_FpsAccumulator++;
        if (Time.realtimeSinceStartup > m_FpsNextPeriod)
        {
            m_CurrentFps = (int)(m_FpsAccumulator / fpsMeasurePeriod);
            m_FpsAccumulator = 0;
            m_FpsNextPeriod += fpsMeasurePeriod;
            m_Text.text = string.Format(display, m_CurrentFps);
        }
    }
}
