public class PlayASpellCardCommand: Command
{
    private CardLogic _card;
    private Player _p;

    public PlayASpellCardCommand(Player p, CardLogic card)
    {
        this._card = card;
        this._p = p;
    }

    public override void StartCommandExecution()
    {
        Command.CommandExecutionComplete();
        _p.PArea.handVisual.PlayASpellFromHand(_card.UniqueCardID);
    }
}
