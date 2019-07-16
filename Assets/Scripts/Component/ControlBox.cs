using System.Collections;
using System.Collections.Generic;
using komal.puremvc;
using UnityEngine;

public class ControlBox : ComponentEx, INotificationHandler {
    public GameObject cubOne; //单个数字方体
    public GameObject linePrefab; //单条线
    [Range (2, 8)] public int heng = 5; //横有几个正方体
    [Range (2, 8)] public int shu = 5; //竖有几个正方体
    public float padding = 0.5f; //方形间的间距
    GameContrProxy GameContrProxy;
    ObjPoolProxy ObjPoolProxy;
    
    void Start () {
        GameContrProxy = this.facade.RetrieveProxy (ProxyNameEnum.GameContrProxy) as GameContrProxy;
        ObjPoolProxy = this.facade.RetrieveProxy (ProxyNameEnum.ObjPoolProxy) as ObjPoolProxy;
        GameContrProxy.ControlBox = this;
        InitBox ();
    }

    void Update () {

    }
    //创建或从缓冲池拿预制
    public GameObject CreatorCub () {
        ObjectPool cubPool = ObjPoolProxy.GetCubPool;
        GameObject cub = cubPool.GetObject ();
        if (cub == null) {
            cub = Instantiate (cubOne);
            cub.transform.parent = transform;
        }
        return cub;
    }
    //创建或从缓冲池拿预制
    public GameObject CreatorLine () {
        ObjectPool linePool = ObjPoolProxy.GetLinePool;
        GameObject line = linePool.GetObject ();
        if (line == null) {
            line = Instantiate (linePrefab);
            line.transform.parent = transform;
        }
        return line;
    }
    void InitBox () {
        for (int x = 0; x < heng; x++) {
            for (int y = 0; y < shu; y++) {
                GameObject cub = CreatorCub ();
                float pz = (x - (heng - 1) / 2) * (1 + padding);
                float py = (y - (shu - 1) / 2) * (1 + padding);
                cub.GetComponent<Cub> ().myCoordinate = new Vector2 (x, y);
                int num = GameContrProxy.getRandomNum ();
                cub.GetComponent<Cub> ().changNumber (num);
                cub.transform.localPosition = new Vector3 (0, py, pz);
            }
        }
    }
}