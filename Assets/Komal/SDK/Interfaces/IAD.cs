namespace komal.sdk {
    public interface IAD
    {
        void ShowBanner();

        void HideBanner();

        void ShowInterstitial(System.Action<ADResult.InterstitialResult> callback);

        void ShowRewardedVideo(System.Action<ADResult.RewardedVideoResult> callback);

        void ValidateIntegration();
    }
}
