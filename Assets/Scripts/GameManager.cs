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
        if (instance != null)   //如果有重複的就砍掉其中一個
        {
            Debug.LogErrorFormat(gameObject,
                    "Multiple instances of {0} is not allow", GetType().Name);
            DestroyImmediate(gameObject);
            return;
        }
        PhotonNetwork.AutomaticallySyncScene = true;
        DontDestroyOnLoad(gameObject);  //讓Unity在場景切換時不要將此物件刪除
        instance = this;
    }
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();   //讓Unity自動使用PUN設定的值
        PhotonNetwork.GameVersion = gameVersion;    //控管遊戲版本
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

}
