using UnityEngine;

public class Ram : CollectableObject
{
    public override void Collect()
    {
        base.Collect();
        ScoreManager.Instance.AddScore(_scorePerObject);
        Destroy(gameObject);
    }
}
