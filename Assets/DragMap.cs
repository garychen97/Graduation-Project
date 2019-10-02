using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DragMap : MonoBehaviour
{
    public bool can_drag=false;//能否拖拽
    public Vector3 cameraFromPos;//拖拽时相机起始坐标
    public Vector3 hitFromPos;//拖拽时鼠标起始坐标
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /*——————————————————————————————————————————————————————————————*/
    /*———————————————————————— 拖拽地图相关———————————————————————————————*/
    /*——————————————————————————————————————————————————————————————*/

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);//获取鼠标对应世界坐标
        if (Input.GetMouseButtonDown(1))
        {
            cameraFromPos = this.transform.position;//记录右键时相机坐标
            hitFromPos = new Vector3(hit.point.x,hit.point.y,0);//记录右键时鼠标位置
            can_drag = true;
        }
        
        if (can_drag == true)//相机移动实现拖拽
        {
            this.transform.position -= new Vector3(hit.point.x,hit.point.y,0) - hitFromPos;
        }
        if (Input.GetMouseButtonUp(1))//松开鼠标右键，回到相机记录原位置
        {
            can_drag = false;
            transform.DOLocalMoveX(cameraFromPos.x, 1);
            transform.DOLocalMoveY(cameraFromPos.y, 1);
        }
    }
}
