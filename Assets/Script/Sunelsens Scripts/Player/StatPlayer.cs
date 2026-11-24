using UnityEngine;

[System.Serializable]
public struct StatPlayer
{
    [Header("Walk")]
    public float WalkSpeed;

    [Header("Sprint")]
    public float SprintSpeed;
    public float SprintCost;

    [Header("Stamina")]
    public float MaxStamina;
    public float StaminaRegen;
    public float StaminaRegenWaitTime;
}
