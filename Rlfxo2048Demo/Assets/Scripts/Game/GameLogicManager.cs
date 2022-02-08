using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLogicManager : MonoBehaviour{
    int LessHP = 100;
    int StageGoal = 10;
    int nokori = 10;

    public GameObject[] n;
    public GameObject Quit;
    public Text HP, Damage, Nokori;
    bool inputWait, moveWait;
    bool stop = false;

    string PlayerDir = "";
    string InputDir = "";
    int damage;

    int x, y, i;// Block Move
    int j;// Monster Level
    int k, l;// Game Over

    int gridSize = 5;
    Vector3 firstPos, gap;
    GameObject[,] Grid = new GameObject[5, 5];

    void Start(){// init
        HP.text = LessHP.ToString();
        Nokori.text = StageGoal.ToString() + "/" + StageGoal.ToString();
        PlayerSpawn();
        Spawn();
        Spawn();
    }

    void Update(){
        // 뒤로가기 -> 종료
        if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if(stop) return;

        // 입력
        if(Input.GetMouseButtonDown(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)){
            inputWait = true;
            firstPos = Input.GetMouseButtonDown(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position;
        }else if (Input.anyKeyDown){
            inputWait = true;
            InputDir = "Pass";
        }
        
        if(InputDir == "Pass" || Input.GetMouseButton(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)){
            if(InputDir != "Pass"){
                gap = (Input.GetMouseButton(0)? Input.mousePosition : (Vector3)Input.GetTouch(0).position) - firstPos;
                if(gap.magnitude < 100) return;
                gap.Normalize();
            }else {
                gap = firstPos - firstPos;
                if(Input.GetKeyDown(KeyCode.UpArrow)){ InputDir = "Up"; }
                else if(Input.GetKeyDown(KeyCode.DownArrow)){ InputDir = "Down"; }
                else if(Input.GetKeyDown(KeyCode.RightArrow)){ InputDir = "Right"; }
                else if(Input.GetKeyDown(KeyCode.LeftArrow)){ InputDir = "Left"; }
            }

            if(inputWait) {
                inputWait = false;

                if(InputDir == "Up" || gap.y > 0 && gap.x > -0.5f && gap.x < 0.5f){ UpMove(); }
                else if(InputDir == "Down" || gap.y < 0 && gap.x > -0.5f && gap.x < 0.5f){ DownMove(); }
                else if(InputDir == "Right" || gap.x > 0 && gap.y > -0.5f && gap.y < 0.5f){ RightMove(); }
                else if(InputDir == "Left" || gap.x < 0 && gap.y > -0.5f && gap.y < 0.5f){ LeftMove(); }
                else return;

                if(moveWait) {// Move -> Spawn
                    moveWait = false;
                    Spawn();

                    // damage 연산
                    if(damage > 0){
                        Damage.text = "-" + damage.ToString() + "    ";
                        Damage.GetComponent<Animator>().SetTrigger("PlusBack");
                        Damage.GetComponent<Animator>().SetTrigger("Plus");
                        HP.text = (int.Parse(HP.text) - damage).ToString();
                        damage = 0;
                        if(int.Parse(HP.text) < 0){// Hp0 -> GameOver -> tryAgain
                            HP.text = "0";
                            stop = true;
                            Quit.SetActive(true);
                            return;
                        }
                    }
                    for(x = 0; x < gridSize; x++){// Grid tag Reset
                        for(y = 0; y < gridSize; y++){
                            if(Grid[x, y] == null) continue;
                            if(Grid[x, y].tag == "Combine" && Grid[x, y].name != n[4].name + "(Clone)") Grid[x, y].tag = "Untagged";
                            if(Grid[x, y].tag == "PlayerDone") Grid[x, y].tag = "Player";
                        }
                    }
                }
                InputDir = "";
            }
        }
    }

    void MoveOrMerge(int x1, int y1, int x2, int y2){
        // [x1, y2] now -> [x2, y2] target
        if(Grid[x2, y2] == null && Grid[x1, y1] != null){// 현제 좌표에 블럭이 있고 목표 좌표가 비어있을 때
            moveWait = true;
            //Grid[x1, y1].transform.position = Vector3.MoveTowards(Grid[x1, y1].transform.position, new Vector3(1.1f * x2 -2.2f, 1.1f * y2 -2.5f, 0), 10000);
            Grid[x1, y1].GetComponent<Moving>().Move(x2, y2, false);
            if(Grid[x1, y1].tag == "Player"){
                Grid[x1, y1].GetComponent<Animator>().SetTrigger(PlayerDir);
            }
            Grid[x2, y2] = Grid[x1, y1];
            Grid[x1, y1] = null;
        }
        
        if(Grid[x1, y1] != null && Grid[x1, y1].tag == "Player" && Grid[x2, y2] != null){// 내가 플레이어고 목표에 몬스터가 있으면 잡음
            moveWait = true;
            for(j = 0; j <5; j++){// Monster Level Check
                if(Grid[x2, y2].name == n[j].name + "(Clone)"){
                    break;
                }
            }
            Grid[x1, y1].GetComponent<Moving>().Move(x2, y2, true);
            Destroy(Grid[x2, y2]);
            Grid[x1, y1] = null;
            Grid[x2, y2] = Instantiate(n[5], new Vector3(1.94f * x2 -3.87f, 1.94f * y2 -3.0f, 0), Quaternion.identity);
            Grid[x2, y2].GetComponent<Animator>().SetTrigger(PlayerDir);
            Grid[x2, y2].tag = "PlayerDone";

            damage += (int)Mathf.Pow(2, j + 2); // 제곱 값 구하기
        }
        
        if(Grid[x1, y1] != null && Grid[x2, y2] != null && Grid[x1, y1].name == Grid[x2, y2].name && Grid[x1, y1].tag != "Combine" && Grid[x2, y2].tag != "Combine"){// 현재 좌표의 블럭이 목표 좌표의 블럭과 병합
            moveWait = true;
            for(j = 0; j <6; j++){// Monster Level Check
                if(Grid[x2, y2].name == n[j].name + "(Clone)"){
                    break;
                }
            }
            Grid[x1, y1].GetComponent<Moving>().Move(x2, y2, true);
            Destroy(Grid[x2, y2]);
            Grid[x1, y1] = null;
            Grid[x2, y2] = Instantiate(n[j+1], new Vector3(1.94f * x2 -3.87f, 1.94f * y2 -3.0f, 0), Quaternion.identity);
            Grid[x2, y2].tag = "Combine";
            //Grid[x2, y2].GetComponent<Animator>().SetTrigger("Merge");
        }
    }

    void UpMove(){
        PlayerDir = "Up";
        for(x = 0; x < gridSize; x++){
            for(y = 0; y < gridSize-1; y++){
                for(i = gridSize-1; i > y; i--){
                    MoveOrMerge(x, i - 1, x, i);
                }
            }
        }
    }
    void DownMove(){
        PlayerDir = "Down";
        for(x = 0; x < gridSize; x++){
            for(y = gridSize-1; y > 0; y--){
                for(i = 0; i < y; i++){
                    MoveOrMerge(x, i + 1, x, i);
                }
            }
        }
    }
    void RightMove(){
        PlayerDir = "Right";
        for(y = 0; y < gridSize; y++){
            for(x = 0; x < gridSize-1; x++){
                for(i = gridSize-1; i > x; i--){
                    MoveOrMerge(i - 1, y, i, y);
                }
            }
        }
    }
    void LeftMove(){
        PlayerDir = "Left";
        for(y = 0; y < gridSize; y++){
            for(x = gridSize-1; x > 0; x--){
                for(i = 0; i < x; i++){
                    MoveOrMerge(i + 1, y, i, y);
                }
            }
        }
    }

    void PlayerSpawn(){// Player Object 생성
        x = 2;
        y = 0;
        Grid[x, y] = Instantiate(n[5], new Vector3(1.94f * x -3.87f, 1.94f * y -3.0f, 0), Quaternion.identity);
        Grid[x, y].tag = "Player";
    }
    void Spawn(){// Monster Object 생성
        if (nokori == 0) return;
        while(true){
            x = Random.Range(0,gridSize);
            y = Random.Range(0,gridSize);
            if(Grid[x, y] == null) break;
        }
        Grid[x, y] = Instantiate(Random.Range(0,5) > 0? n[0]:n[1], new Vector3(1.94f * x -3.87f, 1.94f * y -3.0f, 0), Quaternion.identity);
        nokori--;
        Nokori.text = nokori.ToString() + "/" + StageGoal.ToString();
        //Grid[x, y].GetComponent<Animator>().SetTrigger("Spawn");
    }

    void Quit2048(){
        for(x = 0; x < gridSize; x++){
            for(y = 0; y < gridSize; y++){
                if(Grid[x, y] == null) {
                    k++;// 빈 타일의 수
                    continue;
                }// Max merge block
            }
        }
        if(k == 0){
            for(y = 0; y < 5; y++){// 가로에 결합가능한 블럭 확인
                for(x = 0; x < 5-1; x++){
                    if(Grid[x, y].name == Grid[x + 1, y].name) l++;
                }
            }
            for(x = 0; x < 5; x++){// 세로에 결합가능한 블럭 확인
                for(y = 0; y < 5-1; y++){
                    if(Grid[x, y].name == Grid[x, y + 1].name) l++;
                }
            }
            if(l == 0){// 종료
                stop = true;
                Quit.SetActive(true);
                return;
            }
        }
    }
    public void Restart(){// 재시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
