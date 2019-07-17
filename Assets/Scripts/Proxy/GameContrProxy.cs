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
    public List<GameObject> arrLineNode = new List<GameObject> (); //正在显示的线条
    public ControlBox ControlBox; //交互的脚本
    public List<GameObject> arrCubInView = new List<GameObject> (); //在界面的cub
    public bool isRound = false; //标记当前连线是否形成闭环了
    public float cubDownAnctionTime = 0.5f; //消除时方块下落的时间

    //获取新生成方块的随机数
    public int getRandomNum () {
        return arrGetNum[Random.Range (0, arrGetNum.Length)];
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
            if (isRound) { //如果闭环了
                arrPassCub.RemoveAt (0);
            }
            int newNum = calculateLinkNum ();
            Transform lastCub = arrPassCub[arrPassCub.Count - 1];
            arrPassCub.RemoveAt (arrPassCub.Count - 1);
            lastCub.GetComponent<Cub> ().changNumber (newNum);
            refreshAllCubPos (arrPassCub,lastCub);
        }
        arrLineNode.ForEach (lineOne => {
            GetLinePool.PushObject (lineOne);
        });
        arrLineNode.Clear ();
        arrPassCub.Clear ();
        operateNum = null;
        isRound = false;
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
            case 6:
            case 7:
                return (int) operateNum * 8;
            case 8:
            case 9:
                return (int) operateNum * 16;
            default:
                return (int) operateNum * 32;
        }
    }

    //刷新所有方块的位置并生成需要补充位置的二维坐标序列
    void refreshAllCubPos (List<Transform> hidList,Transform lastCub) {
        // Transform lastCub = arrPassCub[arrPassCub.Count - 1];
        Vector3 lastV3 = lastCub.position; // 最后剩下的方块的坐标，其他放开需要兑入这里
        List<Vector2> arrHidVec2 = new List<Vector2> ();
        Dictionary<float, int> xToCount = new Dictionary<float, int> ();
        hidList.ForEach (hidOne => {
            Vector2 hidVec2 = hidOne.GetComponent<Cub> ().myCoordinate;
            arrHidVec2.Add (hidVec2);
            if (!xToCount.ContainsKey (hidVec2.x)) {
                xToCount.Add (hidVec2.x, 0);
            }
            xToCount[hidVec2.x]++;
        });
        arrCubInView.ForEach (cubOne => {
            if (cubOne.activeSelf) {
                Vector2 myVec2 = cubOne.GetComponent<Cub> ().myCoordinate;
                int downCount = 0;
                arrHidVec2.ForEach (hidVec2 => {
                    if (hidVec2.x == myVec2.x && hidVec2.y < myVec2.y) {
                        downCount++;
                    }
                });
                if (downCount != 0) {
                    Vector2 newVec2 = new Vector2 ((int) myVec2.x, (int) myVec2.y - downCount);
                    Vector3 newPos = getPosByMyCoord (newVec2);
                    if (cubOne.transform == lastCub) {
                        lastV3 = newPos;
                    }
                    cubOne.GetComponent<Cub> ().moveAnction (newVec2, newPos, cubDownAnctionTime);
                }
            }
        });
        List<Vector2> needAddVec2 = new List<Vector2> (); //需要补充的列表
        foreach (float x in xToCount.Keys) {
            int count = xToCount[x];
            for (int i = 0; i < count; i++) {
                Vector2 newV2 = new Vector2 (x, shu - 1 - i);
                needAddVec2.Add (newV2);
            }
        }
        addCubByList (needAddVec2);
        moveIntoCub (lastV3);
    }
    //兑入方法
    void moveIntoCub (Vector3 lastCubV3) {
        arrPassCub.ForEach (cubOne => {
            cubOne.GetComponent<Cub> ().moveIntoCub (lastCubV3, 0.5f, () => {
                GetCubPool.PushObject (cubOne.gameObject);
                arrCubInView.Remove (cubOne.gameObject);
            });
        });
    }
    //根据二维坐标序列添加方块添加方块
    public void addCubByList (List<Vector2> needAddVec2) {
        Dictionary<float, int> xToCount = new Dictionary<float, int> ();
        needAddVec2.ForEach (vec2 => {
            if (!xToCount.ContainsKey (vec2.x)) {
                xToCount.Add (vec2.x, 0);
            }
            xToCount[vec2.x]++;
        });
        needAddVec2.ForEach (vec2 => {
            Vector3 toV3 = getPosByMyCoord (vec2);
            Vector3 formV3 = new Vector3 (toV3.x, toV3.y + getOffsetYByCount (xToCount[vec2.x]), toV3.z);
            GameObject cub = ControlBox.CreatorCub ();
            int num = getRandomNum ();
            cub.transform.localScale = new Vector3 (1, 1, 1);
            cub.transform.position = formV3;
            arrCubInView.Add (cub);
            cub.GetComponent<Cub> ().changNumber (num);
            cub.GetComponent<Cub> ().moveAnction (vec2, toV3, 0.5f);
        });
    }
    //当触碰到cub时
    public void onTouchCub (Vector3 pos, Transform cub) {
        if (startCub == cub) { //如果触碰的还是当前的节点
            return;
        }
        if (linePrefabNow != null) {
            switch (checkedCanLink (cub)) { //判断是否可连
                case 0: //不可连
                case 1:
                case 4:
                case 5:
                    linePrefabNow.transform.GetComponent<DrawLineTool> ().drag (new Vector3 (0, pos.y, pos.z));
                    break;
                case 6: //可连
                    arrLineNode.Add (linePrefabNow);
                    linePrefabNow.transform.GetComponent<DrawLineTool> ().drag (cub.position);
                    linePrefabNow = null;
                    startCub = null;
                    addLine (cub);
                    break;
                case 2: //回退
                    GameObject line1 = arrLineNode[arrLineNode.Count - 1];
                    arrLineNode.Remove (line1);
                    GetLinePool.PushObject (line1);
                    GetLinePool.PushObject (linePrefabNow);
                    arrPassCub.RemoveAt (arrPassCub.Count - 1);
                    arrPassCub.RemoveAt (arrPassCub.Count - 1); //多移除一次，因为addLine会再添加一次
                    addLine (cub);
                    isRound = false;
                    break;
                case 3: //闭环
                    Debug.LogError ("形成闭环了");
                    isRound = true;
                    arrLineNode.Add (linePrefabNow);
                    linePrefabNow.transform.GetComponent<DrawLineTool> ().drag (cub.position);
                    linePrefabNow = null;
                    startCub = null;
                    addLine (cub);
                    break;
            }
        } else {
            addLine (cub);
        }
    }
    //检测是否可以加连  每帧计算  返回数字，数字大小为检测优先级
    //0数字不同。1横竖不同或间隔不为1。2已经连过并且是上一个，可回退。3形成闭环了。4已经连过了且不是上一个。5已经形成闭环了。6表示可以连接。
    public int checkedCanLink (Transform targetCub) {
        Cub startCom = startCub.GetComponent<Cub> ();
        Cub targetCom = targetCub.GetComponent<Cub> ();
        bool numIsSame = startCom.num == targetCom.num;
        if (!numIsSame) { //数字不同直接返回
            return 0;
        }
        bool isHeng = startCom.myCoordinate.x == targetCom.myCoordinate.x && Mathf.Abs (startCom.myCoordinate.y - targetCom.myCoordinate.y) == 1;
        bool isShu = startCom.myCoordinate.y == targetCom.myCoordinate.y && Mathf.Abs (startCom.myCoordinate.x - targetCom.myCoordinate.x) == 1;
        if (!(isHeng || isShu)) {
            return 1;
        }
        Transform cubOne = null;
        for (int i = 0; i < arrPassCub.Count; i++) {
            if (targetCub == arrPassCub[i]) {
                cubOne = arrPassCub[i];
                break;
            }
        }
        bool isBack = arrPassCub.Count >= 2 && cubOne == arrPassCub[arrPassCub.Count - 2];
        if (isBack) {
            return 2;
        }
        bool isGoRound = arrPassCub.Count > 3 && (numIsSame && (isHeng || isShu)) && targetCub == arrPassCub[0]; //是否封闭了//至少4个才能闭环
        if (isGoRound) {
            return 3;
        }
        bool isPass = cubOne != null;
        if (isPass) {
            return 4;
        }
        if (isRound) {
            return 5;
        }
        return 6;
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
        List<Vector2> needAddVec2 = new List<Vector2> ();
        for (int x = 0; x < heng; x++) {
            for (int y = 0; y < shu; y++) {
                Vector2 myVec2 = new Vector2 (x, y);
                needAddVec2.Add (myVec2);
            }
        }
        addCubByList (needAddVec2);
    }
    //通过自己定义的二维坐标获取位置
    Vector3 getPosByMyCoord (Vector2 vec2) {
        float pz = (vec2.x - (heng - 1) / 2) * (1 + padding);
        float py = (vec2.y - (shu - 1) / 2) * (1 + padding);
        return ControlBox.transform.TransformDirection (new Vector3 (0, py, pz));
    }
    //根据个数计算生成方块的偏移值
    float getOffsetYByCount (float count) {
        return (1 + padding) * count;
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