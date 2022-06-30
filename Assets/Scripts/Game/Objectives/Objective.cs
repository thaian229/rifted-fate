using System;
using UnityEngine;

public abstract class Objective : MonoBehaviour
{
    public string Title;
    public string Description;
    public bool IsCompleted { get; private set; }

    public static event Action<Objective> OnObjectiveCreated;
    public static event Action<Objective> OnObjectiveCompleted;

    protected virtual void Start()
    {
        OnObjectiveCreated?.Invoke(this);
    }

    public void UpdateObjective(string descriptionText, string counterText, string notificationText)
    {
        ObjectiveUpdateEvent evt = Events.ObjectiveUpdateEvent;
        evt.Objective = this;
        evt.DescriptionText = descriptionText;
        evt.CounterText = counterText;
        evt.NotificationText = notificationText;
        evt.IsComplete = IsCompleted;
        EventManager.Broadcast(evt);
    }

    public void CompleteObjective(string descriptionText, string counterText, string notificationText)
    {
        IsCompleted = true;

        ObjectiveUpdateEvent evt = Events.ObjectiveUpdateEvent;
        evt.Objective = this;
        evt.DescriptionText = descriptionText;
        evt.CounterText = counterText;
        evt.NotificationText = notificationText;
        evt.IsComplete = IsCompleted;
        EventManager.Broadcast(evt);

        OnObjectiveCompleted?.Invoke(this);
    }
}
