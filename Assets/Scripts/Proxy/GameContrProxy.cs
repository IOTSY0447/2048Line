//管理游戏逻辑
using System.Collections.Generic;
using komal.puremvc;
using UnityEngine;
class GameContrData {

}
public class GameContrProxy : Proxy {
    public GameContrProxy (string name, object obj) : base (name, obj) {

    }
    //-------------------------------------------预制池-------------------------------------------
    //线条预制池
    ObjectPool linePool = null;
    public ObjectPool GetLinePool {
        get {
            if (linePool == null) {
                linePool = new ObjectPool ();
            }
            return linePool;
        }
    }
    //方块预制池
    ObjectPool cubPool = null;
    public ObjectPool GetCubPool {
        get {
            if (cubPool == null) {
                cubPool = new ObjectPool ();
            }
            return cubPool;
        }
    }
    //-------------------------------------------数据部分-------------------------------------------
    [Range (2, 8)] public int heng = 5; //横有几个正方体
    [Range (2, 8)] public int shu = 5; //竖有几个正方体
    public float padding = 0.5f; //方形间的间距
    int[] arrGetNum = { 2, 4, 8 }; //用于生成的数字库
    public GameObject linePrefabNow = null; //当前在连的线段
    public Transform startCub = null; //当前在连的方块的起始方块
    public int? operateNum = null; //当前操作的数字
    public List<Transform> arrPassCub = new List<Transform> (); //连线经过的方块
    public List<GameObject> arrLineNode = new List<GameObject> (); //存正在显示的线条
    public ControlBox ControlBox; //交互的脚本
    public List<GameObject> arrCubInView = new List<GameObject> (); //在界面的cub

    //获取新生成方块的随机数
    public int getRandomNum () {
        return arrGetNum[Random.Range (0, arrGetNum.Length)];
    }
    //检测是否可以加连 ！！！每帧计算
    public bool checkedCanLink (Transform targetCub) {
        Cub startCom = startCub.GetComponent<Cub> ();
        Cub targetCom = targetCub.GetComponent<Cub> ();
        bool numIsSame = startCom.num == targetCom.num;
        if (!numIsSame) { //数字不同直接返回
            return false;
        }
        bool isHeng = startCom.myCoordinate.x == targetCom.myCoordinate.x && Mathf.Abs (startCom.myCoordinate.y - targetCom.myCoordinate.y) == 1;
        bool isShu = startCom.myCoordinate.y == targetCom.myCoordinate.y && Mathf.Abs (startCom.myCoordinate.x - targetCom.myCoordinate.x) == 1;
        Transform cubOne = null;
        for (int i = 0; i < arrPassCub.Count; i++) {
            if (targetCub == arrPassCub[i]) {
                cubOne = arrPassCub[i];
                break;
            }
        }
        bool isNoPass = cubOne == null;
        bool isGoRound = arrPassCub.Count > 3 && (numIsSame && (isHeng || isShu)) && targetCub == cubOne; //是否封闭了//至少4个才能闭环
        if (isGoRound) {
            Debug.LogError ("形成闭环了");
            return true;
        }
        return numIsSame && (isHeng || isShu) && isNoPass;
    }
    //当松开触摸时
    public void onTouchuUp () {
        if (linePrefabNow != null) {
            GetLinePool.PushObject (linePrefabNow);
        }
        linePrefabNow = null;
        startCub = null;
        linkAnction ();
    }
    //检测并连线并播放连线动画
    void linkAnction () {
        if (arrPassCub.Count > 1) {
            int newNum = calculateLinkNum ();
            Transform lastCub = arrPassCub[arrPassCub.Count - 1];
            arrPassCub.RemoveAt (arrPassCub.Count - 1);
            lastCub.GetComponent<Cub> ().changNumber (newNum);
            refreshAllCubPos (arrPassCub);
            arrPassCub.ForEach (cubOne => {
                GetCubPool.PushObject (cubOne.gameObject);
            });
        }
        arrLineNode.ForEach (lineOne => {
            GetLinePool.PushObject (lineOne);
        });
        arrLineNode.Clear ();
        arrPassCub.Clear ();
        operateNum = null;
    }
    //计算连线获得的值！！！临时算法
    int calculateLinkNum () {
        switch (arrPassCub.Count) {
            case 2:
            case 3:
                return (int) operateNum * 2;
            case 4:
            case 5:
                return (int) operateNum * 4;
            default:
                return 0;
        }
    }

    //刷新所有方块的位置
    void refreshAllCubPos (List<Transform> hidList) {
        Dictionary<GameObject, Vector3> moveData = new Dictionary<GameObject, Vector3> ();
        arrCubInView.ForEach (cubOne => {
            if (cubOne.activeSelf) {
                Vector2 myVec2 = cubOne.GetComponent<Cub> ().myCoordinate;
                int downCount = 0;
                hidList.ForEach (hidOne => {
                    Vector2 hidVec2 = hidOne.GetComponent<Cub> ().myCoordinate;
                    if (hidVec2.x == myVec2.x && hidVec2.y < myVec2.y) {
                        downCount++;
                    }
                });
                if (downCount != 0) {
                    Vector3 newPos = getVec2ByMyCoord ((int) myVec2.x, (int) myVec2.y - downCount);
                    moveData.Add (cubOne, newPos);
                }
            }
        });
        ControlBox.moveData = moveData;
        ControlBox.test = true;
    }
    //当触碰到cub时
    public void onTouchCub (Vector3 pos, Transform cub) {
        if (startCub == cub) { //如果触碰的还是当前的节点
            return;
        }
        if (linePrefabNow != null) {
            if (checkedCanLink (cub)) { //判断是否可连
                arrLineNode.Add (linePrefabNow);
                linePrefabNow.transform.GetComponent<DrawLineTool> ().drag (cub.position);
                linePrefabNow = null;
                startCub = null;
            } else {
                linePrefabNow.transform.GetComponent<DrawLineTool> ().drag (new Vector3 (0, pos.y, pos.z));
            }
        } else {
            addLine (cub);
        }
    }
    //添加一条线
    void addLine (Transform cub) {
        if (operateNum == null) {
            operateNum = cub.GetComponent<Cub> ().num;
        }
        startCub = cub;
        arrPassCub.Add (cub);
        GameObject line = ControlBox.CreatorLine ();
        line.transform.position = cub.position;
        line.transform.GetComponent<DrawLineTool> ().dradLine (0); //因为节点是重复利用的，所以需要把长度默认为0
        linePrefabNow = line;
    }
    //初始化
    public void InitBox () {
        for (int x = 0; x < heng; x++) {
            for (int y = 0; y < shu; y++) {
                GameObject cub = ControlBox.CreatorCub ();
                Vector2 myVec2 = new Vector2 (x, y);
                cub.GetComponent<Cub> ().myCoordinate = myVec2;
                int num = getRandomNum ();
                cub.GetComponent<Cub> ().changNumber (num);
                Vector3 pos = getVec2ByMyCoord (x, y);
                cub.transform.position = pos;
                arrCubInView.Add (cub);
            }
        }
    }
    //通过自己定义的二维坐标获取位置
    Vector3 getVec2ByMyCoord (int x, int y) {
        float pz = (x - (heng - 1) / 2) * (1 + padding);
        float py = (y - (shu - 1) / 2) * (1 + padding);
        return ControlBox.transform.TransformDirection (new Vector3 (0, py, pz));
    }
}
public class ObjectPool {
    private List<GameObject> objectList = new List<GameObject> ();
    //如果没有会返回空，空的话需要调用的地方自己实例化
    public GameObject GetObject () {
        GameObject obj = null;
        if (objectList.Count > 0) {
            obj = objectList[objectList.Count - 1];
            objectList.RemoveAt (objectList.Count - 1);
            obj.SetActive (true);
        }
        return obj;
    }
    public void PushObject (GameObject obj) {
        obj.SetActive (false);
        objectList.Add (obj);
    }
}