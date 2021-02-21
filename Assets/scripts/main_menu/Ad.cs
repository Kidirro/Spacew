using UnityEngine.Advertisements;
using UnityEngine;
using UnityEngine.UI;

public class Ad : MonoBehaviour, IUnityAdsListener
{
#if UNITY_IOS
    private string gameId = "4010864";
#elif UNITY_ANDROID
    private string gameId = "4010865";
#endif

    Button myButton;
    public string myPlacementId = "rewardedVideo";
   

    void Start()
    {
        Advertisement.AddListener(this);
        myButton = GetComponent<Button>();

        // Set interactivity to be dependent on the Placement’s status:
        myButton.interactable = Advertisement.IsReady(myPlacementId);

        // Map the ShowRewardedVideo function to the button’s click listener:
        if (myButton) myButton.onClick.AddListener(ShowRewardedVideo);
        // Initialize the Ads listener and service:
        Advertisement.Initialize(gameId, true);
    }

    // Implement a function for showing a rewarded video ad:
    void ShowRewardedVideo()
    {
        Advertisement.Show(myPlacementId);
        PlayerPrefs.SetInt("First_play", 0);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == myPlacementId)
        {
            myButton.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            Debug.Log("Finished");
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            Debug.Log("Skipped");
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}
