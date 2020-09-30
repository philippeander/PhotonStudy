using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNameInputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerName(string playerName) 
    {
        if (string.IsNullOrEmpty(playerName)) 
        {
            Debug.LogWarning("Atention: The player name is empty!");
            return;
        }

        PhotonNetwork.NickName = playerName;
    }
}
