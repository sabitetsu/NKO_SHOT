using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NKOManeger : MonoBehaviour
{
    [SerializeField] GameObject nkoDice;
    GameObject dice;
    GameObject LineObj;

    NKOCheck nkoCheck;
    Rigidbody rb;

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
    }

    //SETボタン
    public void SetNKO()
    {
        if(state == MyState.SET || state == MyState.CHECK)
        {
            dice = Instantiate(nkoDice, myPos, Random.rotation);
            rb = dice.GetComponent<Rigidbody>();
            rb.useGravity = false;
            TargetLine(myPos, new Vector3(0, 0, 0));
            state = MyState.SHOT;
        }
    }

    void TargetNKO()
    {
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
        Debug.Log(state);
        checkEnd = false;
    }

    //Checkボタン
   public  void CheckNKO()
    {
        if (state != MyState.CHECK)
        {
            return;
        }
        nkoCheck.CheckWord();
        Invoke("ChangeTurn", 5);
    }

    void ChangeTurn()
    {
        state = MyState.ENEMY;
    }
}
