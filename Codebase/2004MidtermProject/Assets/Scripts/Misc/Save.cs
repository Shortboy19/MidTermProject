using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Save : MonoBehaviour
{
    public static void WriteString(string thingToWrite)
    {
#if !UNITY_EDITOR
        string path = Application.dataPath + "/SaveData.txt";
        using (StreamWriter writer = new StreamWriter(path, true))
        {
            writer.WriteLine(thingToWrite);
        }
#endif
        Debug.Log("Score Saved");
        //AssetDatabase.ImportAsset(path);
    }
    public static List<string> ReadString()
    {
        string path = Application.dataPath + "/SaveData.txt";
        List<string> result = new List<string>();

        //Read the text from directly from the test.txt file
        using (StreamReader reader = new StreamReader(path))
        {
            while (!reader.EndOfStream)
            {
                result.Add(reader.ReadLine());
            }
            return result;
        }
    }
}
