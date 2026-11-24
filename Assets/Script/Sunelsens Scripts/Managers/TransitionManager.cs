using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;
    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(StartLoadScene(sceneName));
    }

    IEnumerator StartLoadScene(string sceneName)
    {
        if (!isTransitioning)
        {
            isTransitioning = true;

            //animator.SetTrigger("FadeIn");

            yield return new WaitForSecondsRealtime(1f);
            SceneManager.LoadScene(sceneName);
            //animator.SetTrigger("FadeOut");
            isTransitioning = false;
            Time.timeScale = 1f;
        }

    }
}
