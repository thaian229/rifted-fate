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

    public static byte[] SerializeGameData(GameManager gameManager)
    {
        GameData gameData = new GameData(gameManager);
        byte[] bytes;
        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream stream = new MemoryStream())
        {
            formatter.Serialize(stream, gameData);
            bytes = stream.ToArray();
        }
        return bytes;
    }

    public static GameData DeserializeGameData(byte[] bytes)
    {
        GameData gameData = null;
        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream stream = new MemoryStream(bytes))
        {
            gameData = formatter.Deserialize(stream) as GameData;
        }
        return gameData;
    }
}
