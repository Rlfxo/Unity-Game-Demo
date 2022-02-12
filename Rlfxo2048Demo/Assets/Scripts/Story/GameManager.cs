using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text talkText;
    public TalkManager talkManager;
    public GameObject talkPanel;
    public GameObject scanObject;
    public bool isAction = false;
    public int talkindex;

    void Start(){
        talkPanel.SetActive(isAction);
    }

    // Update is called once per frame
    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData.id, objData.isNPC);
        
        talkPanel.SetActive(isAction);
    }

    void Talk(int id, bool isNPC){
        string tlakData = talkManager.GetTalk(id, talkindex);

        if(tlakData == null){
            isAction = false;
            talkindex = 0;
            return;
        }

        if(isNPC){
            talkText.text = tlakData;
        }else {
            talkText.text = tlakData;
        }

        isAction = true;
        talkindex++;
    }
}
