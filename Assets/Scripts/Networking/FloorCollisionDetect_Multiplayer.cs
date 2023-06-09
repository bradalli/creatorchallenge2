﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Photon.Pun;

namespace Brad.KeepyUp
{
    public class FloorCollisionDetect_Multiplayer : MonoBehaviourPunCallbacks
    {
        [SerializeField] UnityEvent collisionEnter2DEvent;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Floor"))
            {
                collisionEnter2DEvent.Invoke();
                transform.position = Vector3.up * 5;
                transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                //gameObject.SetActive(false);
            }
        }
    }
}

