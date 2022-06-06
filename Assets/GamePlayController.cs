using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField] int score;
    [SerializeField] int highscore;
    public Color[] template = { new Color32(255, 81, 81, 255), new Color32(255, 129, 82, 255), new Color32(255, 233, 82, 255), new Color32(163, 255, 82, 255), new Color32(82, 207, 255, 255), new Color32(170, 82, 255, 255) };

    private UIController uiController;

    private float time;
    [SerializeField] float timeOfGame;

    [SerializeField] NumberContentController numberContentController;
    [SerializeField] ContentController contentController;

    [SerializeField] List<string> currentArr;
    [SerializeField] int currentUserValue;
    [SerializeField] int leng;

    [SerializeField] int theFirstNumber;
    [SerializeField] int theSecondNumber;
    [SerializeField] int theResultNumber;

    [SerializeField] TextAsset data;
    [SerializeField] string[] listSentences;

    private string[] listWords;

    private int currentMath;

    enum math
    {
        Summation = 0,
        Subtraction = 1
    }

    // Start is called before the first frame update
    void Start()
    {
        uiController = GetComponent<UIController>();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        UpdateSlider();

        if(time < 0)
        {
            GameOver();
        }
    }

    public void UpdateSlider()
    {
        uiController.UpdateSlider(time);
    }

    public void SetSlider()
    {
        uiController.SetSlider(timeOfGame);
    }

    public void OnPressHandle(string value)
    {
        if (currentMath == (int)math.Summation)
        {
            if(currentUserValue == 0 || currentUserValue == 2)
            {
                if (value == currentArr[0] || value == currentArr[2])
                {
                    UpdateInfo(value);
                }
                else
                {
                    GameOver();
                }
            }
            else if (value == currentArr[currentUserValue])
            {
                UpdateInfo(value);
            }
            else
            {
                GameOver();
            }
        }
        else
        {
            if (currentUserValue == 2 || currentUserValue == 4)
            {
                if (value == currentArr[2] || value == currentArr[4])
                {
                    UpdateInfo(value);
                }
                else
                {
                    GameOver();
                }
            }
            else if (value == currentArr[currentUserValue])
            {
                UpdateInfo(value);
            }
            else
            {
                GameOver();
            }
        }
    }

    private void UpdateInfo(string value)
    {
        numberContentController.UpdateInfo(currentUserValue, value);
        currentUserValue++;
        if (currentUserValue >= leng)
        {
            UpdateScore();
            StartCoroutine(StartNextTurn());
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        uiController.GameOver();
    }

    public void UpdateScore()
    {
        score++;
        if(highscore <= score)
        {
            highscore = score;
            PlayerPrefs.SetInt("score", highscore);
            uiController.UpdateHighScore(highscore);
        }
        uiController.UpdateScore(score);
    }

    IEnumerator StartNextTurn()
    {
        yield return new WaitForSeconds(0.5f);
        NextTurn();
    }

    private void ParseData()
    {
        listSentences = data.ToString().Split('\n');
    }

    public void NextTurn()
    {
        // Get random sentence
        currentMath = Random.Range(0, listSentences.Length);

        currentUserValue = 0;

        listWords = listSentences[currentMath].Split(' ');
        leng = listWords.Length;

        currentArr = new List<string>();
        numberContentController.Spaw(leng);

        for(int i = 0; i< leng; i++)
        {
            currentArr.Add(listWords[i]);
        }

        contentController.SpawButton(currentArr);

        time = timeOfGame;
    }

    public void Reset()
    {
        ParseData();
        Time.timeScale = 1;

        time = timeOfGame;
        SetSlider();
        score = 0;
        highscore = PlayerPrefs.GetInt("score");
        uiController.UpdateScore(score);
        uiController.UpdateHighScore(highscore);

        NextTurn();
    }

}
