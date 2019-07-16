/* Brief: 模拟 UnityEdtior 模式下的广告
 * Author: Komal
 * Date: "2019-07-12"
 */

using System;
using komal.sdk.ADResult;
using UnityEngine;

namespace komal.sdk {
    public class ADSimulatorSDK : SDKBase, IAD
    {
        public void HideBanner()
        {
            Debug.Log("TODO >> 模拟 HideBanner");
        }

        public void ShowBanner()
        {
            Debug.Log("TODO >> 模拟 ShowBanner");
        }

        public void ShowInterstitial(Action<InterstitialResult> callback)
        {
            Debug.Log("TODO >> 模拟 ShowInterstitial");
            callback(InterstitialResult.DISMISS);
        }

        public void ShowRewardedVideo(Action<RewardedVideoResult> callback)
        {
            Debug.Log("TODO >> 模拟 ShowRewardedVideo");
            callback(RewardedVideoResult.REWARDED);
            callback(RewardedVideoResult.DISMISS);
        }

        public void ValidateIntegration()
        {
            // Do nothing.
        }
    }
}