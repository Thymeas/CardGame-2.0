public class UpdateManaCrystalsCommand : Command {

    private Player _p;
    private int _totalMana;
    private int _availableMana;

    public UpdateManaCrystalsCommand(Player p, int TotalMana, int AvailableMana)
    {
        this._p = p;
        this._totalMana = TotalMana;
        this._availableMana = AvailableMana;
    }

    public override void StartCommandExecution()
    {
        _p.PArea.ManaBar.TotalCrystals = _totalMana;
        _p.PArea.ManaBar.AvailableCrystals = _availableMana;
        Command.CommandExecutionComplete();
    }
}
