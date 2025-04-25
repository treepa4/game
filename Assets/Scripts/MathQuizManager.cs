using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MathQuizManager : MonoBehaviour
{
    [SerializeField]
    GameObject quizPanel;

    [SerializeField]
    Text questionText;
    
    [SerializeField]
    Button[] answerButtons;
    
    [SerializeField]
    int rewardCoins = 10;

    private int correctAnswer;
    private int playerCoins = 0;

    private Manager Manager;
    void Start()
    {
        quizPanel.SetActive(false); // изначально скрыт
        Manager = FindObjectOfType<Manager>();
    }

    public void ToggleQuizPanel()
{
    bool isActive = quizPanel.activeSelf;
    quizPanel.SetActive(!isActive);

    if (!isActive)
    {
        GenerateQuestion();
    }
}


    public void ShowQuiz()
    {
        quizPanel.SetActive(true);
        GenerateQuestion();
    }

    void GenerateQuestion()
    {
        int a = Random.Range(1, 10);
        int b = Random.Range(1, 10);
        correctAnswer = a + b;
        questionText.text = $"{a} + {b} = ?";

        List<int> options = new List<int> { correctAnswer };
        while (options.Count < 3)
        {
            int wrong = Random.Range(correctAnswer - 5, correctAnswer + 6);
            if (wrong != correctAnswer && !options.Contains(wrong))
            {
                options.Add(wrong);
            }
        }

        // Перемешиваем
        for (int i = 0; i < 3; i++)
        {
            int randIndex = Random.Range(i, 3);
            (options[i], options[randIndex]) = (options[randIndex], options[i]);
        }

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int answer = options[i];
            answerButtons[i].GetComponentInChildren<Text>().text = answer.ToString();
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(answer));
        }
    }

    void OnAnswerSelected(int selected)
    {
    if (selected == correctAnswer)
        {
            if (Manager != null)
            {
                Manager.getMoney(rewardCoins); // <--- вот тут начисляем монеты
            }

            quizPanel.SetActive(false);
            Debug.Log("Правильный ответ!");
        }
        else
        {
            Debug.Log("Неправильный ответ!");
            GenerateQuestion(); // новый пример
        }

    }

    public int GetCoins()
    {
        return playerCoins;
    }

    
}

