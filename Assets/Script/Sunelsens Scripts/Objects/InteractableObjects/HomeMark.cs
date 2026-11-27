using UnityEngine;
using UnityEngine.Audio;

public class HomeMark : InteractableObject
{
    [SerializeField] private AudioSource _homeAudioSource;

    public override void Interact()
    {
        if (!CanInteract())
        {
            return;
        }
        GoHome();
    }
    private void Update()
    {
        if (_isInteracting)
        {
            Debug.Log("Make Sound");
            if (!_homeAudioSource.isPlaying)
            {
                _homeAudioSource.clip = AudioManager.instance.InteractingHomeMark_SFX;
                _homeAudioSource.loop = true;
                _homeAudioSource.Play();
            }
            SendSound();
        }
        else
        {
            _homeAudioSource.Stop();
        }
    }



    private void GoHome()
    {
        Debug.Log("Went Home");
        IsInteracted = true;
        GameManager.Instance.Victory();
    }
}
