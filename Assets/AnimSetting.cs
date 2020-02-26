using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class AnimSetting : MonoBehaviour
{
    public RectTransform Status1;
    public RectTransform Status2;
    public RectTransform Status3;
    public static bool IsPause = false;

    

    

    public Transform Btn_Pause;
    public Transform Btn_Resume;

    //Sound
    public RectTransform OnSound;
    public RectTransform OffSoud;
   // Start is called before the first frame update
   void Start()
    {

       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeStatus();
        }
       
    }

    public void ChangeStatus()
    {
        IsPause = !IsPause;
        if (IsPause)
        {
            Pause();
        }
        else
        {
            Continue();
        }
    }
   
    public void Pause()
    {
        Btn_Pause.gameObject.SetActive(true);
        Btn_Resume.gameObject.SetActive(false);
        Status2.localScale = Vector3.one*0.5f;
        Status2.gameObject.SetActive(false);
        Status1.DOAnchorPos(new Vector2(-252f, 516f), 0.5f).OnComplete(()=>
        {
            Status3.gameObject.SetActive(false);
            Status2.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f).OnComplete
            (
            () =>
            {
                Status2.DOScale(new Vector3(1f, 1f, 1f),0.2f);
            }

            );
            Status2.DOAnchorPos(new Vector3(77f, 524, 1), 0.2f);
            Status2.gameObject.SetActive(true);
        });
    }
    public void Continue()
    {
        Btn_Pause.gameObject.SetActive(false);
        Btn_Resume.gameObject.SetActive(true);
        Status3.gameObject.SetActive(true);
        Status1.DOAnchorPos(new Vector2(-277f, 516), 0.2f);
        Status2.DOScale(new Vector3(1.15f, 1.15f, 1f), 0.2f).OnComplete

            (
                       () =>
                       {
                           Status1.DOAnchorPos(new Vector3(2, 516), 0.5f);
                           Status2.DOScale(new Vector3(0.3f, 0.3f, 1f), 0.1f).OnComplete
                           (
                            () =>
                            {
                                Status2.gameObject.SetActive(false);
                            }
                           );
                       }
            );
       
    }
    public void ChanceSound()
    {
        CtrlData.Ins.ChangeSound();

        if (CtrlData.Ins.IsSound())
        {
            OnSound.gameObject.SetActive(true);
            OffSoud.gameObject.SetActive(false);
        }
        else
        {
            OnSound.gameObject.SetActive(false);
            OffSoud.gameObject.SetActive(true);
        }
    }
    private void OnEnable()
    {
        if (CtrlData.Ins.IsSound())
        {
            OnSound.gameObject.SetActive(true);
            OffSoud.gameObject.SetActive(false);
        }
        else
        {
            OnSound.gameObject.SetActive(false);
            OffSoud.gameObject.SetActive(true);
        }
    }
}
