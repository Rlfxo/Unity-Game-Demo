using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour{
    Text msgText;
    AudioSource audioSource;
    public AudioClip[] sounds;
    public GameObject EndCursor;
    public int TextSpeed;
    public bool isAnim;

    float interval;
    string targetMsg;
    int index;
    public void Awake(){
        msgText = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetMsg(int id, string msg){
        if(id < 999){
            audioSource.clip = sounds[3];
        }else{
            if(id >= 5000){
                audioSource.clip = sounds[0];
            }else{
                audioSource.clip = sounds[1];
            }
        }

        if(isAnim) {
            msgText.text = targetMsg;
            CancelInvoke();
            EffectEnd();
        }else {
            targetMsg = msg;
            EffectStart();
        }
    }

    void EffectStart(){
        isAnim = true;

        msgText.text = "";
        index = 0;
        EndCursor.SetActive(false);

        interval = 1.0f/ TextSpeed;
        Invoke("Effecting", interval);
    }

    void Effecting(){
        if(msgText.text == targetMsg){
            EffectEnd();
            return;
        }
        msgText.text += targetMsg[index];

        if(targetMsg[index] != ' ' || targetMsg[index] != '.' || targetMsg[index] != '!' || targetMsg[index] != '?');
        audioSource.Play();

        index++;

        Invoke("Effecting", interval);
    }

    void EffectEnd(){
        isAnim = false;
        EndCursor.SetActive(true);
    }
}
