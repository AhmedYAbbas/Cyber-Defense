using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private bool _canRaiseEvent;
    private float remainingTime;

    private void Update()
    {
        if (MatchManager.Instance.roundTime > 0)
        {
            MatchManager.Instance.roundTime -= Time.deltaTime;
            _canRaiseEvent = true;

            Debug.Log(MatchManager.Instance.roundTime);
        }
        else
        {
            if (_canRaiseEvent)
            {
                MatchManager.Instance.SwitchSideRaiseEvent();
                _canRaiseEvent = false;
            }
        }

        DisplayTime(MatchManager.Instance.roundTime);
    }

    public void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
