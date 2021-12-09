using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPlayer : MonoBehaviourPunCallbacks
{
    private Complete.TankMovement m_Movement;
    private Complete.TankShooting m_Shooting;

    private void Awake()
    {
        m_Movement = GetComponent<Complete.TankMovement>(); //��o����W��Component
        m_Shooting = GetComponent<Complete.TankShooting>();

        if (!photonView.IsMine) //�p�G���O�ڦۤv���N����ޱ�
        {
            m_Movement.enabled = false;
            m_Shooting.enabled = false;

            enabled = false;
        }
    }
}
