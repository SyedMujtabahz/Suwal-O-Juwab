using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Reflection;

public class Quiz : MonoBehaviour
{
    [Header("Question")]
    [SerializeField]
    TextMeshProUGUI questionText;
    [SerializeField]
    List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO Currentquestion;

    [Header("Answers")]
    [SerializeField]
    GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;

    [Header("ButtonColors")]
    [SerializeField]
    Sprite DefaultAnswerSprite;
    [SerializeField]
    Sprite CorrectAnswerSprite;

    [Header("Timer")]
    [SerializeField]
    Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField]
    TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("ProgressBar")]
    [SerializeField]
    Slider ProgressBar;

    public bool isComplete;

    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreText.text = "Score:";
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        ProgressBar.maxValue = questions.Count;
        ProgressBar.value = 0;
        //GetNextQuestion();
        //DisplayQuestion();

    }

    private void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        if(timer.loadNextQuestion)
        {
            if (ProgressBar.value == ProgressBar.maxValue)
            {
                isComplete = true;
                return;
            }

            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if(!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";
    }

    void DisplayAnswer(int index)
    {
        Image buttonImage;
        if (index == Currentquestion.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = CorrectAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            correctAnswerIndex = Currentquestion.GetCorrectAnswerIndex();
            string correctAnswer = Currentquestion.GetAnswer(correctAnswerIndex);
            questionText.text = "Sorry, you have Skill Issue, correct answer was;\n" + correctAnswer;
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = CorrectAnswerSprite;
        }
    }

    void GetNextQuestion()
    {
        if(questions.Count > 0)
        {
            SetButtonState(true);
            SetDefaultButtonSprite();
            GetRandomQuestion();
            DisplayQuestion();
            ProgressBar.value++;
            scoreKeeper.IncrementQuestionsSeen();
        }
       
    }

    void GetRandomQuestion()
    {
        int index = UnityEngine.Random.Range(0, questions.Count);
        Currentquestion = questions[index];

        if(questions.Contains(Currentquestion))
        {
            questions.Remove(Currentquestion);
        }
    }
    void DisplayQuestion()
    {
        questionText.text = Currentquestion.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = Currentquestion.GetAnswer(i);
        }
    }

    void SetButtonState(bool state)
    {
        for(int i = 0;i < answerButtons.Length;i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    void SetDefaultButtonSprite()
    {
        for (int i = 0;i < answerButtons.Length; i++)
        {
            Image button = answerButtons[i].GetComponent<Image>();
            button.sprite = DefaultAnswerSprite;
        }
    }

}
