using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogicManager : MonoBehaviour{

    public GameObject[] n;

    bool inputWait, moveWait;

    int x, y, i;// Block Move
    int j;// Monster Level
    int gridSize = 5;
    Vector3 firstPos, gap;
    GameObject[,] Grid = new GameObject[5, 5];
    void Start(){
        Spawn();
        Spawn();
    }

    void Update(){
        // 뒤로가기 -> 종료
        if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
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

                    for(x = 0; x < gridSize; x++){
                        for(y = 0; y < gridSize; y++){
                            if(Grid[x, y] == null) continue;
                            if(Grid[x, y].tag == "Combine") Grid[x, y].tag = "Untagged";
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
