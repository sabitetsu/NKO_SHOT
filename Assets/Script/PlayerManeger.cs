using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManeger : MonoBehaviour
{
    public int playerHP = 1000;
    public int playerDices = 15;
    public int enemyHP = 1000;
    public int enemyDices = 15;

    public bool battleEnd = false;
    bool playerWin = true;

    [SerializeField] Text playerText;
    [SerializeField] Text enemyText;
    [SerializeField] GameObject centerPanel;
    [SerializeField] Text centerText;

    MusicManeger musicManeger;

    void Start()
    {
        musicManeger = GetComponent<MusicManeger>();
    }

    // Update is called once per frame
    void Update()
    {
        if(battleEnd == false)
        {
            
            CheckBattleEnd();
            StateText();
        }
    }

    void CheckBattleEnd()
    {
        //最期の攻撃でしとめれたら勝ちなのでこの順で判定
        if (playerHP <= 0)
        {
            playerWin = false;
            Invoke("EndBattle",3);
            battleEnd = true;
            // EndBattle(false);
        }
        else if (enemyHP <= 0)
        {
            playerWin = true;
            Invoke("EndBattle", 3);
            battleEnd = true;
        }
        else if (playerDices < 0)
        {
            playerWin = false;
            Invoke("EndBattle", 3);
            battleEnd = true;
        }
        else if (enemyDices < 0)
        {
            playerWin = true;
            Invoke("EndBattle", 3);
            battleEnd = true;
        }
    }

    void EndBattle()
    {
        if (playerWin)
        {
            musicManeger.WineerSound();
            centerPanel.SetActive(true);
            centerText.text = "PLAYER WIN";
            Debug.Log("PLAYER WIN");
        }
        else
        {
            musicManeger.LoseSound();
            centerPanel.SetActive(true);
            centerText.text = "PLAYER LOSE";
            Debug.Log("PLAYER LOSE");
        }
        Invoke("ReStart", 5);
    }

    void ReStart()
    {
        SceneManager.LoadScene("Gamescene");
    }

    void StateText()
    {
        playerText.text = "HP　   :" + playerHP.ToString() + "\n手持ち:" + playerDices.ToString();
        enemyText.text = "HP:" + enemyHP.ToString() + "\n手持ち:" + enemyDices.ToString()+ "　";
    }
}
