using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum WardrobeState
{
    rolling, wardrobe
}

public class Wardrobe : MonoBehaviour
{
    public CosmeticData data;
    public List<IWardrobe> crates;
    [Header("File")]
    [SerializeField] string fileName;
    [SerializeField] FileHandler fileHandler;
    [Header("Lists")]
    [SerializeField] List<Cosmetic> costumes;
    [SerializeField] List<Cosmetic> trails;
    [SerializeField] Transform collectionPanel;
    [Header("State Transitions")]
    [SerializeField] WardrobeState wardrobeState;
    [SerializeField] GameObject rollPanel;
    [SerializeField] GameObject wardrobePanel;
    [SerializeField] Animator animator;

    void Start()
    {
        fileHandler = new FileHandler(Application.persistentDataPath, fileName);
        data = fileHandler.Load();
        if (data == null) data = new CosmeticData();

        crates = Initialize();
        foreach (Cosmetic c in data.costumes) AddToPanel(c.sprite);
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

    public void AddToPanel(Sprite sprite)
    {
        GameObject g = new GameObject();
        Image i = g.AddComponent<Image>();
        i.sprite = sprite;

        g.transform.parent = collectionPanel;
    }

    public void SwitchWardrobeState()
    {
        if (wardrobeState == WardrobeState.rolling)
        {
            rollPanel.SetActive(false);
        }
        if (wardrobeState == WardrobeState.wardrobe)
        {
            wardrobePanel.SetActive(false);
        }
    }
}
