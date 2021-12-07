using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    string gameVersion = "1";
    void Awake()
    {
        if (instance != null)   //�p�G�����ƪ��N�屼�䤤�@��
        {
            Debug.LogErrorFormat(gameObject,
                    "Multiple instances of {0} is not allow", GetType().Name);
            DestroyImmediate(gameObject);
            return;
        }
        PhotonNetwork.AutomaticallySyncScene = true;
        DontDestroyOnLoad(gameObject);  //��Unity�b���������ɤ��n�N������R��
        instance = this;
    }
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();   //��Unity�۰ʨϥ�PUN�]�w����
        PhotonNetwork.GameVersion = gameVersion;    //���޹C������
    }
   
    public override void OnConnected()
    {
        Debug.Log("PUN�w�s��");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN�s�u��Master");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat($"PUN �ѩ� {cause} �ӥ��h�s�u");
    }

}
