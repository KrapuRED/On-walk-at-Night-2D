using UnityEngine;

public class Ram : CollectableObject
{
    public override void Collect()
    {
        base.Collect();
        ScoreManager.Instance.AddScore(_scorePerObject);
        PlayRAMSFX();
        Destroy(gameObject);
    }
    private void PlayRAMSFX()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.RAMPickup_SFX);
        }
    }
}
