using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public Texture2D mouseTexture;//鼠标图片
    // Start is called before the first frame update
    void Start()
    {

    }

    /*——————————————————————————————————————————————————————————————*/
    /*———————————————————————— 碰撞体变鼠标———————————————————————————————*/
    /*——————————————————————————————————————————————————————————————*/

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);//获取鼠标对应世界坐标

        if (hit.collider.tag=="Untagged"||hit.collider.tag!="touch")//没碰到“tag=touch的物体”，鼠标不变
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
        if (hit.collider.tag == "touch")//碰到“tag标记为touch的物体”，变鼠标图标
        {
            Cursor.SetCursor(mouseTexture, Vector2.zero, CursorMode.Auto);
        }

    }



}
