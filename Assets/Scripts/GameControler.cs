using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControler : MonoBehaviour
{
    public static GameControler instance = null;

    public MetalBarController metalBallController;
    public Ball ballReference;

    public List<GameObject> holes;
    public GameObject holeIndicatorPrefab;

    public List<GameObject> holeIndicatorList;

    float score = 0;
    float bonusScore = 100;

    int currentBallNumber = 1;
    public int numberOfBallsPerGame = 3;

    int currentHoleIndex = 0;

    public float timePerDecrement = 3.0f;
    public float bonusScoreDecrement = 10;

    public bool gameCompletedState = false;
    public bool gameOverState = false;

    public Text ballText;
    public Text scoreText;
    public Text bonusText;
    public Text gameOverText;

    public Color textLitColor;
    public Color texctUnlitColor;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void UpdateUI()
    {
        ballText.text = currentBallNumber.ToString();
        scoreText.text = score.ToString();
        bonusText.text = bonusScore.ToString();
    }

    private void RecalculateScore()
    {
        score += bonusScore;
        UpdateUI();
    }

    private void DecreaseBonusScore()
    {
        if(bonusScore >= bonusScoreDecrement)
        {
            bonusScore -= bonusScoreDecrement;
            UpdateUI();                
        }
        else
        {
            bonusScore = 0;
            UpdateUI();
        }
    }

    public GameObject GetCurrentHole()
    {
        return holes[currentHoleIndex];
    }
    public void ResetBall()
    {
        ballReference.ResetBallPosition();
    }

    public void NextHole()
    {
        currentHoleIndex++;
        bonusScore = (currentHoleIndex + 1) * 100.0f;
        UpdateUI();
    }

    void ResetGame()
    {
        currentBallNumber = 1;
        score = 0;
        currentHoleIndex = 0;
        bonusScore = (currentHoleIndex + 1) * 100.0f;
        gameCompletedState = false;
        UpdateUI();
    }

    public void StartGame()
    {
        ResetGame();

        metalBallController.MoveBarToBottomPositionFunction();
    }

    public void ReadyForNextHole()
    {
        holeIndicatorList[currentHoleIndex].GetComponent<HoleIndicator>().StartPulsating();

        InvokeRepeating(nameof(DecreaseBonusScore), timePerDecrement, timePerDecrement);

        UpdateUI();
    }

    public void HandleBallInHole(bool rightHole)
    {
        CancelInvoke(nameof(DecreaseBonusScore));
        if(rightHole == true)
        {
            holeIndicatorList[currentHoleIndex].GetComponent<HoleIndicator>().EndPulsating();
            RecalculateScore();

            if(currentHoleIndex < holes.Count - 1)
            {
                NextHole();
            }
            else
            {
                GameCompleted();
            }

        }
        else
        {
            currentBallNumber++;
            if(currentBallNumber > numberOfBallsPerGame)
            {
                GameOver();
                holeIndicatorList[currentHoleIndex].GetComponent<HoleIndicator>().EndPulsating();
            }
        }
        metalBallController.MoveBarToBottomPositionFunction();
    }

    public void GameOver()
    {
        gameOverState = true;
        gameOverText.color = textLitColor;

    }

    private void GameCompleted()
    {
        gameCompletedState = true;
        Debug.Log("Has completado el juego");
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }

    [ContextMenu("Asigna Hoyos Indicadores y Numeros")]
    void AsignHoleIndicator()
    {
        foreach (GameObject g in holeIndicatorList)
        {
            DestroyImmediate(g);
        }
        holeIndicatorList.Clear();

        for(int i = 0; i < holes.Count; i++)
        {
            GameObject holeIndicatorInstantiated = Instantiate(holeIndicatorPrefab, holes[i].transform.position, Quaternion.identity);
            holeIndicatorList.Add(holeIndicatorInstantiated);

            holeIndicatorInstantiated.GetComponent<HoleIndicator>().SetHoleNumber(i + 1);
        }
    }
}
