using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] private Camera m_fpsCamera;
    [SerializeField] private TextMeshProUGUI m_txtplayerName;


    private void Awake()
    {
        m_fpsCamera = GetComponentInChildren<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this);

        if (photonView.IsMine)
        {
            transform.GetComponent<MovementController>().enabled = true;
            m_fpsCamera.enabled = true;




        }
        else 
        {
            transform.GetComponent<MovementController>().enabled = false;
            m_fpsCamera.enabled = false;



        }

        SetPlayerUi();
    }

    private void SetPlayerUi() 
    {
        if (m_txtplayerName != null)
        {
            m_txtplayerName.text = photonView.Owner.NickName; 
        }
    }
}
