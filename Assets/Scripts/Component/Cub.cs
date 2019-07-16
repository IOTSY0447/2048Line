using System.Collections;
using System.Collections.Generic;
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
        GetComponent<Renderer> ().material.mainTexture = img;
    }
}