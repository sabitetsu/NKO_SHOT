using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NKOManeger : MonoBehaviour
{
    [SerializeField] GameObject nkoDice;
    GameObject dice;
    GameObject LineObj;
    [SerializeField] Button setButton;
    [SerializeField] Button shotButton;
    [SerializeField] Button checkButton;

    NKOCheck nkoCheck;
    PlayerManeger pm;
    Rigidbody rb;
    MusicManeger musicManeger;

    const int MY_TURN = 1;
    const int ENEMY_TURN = -1;
    int turn = MY_TURN;
    public bool checkEnd = false;

    Vector3 myPos = new Vector3(0, 15, 15);
    Vector3 enePos = new Vector3(0, 15, -15);
    Vector3 targetPos = new Vector3(0, 0, 0);
    Vector3 powerVector = new Vector3(5, 5, 5);

    public enum MyState
    {
        NONE,
        SET,
        SHOT,
        WAIT,
        CHECK,
        ENEMY
    };

    public MyState state = MyState.SET;

    void Start()
    {
        Application.targetFrameRate = 30;
        nkoCheck = GetComponent<NKOCheck>();
        pm = GetComponent<PlayerManeger>();
        musicManeger = GetComponent<MusicManeger>();
    }

    void Update()
    {
        if(state == MyState.SHOT)
        {
            if (Input.GetMouseButtonDown(0))
            {
                TargetNKO();
            }
        }
        if(state == MyState.WAIT)
        {
            if(checkEnd == true)
            {
                CheckEnd();
            }
        }
        switch (state)
        {
            case MyState.SET:
                setButton.interactable = true;
                shotButton.interactable = false;
                checkButton.interactable = false;
                break;
            case MyState.SHOT:
                setButton.interactable = false;
                shotButton.interactable = true;
                checkButton.interactable = false;
                break;
            case MyState.WAIT:
                setButton.interactable = false;
                shotButton.interactable = false;
                checkButton.interactable = false;
                break;
            case MyState.CHECK:
                setButton.interactable = true;
                shotButton.interactable = false;
                checkButton.interactable = true;
                break;
            case MyState.ENEMY:
                setButton.interactable = false;
                shotButton.interactable = false;
                checkButton.interactable = false;
                break;
        }
    }

    //SETボタン
    public void SetNKO()
    {
        if(state == MyState.SET || state == MyState.CHECK)
        {
            musicManeger.SpawnSound();
            dice = Instantiate(nkoDice, myPos, Random.rotation);
            rb = dice.GetComponent<Rigidbody>();
            rb.useGravity = false;
            targetPos = new Vector3(0, 0, 0);
            TargetLine(myPos, new Vector3(0, 0, 0));
            state = MyState.SHOT;
            pm.playerDices -= 1;
        }
    }

    void TargetNKO()
    {
        musicManeger.MarkSound();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50.0f))
        {
            if (hit.collider.gameObject.tag != "Out")
            {
                targetPos = hit.point;
                // markerTF.position = targetPos;
                if (turn == MY_TURN)
                {
                    Destroy(LineObj);
                    TargetLine(myPos, targetPos);
                }
                else if (turn == -1)
                {
                    Destroy(LineObj);
                    TargetLine(enePos, targetPos);
                }

            }
        }
    }

    void TargetLine(Vector3 startVec, Vector3 endVec)
    {
        LineObj = new GameObject("Line");
        LineRenderer lRend = LineObj.AddComponent<LineRenderer>();
        lRend.positionCount = 2;
        lRend.startWidth = 0.5f;
        lRend.endWidth = 0.5f;
        lRend.SetPosition(0, startVec);
        lRend.SetPosition(1, endVec);
    }

    //SHOTボタン
    public void ShotNKO()
    {
        if (state == MyState.SHOT)
        {
            musicManeger.ShotSound();
            targetPos.y = -10;
            targetPos.z -= 10 * turn;
            Vector3 shotPower = Vector3.Scale(targetPos, powerVector);
            rb.AddForce(shotPower, ForceMode.Impulse);
            rb.useGravity = true;
            Destroy(LineObj);
            state = MyState.WAIT;
            Invoke("ErrorCheck", 2);
        }
    }

    void ErrorCheck()
    {
        checkEnd = false;
        nkoCheck.ErrorCheckStart();
    }

    void CheckEnd()
    {
        state = MyState.CHECK;
        checkEnd = false;
    }

    //Checkボタン
   public  void CheckNKO()
    {
        if (state != MyState.CHECK)
        {
            return;
        }
        pm.enemyHP -= nkoCheck.CheckWord();
        state = MyState.WAIT;
        Invoke("ChangeTurn", 2);
    }

    void ChangeTurn()
    {
        state = MyState.ENEMY;
    }
}
