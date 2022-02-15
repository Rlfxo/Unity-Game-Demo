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
    public GameObject subMenu;
    public GameObject player;
    public Image NPCImg;
    public bool isAction = false;
    public int talkindex;

    void Start(){
        talkPanel.SetActive(isAction);
    }

    void Update(){
        if(Input.GetButtonDown("Cancel")){
            if(subMenu.activeSelf){
                subMenu.SetActive(false);
            }else{
                subMenu.SetActive(true);
            }
        }
    }

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
            talk.SetMsg(id, "");
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
            talk.SetMsg(id, tlakData.Split(':')[0]);

            NPCImg.sprite = talkManager.GetNPCImg(id, int.Parse(tlakData.Split(':')[1]));
            NPCImg.color = new Color(1, 1, 1, 1);
        }else {
            talk.SetMsg(id, tlakData);

            NPCImg.color = new Color(1, 1, 1, 0);
        }

        isAction = true;
        talkindex++;
    }
    public void GameSave(){
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        //player.x
        //player.y
        //Save point or Index
        PlayerPrefs.Save();

        subMenu.SetActive(false);
    }

    public void GameLoad(){
        if(!PlayerPrefs.HasKey("PlayerX")){
            return;
        }

        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");

        player.transform.position = new Vector3(x, y, 0);
    }

    public void GameQuit(){
        Application.Quit();
    }
}
