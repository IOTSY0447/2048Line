using System.Collections.Generic;

namespace komal.sdk{
    public class SDKManager: puremvc.Singleton<SDKManager>, ILifeCycle, ISDKNotify, IAD, IDebugLog
    {
        private List<SDKBase> m_SDKList = new List<SDKBase>();
        private bool m_isInitialized = false;

#region Proxys
        private IAD m_target_ad = null;
        private IDebugLog m_target_DebugLog = null;
#endregion

        public override void OnSingletonInit(){
            OnInit();
        }

        public void OnInit()
        {
            // load all sdk
            if(!m_isInitialized){
                m_isInitialized = true;

                // 日志记录模块
                var debugLogSDK = new DebugLogSDK();
                m_SDKList.Add(debugLogSDK);
                this.m_target_DebugLog = debugLogSDK;

#if (UNITY_ANDROID || UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
                // 广告模块
                var ironSourceSDK = new IronSourceSDK();
                m_SDKList.Add(ironSourceSDK);
                m_target_ad = ironSourceSDK;
#else
                var adSimulatorSDK = new ADSimulatorSDK();
                m_SDKList.Add(adSimulatorSDK);
                m_target_ad = adSimulatorSDK;
#endif

                m_SDKList.ForEach(sdk=>{
                    sdk.OnInit();
                });
            }
        }

        public void OnPause()
        {
            m_SDKList.ForEach(sdk=>{
                sdk.OnPause();
            });
        }

        public void OnResume()
        {
            m_SDKList.ForEach(sdk=>{
                sdk.OnResume();
            });
        }

        public void OnDestroy()
        {
            m_SDKList.ForEach(sdk=>{
                sdk.OnDestroy();
            });
        }

        public void OnUpdate(){
            m_SDKList.ForEach(sdk=>{
                sdk.OnUpdate();
            });
        }
        //////////////// User Proxy (TODO) ////////////////
        //登录成功响应
        public void NotifyLogin(string _in_data){
            // TODO
        }
        //登出响应
        public void NotifyLogout(string _in_data){
            // TODO
        }
        //支付结果响应
        public void NotifyPayResult(string _in_data){
            // TODO
        }
        //初始化完毕响应
        public void NotifyInitFinish(string _in_data){
            // TODO
        }
        //拓展函数回调响应
        public void NotifyExtraFunction(string _json_string){
            // TODO
        }


        //////////////// AD Proxy ////////////////
        public void ShowBanner(){
            m_target_ad.ShowBanner();
        }

        public void HideBanner(){
            m_target_ad.HideBanner();
        }

        public void ShowInterstitial(System.Action<ADResult.InterstitialResult> callback){
            m_target_ad.ShowInterstitial(callback);
        }

        public void ShowRewardedVideo(System.Action<ADResult.RewardedVideoResult> callback){
            m_target_ad.ShowRewardedVideo(callback);
        }

        public void ValidateIntegration()
        {
            m_target_ad.ValidateIntegration();
        }

#region DebugLog interface 
        public string GetLogFileFullPath(){
            return m_target_DebugLog.GetLogFileFullPath();
        }
        public string GetRunTimeLogText()
        {
            return m_target_DebugLog.GetRunTimeLogText();
        }
#endregion
    }
}
