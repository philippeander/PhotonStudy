using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LaunchManager : MonoBehaviourPunCallbacks
{

    public GameObject EnterGamePanel;
    public GameObject ConnectionsStatusPanel;
    public GameObject LobbyPanel;


    #region UnityMethods
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        EnterGamePanel.SetActive(true);
        ConnectionsStatusPanel.SetActive(false);
        LobbyPanel.SetActive(false);
    }

    
    void Update()
    {

    }
    #endregion

    #region PublicMethods

    public void ConnectToPhotonServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();

            EnterGamePanel.SetActive(false);
            ConnectionsStatusPanel.SetActive(true);
        }
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    #endregion

    #region  regionPhotonCallbacks
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        EnterGamePanel.SetActive(false);
        ConnectionsStatusPanel.SetActive(false);
        LobbyPanel.SetActive(true);

        Debug.Log(PhotonNetwork.NickName + ": is connected to the Photon Server!");
    }

    public override void OnConnected()
    {
        base.OnConnected();

        Debug.Log("Connected to the Internet!");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.LogWarning(message);

        CreateJoinRoom();

    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " - " + PhotonNetwork.CurrentRoom.PlayerCount + "º");
    }
    #endregion

    #region Private Methods

    private void CreateJoinRoom() 
    {
        string randomRoomName = "room " + UnityEngine.Random.Range(0, 10000);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 20;

        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }

    #endregion

}
