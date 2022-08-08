using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    public static void SaveGame(GameManager gameManager)
    {
        string savePath = Application.persistentDataPath + "/rifted_fate.data";
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(savePath, FileMode.Create))
        {
            GameData gameData = new GameData(gameManager);
            formatter.Serialize(stream, gameData);
        }
    }

    public static GameData LoadGame()
    {
        string savePath = Application.persistentDataPath + "/rifted_fate.data";
		if (File.Exists(savePath))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			GameData gameData;
			using (FileStream stream = new FileStream(savePath, FileMode.Open))
			{
				gameData = formatter.Deserialize(stream) as GameData;
			}
			return gameData;
		}
		else
		{
			return null;
		}
    }
}
