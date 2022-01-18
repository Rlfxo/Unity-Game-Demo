using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text talkText;
    public GameObject talkPanel;
    public GameObject scanObject;
    public bool isAction = false;
    // Start is called before the first frame update
    void Start()
    {
        talkPanel.SetActive(isAction);
    }

    // Update is called once per frame
    public void Action(GameObject scanObj)
    {
        if(isAction) {
            isAction = false;
        }else {
            isAction = true;
            scanObject = scanObj;
            talkText.text = scanObject.name + " 발견 함.";
        }
        talkPanel.SetActive(isAction);
    }
}
