﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour {
	// Use this for initialization

	private static GameController Instance;


    private bool hacking_ = false;
    public bool hacking
    {
        get { return hacking_; }
        set
        {
            hackingPanel.SetActive(value);
            hacking_ = value;
        }
    }

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	private bool _inPauseMenu = false;
	private bool _isGameOver = false;
	private bool _inElevatorMenu = false;

	public bool isGamePaused
	{
		get { return _inPauseMenu || _isGameOver; }
	}

	public bool inElevatorMenu
	{
		get { return _inElevatorMenu; }
	}


	public GameObject hackingTarget;
    public GameObject hackingPanel;

	private HashSet<string> keysInInventory = new HashSet<string>();
	public GameObject elevatorMenu;

	public GameObject gameOverMenuObject;
	private GameObject pauseMenuObject;
	private float originalTimeScale;

	public void AddKey(string key)
	{
		Debug.Log("Added key: " + key);
		keysInInventory.Add(key);
	}

	public bool HasKey(string key)
	{
		return keysInInventory.Contains(key);
	}

	public void GameOver()
	{
		_isGameOver = true;
		gameOverMenuObject.SetActive(true);
	}

	void Setup()
	{
		originalTimeScale = Time.timeScale;
		_isGameOver = false;

       
        if (Application.loadedLevel != 0)
		{
			foreach (MenuActions action in FindObjectsOfType<MenuActions>())
			{
				if (action.CompareTag("GameOverMenu"))
				{
					gameOverMenuObject = action.gameObject;
				}
				else
				{
					pauseMenuObject = action.gameObject;
					pauseMenuObject.SetActive(false);
				}
			}
		}

		
       
	}

	void OnLevelWasLoaded () {
		Setup();
	}
	
	void Start()
	{
		Setup();
	}

	// Update is called once per frame
	void Update () {
		
	}

    public void Hack(GameObject target) {
		if (!_isGameOver)
		{
			hacking = true;
			hackingTarget = target;
		}
    }
    public void StopHacking()
    {
        hacking = false;
        hackingTarget = null;
        Debug.Log("Stop hacking");
    }

	public void EnterMenu()
	{
		if (!_inElevatorMenu)
		{
			_inPauseMenu = true;
			pauseMenuObject.SetActive(true);
			Time.timeScale = 0;
		}
	}

	public void ExitMenu()
	{
		_inPauseMenu = false;
		pauseMenuObject.SetActive(false);
		Time.timeScale = originalTimeScale;
	}

	public void OpenElevatorMenu()
	{
		if (elevatorMenu)
		{
			elevatorMenu.SetActive(true);
			_inElevatorMenu = true;
		}
        else
        {
            Debug.LogError("Elevator fnot foudn ");
        }
	}

	public void CloseElevatorMenu()
	{
		if (elevatorMenu)
		{
			elevatorMenu.SetActive(false);
			_inElevatorMenu = false;
		}
	}

	public void ValidateHacking(HackingMinigame minigame)
    {
        Debug.Log("Solved");
        for (int i = 0; i < minigame.matchAmplitudes.Length; ++i)
            if (minigame.matchAmplitudes[i] != minigame.playerAmplitudes[i])
                return;
        switch (hackingTarget.tag)
        {
            case "Camera":
                hackingTarget.GetComponent<SurveilanceCam>().coneVision.SetActive(false);
                hackingTarget.GetComponent<SurveilanceCam>().enabled = false;
                hackingTarget.GetComponentInChildren<SphereCollider>().enabled = false;
                break;
            case "Last Computer":
                Debug.Log("Solved");
                SceneManager.LoadScene("Victory");
                break;
            default:
                Debug.Log(hackingTarget.tag);
                break;
        }
        //hackingTarget.SetActive(false);
        StopHacking();
        return;
    }
}
