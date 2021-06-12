using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NKOManeger : MonoBehaviour
{
    [SerializeField] GameObject nkoDice;
    const int MY_TURN = 1;
    const int ENEMY_TURN = -1;
    int turn = MY_TURN;

    public enum MyState
    {
        NONE,
        SET,
        TAGET,
        SHOT,
        WAIT,
        CHANGE
    };

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNKO()
    {
        Vector3 pos = new Vector3(0, 15, 15);
        if(turn == MY_TURN)
        {
            Instantiate(nkoDice, pos, Random.rotation);
            Rigidbody rb = nkoDice.GetComponent<Rigidbody>();
            rb.useGravity = false;
        }
    }

    public void ShotNKO()
    {

    }
}
