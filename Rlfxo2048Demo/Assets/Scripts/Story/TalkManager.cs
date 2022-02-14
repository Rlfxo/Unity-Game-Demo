using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> NPCImgData;
    public Sprite[] NPCImgArr;

    void Awake(){
        talkData = new Dictionary<int, string[]>();
        NPCImgData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    void GenerateData(){
        talkData.Add(5000, new string[] {"king aqwqwrq?:0", "sfasasddsd:1"});
        talkData.Add(4000, new string[] {"4000 mageasdasd", "sfasasddsd"});
        talkData.Add(3000, new string[] {"3000 wetwet", "sfasasddsd"});
        talkData.Add(2000, new string[] {"2000 qwerqwr", "sfasasddsd"});
        talkData.Add(1000, new string[] {"1000 trugwerrrrr", "sfasasddsd"});

        NPCImgData.Add(5000 + 0, NPCImgArr[0]);
        NPCImgData.Add(5000 + 1, NPCImgArr[1]);
    }

    public string GetTalk(int id, int talkindex){
        if(talkindex == talkData[id].Length){
            return null;
        }else {
            return talkData[id][talkindex];
        }
    }

    public Sprite GetNPCImg(int id, int ImgIndex){
        return NPCImgData[id + ImgIndex];
    }
}
