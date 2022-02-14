using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour{
    Text msgText;
    AudioSource audioSource;
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

    public void SetMsg(string msg){
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
