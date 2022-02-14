using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextEffect talk;
    public TalkManager talkManager;
    public GameObject talkPanel;
    public GameObject scanObject;
    public Image NPCImg;
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
        string tlakData = "";

        if(talk.isAnim){
            talk.SetMsg("");
            return;
        }else {
            tlakData = talkManager.GetTalk(id, talkindex);
        }

        if(tlakData == null){
            isAction = false;
            talkindex = 0;
            return;
        }

        if(isNPC){
            talk.SetMsg(tlakData.Split(':')[0]);

            NPCImg.sprite = talkManager.GetNPCImg(id, int.Parse(tlakData.Split(':')[1]));
            NPCImg.color = new Color(1, 1, 1, 1);
        }else {
            talk.SetMsg(tlakData);

            NPCImg.color = new Color(1, 1, 1, 0);
        }

        isAction = true;
        talkindex++;
    }
}
