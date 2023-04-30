using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

using Photon.Pun;
using Photon.Realtime;

namespace Brad.KeepyUp.Networking
{
    public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Public Fields
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;
        public GameObject sceneBall;
        #endregion

        #region Private Fields
        int playerTurnIndex;
        #endregion

        #region Monobehaviour Callbacks
        private void Start()
        {
            if(playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }

            else
            {
                if(Paddle_Multiplayer.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                    PhotonNetwork.Instantiate(this.playerPrefab.name, Vector3.zero, Quaternion.identity, 0);
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }
        #endregion

        #region Photon Callbacks
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName); // not seen if you're the player connecting.

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName); // seen when other disconnects

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // Called before OnPlayerLeftRoom

                LoadArena();
            }
        }

        // Called when the local player left the room. We need to load the launcher scene.
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }
        #endregion

        #region Private Methods
        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.Log("PhotonNetwork: Trying to load a level but we are not the master client.");
                return;
            }
            Debug.LogFormat("PhotonNetwork: Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Multiplayer_Room");
        }
        #endregion

        #region Public Methods
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void NextPlayerTurn()
        {
            if (playerTurnIndex > PhotonNetwork.CurrentRoom.PlayerCount)
                playerTurnIndex = 0;

            else
                playerTurnIndex++;

            sceneBall.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.CurrentRoom.GetPlayer(playerTurnIndex));
        }
        #endregion

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(playerTurnIndex);
            }
            else
            {
                // Network player, receive data
                this.playerTurnIndex = (int)stream.ReceiveNext();
            }
        }
    }
}

