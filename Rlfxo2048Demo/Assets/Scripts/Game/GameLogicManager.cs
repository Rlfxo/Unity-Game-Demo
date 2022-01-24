using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogicManager : MonoBehaviour{

    public GameObject[] n;

    bool inputWait, moveWait;

    int x, y, i;
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
                //Up
                if(gap.y > 0 && gap.x > -0.5f && gap.x < 0.5f) { Debug.Log("Up"); }
                //Down
                else if(gap.y < 0 && gap.x > -0.5f && gap.x < 0.5f) { }
                //Right
                else if(gap.x > 0 && gap.y > -0.5f && gap.y < 0.5f) { }
                //Left
                else if(gap.x < 0 && gap.y > -0.5f && gap.y < 0.5f){
                    for(y = 0; y < gridSize; y++){
                        for(x = gridSize - 1; x > 0; x--){
                            for(i = 0; i < x; i++){
                                MoveOrMerge(i + 1, y, i, y);
                            }
                        }
                    }
                }
                else return;

                if(moveWait) {
                    moveWait = false;
                    Spawn();
                }
            }
        }
    }

    void MoveOrMerge(int x1, int y1, int x2, int y2){
        // [x1, y2] now -> [x2, y2] target
        if(Grid[x2, y2] == null && Grid[x1, y1] != null){// 현제 좌표에 블럭이 있고 목표 좌표가 비어있을 때
            moveWait = true;
            //Grid[x1, y1].transform.position = Vector3.MoveTowards(Grid[x1, y1].transform.position, new Vector3(1.1f * x2 -2.2f, 1.1f * y2 -2.5f, 0), 10000);
            Grid[x1, y1].GetComponent<Moving>().Move(x2, y2);
            Grid[x2, y2] = Grid[x1, y2];
            Grid[x1, y1] = null;
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
