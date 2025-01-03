using UnityEngine;

public class EnenyAnimController
{
    private Animator animator;

    private const string IsIdleStandingKeyName = "IsIdleStanding";
    private const string IsPlayerDetectedKeyName = "IsPlayerDetected";
    private const string IsCloseToPlayerKeyName = "IsCloseToPlayer";
    private const string OnHitKeyName = "OnHit";
    private const string OnResetKeyName = "OnReset";

    public EnenyAnimController (Animator anim)
    {
        animator = anim;
    }

    public void SetRootMotion(bool r)
    {
        animator.applyRootMotion = r;

    }

    public void SetIdleStandingOrWalking(bool isIdleStanding)
    {
        animator.SetBool(IsIdleStandingKeyName, isIdleStanding);
        SetIsPlayerDetected(false);
    }

    public void SetIsPlayerDetected(bool isPlayerDetected)
    {
     //   animator.applyRootMotion = !isPlayerDetected;
        animator.SetBool(IsPlayerDetectedKeyName, isPlayerDetected);

    }
    public void OnGetHit()
    {
        animator.SetTrigger(OnHitKeyName);
    }

    public void SetCloseToPlayer(bool isCloseToPlayer)
    {
        animator.SetBool(IsCloseToPlayerKeyName, isCloseToPlayer);
        //SetIsPlayerDetected(true); //mb add late
    }

    public void Reset()
    {
        animator.SetTrigger(OnResetKeyName);
        SetIdleStandingOrWalking(true);
        SetCloseToPlayer(false);
        SetIsPlayerDetected(false);
    }
}
