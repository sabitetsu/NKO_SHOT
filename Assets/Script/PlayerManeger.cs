using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManeger : MonoBehaviour
{
    public int playerHP = 1000;
    public int playerDices = 15;
    public int enemyHP = 1000;
    public int enemyDices = 15;

    public bool battleEnd = false;

    [SerializeField] Text playerText;
    [SerializeField] Text enemyText;
    [SerializeField] GameObject centerPanel;
    [SerializeField] Text centerText;
    void Start()
    {
        
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
            EndBattle(false);
        }
        else if (enemyHP <= 0)
        {
            EndBattle(true);
        }
        else if (playerDices < 0)
        {
            EndBattle(false);
        }
        else if (enemyDices < 0)
        {
            EndBattle(true);
        }
    }

    void EndBattle(bool playerWin)
    {
        if (playerWin)
        {
            centerPanel.SetActive(true);
            centerText.text = "PLAYER WIN";
            Debug.Log("PLAYER WIN");
        }
        else
        {
            centerPanel.SetActive(true);
            centerText.text = "PLAYER LOSE";
            Debug.Log("PLAYER LOSE");
        }
        battleEnd = true;
    }

    void StateText()
    {
        playerText.text = "HP　   :" + playerHP.ToString() + "\n手持ち:" + playerDices.ToString();
        enemyText.text = "HP　   :" + enemyHP.ToString() + "\n手持ち:" + enemyDices.ToString();
    }
}
