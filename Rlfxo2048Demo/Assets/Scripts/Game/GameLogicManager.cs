using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLogicManager : MonoBehaviour{

    public GameObject[] n;
    public GameObject Quit;
    public Text Score, Plus;
    bool inputWait, moveWait;
    bool stop = false;

    int HP = 100;

    int x, y, i;// Block Move
    int j;// Monster Level
    int k, l;// Game Over
    int score;
    int gridSize = 5;
    Vector3 firstPos, gap;
    GameObject[,] Grid = new GameObject[5, 5];
    void Start(){
        Score.text = HP.ToString();
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
        }
        if(Input.GetMouseButton(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)){
            gap = (Input.GetMouseButton(0)? Input.mousePosition : (Vector3)Input.GetTouch(0).position) - firstPos;
            if(gap.magnitude < 100) return;
            gap.Normalize();

            if(inputWait) {
                inputWait = false;

                if(gap.y > 0 && gap.x > -0.5f && gap.x < 0.5f){// Up
                    for(x = 0; x < gridSize; x++){
                        for(y = 0; y < gridSize-1; y++){
                            for(i = gridSize-1; i > y; i--){
                                MoveOrMerge(x, i - 1, x, i);
                            }
                        }
                    }
                }
                else if(gap.y < 0 && gap.x > -0.5f && gap.x < 0.5f){// Down
                    for(x = 0; x < gridSize; x++){
                        for(y = gridSize-1; y > 0; y--){
                            for(i = 0; i < y; i++){
                                MoveOrMerge(x, i + 1, x, i);
                            }
                        }
                    }
                }
                else if(gap.x > 0 && gap.y > -0.5f && gap.y < 0.5f){// Right
                    for(y = 0; y < gridSize; y++){
                        for(x = 0; x < gridSize-1; x++){
                            for(i = gridSize-1; i > x; i--){
                                MoveOrMerge(i - 1, y, i, y);
                            }
                        }
                    }
                }
                else if(gap.x < 0 && gap.y > -0.5f && gap.y < 0.5f){// Left
                    for(y = 0; y < gridSize; y++){
                        for(x = gridSize-1; x > 0; x--){
                            for(i = 0; i < x; i++){
                                MoveOrMerge(i + 1, y, i, y);
                            }
                        }
                    }
                }
                else return;

                if(moveWait) {// Move -> Spawn
                    moveWait = false;
                    Spawn();

                    k = 0;
                    j = 0;

                    // score 연산
                    if(score > 0){
                        Plus.text = "-" + score.ToString() + "    ";
                        Plus.GetComponent<Animator>().SetTrigger("PlusBack");
                        Plus.GetComponent<Animator>().SetTrigger("Plus");
                        Score.text = (int.Parse(Score.text) - score).ToString();
                        score = 0;
                    }

                    for(x = 0; x < gridSize; x++){
                        for(y = 0; y < gridSize; y++){
                            if(Grid[x, y] == null) {
                                k++;// 빈 타일의 수
                                continue;
                            }
                            if(Grid[x, y].tag == "Combine") Grid[x, y].tag = "Untagged";
                        }
                    }
                    if(k == 0){
                        for(y = 0; y < 5; y++){//가로에 결합가능한 블럭 확인
                            for(x = 0; x < 5-1; x++){
                                if(Grid[x, y].name == Grid[x + 1, y].name) l++;
                            }
                        }
                        for(x = 0; x < 5; x++){//세로에 결합가능한 블럭 확인
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
            }
        }
    }

    void MoveOrMerge(int x1, int y1, int x2, int y2){
        // [x1, y2] now -> [x2, y2] target
        if(Grid[x2, y2] == null && Grid[x1, y1] != null){// 현제 좌표에 블럭이 있고 목표 좌표가 비어있을 때
            moveWait = true;
            //Grid[x1, y1].transform.position = Vector3.MoveTowards(Grid[x1, y1].transform.position, new Vector3(1.1f * x2 -2.2f, 1.1f * y2 -2.5f, 0), 10000);
            Grid[x1, y1].GetComponent<Moving>().Move(x2, y2, false);
            Grid[x2, y2] = Grid[x1, y1];
            Grid[x1, y1] = null;
        }

        if(Grid[x1, y1] != null && Grid[x2, y2] != null && Grid[x1, y1].name == Grid[x2, y2].name && Grid[x1, y1].tag != "Combine" && Grid[x2, y2].tag != "Combine"){// 현재 좌표의 블럭이 목표 좌표의 블럭과 병합
            moveWait = true;
            for(j = 0; j <5; j++){// Monster Level Check
                if(Grid[x2, y2].name == n[j].name + "(Clone)"){
                    break;
                }
            }
            Grid[x1, y1].GetComponent<Moving>().Move(x2, y2, true);
            Destroy(Grid[x2, y2]);
            Grid[x1, y1] = null;
            Grid[x2, y2] = Instantiate(n[j+1], new Vector3(1.1f * x2 -2.2f, 1.1f * y2 -2.5f, 0), Quaternion.identity);
            Grid[x2, y2].tag = "Combine";
            Grid[x2, y2].GetComponent<Animator>().SetTrigger("Merge");

            score += (int)Mathf.Pow(2, j + 2); // 제곱 값 구하기
        }
    }

    void Spawn(){// Object 생성
        while(true){
            x = Random.Range(0,gridSize);
            y = Random.Range(0,gridSize);
            if(Grid[x, y] == null) break;
        }
        Grid[x, y] = Instantiate(Random.Range(0,5) > 0? n[0]:n[1], new Vector3(1.1f * x -2.2f, 1.1f * y -2.5f, 0), Quaternion.identity);
        Grid[x, y].GetComponent<Animator>().SetTrigger("Spawn");
    }

    public void Restart(){// 재시작 (ReStart, TryAgain)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
