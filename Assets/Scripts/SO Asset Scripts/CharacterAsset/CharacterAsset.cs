using UnityEngine;

public enum CharClass{ Hunter, Warlock, Warrior}

public class CharacterAsset : ScriptableObject 
{
	public CharClass Class;
	public string ClassName;
	public int MaxHealth = 30;
	public string HeroPowerName;
	public Sprite AvatarImage;
    public Sprite HeroPowerIconImage;
    public Sprite AvatarBgImage;
    public Sprite HeroPowerBgImage;
    public Color32 AvatarBgTint;
    public Color32 HeroPowerBgTint;
    public Color32 ClassCardTint;
    public Color32 ClassRibbonsTint;
}
