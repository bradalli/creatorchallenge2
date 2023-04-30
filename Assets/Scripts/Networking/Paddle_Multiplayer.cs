using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

namespace Brad.KeepyUp
{
    public class Paddle_Multiplayer : MonoBehaviourPunCallbacks
    {
        [Tooltip("The local player instance. Use this to know if the local player is represented in the scene")]
        public static GameObject LocalPlayerInstance;

        Rigidbody2D rb;
        [SerializeField] Rigidbody2D playerRb;
        [SerializeField] float movementSpeed;

        private void Awake()
        {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                Paddle_Multiplayer.LocalPlayerInstance = this.gameObject;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine)
            {
                Vector2 mousePosition = new Vector2(Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, -15, 15),
                Mathf.Clamp(Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -7, 0));
                //transform.position = mousePosition;

                float step = movementSpeed * Time.deltaTime; // calculate distance to move
                playerRb.position = Vector3.MoveTowards(playerRb.position, mousePosition, step);
            }
        }

        public void BallHit()
        {
            if (photonView.IsMine)
            {
                gameObject.SendMessage("BallHitRewardIncrement");
            }
        }
    }
}

