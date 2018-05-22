using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
public class OneCardManager : MonoBehaviour {

    public CardAsset CardAsset;
    public OneCardManager PreviewManager;
    [Header("Text Component References")]
    public Text NameText;
    public Text ManaCostText;
    public Text DescriptionText;
    public Text HealthText;
    public Text AttackText;
    [Header("Image References")]
    public Image CardTopRibbonImage;
    public Image CardLowRibbonImage;
    public Image CardGraphicImage;
    public Image CardBodyImage;
    public Image CardFaceFrameImage;
    public Image CardFaceGlowImage;
    public Image CardBackGlowImage;
    public Image RarityStoneImage;

    void Awake()
    {
        if (CardAsset != null)
            ReadCardFromAsset();
    }

    private bool canBePlayedNow = false;
    public bool CanBePlayedNow
    {
        get
        {
            return canBePlayedNow;
        }

        set
        {
            canBePlayedNow = value;

            CardFaceGlowImage.enabled = value;
        }
    }

    public void ReadCardFromAsset()
    {
        if (CardAsset.CharacterAsset != null)
        {
            CardBodyImage.color = CardAsset.CharacterAsset.ClassCardTint;
            CardFaceFrameImage.color = CardAsset.CharacterAsset.ClassCardTint;
            CardTopRibbonImage.color = CardAsset.CharacterAsset.ClassRibbonsTint;
            CardLowRibbonImage.color = CardAsset.CharacterAsset.ClassRibbonsTint;
        }
        else
            CardFaceFrameImage.color = Color.white;

        NameText.text = CardAsset.name;
        ManaCostText.text = CardAsset.ManaCost.ToString();
        DescriptionText.text = CardAsset.Description;
        CardGraphicImage.sprite = CardAsset.CardImage;

        if (CardAsset.TypeOfCard == TypesOfCards.Creature)
        {
            AttackText.text = CardAsset.Attack.ToString();
            HealthText.text = CardAsset.MaxHealth.ToString();
        }

        if (PreviewManager != null)
        {
            PreviewManager.CardAsset = CardAsset;
            PreviewManager.ReadCardFromAsset();
        }

        RarityStoneImage.color = RarityColors.Instance.ColorsDictionary[CardAsset.Rarity];
    }
}
