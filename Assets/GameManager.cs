using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Ins = null;
    public List<Windown> Windowns = new List<Windown>();
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
        OpenWindow(TypeWindow.Loading);
    }
    public bool isGameOver = false;
    public bool isGamePause = false;
    public bool isStartGame = false;
    

    // Start is called before the first frame update
    void Start()
    {
        
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
        OpenWindow(TypeWindow.GamePlay);
        CtrlGamePlay.Ins.Start_Game();
      
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
  
}
