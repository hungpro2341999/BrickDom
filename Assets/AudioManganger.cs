using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManganger : MonoBehaviour
{
    public static AudioManganger Ins;
    public List<AudioSource> List_Audio;
    // Start is called before the first frame update
    void Start()
    {
        if (Ins != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Ins = this;
        }
        PlaySound("BG");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public AudioSource PlaySound(string name)
    {
        for(int i = 0; i < List_Audio.Count; i++)
        {
            if (CtrlData.Ins.IsSound())
            {
                if(List_Audio[i].name == name)
                {
                    List_Audio[i].Play();
                } 
            }
        }
        return null;
    }
    
    public void MuteAll()
    {
        CtrlData.Ins.ChangeSound();
        AnimSetting.Ins.ChanceSound();
        if (CtrlData.Ins.IsSound())
        {
            Debug.Log("Not sMute");
            for (int i = 0; i < List_Audio.Count; i++)
            {
               
                          
                        List_Audio[i].mute = false;
                    
                
            }
        }
        else
        {
            Debug.Log("Mute");
            for (int i = 0; i < List_Audio.Count; i++)
            {
               
                  
                        List_Audio[i].mute = true;
                    
                
            }
        }
        
    }
}
