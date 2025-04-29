using UnityEngine;

public class TutorialStarter : MonoBehaviour
{
    public TutorialHandAnimator handAnimator;

    private const string TutorialKey = "FirstTimeTutorialShown1";
    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        if (PlayerPrefs.HasKey(TutorialKey))
        {
          handAnimator.gameObject.SetActive(false);
        }
    }
    public void StartHandutorial(Transform tutorialTarget)
    {
        Debug.Log("StartHandutorial");
        if (!PlayerPrefs.HasKey(TutorialKey))
        {
            // İlk dəfə oynayır → tutorialı göstər
            handAnimator.ShowHand(tutorialTarget);
            Debug.Log("StartHandutorial 2");

            // İstədiyin vaxt sonra gizlət (məs: 3 saniyə sonra)
            // Invoke(nameof(EndTutorial), 3f);

            // Flag yaz → artıq göstərilməyəcək
            PlayerPrefs.SetInt(TutorialKey, 1);
            PlayerPrefs.Save();
        }
    }

    void EndTutorial()
    {
        handAnimator.HideHand();
    }
}
