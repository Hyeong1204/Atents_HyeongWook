using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IUseavleObject
{
    protected Animator anim;

    bool playerIn = false;
    bool ondoor = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("문 열림");
            playerIn = true;

            
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("문 닫힘");
            playerIn = false;

            
        }

    }

    public void Use()
    {
        if (playerIn)
        {
            anim.SetBool("IsOpen", ondoor);
            ondoor = !ondoor;
        }
    }
}
