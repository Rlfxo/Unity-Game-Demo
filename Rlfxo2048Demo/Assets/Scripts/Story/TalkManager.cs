using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    void Awake(){
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData(){
        talkData.Add(5000, new string[] {"king aqwqwrqr", "sfasasddsd"});
        talkData.Add(4000, new string[] {"4000 mageasdasd", "sfasasddsd"});
        talkData.Add(3000, new string[] {"3000 wetwet", "sfasasddsd"});
        talkData.Add(2000, new string[] {"2000 qwerqwr", "sfasasddsd"});
        talkData.Add(1000, new string[] {"1000 trugwerrrrr", "sfasasddsd"});
    }

    public string GetTalk(int id, int talkindex){
        if(talkindex == talkData[id].Length){
            return null;
        }else {
            return talkData[id][talkindex];
        }
    }
}
