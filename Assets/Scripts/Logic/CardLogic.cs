using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class CardLogic: IIdentifiable, IComparable<CardLogic>
{
    public Player owner;
    public int UniqueCardID; 
    public CardAsset ca;
    public SpellEffect effect;
    public static Dictionary<int, CardLogic> CardsCreatedThisGame = new Dictionary<int, CardLogic>();

    public int ID
    {
        get{ return UniqueCardID; }
    }

    public int CurrentManaCost{ get; set; }

    public bool CanBePlayed
    {
        get
        {
            bool ownersTurn = (TurnManager.Instance.whoseTurn == owner);
            bool fieldNotFull = true;
            return ownersTurn && fieldNotFull && (CurrentManaCost <= owner.ManaLeft);
        }
    }

    public CardLogic(CardAsset ca, Player owner)
    {
        this.owner = owner;
        this.ca = ca;
        UniqueCardID = IDFactory.GetUniqueID();
        ResetManaCost();
        if (!string.IsNullOrEmpty(ca.SpellScriptName))
        {
            effect = System.Activator.CreateInstance(System.Type.GetType(ca.SpellScriptName)) as SpellEffect;
            if (effect != null) effect.owner = owner;
        }
        CardsCreatedThisGame.Add(UniqueCardID, this);
    }

    public int CompareTo (CardLogic other)
    {
        if (other.ca < this.ca)
            return 1;
        return other.ca > this.ca ? -1 : 0;
    }
    
    public void ResetManaCost()
    {
        CurrentManaCost = ca.ManaCost;
    }

}
