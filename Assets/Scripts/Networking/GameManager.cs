using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

using Photon.Pun;
using Photon.Realtime;

namespace Brad.KeepyUp.Networking
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
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
            PhotonNetwork.LoadLevel("Multiplayer Room");
        }
        #endregion

        #region Public Methods
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        #endregion
    }
}

