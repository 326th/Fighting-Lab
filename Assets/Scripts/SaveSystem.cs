using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer (Character_Base character_Base)
    {
        Debug.Log("saving...");
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.bin";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerStats data = new PlayerStats(character_Base);
        Debug.Log(data.hitCount);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerStats LoadPlayer()
    {
        Debug.Log("loading...");
        string path = Application.persistentDataPath + "/player.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerStats data = formatter.Deserialize(stream) as PlayerStats;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found");
            return null;
        }
    }
}
