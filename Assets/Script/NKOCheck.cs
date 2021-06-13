using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NKOCheck : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    [SerializeField] Text wordText;
    NKOManeger nkoMng;
    GameObject[] dices;
    int diceNum;

    List<int> diceValueList = new List<int>();
    List<string> wordList = new List<string>();
    List<int> zeroDiceList = new List<int>();
    int[] sums;
    int[] deleteNums;

    int errorCount = 0;


    void Start()
    {
        nkoMng = GetComponent<NKOManeger>();
    }

    void Update()
    {

    }

    public void CheckDiceValue()
    {
        diceValueList.Clear();
        zeroDiceList.Clear();

        //場のサイコロの目を調べる
        dices = GameObject.FindGameObjectsWithTag("Dice");
        diceNum = dices.Length;
        for (int i = 0; i < diceNum; i++)
        {
            Die_d6 roll = dices[i].GetComponent<Die_d6>();
            diceValueList.Add(roll.value);

            if(roll.value == 0)
            {
                zeroDiceList.Add(i);
            }
        }

        // 各目が何個出たか数える、エラーは0
        sums = new int[7];
        for (int i = 0; i < diceValueList.Count; i++)
        {
            sums[diceValueList[i]] += 1;
        }
    }

    bool IsStopped()
    {
        dices = GameObject.FindGameObjectsWithTag("Dice");
        diceNum = dices.Length;
        Rigidbody rb;
        bool isStopped = false;
        for (int i = 0; i < diceNum; i++)
        {
            rb = dices[i].GetComponent<Rigidbody>();
            if (rb.IsSleeping())
            {
                isStopped = true;
                continue;
            }
            //動いているものがあれば呼び出される
            //ダイスが範囲外の場合消す
            if(dices[i].transform.position.y < 0)
            {
                Destroy(dices[i]);
            }
            //動いている物がある場合falseを返して終了
            isStopped = false;
            Debug.Log("Moving");
            break;
        }
        return isStopped;
    }

    //CHECKボタンで発動
    public void CheckWord()
    {
        // 1  2  3  4  5  6
        // お う ん こ ま ち

        CheckDiceValue();

        wordList.Clear();
        deleteNums = new int[7];
        int damage = 0;

        if (sums[2] > 0 && sums[3] > 0 && sums[4] > 0)
        {
            wordList.Add("UNKO\n");
            damage += 1;
            deleteNums[2] = 1;
            deleteNums[3] = 1;
            deleteNums[4] = 1;
        }
        if (sums[2] > 0 && sums[3] > 0 && sums[6] > 0)
        {
            wordList.Add("UNCHI\n");
            damage += 1;
            deleteNums[2] = 1;
            deleteNums[3] = 1;
            deleteNums[6] = 1;
        }

        if (sums[1] > 0 && sums[5] > 0 && sums[3] > 0 && sums[4] > 0)
        {
            wordList.Add("OMANKO\n");
            damage += 2;
            deleteNums[1] = 1;
            deleteNums[5] = 1;
            deleteNums[3] = 1;
            deleteNums[4] = 1;
        }
        else if (sums[5] > 0 && sums[3] > 0 && sums[4] > 0)
        {
            wordList.Add("MANKO\n");
            damage += 1;
            deleteNums[5] = 1;
            deleteNums[3] = 1;
            deleteNums[4] = 1;
        }

        if (sums[6] > 0 && sums[3] > 0 && sums[4] > 0)
        {
            wordList.Add("CHINKO\n");
            damage += 1;
            deleteNums[6] = 1;
            deleteNums[3] = 1;
            deleteNums[4] = 1;
        }
        
        if (sums[1] > 0 && sums[6] > 1 && sums[3] > 1)
        {
            wordList.Add("OCHINCHIN\n");
            damage += 3;
            deleteNums[1] = 1;
            deleteNums[6] = 2;
            deleteNums[3] = 2;
        }
        else if (sums[6] > 1 && sums[3] > 1)
        {
            wordList.Add("CHINCHIN\n");
            damage += 2;
            deleteNums[6] = 2;
            deleteNums[3] = 2;
        }
        Debug.Log("ダメージ："+damage);

        //使った物消す処理
        Invoke("DeleteNko", 3);

        //コンボ表示処理
        if (wordList != null)
        {
            Panel.SetActive(true);
            wordText.text = "";
            for (int i = 0; i < wordList.Count; i++)
            {
                wordText.text += wordList[i];
            }
            Invoke("PanelDelete", 5);
        }
    }

    //Shot後呼び出される
    public void ErrorCheckStart()
    {
        StartCoroutine("ErrorMoveCheck");
    }
    IEnumerator ErrorMoveCheck()
    {
        bool checkStart;
        for(int waitTime =0; waitTime < 10; waitTime++)
        {
            checkStart = IsStopped();
            if(checkStart == true)
            {
                break; //ダイスが止まったらforを終了
            }
            // Debug.Log("Checking...");
            yield return new WaitForSeconds(1);
        }
        CheckDiceValue();
        if (zeroDiceList.Count == 0)
        {
            Debug.Log("NoError");
            nkoMng.checkEnd = true;
            errorCount = 0;
            yield break;
        }
        else
        {
            errorCount += 1;
            ErrorDices();
        }
    }

    void ErrorDices()
    {
        int zeroDiceNum = zeroDiceList.Count; //エラーダイスの数
        if (errorCount > 10)
        {
            //エラーダイス全て削除
            for(int i = 0; i < zeroDiceNum; i++)
            {
                Destroy(dices[zeroDiceList[i]]);
            }
            zeroDiceList.Clear();
            return;
        }

        //斜めだったら振り直し
        ReRoll();
        //範囲外だったら削除
        
        //再びチェック
        Invoke("ErrorCheckStart", 2);
        Debug.Log("エラー回数：" + errorCount);
    }

    void ReRoll()
    {
        int zeroDiceNum = zeroDiceList.Count; //エラーダイスの数
        for (int i = 0; i < zeroDiceNum; i++)
        {
            GameObject zeroDice = dices[zeroDiceList[i]];
            // Vector3 zeroDicePos = zeroDice.transform.position;
            Rigidbody rb = zeroDice.GetComponent<Rigidbody>();
            rb.AddForce(new Vector3(0, 50, 0), ForceMode.Impulse);
        }
    }

    void PanelDelete()
    {
        Panel.SetActive(false);
    }

    void DeleteNko()
    {
        int diceNum = diceValueList.Count;
        for(int i = 0;i < diceNum; i++)
        {
            switch (diceValueList[i])
            {
                case 0:
                    Debug.Log("ダイスバリューリストに0がある");
                    break;
                case 1:
                    if(deleteNums[1] > 0)
                    {
                        Destroy(dices[i]);
                        deleteNums[1] -= 1;
                    }
                    break;
                case 2:
                    if (deleteNums[2] > 0)
                    {
                        Destroy(dices[i]);
                        deleteNums[2] -= 1;
                    }
                    break;
                case 3:
                    if (deleteNums[3] > 0)
                    {
                        Destroy(dices[i]);
                        deleteNums[3] -= 1;
                    }
                    break;
                case 4:
                    if (deleteNums[4] > 0)
                    {
                        Destroy(dices[i]);
                        deleteNums[4] -= 1;
                    }
                    break;
                case 5:
                    if (deleteNums[5] > 0)
                    {
                        Destroy(dices[i]);
                        deleteNums[5] -= 1;
                    }
                    break;
                case 6:
                    if (deleteNums[6] > 0)
                    {
                        Destroy(dices[i]);
                        deleteNums[6] -= 1;
                    }
                    break;
            }
        }
        //全部消えたか確認 
        for(int i = 0; i < 7; i++)
        {
            if(deleteNums[i] != 0)
            {
                Debug.Log("デリートナムに数字が残ってる");
            }
        }
    }
}
