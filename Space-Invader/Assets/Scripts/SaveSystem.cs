using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.Timeline.Actions;

/// <summary>
/// A class designed to operate the game saving and loading system
/// </summary>
public static class SaveSystem
{
    /// <summary>
    /// Saving game
    /// </summary>
    /// <param name="player">Player instance</param>
    public static void SavePlayer(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.data";
        //string path = "D:/Games/Space-Invader/player.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    /// <summary>
    /// Loading game
    /// </summary>
    /// <returns>Data to load</returns>
    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
