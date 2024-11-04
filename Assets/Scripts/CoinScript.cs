using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] float spinspeedDegPerSec = 90;
    Vector3 startPos;
    bool movingup=true;
    public int value = 1;
    [SerializeField] float upDownMoveSpeed = 10;
    [SerializeField] float upDownMaxDist = 2;

    
    // Update is called once per frame
    private void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        // rotation
        transform.Rotate(new Vector3(0,0,1), spinspeedDegPerSec*Time.deltaTime, Space.Self);

        // up down motion
        float currentDist = Mathf.Abs(transform.position.y - startPos.y);

        if (currentDist >= upDownMaxDist)
        {
            movingup = !movingup;
        }

        Vector3 direction = movingup ? new Vector3(0,0,1) : direction = new Vector3(0, 0, -1);
        transform.Translate(direction * upDownMoveSpeed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("collision");
        if(other.tag == "Player")
        {
            CoinManager.instance.CollectCoin(value);
            Destroy(gameObject);
        }
    }
}
