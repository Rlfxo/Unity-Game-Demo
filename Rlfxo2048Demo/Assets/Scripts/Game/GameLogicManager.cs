using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogicManager : MonoBehaviour{

    public GameObject[] n;

    int x,y;
    int gridSize = 5;
    GameObject[,] Grid = new GameObject[5, 5];
    void Start(){
        Spawn();
        Spawn();
    }

    void Update(){
        // 뒤로가기 -> 종료
        if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    void Spawn(){// Object 생성
        while(true){
            x = Random.Range(0,gridSize);
            y = Random.Range(0,gridSize);
            if(Grid[x, y] == null) break;
        }
        Grid[x,y] = Instantiate(Random.Range(0,5) > 0? n[0]:n[1], new Vector3(1.1f * x -2.2f, 1.1f * y -2.5f, 0), Quaternion.identity);
    }

    public void Restart(){// 재시작 (ReStart, TryAgain)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
