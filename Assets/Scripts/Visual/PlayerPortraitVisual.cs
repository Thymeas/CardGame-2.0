using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerPortraitVisual : MonoBehaviour {

    public CharacterAsset charAsset;
    [Header("Text Component References")]
    public Text HealthText;

    [Header("Image References")]
    public Image HeroPowerIconImage;
    public Image HeroPowerBackgroundImage;
    public Image PortraitImage;
    public Image PortraitBackgroundImage;

    void Awake()
	{
		if(charAsset != null)
			ApplyLookFromAsset();
	}
	
	public void ApplyLookFromAsset()
    {
        if (HeroPowerIconImage != null)
        {
            HeroPowerIconImage.sprite = charAsset.HeroPowerIconImage;
            HeroPowerBackgroundImage.sprite = charAsset.HeroPowerBgImage;
            HeroPowerBackgroundImage.color = charAsset.HeroPowerBgTint;
        }

        HealthText.text = charAsset.MaxHealth.ToString();
        PortraitImage.sprite = charAsset.AvatarImage;
        PortraitBackgroundImage.sprite = charAsset.AvatarBgImage;       
        PortraitBackgroundImage.color = charAsset.AvatarBgTint;

    }

    public void TakeDamage(int amount, int healthAfter)
    {
        if (amount > 0)
        {
            DamageEffect.CreateDamageEffect(transform.position, amount);
            HealthText.text = healthAfter.ToString();
        }
    }

    public void Explode()
    {
        Instantiate(GlobalSettings.Instance.ExplosionPrefab, transform.position, Quaternion.identity);
        Sequence s = DOTween.Sequence();
        s.PrependInterval(2f);
        s.OnComplete(() => GlobalSettings.Instance.GameOverPanel.SetActive(true));
    }



}
