using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public struct DamageCommandInfo
{
    public int targetID;
    public int healthAfter;
    public int amount;

    public DamageCommandInfo (int targetID, int healthAfter, int amount)
    {
        this.targetID = targetID;
        this.amount = amount;
        this.healthAfter = healthAfter;
    }
}

public class DealDamageCommand : Command {

    private List<DamageCommandInfo> Targets;

    public DealDamageCommand( List<DamageCommandInfo> Targets)
    {
        this.Targets = Targets;
    }

    public override void StartCommandExecution()
    {
        foreach(DamageCommandInfo info in Targets)
        {
            var target = IDHolder.GetGameObjectWithID(info.targetID);
            if (GlobalSettings.Instance.IsPlayer(info.targetID))
                target.GetComponent<PlayerPortraitVisual>().TakeDamage(info.amount, info.healthAfter);
            else
                target.GetComponent<OneCreatureManager>().TakeDamage(info.amount, info.healthAfter);
        }
        Sequence s = DOTween.Sequence();
        s.PrependInterval(1f);
        s.OnComplete(Command.CommandExecutionComplete);
    }
}
