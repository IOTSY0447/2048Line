using komal.puremvc;
//枚举所有的proxy的名字
public class ProxyNameEnum {
    public static string GameContrProxy = "GameContrProxy";
}
public class MessageCommand {
    public static string StartCommand = "StartCommand";
}

public class StartCommand : SimpleCommand {
    public override void Execute (INotification notification) {
        this.facade.RegisterProxy (new GameContrProxy (ProxyNameEnum.GameContrProxy, new GameContrData ()));
    }
}