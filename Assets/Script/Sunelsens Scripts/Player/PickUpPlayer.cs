using UnityEngine;

public class PickUpPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent<ICollectable>(out ICollectable collectable);
        collectable?.Collect();
    }
}
