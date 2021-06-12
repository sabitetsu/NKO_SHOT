using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NKOCheck : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    [SerializeField] Text wordText;

    List<int> diceValueList = new List<int>();
    List<string> wordList = new List<string>();

    void Start()
    {

    }

    void Update()
    {

    }

    public void CheckDiceValue()
    {
        diceValueList.Clear();
        GameObject[] dices = GameObject.FindGameObjectsWithTag("Dice");
        int diceNum = dices.Length;
        for (int i = 0; i < diceNum; i++)
        {
            Die_d6 roll = dices[i].GetComponent<Die_d6>();
            diceValueList.Add(roll.value);
        }
        Invoke("CheckWord", 1);
    }

    void CheckWord()
    {
        wordList.Clear();
        // 各文字が何個出たか数える
        int[] sums = new int[7];
        for(int i = 0; i < diceValueList.Count; i++)
        {
            sums[diceValueList[i]] += 1;
        }

        // 1  2  3  4  5  6
        // お う ん こ ま ち

        if(sums[2] > 0 && sums[3] > 0 && sums[4] > 0)
        {
            wordList.Add("UNKO\n");
        }
        if (sums[5] > 0 && sums[3] > 0 && sums[4] > 0)
        {
            wordList.Add("MANKO\n");
        }
        if (sums[1] > 0 && sums[5] > 0 && sums[3] > 0 && sums[4] > 0)
        {
            wordList.Add("OMANKO\n");
        }
        if (sums[6] > 0 && sums[3] > 0 && sums[4] > 0)
        {
            wordList.Add("CHINKO\n");
        }
        if (sums[6] > 1 && sums[3] > 1)
        {
            wordList.Add("CHINCHIN\n");
        }
        if (sums[1] > 0 && sums[6] > 1 && sums[3] > 1)
        {
            wordList.Add("OCHINCHIN\n");
        }


        if(wordList != null)
        {
            Panel.SetActive(true);
            wordText.text = "";
            for (int i = 0; i < wordList.Count; i++)
            {
                wordText.text += wordList[i];
            }
            Invoke("PanelDelete", 5);
        }
        for (int i = 0; i < wordList.Count; i++)
        {
            Debug.Log(wordList[i]);
        }
    }
    void PanelDelete()
    {
        Panel.SetActive(false);
    }
}
