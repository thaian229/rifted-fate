using UnityEngine;

public static class Events
{
    public static ObjectiveUpdateEvent ObjectiveUpdateEvent = new ObjectiveUpdateEvent();
    public static AllObjectivesCompletedEvent AllObjectivesCompletedEvent = new AllObjectivesCompletedEvent();
    public static GameOverEvent GameOverEvent = new GameOverEvent();
    public static PlayerDeathEvent PlayerDeathEvent = new PlayerDeathEvent();
    public static EnemyKillEvent EnemyKillEvent = new EnemyKillEvent();
    public static PickupEvent PickupEvent = new PickupEvent();
    public static DamageEvent DamageEvent = new DamageEvent();
}

public class ObjectiveUpdateEvent : GameEvent
{
    public Objective Objective;
    public string DescriptionText;
    public string CounterText;
    public bool IsComplete;
    public string NotificationText;
}

public class AllObjectivesCompletedEvent : GameEvent { }

public class GameOverEvent : GameEvent
{
    public bool Win;
}

public class PlayerDeathEvent : GameEvent { }

public class EnemyKillEvent : GameEvent
{
    public GameObject Enemy;
    public int RemainingEnemyCount;
}

public class PickupEvent : GameEvent
{
    public GameObject Pickup;
}

public class DamageEvent : GameEvent
{
    public GameObject Sender;
    public float DamageValue;
}