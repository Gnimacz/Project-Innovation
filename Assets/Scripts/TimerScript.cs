using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float currentTime = 0;
    [SerializeField] float startTime = 600;
    bool hasBeenInvoked = false;
    public delegate void OnTimerEnd();
    public static OnTimerEnd OnTimerEndEvent;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            currentTime = 0;
        }
        UpdateTimer(currentTime);
    }

    void UpdateTimer(float timeLeft)
    {
        if (timeLeft < 0)
        {
            timeLeft = 0;

            if (!hasBeenInvoked)
            {
                OnTimerEndEvent?.Invoke();
                hasBeenInvoked = true;
            }
        }

        float minutes = Mathf.FloorToInt(timeLeft / 60);
        float seconds = Mathf.FloorToInt(timeLeft % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
