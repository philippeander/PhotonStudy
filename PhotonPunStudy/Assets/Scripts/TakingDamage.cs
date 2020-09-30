using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class TakingDamage : MonoBehaviourPunCallbacks
{
    [SerializeField] private float m_health;
    [SerializeField] private Slider m_sliderLife;

    // Start is called before the first frame update
    void Start()
    {
        m_health = 100;
        m_sliderLife.maxValue = 100;
        m_sliderLife.value = m_health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void TakeDemage(float demageVal)
    {
        m_health -= demageVal;
        m_sliderLife.value = m_health;

        if (m_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (photonView.IsMine)
        {
            PixelGunGameManager.Instance.LeaveRoom();
        }
    }
}
