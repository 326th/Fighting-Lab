using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer (Character_Base character_Base, float playTime)
    {
        Debug.Log("saving...");
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.bin";
        

        PlayerStats new_match = new PlayerStats(character_Base, playTime);

        if (File.Exists(path))
        {
            Debug.Log("Append to old file");
            PlayerStats old_match = LoadPlayer();
            new_match.matchesData.ForEach(match => {
                old_match.matchesData.Add(match);
            });
            new_match = old_match;
        }
        else
        {
            Debug.Log("Create new file");
            
        }

        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, new_match);
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
