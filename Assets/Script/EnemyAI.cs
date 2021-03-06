using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] GameObject nkoDice;
    GameObject dice;
    NKOManeger nkoMng;
    NKOCheck nkoCheck;
    PlayerManeger pm;
    Rigidbody rb;
    MusicManeger musicManeger;

    Vector3 enePos = new Vector3(0, 10, -15);

    bool starting = false;
    bool checkStart = false;

    GameObject[] dices;
    int diceNum;

    List<int> diceValueList = new List<int>();
    List<string> wordList = new List<string>();
    int[] sums;

    void Start()
    {
        nkoMng = GetComponent<NKOManeger>();
        nkoCheck = GetComponent<NKOCheck>();
        pm = GetComponent<PlayerManeger>();
        rb = GetComponent<Rigidbody>();
        musicManeger = GetComponent<MusicManeger>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pm.battleEnd == true)
        {
            return;
        }
        if(nkoMng.state == NKOManeger.MyState.ENEMY && starting == false)
        {
            starting = true;
            AISet();
        }
        if (checkStart == true) {
            if(nkoMng.checkEnd == true)
            {
                AIMoveCheckEnd();
            }
        }
    }
    void AISet()
    {
        musicManeger.SpawnSound();
        dice = Instantiate(nkoDice, enePos, Random.rotation);
        rb = dice.GetComponent<Rigidbody>();
        rb.useGravity = false;
        Invoke("AIShot", 1);//ランダム秒数にする
        pm.enemyDices -= 1;
    }

    void AIShot()
    {
        musicManeger.ShotSound();
        Vector3 shotPower = new Vector3(0, -30, 30);
        rb.AddForce(shotPower, ForceMode.Impulse);
        rb.useGravity = true;
        Invoke("AIMoveCheck", 2);
    }

    void AIMoveCheck()
    {
        checkStart = true;
        nkoMng.checkEnd = false;
        nkoCheck.ErrorCheckStart();
    }

    void AIMoveCheckEnd()
    {
        checkStart = false;
        nkoMng.checkEnd = false;
        AIWordCheck();
    }

    void AIWordCheck()
    {
        pm.playerHP  -= nkoCheck.CheckWord();
        nkoMng.state = NKOManeger.MyState.SET;
        starting = false;
    }
}
