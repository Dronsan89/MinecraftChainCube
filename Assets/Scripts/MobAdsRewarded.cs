using GoogleMobileAds.Api;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MobAdsRewarded : MonoBehaviour
{
    private RewardedAd rewardedAd;
    private const string rewardUnitId = "ca-app-pub-3940256099942544/5224354917";
    private GameController gameController;
    private AudioSource audioSource;
    private int money;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        rewardedAd = new RewardedAd(rewardUnitId);
        AdRequest adRequest = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(adRequest);
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;//когда игрок досмотрел рекламу
    }

    private void OnDisable()
    {
        rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
    }

    public void HandleUserEarnedReward(object sender, Reward e)
    {
        money = PlayerPrefs.GetInt("money");
        StartCoroutine(DoubleGold());
    }

    IEnumerator DoubleGold()
    {
        audioSource.Play();
        for (int i = 0; i < money; i++)
        {
            yield return new WaitForSeconds(0.015f);
            gameController.Money++;
            gameController.textScoreLevel.text = gameController.Money.ToString();
        }
    }
}
