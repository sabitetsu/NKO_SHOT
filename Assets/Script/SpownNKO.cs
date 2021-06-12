using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpownNKO : MonoBehaviour
{
    [SerializeField] GameObject nkoDice;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNKO()
    {
        Vector3 pos = new Vector3(0, 10, 0);
        Instantiate(nkoDice, pos, Random.rotation);
    }
}
