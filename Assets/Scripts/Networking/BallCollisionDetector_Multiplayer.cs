using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Photon.Pun;

namespace Brad.KeepyUp
{
    public class BallCollisionDetector_Multiplayer : MonoBehaviourPunCallbacks
    {
        [SerializeField] UnityEvent collisionEnter2DEvent;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                collisionEnter2DEvent.Invoke();
            }
        }
    }
}

