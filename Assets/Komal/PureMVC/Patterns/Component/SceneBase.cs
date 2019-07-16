/* Brief: All Unity Scene must add SceneBase based Component, for all SDK lifecycle
 * Author: Komal
 * Date: "2019-07-05"
 */

using UnityEngine;

namespace komal.puremvc {
    public class SceneBase : komal.puremvc.ComponentEx 
    {
        protected override void Awake(){
            base.Awake();
            Debug.Log(">>>> SceneBase:Awake");
            sdk.SDKManager.Instance.OnInit();
        }
        
        void OnApplicationPause(bool isPaused) {
            Debug.Log(">>>> SceneBase:OnApplicationPause isPaused:" + isPaused.ToString());
            if(isPaused){
                sdk.SDKManager.Instance.OnPause();
            }else{
                sdk.SDKManager.Instance.OnResume();
            }
        }

        void OnApplicationQuit(){
            Debug.Log(">>>> SceneBase:OnApplicationQuit");
            sdk.SDKManager.Instance.OnDestroy();
        }

        public virtual void Update(){
            sdk.SDKManager.Instance.OnUpdate();
        }
    }
}
