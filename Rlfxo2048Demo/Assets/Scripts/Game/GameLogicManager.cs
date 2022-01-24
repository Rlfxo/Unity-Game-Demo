using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogicManager : MonoBehaviour{

    public GameObject[] n;

    bool inputWait;

    int x,y;
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
                else if(gap.x < 0 && gap.y > -0.5f && gap.y < 0.5f) { }
                else return;
            }
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
