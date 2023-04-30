using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

namespace Brad.KeepyUp.Networking
{
    //Player name input field. Let the user input their name.
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants
        // Store the PlayerPrefs Key to avoid typos.
        const string playerNamePrefKey = "PlayerName";
        #endregion

        #region MonoBehaviour Callbacks
        private void Start()
        {
            string defaultName = string.Empty;
            TMP_InputField _inputField = this.GetComponent<TMP_InputField>();
            if(_inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }
        #endregion

        #region Public Methods
        // Sets the name of the player, and save it in the PlayerPrefs for future sessions.
        public void SetPlayerName(string value)
        {
            // #IMPORTANT
            if (string.IsNullOrEmpty(value))
            {
                Debug.Log("Player Name is null or empty");
                return;
            }
            PhotonNetwork.NickName = value;

            PlayerPrefs.SetString(playerNamePrefKey, value);
        }
        #endregion
    }
}

