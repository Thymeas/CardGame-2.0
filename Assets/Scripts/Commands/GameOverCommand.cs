using UnityEngine;
using System.Collections;

public class GameOverCommand : Command{

    private Player _loser;

    public GameOverCommand(Player looser)
    {
        this._loser = looser;
    }

    public override void StartCommandExecution()
    {
        _loser.PArea.Portrait.Explode();
    }
}
