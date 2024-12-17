//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using GoogleMobileAds;
//using GoogleMobileAds.Api;
//using System;

//public class GoogleMobileAdsDemoScript : MonoBehaviour
//{
//#if UNITY_ANDROID
//    private string adUnitId = "ca-app-pub-3940256099942544/6300978111";
//#endif

//    private BannerView bannerView;

//    private void Awake()
//    {
//        MobileAds.Initialize((InitializationStatus initStatus) =>
//        {

//        });
//    }

//    private void Start()
//    {
//        LoadAd();
//    }

//    private void LoadAd()
//    {
//        if (bannerView == null) CreateBannerView();

//        AdRequest adRequest = new AdRequest();
//        bannerView.LoadAd(adRequest);
//    }

//    private void CreateBannerView()
//    {
//        if (bannerView != null) bannerView.Destroy();

//        ////커스텀 배너 사이즈
//        //AdSize adSize = new AdSize(300, 300);
//        //bannerView = new BannerView(adUnitId, adSize, AdPosition.Bottom);

//        //표준 배너 사이즈
//        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
//    }

//    //private void ListenToAdEvents()
//    //{
//    //    // Raised when an ad is loaded into the banner view.
//    //    bannerView.OnBannerAdLoaded += () =>
//    //    {
//    //        Debug.Log("Banner view loaded an ad with response : "
//    //            + bannerView.GetResponseInfo());
//    //    };

//    //    // Raised when an ad fails to load into the banner view.
//    //    bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
//    //    {
//    //        Debug.LogError("Banner view failed to load an ad with error : "
//    //            + error);
//    //    };

//    //    // Raised when the ad is estimated to have earned money.
//    //    bannerView.OnAdPaid += (AdValue adValue) =>
//    //    {
//    //        Debug.Log(String.Format("Banner view paid {0} {1}.",
//    //            adValue.Value,
//    //            adValue.CurrencyCode));
//    //    };

//    //    // Raised when an impression is recorded for an ad.
//    //    bannerView.OnAdImpressionRecorded += () =>
//    //    {
//    //        Debug.Log("Banner view recorded an impression.");
//    //    };

//    //    // Raised when a click is recorded for an ad.
//    //    bannerView.OnAdClicked += () =>
//    //    {
//    //        Debug.Log("Banner view was clicked.");
//    //    };

//    //    // Raised when an ad opened full screen content.
//    //    bannerView.OnAdFullScreenContentOpened += () =>
//    //    {
//    //        Debug.Log("Banner view full screen content opened.");
//    //    };

//    //    // Raised when the ad closed full screen content.
//    //    bannerView.OnAdFullScreenContentClosed += () =>
//    //    {
//    //        Debug.Log("Banner view full screen content closed.");
//    //    };
//    //}

//    private void DestroyBannerView()
//    {
//        if (bannerView != null)
//        {
//            bannerView.Destroy();
//            bannerView = null;
//        }
//    }
//}
