using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Camera m_fpsCamera;
    [SerializeField] private Image m_targetGfx;
    [SerializeField] private float m_fireRate = .1f;

    [SerializeField] private bool isTargetInSight = false;
    private float m_fireTimer;

    private Color clrGreen => Color.green;
    private Color clrRed => Color.red;

    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent<PhotonView>(out var photonView))
        {
            m_targetGfx.enabled = photonView.IsMine;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_fireTimer < m_fireRate) m_fireTimer += Time.deltaTime;

        RaycastHit hit;
        Ray ray = m_fpsCamera.ViewportPointToRay(new Vector3(.5f, .5f));

        PhotonView TargetPhotonView = null;

        if (Application.isEditor)
        {
            //Debug.DrawRay(ray.origin, ray.direction, Color.green);
            Debug.DrawLine(ray.origin, ray.direction * 100, Color.green);
        }

        if (Physics.Raycast(ray, out hit, 100))
        {
            bool isPlayerTag = hit.collider.gameObject.CompareTag("Player");

            if (isPlayerTag)
            {
                GameObject player = hit.collider.gameObject;

                if (player.TryGetComponent<PhotonView>(out var photonView))
                {
                    TargetPhotonView = photonView;
                    
                } 
            }
        }

        bool otherPlayerInSight = TargetPhotonView != null && !TargetPhotonView.IsMine;

        if (!isTargetInSight && otherPlayerInSight)
        {
            OnTargetInSight();
        }
        else if(isTargetInSight && !otherPlayerInSight)
        {
            OnTargetOutSight();
        }


        if (Input.GetButton("Fire1") && m_fireTimer > m_fireRate)
        {
            m_fireTimer = 0f;

            if (isTargetInSight)
            {
                TargetPhotonView.RPC("TakeDemage", RpcTarget.AllBuffered, 10f);
            }
        }
    }

    private void OnTargetInSight()
    {
        isTargetInSight = true;
        m_targetGfx.color = clrRed;


    }
    
    private void OnTargetOutSight()
    {
        isTargetInSight = false;
        m_targetGfx.color = clrGreen;


    }
}
