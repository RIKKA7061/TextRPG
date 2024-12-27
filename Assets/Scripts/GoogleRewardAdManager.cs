using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleRewardAdManager : MonoBehaviour
{
	public string rewardedId = "ca-app-pub-4284385980753154/8144905328"; // Real
	//public string rewardedId = "ca-app-pub-3940256099942544/5224354917"; // test

	RewardedAd rewardedAd;

	public void OnClick_AdAction()
	{
		OnClick_LoadRewardedAd();
		StartCoroutine(OnClick_DelayedShowRewardedAd());
	}

	public void OnClick_LoadRewardedAd()
	{
		if (rewardedAd != null)
		{
			rewardedAd.Destroy();
			rewardedAd = null;
		}
		var adRequest = new AdRequest();
		adRequest.Keywords.Add("unity-admob-sample");

		RewardedAd.Load(rewardedId, adRequest, (RewardedAd ad, LoadAdError error) =>
		{
			if (error != null || ad == null)
			{
				print("Rewarded failed to load" + error);
				return;
			}

			Debug.Log("Loading Reward AD..");
			rewardedAd = ad;
			RewardedAdEvents(rewardedAd);
			Debug.Log("Loading Success!");
		});
	}

	public void OnClick_ShowRewardedAd()
	{
		if (rewardedAd != null && rewardedAd.CanShowAd())
		{
			rewardedAd.Show((Reward Reward) =>
			{
				Debug.Log("Reward Watched. you got a coin!");
			});
		}
		else
		{
			Debug.Log("Reward Ad was not Ready..");
		}
	}

	public IEnumerator OnClick_DelayedShowRewardedAd()
	{
		yield return new WaitForSeconds(3f);

		// Wait for the ad to be ready if it's not already
		if (rewardedAd == null || !rewardedAd.CanShowAd())
		{
			Debug.Log("Reward Ad was not Ready...");
			yield break; // Exit coroutine early if the ad is not ready
		}

		// Show the ad
		rewardedAd.Show((Reward Reward) =>
		{
			// This is the callback when the ad finishes
			Debug.Log("Reward Watched. You got a coin!");
		});

		// Optionally, you could yield here if you need to wait for some other event, like user interaction or ad completion
		// For example, wait until the ad is fully shown before continuing (depends on your ad SDK)
		// yield return new WaitForSeconds(1f); // Adjust as necessary for your ad SDK
	}


	public void RewardedAdEvents(RewardedAd ad)
	{
		// Raised when the ad is estimated to have earned money.
		ad.OnAdPaid += (AdValue adValue) =>
		{
			Debug.Log("Rewarded ad paid {0} {1}." +
				adValue.Value +
				adValue.CurrencyCode);
		};
		// Raised when an impression is recorded for an ad.
		ad.OnAdImpressionRecorded += () =>
		{
			Debug.Log("Rewarded ad recorded an impression.");
		};
		// Raised when a click is recorded for an ad.
		ad.OnAdClicked += () =>
		{
			Debug.Log("Rewarded ad was clicked.");
		};
		// Raised when an ad opened full screen content.
		ad.OnAdFullScreenContentOpened += () =>
		{
			Debug.Log("Rewarded ad full screen content opened.");
		};
		// Raised when the ad closed full screen content.
		ad.OnAdFullScreenContentClosed += () =>
		{
			Debug.Log("Rewarded ad full screen content closed.");
		};
		// Raised when the ad failed to open full screen content.
		ad.OnAdFullScreenContentFailed += (AdError error) =>
		{
			Debug.LogError("Rewarded ad failed to open full screen content " +
						   "with error : " + error);
		};
	}
}
