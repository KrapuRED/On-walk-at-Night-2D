using UnityEngine;

public class AnimatorTrashBin : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Start()
    {
        if(_animator == null)
            _animator = GetComponent<Animator>();
    }

    public void AnimationTrashBin_On()
    {
        _animator.SetBool("isInteract",true);
    }
    public void AnimationTrashBin_Off()
    {
        _animator.SetBool("isInteract", false);
    }
}
