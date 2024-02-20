using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADS : MonoBehaviour
{
#if UNITY_ANDROID
    string bannerAdUnitId = "84ce8fe5b4fc0069"; // Retrieve the ID from your account

#elif UNITY_IOS
    string bannerAdUnitId = "1302aa9a8c134ea1"; // Retrieve the ID from your account
#endif

    // Start is called before the first frame update
    void Start()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {

            // AppLovin SDK is initialized, start loading ads
            // Banners are automatically sized to 320�50 on phones and 728�90 on tablets
            // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
            MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

            // Set background or background color for banners to be fully functional
            MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, Color.black);
            MaxSdk.ShowBanner(bannerAdUnitId);
        };

        MaxSdk.SetSdkKey("KsaJe2Utx5PPaSznDTOPOuySgU_ITMOWluZfT_HKcRrXwGAszhb__tF1hhTZTX3T1W1LkbmmG0iZTz9MwF5K7c");
        MaxSdk.SetUserId("USER_ID");
        MaxSdk.InitializeSdk();
    }
}
