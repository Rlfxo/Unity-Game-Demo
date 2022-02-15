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
        talkData.Add(5000, new string[] {"뭐 필요한거 있나?:0", "천천히 둘러 보시게나:1"});
        talkData.Add(4000, new string[] {"5년이나 걸려서 왕국 마법사 고시에 합격했습니다. \n역시 왕국 마법사를 하는게 안전하고 든든하겠죠?:0"});
        talkData.Add(3000, new string[] {"아... 퇴근 하고 싶다.:0"});
        talkData.Add(2000, new string[] {"왕국 최고 연금술사가 되려고 상위 마법에 모든 것을 투자했어요. \n 하위 마법 따위 간단하고 누구나 배울 수 있는건 가치가 없잖아요?:0"});
        talkData.Add(1000, new string[] {"대충 왕 죽일 사람입니다.:0"});

        NPCImgData.Add(1000 + 0, NPCImgArr[0]);
        NPCImgData.Add(2000 + 0, NPCImgArr[1]);
        NPCImgData.Add(3000 + 0, NPCImgArr[2]);
        NPCImgData.Add(4000 + 0, NPCImgArr[3]);
        NPCImgData.Add(5000 + 0, NPCImgArr[4]);
        NPCImgData.Add(5000 + 1, NPCImgArr[5]);
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
