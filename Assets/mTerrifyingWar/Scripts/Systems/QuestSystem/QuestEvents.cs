public class QuestEvents
{
    private readonly QuestTracker _questTracker;

    public QuestEvents(QuestTracker questTracker)
    {
        _questTracker = questTracker;
    }
    
    public void Report(string objectiveId, int amount = 1)
    {
        _questTracker.ReportProgress(objectiveId, amount);
    }
}