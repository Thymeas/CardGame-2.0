using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class TurnManager : MonoBehaviour {

    public CardAsset CoinCard;
    public static TurnManager Instance;
    public static Player[] Players;
    
    private RopeTimer _timer;

    private Player _whoseTurn;
    public Player whoseTurn
    {
        get
        {
            return _whoseTurn;
        }

        set
        {
            _whoseTurn = value;
            _timer.StartTimer();

            GlobalSettings.Instance.EnableEndTurnButtonOnStart(_whoseTurn);

            TurnMaker tm = whoseTurn.GetComponent<TurnMaker>();
            tm.OnTurnStart();
            if (tm is PlayerTurnMaker)
                whoseTurn.HighlightPlayableCards();
            whoseTurn.otherPlayer.HighlightPlayableCards(true);
                
        }
    }

    void Awake()
    {
        Players = GameObject.FindObjectsOfType<Player>();
        Instance = this;
        _timer = GetComponent<RopeTimer>();
    }

    void Start()
    {
        OnGameStart();
    }

    public void OnGameStart()
    {
        CardLogic.CardsCreatedThisGame.Clear();
        CreatureLogic.CreaturesCreatedThisGame.Clear();

        foreach (Player p in Players)
        {
            p.ManaThisTurn = 0;
            p.ManaLeft = 0;
            p.LoadCharacterInfoFromAsset();
            p.TransmitInfoAboutPlayerToVisual();
            p.PArea.PDeck.CardsInDeck = p.deck.cards.Count;
            p.PArea.Portrait.transform.position = p.PArea.InitialPortraitPosition.position;
        }

        Sequence s = DOTween.Sequence();
        s.Append(Players[0].PArea.Portrait.transform.DOMove(Players[0].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.Insert(0f, Players[1].PArea.Portrait.transform.DOMove(Players[1].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.PrependInterval(3f);
        s.OnComplete(() =>
        {
            int rnd = Random.Range(0, 2);
            Player whoGoesFirst = Players[rnd];
            Player whoGoesSecond = whoGoesFirst.otherPlayer;
            int initDraw = 4;

            for (int i = 0; i < initDraw; i++)
            {
                whoGoesSecond.DrawACard(true);
                whoGoesFirst.DrawACard(true);
            }

            whoGoesSecond.DrawACard(true);
            whoGoesSecond.GetACardNotFromDeck(CoinCard);
            new StartATurnCommand(whoGoesFirst).AddToQueue();
        });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            EndTurn();
    }

    public void EndTurn()
    {
        Draggable[] AllDraggableObjects = GameObject.FindObjectsOfType<Draggable>();
        foreach (Draggable d in AllDraggableObjects)
            d.CancelDrag();
        _timer.StopTimer();
        whoseTurn.OnTurnEnd();

        new StartATurnCommand(whoseTurn.otherPlayer).AddToQueue();
    }

    public void StopTheTimer()
    {
        _timer.StopTimer();
    }
}

