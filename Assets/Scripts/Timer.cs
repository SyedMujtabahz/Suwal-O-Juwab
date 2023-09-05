using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    float timeToAnswer = 30f;

    [SerializeField]
    float timeToShowCorrectAnswer = 10f;

    public bool loadNextQuestion;
    public float fillFraction;
    float timerValue;
    public bool isAnsweringQuestion = false;


    void Update()
    {
        UpdateTimer();
    }


    public void CancelTimer()
    {
        timerValue = 0;
    }
    void UpdateTimer()
    {
        timerValue -= Time.deltaTime;

        if (isAnsweringQuestion)
        {
            if (timerValue > 0)
            {
                fillFraction = timerValue / timeToAnswer;
            }
            else
            {
                isAnsweringQuestion=false;
                timerValue = timeToShowCorrectAnswer;
            }
        }
        else
        {
            if (timerValue > 0)
            {
                fillFraction = timerValue / timeToShowCorrectAnswer;
            }
            else
            {
                isAnsweringQuestion = true;
                timerValue = timeToAnswer;
                loadNextQuestion = true;
            }
        }

    }
}
