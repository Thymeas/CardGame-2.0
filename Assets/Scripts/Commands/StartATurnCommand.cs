public class StartATurnCommand : Command {

    private Player _p;

    public StartATurnCommand(Player p)
    {
        this._p = p;
    }

    public override void StartCommandExecution()
    {
        TurnManager.Instance.whoseTurn = _p;
        Command.CommandExecutionComplete();
    }
}
