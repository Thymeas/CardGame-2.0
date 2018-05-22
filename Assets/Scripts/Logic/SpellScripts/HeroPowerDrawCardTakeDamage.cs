using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroPowerDrawCardTakeDamage : SpellEffect {

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        new DealDamageCommand(new List<DamageCommandInfo>{ new DamageCommandInfo(TurnManager.Instance.whoseTurn.PlayerID, TurnManager.Instance.whoseTurn.Health - 2, 2)}).AddToQueue();
        TurnManager.Instance.whoseTurn.Health -= 2;
        TurnManager.Instance.whoseTurn.DrawACard();

    }
}
