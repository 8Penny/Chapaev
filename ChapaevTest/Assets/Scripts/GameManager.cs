using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject board, rulePanel, choicePanel, scorePanel, finishPanel;
    
    int playerColor; //1-white, 0-black
    int playerRow, botRow; // position: 0 - first row, 1 - second row,  ...
    int playerCheckersCount, botCheckersCount, roundCount;
    bool isPlayerMove;
    bool run; // game is running
    bool isBegin; // begin of movement
    public bool blockCheckers; // restrict to move chechers


	void Start ()
    {
        // set parametres:
        botRow = 0;
        playerRow = 0;
        roundCount = 1;
        blockCheckers = true;
        isBegin = false;
        run = false;
        isPlayerMove = true;

        if (!PlayerPrefs.HasKey("RulesShowed")) // show rules if we didn't
        { 
            rulePanel.SetActive(true);
        }
        else
        {
            choicePanel.SetActive(true);
        }
	}
	
	void Update ()
    {
        Gameloop();
	}

    public void CloseRules()
    {
        rulePanel.SetActive(false);
        PlayerPrefs.SetInt(("RulesShowed"), 1);
        choicePanel.SetActive(true);
    }

    public void ChooseColor(bool isWhite)
    {
        playerColor = isWhite? 1 : 0;
        isPlayerMove = isWhite ? true : false;
        choicePanel.SetActive(false);
        PutCheckers();
        run = true;
        isBegin = true;
    }

    void Gameloop()
    {
        if (run & isBegin)
        {
            if (isPlayerMove)
            {
                scorePanel.transform.GetChild(0).GetComponent<Text>().text = "Ваш ход";
                blockCheckers = false;
                isBegin = false;
            }
            else
            {
                scorePanel.transform.GetChild(0).GetComponent<Text>().text = "Ход компьютера";
                isBegin = false;
                FindObjectOfType<BotController>().MakeMove();
            }
        }
    }

    public void NextTurn()
    {
        isPlayerMove = !isPlayerMove;
        isBegin = true;
        if (playerCheckersCount*botCheckersCount == 0)
        {
            FinishRound();
        }
    }

    void PutCheckers()
    {
        playerCheckersCount = botCheckersCount = 8; // standart checkers count in a round's beginning 
        float halfCellSize = board.GetComponent<RectTransform>().rect.height / 16;
        float Xpos = board.transform.localPosition.x - 7 * halfCellSize; // start Xpos
        float playerYpos = board.transform.localPosition.x - halfCellSize * (7 - playerRow*2); // player row
        float botYpos = board.transform.localPosition.x + halfCellSize * (7 - botRow * 2); // bot row
        GameObject checkerPref = Resources.Load("Prefabs/PlayerChecker") as GameObject;
        GameObject botPref = Resources.Load("Prefabs/BotChecker") as GameObject;
        
        for (int ind = 0; ind <8; ind++) // create and put in places checkers
        {
            RectTransform rPlCh = GameObject.Find("PlayerCheckers").GetComponent<RectTransform>();
            RectTransform rBotCh = GameObject.Find("BotCheckers").GetComponent<RectTransform>();
            GameObject playerCh = Instantiate(checkerPref, rPlCh.position, Quaternion.identity, rPlCh.transform);
            GameObject botCh = Instantiate(botPref, rBotCh.position, Quaternion.identity, rBotCh.transform);

            playerCh.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xpos, playerYpos);
            botCh.GetComponent<RectTransform>().anchoredPosition = new Vector2(Xpos, botYpos);

            if (playerColor == 1)
            {
                playerCh.GetComponent<SpriteRenderer>().color = new Color(1,1,1);
                botCh.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
            }
            Xpos += 2 * halfCellSize; // shift between checkers
        }

        // update score panel
        GameObject.Find("PlayerScore").GetComponent<Text>().text = "Фишки игрока: " + playerCheckersCount.ToString();
        GameObject.Find("BotScore").GetComponent<Text>().text = "Фишки компьютера: " + botCheckersCount.ToString();
        GameObject.Find("Round").GetComponent<Text>().text = "Раунд: " + roundCount.ToString();

    }

    // update score panel
    public void ChangeCheckersCount(int type) // type 1 - player checker was destroyed, 0 - bot checker was destroyed
    {
        if (type == 1)
        {
            playerCheckersCount -= 1;
            GameObject.Find("PlayerScore").GetComponent<Text>().text = "Фишки игрока: " + playerCheckersCount.ToString();
        }
        else
        {
            botCheckersCount -= 1;
            GameObject.Find("BotScore").GetComponent<Text>().text = "Фишки компьютера: " + botCheckersCount.ToString();
        }

    }

    void FinishRound()
    {
        if (playerCheckersCount == 0 & botCheckersCount != 0) // if bot won a round
        {
            foreach (GameObject checker in GameObject.FindGameObjectsWithTag("BotChecker")) // destroy all winner's checkers
            {
                Destroy(checker); 
            }
            if (botRow == 6)
            {
                FinishGame(0);
                return;
            }
            if (botRow + playerRow == 6)
            {
                playerRow -= 1; // change player checker's row
            }
            botRow += 1; // change bot checker's row
            isPlayerMove = false;

            

        }
        else if (playerCheckersCount != 0 & botCheckersCount == 0)
        {
            foreach (GameObject checker in GameObject.FindGameObjectsWithTag("PlayerChecker"))
            {
                Destroy(checker);
            }
            if (playerRow == 6)
            {
                FinishGame(1);
                return;
            }
            
            if (botRow + playerRow == 6)
            {
                botRow -= 1;
            }
            playerRow += 1;
            
            isPlayerMove = true;

        }

        PutCheckers();
        roundCount += 1;
        GameObject.Find("Round").GetComponent<Text>().text = "Раунд: " + roundCount.ToString();
    }
    void FinishGame(int winner) // winner 1 - player, 0 - bot
    {
        run = false;
        string text;
        if (winner == 1)
        { text = "Победа!"; }
        else
        { text = "Поражение"; }
        finishPanel.transform.GetChild(0).GetComponent<Text>().text = text;
        finishPanel.SetActive(true);
    }
    public void Restart()
    {
        finishPanel.SetActive(false);
        Start();
    }
}
