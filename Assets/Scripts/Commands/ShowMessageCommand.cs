using DG.Tweening;

public class ShowMessageCommand : Command {
    private string _message;
    private float _duration;

    public ShowMessageCommand(string message, float duration)
    {
        this._message = message;
        this._duration = duration;
    }

    public override void StartCommandExecution()
    {
        MessageManager.Instance.ShowMessage(_message, _duration);
        Sequence s = DOTween.Sequence();
        s.AppendInterval(_duration);
        s.OnComplete(Command.CommandExecutionComplete);
    }
}
