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
    public static GameManager instance; //Singleton單例
    public static GameObject localPlayer;   //用來獲取玩家資料
    string gameVersion = "1";   //表示目前遊戲版本
    public InputField roomName;
    void Awake()
    {
        if (instance != null)   //如果有重複的就砍掉其中一個
        {
            Debug.LogErrorFormat(gameObject,
                    "Multiple instances of {0} is not allow", GetType().Name);
            DestroyImmediate(gameObject);
            return;
        }
        PhotonNetwork.AutomaticallySyncScene = true;    //自動切換場景的機制
        DontDestroyOnLoad(gameObject);  //讓Unity在場景切換時不要將此物件刪除
        instance = this;
    }
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();   //讓Unity自動使用PUN設定的值
        PhotonNetwork.GameVersion = gameVersion;    //控管遊戲版本
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!PhotonNetwork.InRoom)
        {
            return;
        }
        //生成一個本地玩家物件在指定位置
        localPlayer = PhotonNetwork.Instantiate("TankPlayer", new Vector3(0, 0, 0), Quaternion.identity, 0);
        Debug.Log("玩家ID:" + localPlayer.GetInstanceID());
    }

    public override void OnConnected()
    {
        Debug.Log("PUN已連接");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN連線到Master");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat($"PUN 由於 {cause} 而失去連線");
    }
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)   //如果是第一個開啟的就當主要的Client
        {
            Debug.Log("創建房間成功");
            PhotonNetwork.LoadLevel("GameScene");
        }
        else
        {
            Debug.Log("加入房間成功");
        }
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarningFormat($"由於 {message} 而無法加入房間");
    }
    public void JoinGameRoom()  //創建房間時的設定
    {
        var option = new RoomOptions
        {
            MaxPlayers = 6
        };
        //可藉由輸入房間名稱創建
        PhotonNetwork.JoinOrCreateRoom(roomName.text, option, null);    
    }
}
