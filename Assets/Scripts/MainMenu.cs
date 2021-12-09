using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviourPunCallbacks
{
    static MainMenu instance;
    private GameObject m_ui;
    private Button m_joinGameButton;
    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(gameObject);
        //找到他底下的元件
        m_ui = transform.FindAnyChild<Transform>("UI").gameObject;
        m_joinGameButton = transform.FindAnyChild<Button>("JoinGameButton");

        m_ui.SetActive(true);
        m_joinGameButton.interactable = false;
    }
    public override void OnConnectedToMaster()
    {
        m_joinGameButton.interactable = true;
    }
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public override void OnDisable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        m_ui.SetActive(!PhotonNetwork.InRoom);
    }
}
