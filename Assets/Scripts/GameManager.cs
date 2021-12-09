using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance; //Singleton���
    public static GameObject localPlayer;   //�Ψ�������a���
    string gameVersion = "1";   //��ܥثe�C������
    public InputField roomName;
    void Awake()
    {
        if (instance != null)   //�p�G�����ƪ��N�屼�䤤�@��
        {
            Debug.LogErrorFormat(gameObject,
                    "Multiple instances of {0} is not allow", GetType().Name);
            DestroyImmediate(gameObject);
            return;
        }
        PhotonNetwork.AutomaticallySyncScene = true;    //�۰ʤ�������������
        DontDestroyOnLoad(gameObject);  //��Unity�b���������ɤ��n�N������R��
        instance = this;
    }
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();   //��Unity�۰ʨϥ�PUN�]�w����
        PhotonNetwork.GameVersion = gameVersion;    //���޹C������
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!PhotonNetwork.InRoom)
        {
            return;
        }
        //�ͦ��@�ӥ��a���a����b���w��m
        localPlayer = PhotonNetwork.Instantiate("TankPlayer", new Vector3(0, 0, 0), Quaternion.identity, 0);
        Debug.Log("���aID:" + localPlayer.GetInstanceID());
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
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)   //�p�G�O�Ĥ@�Ӷ}�Ҫ��N��D�n��Client
        {
            Debug.Log("�Ыةж����\");
            PhotonNetwork.LoadLevel("GameScene");
        }
        else
        {
            Debug.Log("�[�J�ж����\");
        }
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarningFormat($"�ѩ� {message} �ӵL�k�[�J�ж�");
    }
    public void JoinGameRoom()  //�Ыةж��ɪ��]�w
    {
        var option = new RoomOptions
        {
            MaxPlayers = 6
        };
        //�i�ǥѿ�J�ж��W�ٳЫ�
        PhotonNetwork.JoinOrCreateRoom(roomName.text, option, null);    
    }
}
