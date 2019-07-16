using System.Collections;
using System.Collections.Generic;
using komal.puremvc;
using UnityEngine;
public class RayPlan : ComponentEx, INotificationHandler {
  GameContrProxy GameContrProxy;
  void Start () {
    GameContrProxy = this.facade.RetrieveProxy (ProxyNameEnum.GameContrProxy) as GameContrProxy;
  }

  void Update () {

  }
  void OnMouseUp () {
    GameContrProxy.onTouchuUp ();
  }
}