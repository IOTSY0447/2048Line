using System.Collections;
using System.Collections.Generic;
using komal.puremvc;
using UnityEngine;
using DG.Tweening;

public class Cub : ComponentEx, INotificationHandler {
    public Vector2 myCoordinate; //用于计算连线是否是横或者竖
    public int num; //当前对应的number
    GameContrProxy GameContrProxy;
    void Start () {
        GameContrProxy = this.facade.RetrieveProxy (ProxyNameEnum.GameContrProxy) as GameContrProxy;
    }
    void Update () {

    }
    void OnMouseUp () {
        GameContrProxy.onTouchuUp ();
    }
    //改变数字
    public void changNumber (int num) {
        this.num = num;
        string path = "numberImg/" + num;
        Texture img = Resources.Load (path) as Texture;
        if (img != null) {
            GetComponent<Renderer> ().material.mainTexture = img;
        } else {
            string ms = "未找到图片" + num;
            Debug.LogError (ms);
        }
    }
    //移动动画
    public void moveAnction(Vector2 myVec2,Vector3 toPos,float time){
        myCoordinate = myVec2;
        Sequence mySequence = DOTween.Sequence();//创建空序列 
        Tweener move1 = transform.DOMove(toPos, time);
        mySequence.Append(move1);
        mySequence.AppendCallback(()=>{
            transform.GetComponent<MeshDeformer>().AnalogTouch(toPos);
        });
    }
}