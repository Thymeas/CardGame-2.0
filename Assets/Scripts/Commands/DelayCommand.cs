using DG.Tweening;

public class DelayCommand : Command 
{
    private float _delay;

    public DelayCommand(float timeToWait)
    {
        _delay = timeToWait;    
    }

    public override void StartCommandExecution()
    {
        Sequence s = DOTween.Sequence();
        s.PrependInterval(_delay);
        s.OnComplete(Command.CommandExecutionComplete);
    }
}
