using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileHandler
{
    private string path;
    private string fileName;

    public FileHandler(string p, string f)
    {
        path = p;
        fileName = f;
    }
    public CosmeticData Load()
    {
        string fullPath = Path.Combine(path, fileName);
        CosmeticData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<CosmeticData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError($"can't load file ouaewfhaweoifjoweiaf at path {fullPath}");
            }
        }
        return loadedData;
    }
    public void Save(CosmeticData data)
    {
        string fullPath = Path.Combine(path, fileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("error saving loooll");
        }
    }
}
