using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoadingWindow : MonoBehaviour
{
    public Image LoadingBar;
    public float time;
    public float Speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        LoadingBar.fillAmount = 0;
        StartCoroutine(LoadingGame());
    }
    private void OnDisable()
    {
        LoadingBar.fillAmount = 0;
    }
    public IEnumerator LoadingGame()
    {
        ReadFile.time = 0;
        while (LoadingBar.fillAmount!=1)
        {
                  LoadingBar.fillAmount += Random.Range(0,0.08f);
               //LoadingBar.fillAmount = 1;
                 yield return new WaitForSeconds(Random.Range(0,0.2f));
        }
        ReadFile.time = 0.5f;
        GameManager.Ins.ActiveScreen(TypeScreen.PlayScreen);




    }
}
