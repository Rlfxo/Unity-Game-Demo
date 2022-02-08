using UnityEngine;

public class Moving : MonoBehaviour
{
    bool move, _merge;
    int _x2, _y2;
    float moveSpeed = 0.5f;
    void Update(){
        if(move) Move(_x2, _y2, _merge);
    }

    public void Move(int x2, int y2, bool merge){
        move = true;
        _x2 = x2;
        _y2 = y2;
        _merge = merge;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(1.94f * x2 -3.87f, 1.94f * y2 -3.0f, 0), moveSpeed);
        if (transform.position == new Vector3(1.94f * x2 -3.87f, 1.94f * y2 -3.0f, 0)){
            move = false;
            if(merge){
                _merge = false;
                Destroy(gameObject);
            }
        }
    }
}
