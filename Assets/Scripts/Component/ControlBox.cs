using System.Collections;
using System.Collections.Generic;
using komal.puremvc;
using UnityEngine;

public class ControlBox : ComponentEx, INotificationHandler {
    public GameObject cubOne; //单个数字方体
    public GameObject linePrefab; //单条线
    GameContrProxy GameContrProxy;
    void Start () {
        GameContrProxy = this.facade.RetrieveProxy (ProxyNameEnum.GameContrProxy) as GameContrProxy;
        GameContrProxy.ControlBox = this;
        GameContrProxy.InitBox ();
    }

    void Update () {

    }
    //创建或从缓冲池拿预制
    public GameObject CreatorCub () {
        ObjectPool cubPool = GameContrProxy.GetCubPool;
        GameObject cub = cubPool.GetObject ();
        if (cub == null) {
            cub = Instantiate (cubOne);
            cub.transform.parent = transform;
        }
        return cub;
    }
    //创建或从缓冲池拿预制
    public GameObject CreatorLine () {
        ObjectPool linePool = GameContrProxy.GetLinePool;
        GameObject line = linePool.GetObject ();
        if (line == null) {
            line = Instantiate (linePrefab);
            line.transform.parent = transform;
        }
        return line;
    }

}