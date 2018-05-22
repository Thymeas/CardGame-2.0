using System.Collections.Generic;

public abstract class Command
{
    public static bool playingQueue {get; set;}
    static Queue<Command> CommandQueue = new Queue<Command>();

    public void AddToQueue()
    {
        CommandQueue.Enqueue(this);
        if (!playingQueue)
            PlayFirstCommandFromQueue();
    }

    public abstract void StartCommandExecution();


    public static bool CardDrawPending()
    {
        foreach (Command c in CommandQueue)
        {
            if (c is DrawACardCommand)
                return true;
        }
        return false;
    }
    public static void CommandExecutionComplete()
    {
        if (CommandQueue.Count > 0)
            PlayFirstCommandFromQueue();
        else
            playingQueue = false;

        if (TurnManager.Instance.whoseTurn != null)
            TurnManager.Instance.whoseTurn.HighlightPlayableCards();
    }

    static void PlayFirstCommandFromQueue()
    {
        playingQueue = true;
        CommandQueue.Dequeue().StartCommandExecution();
    }
        
    public static void OnSceneReload()
    {
        CommandQueue.Clear();
        CommandExecutionComplete();
    }
}
