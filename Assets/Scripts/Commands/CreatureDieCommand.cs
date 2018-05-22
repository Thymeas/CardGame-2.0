
public class CreatureDieCommand : Command 
{
    private Player _p;
    private int _deadCreatureId;

    public CreatureDieCommand(int CreatureID, Player p)
    {
        this._p = p;
        this._deadCreatureId = CreatureID;
    }

    public override void StartCommandExecution()
    {
        _p.PArea.tableVisual.RemoveCreatureWithID(_deadCreatureId);
    }
}
