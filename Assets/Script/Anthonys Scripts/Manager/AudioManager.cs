using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    [Header("Source")]
    [SerializeField] private AudioSource _BGMSource;
    [SerializeField] private AudioSource _SFXSource;

    [Header("Clip -- BGM")]
    public AudioClip MainMenu_BGM;
    public AudioClip Gameplay_BGM;

    [Header("Clip -- SFX")]
    public AudioClip PlayerStepF_SFX;
    public AudioClip ThugAlerted_SFX;
    public AudioClip TrashBinKicked_SFX;
    public AudioClip VendingFall_SFX;
    public AudioClip InteractingHomeMark_SFX;
    public AudioClip CarWalking_SFX;
    public AudioClip PlayerDead_SFX;
    public AudioClip LevelComplete_SFX;
    public AudioClip UIClick_SFX;
    public AudioClip RAMPickup_SFX;


    public void PlayBgm(AudioClip bgm)
    {
        if (bgm == null || _BGMSource == null) return;

        if (_BGMSource.clip == bgm)
        {
            return;
        }

        _BGMSource.Stop();
        _BGMSource.clip = bgm;
        _BGMSource.loop = true;
        _BGMSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || _SFXSource == null) return;

        _SFXSource.PlayOneShot(clip);

    }
}
