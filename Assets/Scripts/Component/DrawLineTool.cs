using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineTool : MonoBehaviour {
    Mesh deformingMesh;
    Vector3[] originalVertices; //初始位置
    Vector3[] displacedVertices; //记录的位置
    private Quaternion initialRotation; //默认旋转角度，拉伸y轴，look方法好像是针对z轴的，所以默认旋转
    public float dis = 2.0f; //物体的长度
    private void Awake () {
        deformingMesh = GetComponent<MeshFilter> ().mesh;
        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; i++) {
            displacedVertices[i] = originalVertices[i];
        }
        initialRotation = Quaternion.Euler (90, 0, 0);
    }
    void Start () {

    }
    //光线与控制节点的交点实现的拖拽效果
    public void drag (Vector3 targetPos) {
        float distance = Vector3.Distance (targetPos, transform.position);
        transform.rotation = Quaternion.LookRotation (targetPos - transform.position) * initialRotation;
        dradLine (distance);
    }
    //画线
    public void dradLine (float distance) {
        for (int i = 0; i < originalVertices.Length; i++) {
            Vector3 posOne = originalVertices[i];
            posOne.y = (posOne.y + dis / 2) / dis * distance;
            displacedVertices[i] = posOne;
        }
        deformingMesh.vertices = displacedVertices;
    }

}