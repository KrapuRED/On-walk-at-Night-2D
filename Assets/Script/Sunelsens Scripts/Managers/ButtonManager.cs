using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void PlayGame()
    {
        TransitionManager.Instance.LoadScene("SceneSunelsen");
    }
}
