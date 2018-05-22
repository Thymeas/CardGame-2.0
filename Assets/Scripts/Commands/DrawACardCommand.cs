public class DrawACardCommand : Command {

    private Player _p;
    private CardLogic _cl;
    private bool _fast;
    private bool _fromDeck;

    public Player player 
    {
        get{ return _p; }
    }

    public DrawACardCommand(CardLogic cl, Player p, bool fast, bool fromDeck)
    {        
        this._cl = cl;
        this._p = p;
        this._fast = fast;
        this._fromDeck = fromDeck;
    }

    public override void StartCommandExecution()
    {
        _p.PArea.PDeck.CardsInDeck--;
        _p.PArea.handVisual.GivePlayerACard(_cl.ca, _cl.UniqueCardID, _fast, _fromDeck);
    }
}
