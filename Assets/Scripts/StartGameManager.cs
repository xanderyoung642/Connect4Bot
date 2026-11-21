using Unity.VisualScripting;
using UnityEngine;

public class StartGameManager : MonoBehaviour
{
    [Header("Arduino Connector")]
    public GameObject arduinoConnector;
    public GameObject arduinoButtonParent;
    public bool arduinoConnectorOn;
    //make a ui checkbox that turns on and off the arduino connector

    [Header("Game Mode")]
    public int gameMode;
    //0 = HvH 1 = HvC 2 = CvC
    public bool humanR;

    [Header("Red AI Difficulty")]
    public GameObject[] rAiDifficultyObjects;
    public int rAiDifficultySelected;

    [Header("Yellow AI Difficulty")]
    public GameObject[] yAiDifficultyObjects;
    public int yAiDifficultySelected;
    //0 = baby (random) 1 = easy (random but it blocks) 2 = medium (barely trained AI)
    //3 = hard (pretty well trained but not perfect AI) 4 = super hard (basically perfect AI)

    [Header("Canvases")]
    public GameObject startGameCanvas;
    public GameObject redWinCanvas;
    public GameObject yellowWinCanvas;
    public GameObject drawCanvas;

    //first menu: pick game mode and turn arduino on and off
    //second menu:
        //if HvH go back and start game
        //if HvC go back and select R/Y
            //third menu:
                //go back and select difficulty
                    //fourth menu:
                        //go back and start game
        //if CvC go back and select R Difficulty
            //third menu:
                //go back and select Y Difficulty
                    //fourth menu:
                        //go back and start game

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        redWinCanvas.SetActive(false);
        yellowWinCanvas.SetActive(false);
        drawCanvas.SetActive(false);
        startGameCanvas.SetActive(true);

        for (int i = 0; i < rAiDifficultyObjects.Length; i++)
        {
            rAiDifficultyObjects[i].SetActive(false);
        }

        for (int i = 0; i < yAiDifficultyObjects.Length; i++)
        {
            yAiDifficultyObjects[i].SetActive(false);
        }

        gameMode = 0;
    }

    public void ArduinoConnectionOnOff(bool on)
    {
        arduinoConnectorOn = on;
    }

    public void SelectGameMode(int selection)
    {
        gameMode = selection;
    }

    public void HumanROrY(bool red)
    {
        humanR = red;
    }

    public void HvCAiDifficulty(int difficulty)
    {
        if (humanR)
        {
            yAiDifficultySelected = difficulty;
        }
        else
        {
            rAiDifficultySelected = difficulty;
        }
    }

    public void RedAIDifficulty(int difficulty)
    {
        rAiDifficultySelected = difficulty;
    }
    public void YellowAIDifficulty(int difficulty)
    {
        yAiDifficultySelected = difficulty;
    }

    public void StartGame()
    {
        arduinoConnector.SetActive(arduinoConnectorOn);
        arduinoButtonParent.SetActive(arduinoConnectorOn);

        if (gameMode == 1)
        {
            if (humanR)
            {
                yAiDifficultyObjects[yAiDifficultySelected].SetActive(true);
            }
            else
            {
                rAiDifficultyObjects[rAiDifficultySelected].SetActive(true);
            }
        }
        else if (gameMode == 2)
        {
            rAiDifficultyObjects[rAiDifficultySelected].SetActive(true);
            yAiDifficultyObjects[yAiDifficultySelected].SetActive(true);
        }

        startGameCanvas.SetActive(false);
    }
}
