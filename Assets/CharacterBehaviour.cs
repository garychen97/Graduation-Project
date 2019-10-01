using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterBehaviour : MonoBehaviour
{

    private DragonBones.UnityArmatureComponent roleArmatureComponent;//龙骨组件

    private const float walkSpeed=5;//移动速度
    private Vector2 currentPositon;//人物当前移动目标地址
    private Vector2 targetPosition=new Vector3(0,0,0);//人物当前移动目标地址
    bool faceright = false;
    private int animaType;
    private int saveAnimaTypeChange;
    // Start is called before the first frame update
    void Start()
    {
        roleArmatureComponent = GetComponent<DragonBones.UnityArmatureComponent>();
        animaType = 0;
        saveAnimaTypeChange = 0;
    }

    // Update is called once per frame
    void Update()
    {
        AnimationController();
        //每帧初始化
        //FrameIinitialization();
        //鼠标点击事件
        ClickDectect();
        //人物移动
        CharacterMove(walkSpeed,this.transform.position, targetPosition);
        //人物转向
        changeDirection(targetPosition);
    }

    //每帧初始化
    //void FrameIinitialization()
    //{
    //    currentPositon = this.transform.position;
    //}
    //鼠标点击相关
    //龙骨动画控制
    void AnimationController()
    {   //如果跟上一桢的动画状态不同了，可变标志“isAnimaTypeChange”置为1
        if (animaType != saveAnimaTypeChange)
        {
            switch (animaType)
            {
                case 0:
                    roleArmatureComponent.animation.GotoAndPlayByFrame("待机动画", 0, 0);
                    break;
                case 1:
                    roleArmatureComponent.animation.GotoAndPlayByFrame("男主走路", 0, 0);
                    break;
            }
            saveAnimaTypeChange = animaType;
        }
        

    }



    void ClickDectect()
    {
        //按下鼠标左键
        if (Input.GetMouseButtonDown(0))
        {   
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);//获取鼠标对应世界坐标

            if (hit.collider.tag=="Ground")//碰地板获取坐标
            {   
                targetPosition = hit.point;//若有效性为真，该表人物当前移动目标位置
                Debug.Log(hit.point);
            }

        }
    }

    //人物移动
    void CharacterMove(float speed,Vector3 start,Vector3 target)
    {
        Vector3 moveDirection = target - start;
        if (Vector3.Distance(transform.position, target) > 0.2f)
        {
            this.transform.position += moveDirection.normalized * speed * Time.deltaTime;
            animaType = 1;
        }
        else
        {
            animaType = 0;
        }
        //设置enum状态来控制动画



    }
    //人物朝向（左/右）切换
    void changeDirection(Vector3 target)
    {
        if (this.transform.position.x < target.x&&faceright==false)
        {
            this.transform.localScale =new Vector3 (-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
            faceright = true;
        }
        else if(this.transform.position.x > target.x && faceright == true)
        {
            this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
            faceright = false;
        }
    }
}
