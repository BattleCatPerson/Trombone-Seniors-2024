using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wardrobe : MonoBehaviour
{
    public CosmeticData data;
    public List<IWardrobe> crates;

    [SerializeField] string fileName;
    [SerializeField] FileHandler fileHandler;
    [SerializeField] List<int> costumes;
    [SerializeField] List<int> trails;

    void Start()
    {
        fileHandler = new FileHandler(Application.persistentDataPath, fileName);
        data = fileHandler.Load();
        if (data == null) data = new CosmeticData();

        crates = Initialize();

        foreach (var v in crates) v.Load(data);
    }

    void Update()
    {
        costumes = data.costumes;
        trails = data.trails;
    }

    private void OnApplicationQuit()
    {
        fileHandler.Save(data);
    }

    public List<IWardrobe> Initialize()
    {
        IEnumerable<IWardrobe> w = FindObjectsOfType<MonoBehaviour>().OfType<IWardrobe>();

        return new List<IWardrobe>(w);
    }

    public void ResetData()
    {
        data = new();
        foreach (var v in crates) v.Load(data);
    }
}
