using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class Save : MonoBehaviour
{
    public static void WriteString(string thingToWrite)
    {
        string path = "Assets/SaveText.txt";

        using (StreamWriter writer = new StreamWriter(path, true))
        {
            writer.WriteLine(thingToWrite);
        }
        AssetDatabase.ImportAsset(path);
    }
    public static List<string> ReadString()
    {
        string path = "Assets/SaveText.txt";
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
