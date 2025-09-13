using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public PlayerController controller;


    private void Awake()
    {
        controller = GetComponent<PlayerController>();

    }

}
