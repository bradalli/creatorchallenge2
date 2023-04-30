using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;

namespace Brad.KeepyUp.Networking
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        // The maximum number of players per room. When a room is full, it can't be joined by new players, and
        // so a new room will be created.
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so a new room will be created.")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        [Tooltip("The UI Panel to let the user enter their name, connect, and play.")]
        [SerializeField]
        private GameObject controlPanel;

        [Tooltip("The UI Label to inform the user that the connection is in progress.")]
        [SerializeField]
        private GameObject progressLabel;
        #endregion

        #region Private Fields
        // This client's version number. Users are seperated from each other by gameVersion (which allows you
        // to make breaking changes).
        string gameVersion = "1";
        #endregion

        #region MonoBehaviour Callbacks
        // MonoBehaviour method called on GameObject by Unity during early initialization phase.
        private void Awake()
        {
            // #CRITICAL - This makes sure we can use PhotonNetwork.LoadLevel() on the master client and all
            // clients in the same room synce their level automatically.
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        // MonoBehaviour method called on GameObject by Unity during early initialization phase.
        private void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }
        #endregion

        #region MonoBehaviourPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("NETWORKING/Launcher: OnConnectedToMaster() was called by PUN");

            // #CRITICAL - The first we try to do is to join a potential existing room. If there is, good, else,
            // we'll be called back with OnJoinRandomFailed().
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);

            Debug.LogWarningFormat("NETWORKING/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("NETWORKING/Launcher: OnJoinRandomFailed() was called by PUN. No random room available, so we " +
                "create on. \nCalling: PhotonNetwork.CreateRoom");

            // # CRITICAL - We failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom});
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("NETWORKING/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Start the connection process.
        ///  - If already connected, we attempt joining a random room.
        ///  - If not yet connected, connect this application instance to Photon Cloud Network.
        /// </summary>
        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            // We check if we are connected or not, we join if we are, else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // #CRITICAL - We need at this point to attempt joining a Random Room. If it fails,
                // we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #CRITICAL - We must first and foremost connect to Photon Online Server.
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        #endregion
    }
}

