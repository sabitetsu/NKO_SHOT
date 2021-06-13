using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] GameObject nkoDice;
    GameObject dice;
    NKOManeger nkoMng;
    NKOCheck nkoCheck;
    Rigidbody rb;

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
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
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
        dice = Instantiate(nkoDice, enePos, Random.rotation);
        rb = dice.GetComponent<Rigidbody>();
        rb.useGravity = false;
        Invoke("AIShot", 1);//ランダム秒数にする
    }

    void AIShot()
    {
        Vector3 shotPower = new Vector3(0, -50, 50);
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
        nkoCheck.CheckWord();
        nkoMng.state = NKOManeger.MyState.SET;
        starting = false;
    }
}
