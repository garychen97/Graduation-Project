using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CharacterBehaviour : MonoBehaviour
{
    /*——————————————————————————————————————————————————————————————*/
    /*———————————————————————————— 函数声明—————————————————————————————*/
    /*——————————————————————————————————————————————————————————————*/

    private DragonBones.UnityArmatureComponent roleArmatureComponent;//龙骨组件

    public const float walkSpeed=1.5f;//人物移动速度
    public Vector2 currentPositon;//人物当前地址（两个变量有点绕，要用位置最好自己定义一个，这两个先别用）
    public Vector2 targetPosition;//人物当前移动目标地址
    public Vector3 moveDirection;  //当前点到鼠标点击位置的向量
    bool faceright = false;//控制朝向


    //动画控制相关
    public int animaType;//动画类型
    public int saveAnimaTypeChange;//防冲突变量（不用管）
    public int special_anima=0;//当值为0时可以循环行走或静止，当值为1进入非循环的动画
    

    //香蕉相关
    public GameObject banana_trigger;//香蕉触发器
    public GameObject banana;//香蕉实体

    //手机相关
    public GameObject phone_trigger;//手机触发点
    public GameObject phone_logo;//可交互图标（拿话框先用着了）
    public GameObject phone;//手机实体
    public GameObject phone_black_interface;//黑屏界面
    public GameObject phone_wechat_interface;//微信界面
    public GameObject phone_reply;//男主回复

    //对白相关
    public int set_aside=0;//要进行对白序号，0表示没在进行对白
    public static int now_aside=0;//当前进行到的最新的对白序号
    public GameObject text_frame;//对白框
    public Image text_image;//对白框左边头像
    public Sprite[] image_Number;//头像种类


    /*——————————————————————————————————————————————————————————————*/
    /*—————————————————————— START 函数 和 UPDATE 函数———————————————————————————*/
    /*——————————————————————————————————————————————————————————————*/

    void Start()
    {
        //初始化
        roleArmatureComponent = GetComponent<DragonBones.UnityArmatureComponent>();//抓龙骨
        animaType = 0;//一开始静止动画
        saveAnimaTypeChange = 0;//当前状态是静止
        targetPosition = new Vector3(this.transform.position.x+3, this.transform.position.y, this.transform.position.z);//目标地址设在主角左边3长度的地方，实现一开始自动走进场景
    }
    void Update()
    {
        AnimationController();
        //鼠标点击事件
        ClickDectect();
        //人物移动
        CharacterMove(walkSpeed, targetPosition);
        //人物转向
        changeDirection(targetPosition);
    }

    /*——————————————————————————————————————————————————————————————*/
    /*—————————————————————— 龙骨动画控制相关———————————————————————————————*/
    /*——————————————————————————————————————————————————————————————*/

    //龙骨动画控制相关
    void AnimationController()
    {   //如果跟上一桢的动画状态不同了，可变标志“isAnimaTypeChange”置为1

        //动画控制
        if (animaType != saveAnimaTypeChange)
        {
            switch (animaType) { 
          
                case 0://静止
                    roleArmatureComponent.animation.GotoAndPlayByFrame("待机动画", 0, 0);
                    break;
                case 1://行走
                    roleArmatureComponent.animation.GotoAndPlayByFrame("男主走路", 0, 0);
                    break;
                case 3://香蕉皮摔跤（播放一次）
                    roleArmatureComponent.animation.GotoAndPlayByFrame("摔倒起身", 0, 1);
                    break;
                case 4://掏出手机（播放一次）
                    roleArmatureComponent.animation.GotoAndPlayByFrame("掏出手机", 0, 1);
                    break;
                case 5://放回手机（播放一次）
                    roleArmatureComponent.animation.GotoAndPlayByFrame("放回手机", 0, 1);
                    break;

            }
            saveAnimaTypeChange = animaType;
            //动画回调事件（判断非循环动画完成）
            if (animaType == 3||animaType==5)
            {
                roleArmatureComponent.AddEventListener(DragonBones.EventObject.COMPLETE, (str, eventObject) => {
                    special_anima = 0; targetPosition = transform.position;
                });
            }
            if (animaType == 4)
            {
                // phone.transform.position += new Vector3(8 - phone.transform.position.x, -1 - transform.position.y, this.transform.position.z)*phonespeed*Time.deltaTime;
                phone.transform.DOMoveY(1, 1.5f);
                
            }

        }

    }

    /*——————————————————————————————————————————————————————————————*/
    /*—————————————————————— 鼠标点击相关相关———————————————————————————————*/
    /*——————————————————————————————————————————————————————————————*/

    //鼠标点击相关
    void ClickDectect()
    {
        //按下鼠标左键
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);//获取鼠标对应世界坐标
            //判断点击触发事件
            switch (set_aside)
            {
                //set_aside非零代表有对白交互，优先对白交互//
                //第一段对白
                case 1:
                    Text_faram_control(true);
                    Image_chenge("痛苦");
                    set_aside = 2;
                    now_aside = 2;
                    break;
                //第二段对白
                case 2:
                    Image_chenge("悲伤");
                    set_aside = 3;
                    now_aside = 3;
                    break;
                //跳出对白
                case 3:
                    Text_faram_control(false);
                    set_aside = 0;//第一、二段对白结束
                    break;
                //aside=0，对白结束，可以其他交互
                case 0:

                    //点击地板（碰地板获取坐标）
                    if (hit.collider.tag == "Ground")
                    {
                        Debug.Log("hit ground");
                        targetPosition = hit.point;//若有效性为真，该表人物当前移动目标位置
                    }

                    //手机事件
                    if (hit.collider.name == "对话图标")
                    {
                        animaType = 4;
                        phone_logo.SetActive(false);
                        phone_trigger.SetActive(false);
                    }
                    if (hit.collider.name == "手机黑屏")
                    {
                        Debug.Log(2);
                        phone_wechat_interface.SetActive(true);//跳出微信界面
                        phone_black_interface.SetActive(false);
                    }
                    if (hit.collider.name == "微信界面")
                    {
                        Debug.Log(1);
                        phone_reply.SetActive(true);//跳出男主的回复
                        SetAsideNumber(1);//转入对白1
                    }
                    if (hit.collider.name == "home键")
                    {
                        phone.transform.DOMoveY(-18, 1.5f);//按home键手机收回
                        animaType = 5;//播放收回手机动画

                    }
                    break;
            }
        }
    }

    /*——————————————————————————————————————————————————————————————*/
    /*—————————————————————— 人物触发碰撞体相关—————————————————————————————*/
    /*——————————————————————————————————————————————————————————————*/


    //人物触发碰撞体相关
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //手机事件
        if (collision.name == "手机触发点")
        {
            //循环动画暂停
            animaType = 0;
            special_anima = 1;
            //对话框图标出现
            phone_logo.SetActive(true);
            //phone_logo.transform.DOShakeRotation(0);

            //玩家点击交互点后 拿出手机——在鼠标点击相关里实现
        }
        if (collision.name == "香蕉触发点")
        {
            banana.transform.DOMoveX(7.0f, 0.5f);
            banana.transform.DOMoveY(-7, 0.5f);
            banana_trigger.SetActive(false);
        }
        //香蕉皮事件
        if (collision.name == "香蕉皮")
        {
            //摔跤
            special_anima = 1;
            animaType = 3;
            banana.GetComponent<Rigidbody2D>().AddForce(new Vector2(100, 30));
        }

    }

    /*——————————————————————————————————————————————————————————————*/
    /*———————————————————————— 人物移动相关相关———————————————————————————————*/
    /*——————————————————————————————————————————————————————————————*/
    //人物移动相关
    void CharacterMove(float speed,Vector3 target)
    {   
        moveDirection = target - this.transform.position;

        if (special_anima == 0)//当没有特殊动画（静止和行走以外）播放时，才可以行走
        {
            if (Vector3.Distance(transform.position, target) > 0.2f && Mathf.Abs(moveDirection.x) > 0.2)
            {
                this.transform.position += new Vector3(moveDirection.normalized.x, 0, 0) * speed * Time.deltaTime;
                animaType = 1;//行走
            }
            else
            {
                animaType = 0;//静止
            }
        }
    }

    /*——————————————————————————————————————————————————————————————*/
    /*———————————————————————— 功能函数相关———————————————————————————————*/
    /*——————————————————————————————————————————————————————————————*/



    void SetAsideNumber(int num)
    {
        //静态变量now_aside增长控制剧情不循环
        if (now_aside < num)
            set_aside = num;
    }//设置旁白set_aside值
    void Image_chenge(string name)
    {
        int i=0;
        switch (name)
        {
            case ("痛苦"):
                i = 0;
                break;
            case ("悲伤"):
                i = 1;
                break;
        }
        text_image.gameObject.GetComponent<SpriteRenderer>().sprite = image_Number[i];
    }//更换对白框左边头像
    void Text_faram_control(bool a)
    {
        text_frame.SetActive(a);//对白框出现或消失
        text_image.gameObject.SetActive(a);//对白框左边头像出现或消失
    }//控制对白框和头像出现和消失
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
    }//人物朝向（左/右）切换



}
