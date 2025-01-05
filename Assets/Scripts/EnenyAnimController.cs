using UnityEngine;

public class EnenyAnimController
{
    private Animator animator;

    private const string IsIdleStandingKeyName = "IsIdleStanding";
    private const string IsPlayerDetectedKeyName = "IsPlayerDetected";
    private const string IsCloseToPlayerKeyName = "IsCloseToPlayer";
    private const string OnHitKeyName = "OnHit";
    private const string OnResetKeyName = "OnReset";
    private const string SpeedMultiplierKeyName = "SpeedMultiplier";

    public EnenyAnimController (Animator anim)
    {
        animator = anim;
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        animator.SetFloat(SpeedMultiplierKeyName, multiplier);
    }

    public void SetRootMotion(bool useRootMotion)
    {
        animator.applyRootMotion = useRootMotion;

    }

    public void SetIdleStandingOrWalking(bool isIdleStanding)
    {
        animator.SetBool(IsIdleStandingKeyName, isIdleStanding);
        SetIsPlayerDetected(false);
    }

    public void SetIsPlayerDetected(bool isPlayerDetected)
    {
        animator.SetBool(IsPlayerDetectedKeyName, isPlayerDetected);

    }
    public void OnGetHit()
    {
        animator.SetTrigger(OnHitKeyName);
    }

    public void SetCloseToPlayer(bool isCloseToPlayer)
    {
        animator.SetBool(IsCloseToPlayerKeyName, isCloseToPlayer);
    }

    public void Reset()
    {
        SetRootMotion(true);
        SetIdleStandingOrWalking(true);
        SetCloseToPlayer(false);
        SetIsPlayerDetected(false);
        animator.SetTrigger(OnResetKeyName);
    }
}
