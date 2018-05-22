using System.Collections.Generic;

[System.Serializable]
public class CreatureLogic: ICharacter 
{
    public Player owner;
    public CardAsset ca;
    public CreatureEffect effect;
    public int UniqueCreatureID;
    public bool Frozen = false;
    
    public int ID
    {
        get{ return UniqueCreatureID; }
    }
        
    private int _baseHealth;
    public int MaxHealth
    {
        get{ return _baseHealth;}
    }
    
    private int _health;
    public int Health
    {
        get{ return _health; }

        set
        {
            if (value > MaxHealth)
                _health = MaxHealth;
            else if (value <= 0)
                Die();
            else
                _health = value;
        }
    }

    public bool Taunt
    {
        get;
        set;
    }
    
    public bool CanAttack
    {
        get
        {
            bool ownersTurn = (TurnManager.Instance.whoseTurn == owner);
            return (ownersTurn && (AttacksLeftThisTurn > 0) && !Frozen);
        }
    }
    
    private int _baseAttack;
    public int Attack
    {
        get{ return _baseAttack; }
    }
     
    private int _attacksForOneTurn = 1;
    public int AttacksLeftThisTurn
    {
        get;
        set;
    }

    public CreatureLogic(Player owner, CardAsset ca)
    {
        this.ca = ca;
        _baseHealth = ca.MaxHealth;
        Health = ca.MaxHealth;
        _baseAttack = ca.Attack;
        _attacksForOneTurn = ca.AttacksForOneTurn;

        if (ca.Charge)
            AttacksLeftThisTurn = _attacksForOneTurn;

        Taunt = ca.Taunt;
        this.owner = owner;
        UniqueCreatureID = IDFactory.GetUniqueID();
        if (!string.IsNullOrEmpty(ca.CreatureScriptName))
        {
            effect = System.Activator.CreateInstance(System.Type.GetType(ca.CreatureScriptName), new System.Object[]{owner, this, ca.SpecialCreatureAmount}) as CreatureEffect;
            effect.RegisterEventEffect();
        }
        CreaturesCreatedThisGame.Add(UniqueCreatureID, this);
    }
    
    public void OnTurnStart()
    {
        AttacksLeftThisTurn = _attacksForOneTurn;
    }

    public void Die()
    {   
        owner.table.CreaturesOnTable.Remove(this);
        
        if (effect != null)
        {
            effect.WhenACreatureDies();
            effect.UnRegisterEventEffect();
            effect = null;
        }

        new CreatureDieCommand(UniqueCreatureID, owner).AddToQueue();
    }

    public void GoFace()
    {
        AttacksLeftThisTurn--;
        int targetHealthAfter = owner.otherPlayer.Health - Attack;
        new CreatureAttackCommand(owner.otherPlayer.PlayerID, UniqueCreatureID, 0, Attack, Health, targetHealthAfter).AddToQueue();
        owner.otherPlayer.Health -= Attack;
    }

    public void AttackCreature (CreatureLogic target)
    {
        AttacksLeftThisTurn--;
        int targetHealthAfter = target.Health - Attack;
        int attackerHealthAfter = Health - target.Attack;
        new CreatureAttackCommand(target.UniqueCreatureID, UniqueCreatureID, target.Attack, Attack, attackerHealthAfter, targetHealthAfter).AddToQueue();

        target.Health -= Attack;
        Health -= target.Attack;
    }

    public void AttackCreatureWithID(int uniqueCreatureID)
    {
        CreatureLogic target = CreatureLogic.CreaturesCreatedThisGame[uniqueCreatureID];
        AttackCreature(target);
    }
    public static Dictionary<int, CreatureLogic> CreaturesCreatedThisGame = new Dictionary<int, CreatureLogic>();

}
