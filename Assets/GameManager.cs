using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public int timer;
    public Slider start_slider;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if(start_slider.IsActive()==true)//当点击开始激活加载条后
            start_slider.value += 0.8f;//加载条速度

        if (start_slider.value == start_slider.maxValue)//进度条满，转移Scene
        {
            SceneManager.LoadScene("scene1");//跳转场景
                                             //Invoke() 这是一个延迟执行函数，以后可能有用
        }
    }
}
