using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopManager : MonoBehaviour {

    public GameObject ScreenContent;
    public GameObject PackPrefab;
    public int PackPrice;
    public Transform PacksParent;
    public Transform InitialPackSpot;
    public float PosXRange = 4f;
    public float PosYRange = 8f;
    public float RotationRange = 10f;
    public Text MoneyText;
    public Text DustText;
    public GameObject MoneyHUD;
    public GameObject DustHUD;
    public PackOpeningArea OpeningArea;

    public int StartingAmountOfDust = 1000;
    public int StartingAmountOfMoney = 1000;

    public static ShopManager Instance;
    public int PacksCreated { get; set;}
    private float packPlacementOffset = -0.01f;

    void Awake()
    {
        Instance = this;
        HideScreen();

        if (PlayerPrefs.HasKey("UnopenedPacks"))     
            StartCoroutine(GivePacks(PlayerPrefs.GetInt("UnopenedPacks"), true));

        LoadDustAndMoneyToPlayerPrefs();

    }

    private int _money; 
    public int Money
    {
        get{ return _money; }
        set
        {
            _money = value;
            MoneyText.text = _money.ToString();
        }
    }

    private int _dust; 
    public int Dust
    {
        get{ return _dust; }
        set
        {
            _dust = value;
            DustText.text = _dust.ToString();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            _money += 100;
    }

    public void BuyPack()
    {
        if (_money >= PackPrice)
        {
            Money -= PackPrice;
            StartCoroutine(GivePacks(1));
        }
    }

    public IEnumerator GivePacks(int NumberOfPacks, bool instant = false)
    {
        for (int i = 0; i < NumberOfPacks; i++)
        {
            GameObject newPack = Instantiate(PackPrefab, PacksParent);
            Vector3 localPositionForNewPack = new Vector3(Random.Range(-PosXRange, PosXRange), Random.Range(-PosYRange, PosYRange), PacksCreated*packPlacementOffset);
            newPack.transform.localEulerAngles = new Vector3(0f, 0f, Random.Range(-RotationRange, RotationRange));
            PacksCreated++;

            newPack.GetComponentInChildren<Canvas>().sortingOrder = PacksCreated;
            if (instant)
                newPack.transform.localPosition = localPositionForNewPack;
            else
            {
                newPack.transform.position = InitialPackSpot.position;
                newPack.transform.DOLocalMove(localPositionForNewPack, 0.5f);
                yield return new WaitForSeconds(0.5f);
            }

        }
        yield break;
    }

    void OnApplicationQuit()
    {
        SaveDustAndMoneyToPlayerPrefs();
        PlayerPrefs.SetInt("UnopenedPacks", PacksCreated);
    }

    public void LoadDustAndMoneyToPlayerPrefs()
    {
        Dust = PlayerPrefs.HasKey("Dust") ? PlayerPrefs.GetInt("Dust") : StartingAmountOfDust;

        Money = PlayerPrefs.HasKey("Money") ? PlayerPrefs.GetInt("Money") : StartingAmountOfMoney;
    }
        
    public void SaveDustAndMoneyToPlayerPrefs()
    {
        PlayerPrefs.SetInt("Dust", _dust);
        PlayerPrefs.SetInt("Money", _money);
    }

    public void ShowScreen()
    {
        ScreenContent.SetActive(true);
        MoneyHUD.SetActive(true);
    }

    public void HideScreen()
    {
        ScreenContent.SetActive(false);
        MoneyHUD.SetActive(false);
    }

}
