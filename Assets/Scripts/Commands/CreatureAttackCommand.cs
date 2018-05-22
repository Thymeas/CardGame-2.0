using UnityEngine;

public class CreatureAttackCommand : Command 
{
    private int _targetUniqueId;
    private int _attackerUniqueId;
    private int _attackerHealthAfter;
    private int _targetHealthAfter;
    private int _damageTakenByAttacker;
    private int _damageTakenByTarget;

    public CreatureAttackCommand(int targetID, int attackerID, int damageTakenByAttacker, int damageTakenByTarget, int attackerHealthAfter, int targetHealthAfter)
    {
        this._targetUniqueId = targetID;
        this._attackerUniqueId = attackerID;
        this._attackerHealthAfter = attackerHealthAfter;
        this._targetHealthAfter = targetHealthAfter;
        this._damageTakenByTarget = damageTakenByTarget;
        this._damageTakenByAttacker = damageTakenByAttacker;
    }

    public override void StartCommandExecution()
    {
        GameObject Attacker = IDHolder.GetGameObjectWithID(_attackerUniqueId);

        Attacker.GetComponent<CreatureAttackVisual>().AttackTarget(_targetUniqueId, _damageTakenByTarget, _damageTakenByAttacker, _attackerHealthAfter, _targetHealthAfter);
    }
}
