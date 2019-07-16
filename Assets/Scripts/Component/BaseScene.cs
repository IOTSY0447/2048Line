using System.Collections;
using System.Collections.Generic;
using komal.puremvc;
using UnityEngine;
public class BaseScene : SceneBase {
    protected override void Awake () {
        base.Awake ();
        this.facade.RegisterCommand(MessageCommand.StartCommand,()=>new StartCommand());
        this.facade.SendNotification(MessageCommand.StartCommand);
    }
}