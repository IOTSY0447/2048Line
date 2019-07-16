//管理游戏逻辑
using System.Collections.Generic;
using komal.puremvc;
using UnityEngine;
class GameContrData {

}
public class GameContrProxy : Proxy {
    public GameContrProxy (string name, object obj) : base (name, obj) {

    }
    int[] arrGetNum = { 2, 4, 8 }; //用于生成的数字库
    public GameObject linePrefabNow = null; //当前在连的线段
    public Transform startCub = null; //当前在连的方块的起始方块
    public List<Vector2> arrPassVec2 = new List<Vector2> (); //连线经过的二维点
    public List<GameObject> arrLineNode = new List<GameObject> (); //存正在显示的线条
    public ControlBox ControlBox; //交互的脚本

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
        Vector2? tVec2 = null;
        for (int i = 0; i < arrPassVec2.Count; i++) {
            if (Vector2.Equals (targetCom.myCoordinate, arrPassVec2[i])) {
                tVec2 = arrPassVec2[i];
                break;
            }
        }
        bool isNoPass = tVec2 == null;
        bool isGoRound = arrPassVec2.Count > 3 && (numIsSame && (isHeng || isShu)) && Vector2.Equals (targetCom.myCoordinate, arrPassVec2[0]); //是否封闭了//至少4个才能闭环
        if (isGoRound) {
            Debug.LogError ("形成闭环了");
            return true;
        }
        return numIsSame && (isHeng || isShu) && isNoPass;
    }
    //当松开触摸时
    public void onTouchuUp () {
        if (linePrefabNow != null) {
            ObjPoolProxy ObjPoolProxy = this.facade.RetrieveProxy (ProxyNameEnum.ObjPoolProxy) as ObjPoolProxy;
            ObjPoolProxy.GetLinePool.PushObject (linePrefabNow);
        }
        linePrefabNow = null;
        startCub = null;
        arrPassVec2.Clear ();

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
        startCub = cub;
        arrPassVec2.Add (cub.GetComponent<Cub> ().myCoordinate);
        GameObject line = ControlBox.CreatorLine ();
        line.transform.position = cub.position;
        line.transform.GetComponent<DrawLineTool> ().dradLine (0); //因为节点是重复利用的，所以需要把长度默认为0
        linePrefabNow = line;
    }
}