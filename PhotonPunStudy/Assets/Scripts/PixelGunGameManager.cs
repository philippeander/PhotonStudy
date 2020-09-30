using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PixelGunGameManager : MonoBehaviourPunCallbacks
{
    static public PixelGunGameManager Instance;

    [SerializeField] private GameObject m_playerPrefab;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        if (PhotonNetwork.IsConnected) 
        {
            if (m_playerPrefab != null)
            {
                int randomPointY = UnityEngine.Random.Range(-20, 20);
                int randomPointX = UnityEngine.Random.Range(-20, 20);

                Vector3 randomPos = new Vector3(randomPointX, 0, randomPointY);

                PhotonNetwork.Instantiate(m_playerPrefab.name, randomPos, Quaternion.identity); 
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " => " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("GameLouncherScene");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
