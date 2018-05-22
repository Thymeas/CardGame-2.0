using UnityEngine;
using System;

public enum TargetingOptions
{
    NoTarget,
    AllCreatures, 
    EnemyCreatures,
    YourCreatures, 
    AllCharacters, 
    EnemyCharacters,
    YourCharacters
}

public enum RarityOptions
{
    Basic, Common, Rare, Epic, Legendary
}

public enum TypesOfCards
{
    Creature, Spell
}

public class CardAsset : ScriptableObject , IComparable<CardAsset>
{
    [Header("General info")]
    public CharacterAsset CharacterAsset;
    [TextArea(2,3)]
    public string Description;
    [TextArea(2,3)]
    public string Tags;
    public RarityOptions Rarity;
	[PreviewSprite]
    public Sprite CardImage;
    public int ManaCost;
    public bool TokenCard = false;
    public int OverrideLimitOfThisCardInDeck = -1;

    public TypesOfCards TypeOfCard;

    [Header("Creature Info")]
    [Range(1, 30)]
    public int MaxHealth =1;   
    [Range(1, 30)]
    public int Attack;
    [Range(1, 4)]
    public int AttacksForOneTurn = 1;
    public bool Charge;
    public bool Taunt;
    public string CreatureScriptName;
    public int SpecialCreatureAmount;

    [Header("SpellInfo")]
    public string SpellScriptName;
    public int SpecialSpellAmount;
    public TargetingOptions Targets;

    public int CompareTo (CardAsset other)
    {
        return other.ManaCost < this.ManaCost
            ? 1
            : (other.ManaCost > this.ManaCost ? -1 : String.Compare(name, other.name, StringComparison.Ordinal));
    }
    
    public static bool operator >  (CardAsset operand1, CardAsset operand2)
    {
        return operand1.CompareTo(operand2) == 1;
    }
    
    public static bool operator <  (CardAsset operand1, CardAsset operand2)
    {
        return operand1.CompareTo(operand2) == -1;
    }
    
    public static bool operator >=  (CardAsset operand1, CardAsset operand2)
    {
        return operand1.CompareTo(operand2) >= 0;
    }
    
    public static bool operator <=  (CardAsset operand1, CardAsset operand2)
    {
        return operand1.CompareTo(operand2) <= 0;
    }

}
