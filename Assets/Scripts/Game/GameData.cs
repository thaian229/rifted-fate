using UnityEngine;

[System.Serializable]
public class GameData
{
    public int Credit;
    public int HealthLevel;
    public int SpeedLevel;
    public int[] OwnedGuns;
    public int[] EquipedGuns;

    public GameData(GameManager gameManager)
    {
        this.Credit = gameManager.Creadit;
        this.HealthLevel = gameManager.HealthLevel;
        this.SpeedLevel = gameManager.SpeedLevel;

        int size = gameManager.OwnedGuns.Count;
        this.OwnedGuns = new int[size];
        for (int i = 0; i < size; i++)
        {
            this.OwnedGuns[i] = gameManager.OwnedGuns[i];
        }

        this.EquipedGuns = new int[2];
        this.EquipedGuns[0] = gameManager.EquipedGuns[0];
        this.EquipedGuns[1] = gameManager.EquipedGuns[1];
    }
}
