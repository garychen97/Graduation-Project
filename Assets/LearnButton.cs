using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class LearnButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GameObject button;//按钮
    public SpriteRenderer background;//开始界面
    public Slider start_slider;//加载条
    public Texture2D mouseTexture;//鼠标图片
    public void OnPointerEnter(PointerEventData eventData)//button悬停时换鼠标
    {
        Cursor.SetCursor(this.mouseTexture, Vector2.zero, CursorMode.Auto);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)//button离开时鼠标变回原形
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    public void Buttondown()//点击按钮事件
    {
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0.3f);//透明度降低
        start_slider.gameObject.SetActive(true);//激活加载条
        start_slider.value = 0;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);//鼠标变回原形
        button.SetActive(false);//按钮消失
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    

    // Update is called once per frame
    void Update()
    {

    }
}
