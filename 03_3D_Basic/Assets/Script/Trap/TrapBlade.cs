using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using UnityEngine;

public class TrapBlade : TrapBase
{
    Transform blade;
    Transform[] wayPoint;
    int index = 0;

    Rigidbody rigid;

    public Transform CuurentWaypoint { get => wayPoint[index]; }

   
    float bladeSpeed = 3.0f;

    

    private void Start()
    {
        SetUp();
        
    }




    private void Awake()
    {
        blade = transform.GetChild(0);
        wayPoint = new Transform[transform.childCount-1];
        for(int i =0; i < wayPoint.Length; i++)
        {
            wayPoint[i] = transform.GetChild(i+1);
        }
        rigid = transform.GetChild(0).GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(blade.position + bladeSpeed * Time.fixedDeltaTime * blade.forward);
    }



    void Move()
    {
        if(index < wayPoint.Length)
        {
            index++;
            index %= wayPoint.Length;
            //dir = (wayPoint[index].position - blade.position).normalized;
            blade.LookAt(wayPoint[index]);
        }
    }


    void SetUp()
    {
        blade.transform.position = wayPoint[index].transform.position;

        StartCoroutine(OnMove());

    }

    IEnumerator OnMove()
    {
        Move();
        while (true)
        {
            if (Vector3.Distance(blade.position, wayPoint[index].position) < 0.02f * bladeSpeed)
            {

                Move();

            }
            yield return null;
        }
    }
}
