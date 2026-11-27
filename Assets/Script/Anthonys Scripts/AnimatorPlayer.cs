using UnityEngine;

public class AnimatorPlayer : MonoBehaviour
{

    [SerializeField] private ControllerPlayer _controllerPlayer;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private void OnEnable()
    {
        _controllerPlayer.OnLastDirectionChanged += SpriteUpdate;
    }

    private void OnDisable()
    {
        _controllerPlayer.OnLastDirectionChanged -= SpriteUpdate;
    }

    public void SpriteUpdate(Vector2 dir)
    {
        if(dir.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if(dir.x >= 0)
        {
            _spriteRenderer.flipX = false;
        }
    }

    public void PlayWalkSFX()
    {
        if(AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.PlayerStepF_SFX);
        }
    }
}
