using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Ins = null;
    public List<Windown> Windowns = new List<Windown>();
    public List<Screen1> Screens = new List<Screen1>();
    private void Awake()
    {
        if (Ins != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Ins = this;
        }
        CloseAll();
        ActiveScreen(TypeScreen.LoadingScreen);
    }
    public bool isGameOver = false;
    public bool isGamePause = false;
    public bool isStartGame = false;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void ActiveScreen(TypeScreen type)
    {
        foreach(Screen1 s in Screens)
        {
            if (s.Type == type)
            {
                s.Open();
                OpenScreen(type);
            }
            else
            {
                s.Close();
            }
        }
    }

    public void OpenScreen(TypeScreen type)
    {
        
        switch (type)
        {
            case TypeScreen.LoadingScreen:

                isGameOver = true;
                isGameOver = true;
                break;

            case TypeScreen.PlayScreen:

                StartGame();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            CtrlGamePlay.Ins.Reset_Game();
        }
    }
    public void GameOver()
    {

    }
    public void Resume()
    {

    }

    public void PauseGame()
    {
        isGamePause = true;
    }

    public void StartGame()
    {
        isGameOver = false;
        isGameOver = false;
        OpenWindow(TypeWindow.GamePlay);
        CtrlGamePlay.Ins.Start_Game();
      
    }
    public void CloseAll()
    {
        foreach (Windown w in Windowns)
            w.Close();
    }
    public void OpenWindow(TypeWindow windown)
    {
        foreach(Windown w in Windowns)
        {
            if(w.TypeWindown == windown)
            {
                w.Open();
            }
            else
            {
                w.Close();
            }
        }
    }
    public void CloseWindow(TypeWindow windown)
    {
        foreach (Windown w in Windowns)
        {
            if (w.TypeWindown == windown)
            {
                w.Close();
            }
        }
    }
    public void RateUs()
    {
        ManagerAds.Ins.RateApp();
    }

    public void More_Game()
    {
        ManagerAds.Ins.MoreGame();
    }



}
