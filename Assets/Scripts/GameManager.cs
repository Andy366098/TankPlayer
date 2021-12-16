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
    public InputField roomName;     //�ж��W�ٿ�J��

    string gameVersion = "1";   //��ܥثe�C������
    
    private GameObject defaultSpawnPoint;
    //�o�̬O��v�����H�������ܼ�
    
    public float followDistance;
    public float followHeight;

    private Camera camera;
    private Transform lookTarget;
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

        defaultSpawnPoint = new GameObject("Default SpawnPoint");
        defaultSpawnPoint.transform.position = new Vector3(0, 0, 0);
        defaultSpawnPoint.transform.SetParent(transform, false);
    }
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();   //��Unity�۰ʨϥ�PUN�]�w����
        PhotonNetwork.GameVersion = gameVersion;    //���޹C������
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Update()
    {
        UpdateCamera();
    }
    void UpdateCamera()
    {
        float horizontal = Input.GetAxis("Horizontal");
        
        if (localPlayer != null)
        {
            camera = Camera.main;
            lookTarget = localPlayer.transform;
            Vector3 horizontalVector = lookTarget.forward;
            //�Τ@�Ӯھ�input�ȹ��Y�y�б��઺�V�q
            horizontalVector = Quaternion.AngleAxis(horizontal, Vector3.up) * horizontalVector;
            horizontalVector.Normalize();
            
            Vector3 followPosition = lookTarget.position - horizontalVector * followDistance;
            followPosition.y += followHeight;
            Vector3 lookForward = lookTarget.position - followPosition;
            
            camera.transform.position = followPosition;
            camera.transform.forward = lookForward;
        }

    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!PhotonNetwork.InRoom)
        {
            return;
        }

        var spawnPoint = GetRandomSpawnPoint();
        //�ͦ��@�ӥ��a���a����b�]�w���H�������I��m
        localPlayer = PhotonNetwork.Instantiate("TankPlayer", spawnPoint.position, spawnPoint.rotation, 0);
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
    //�o��region�O�i�H��{���X�ϰ�����A��p�H�U�O�u����
    #region Utility 
    //�[�J�H�U�禡�Ψӧ���Ҧ������I
    public static List<GameObject> GetAllObjectsOfTypeInScene<T>()
    {
        var objectsInScene = new List<GameObject>();
        foreach (var go in (GameObject [])Resources.FindObjectsOfTypeAll(typeof(GameObject)))
        {
            if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave)
            {
                continue;
            }
            if (go.GetComponent<T>() != null)
                objectsInScene.Add(go);
        }
        return objectsInScene;
    }
    private Transform GetRandomSpawnPoint()
    {
        var spawnPoints = GetAllObjectsOfTypeInScene<SpawnPoint>();
        return spawnPoints.Count == 0 ? 
            defaultSpawnPoint.transform : spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)].transform;
    }
    #endregion
}
