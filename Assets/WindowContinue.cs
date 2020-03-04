using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WindowContinue : Windown
{
    public float delaytime = 0.4f;
    public float time = 0;
   
    public Text textCount;
    private float delay;
    public int count = 0;
    public bool Complete = false;
    public Image Count;
    public List<Sprite> CountText = new List<Sprite>();
    public RectTransform AnimCountText;
    // Start is called before the first frame update
    private void Awake()
    {
    
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!Complete)
        {
            if (delay >= 0)
            {
               
                delay -= Time.deltaTime;
                AnimCountText.localScale = Vector2.one;
            }
            else
            {
                if (count == 5)
                {
                    Complete = true;
                }
                Count.sprite = CountText[count];
                StartAnim();
                delay = delaytime;
               
                count++;
             
            }
        }    
    }

    private void OnEnable()
    {
        AudioManganger.Ins.PlaySound("Continue");
        AnimCountText.localScale = Vector2.one;
        Complete = false;
        count = 0;

    }
    private void OnDisable()
    {
        Count.sprite = CountText[0];
    }
    public void StartAnim()
    {
        AnimCountText.DOScale(new Vector3(1, 1, 1), 0.5f);
    }
}
