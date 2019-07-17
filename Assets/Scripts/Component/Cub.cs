using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using komal.puremvc;
using UnityEngine;

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
    //移动动画 有点弹性
    public void moveAnction (Vector2 myVec2, Vector3 toPos, float time) {
        myCoordinate = myVec2;
        Vector3 dpos = toPos - transform.position;
        Vector3 toPos1 = toPos + dpos * 0.03f;
        Vector3 toPos2 = toPos - dpos * 0.05f;
        Vector3 toPos3 = toPos;
        Sequence mySequence = DOTween.Sequence (); //创建空序列 
        Tweener move1 = transform.DOMove (toPos1, time);
        Tweener move2 = transform.DOMove (toPos2, time * 0.4f);
        Tweener move3 = transform.DOMove (toPos3, time * 0.2f);
        mySequence.Append (move1);
        mySequence.AppendCallback (() => {
            transform.GetComponent<MeshDeformer> ().AnalogTouch (toPos);
        });
        mySequence.Append (move2);
        mySequence.Append (move3);
    }
    //兑入动画 
    public void moveIntoCub2 (Transform toCub, float time, System.Action callback) {
        transform.localScale = new Vector3 (1, 1, 1) * 0.6f;
        Transform beforeParent = transform.parent;
        transform.parent = toCub;
        Sequence mySequence = DOTween.Sequence (); //创建空序列 
        Tweener move = transform.DOLocalMove (Vector3.zero, time);
        mySequence.Append (move);
        mySequence.AppendCallback (() => {
            transform.parent = beforeParent;
            callback ();
        });
    }
}