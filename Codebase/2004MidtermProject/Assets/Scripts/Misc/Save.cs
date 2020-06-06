using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Save : MonoBehaviour
{
    public static void WriteString(string thingToWrite)
    {
        string path = "Assets/SaveText.txt";

        using (StreamWriter writer = new StreamWriter(path, false))
        {
            writer.WriteLine(thingToWrite);
        }
        AssetDatabase.ImportAsset(path);
    }
    public static string ReadString()
    {
        string path = "Assets/SaveText.txt";

        //Read the text from directly from the test.txt file
        using (StreamReader reader = new StreamReader(path))
        {
            string red = reader.ReadLine();
            return red;
        }
    }
}
