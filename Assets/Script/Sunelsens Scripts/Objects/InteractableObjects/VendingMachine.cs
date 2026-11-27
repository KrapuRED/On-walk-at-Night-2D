using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachine : InteractableObject
{
    [SerializeField] private Sprite _knockedDownSprite;
    [SerializeField] private NavMeshSurface _navMeshSurface;

    private void Start()
    {
        if(_navMeshSurface == null)
        {
            _navMeshSurface = GameObject.FindGameObjectWithTag("NavMeshManager").GetComponent<NavMeshSurface>();
        }
    }

    public override void Interact()
    {
        base.Interact();
        KnockVending();
    }

    private void KnockVending()
    {
        Debug.Log("Vending Knocked");
        IsInteracted = true;
        _spriteRenderer.sprite = _knockedDownSprite;
        _navMeshSurface.BuildNavMesh();
        AudioManager.instance.PlaySFX(AudioManager.instance.VendingFall_SFX);
    } 
}
