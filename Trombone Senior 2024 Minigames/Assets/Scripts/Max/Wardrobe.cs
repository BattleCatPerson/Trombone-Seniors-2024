using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
public enum WardrobeState
{
    rolling, wardrobe
}
[Serializable]
public class IdSpritePair
{
    public int id;
    public List<Sprite> sprites;
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
    [SerializeField] Image previewSprite;
    [Header("State Transitions")]
    [SerializeField] WardrobeState wardrobeState;
    [SerializeField] GameObject rollPanel;
    [SerializeField] GameObject wardrobePanel;
    [SerializeField] AnimatorSetTrigger animator;
    [SerializeField] bool transitioning;
    [SerializeField] ListOfIdsToSprites pairs;
    [SerializeField] CostumeButton button;

    void Start()
    {
        fileHandler = new FileHandler(Application.persistentDataPath, fileName);
        data = fileHandler.Load();
        if (data == null) data = new CosmeticData();

        crates = Initialize();
        foreach (Cosmetic c in data.costumes)
        {
            AddToPanel(c.id);
        }
        foreach (var v in crates) v.Load(data);
        previewSprite.sprite = MatchIdToSprite(data.selectedId)[0];
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
    public void SaveData() => fileHandler.Save(data);

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

    public void AddToPanel(int id)
    {
        //GameObject g = new GameObject();
        //Image i = g.AddComponent<Image>();
        //i.sprite = MatchIdToSprite(id)[0];

        //g.transform.parent = collectionPanel;

        CostumeButton c = Instantiate(button, collectionPanel);
        c.Initiate(id, this, MatchIdToSprite(id)[0]);
        //c.transform.localScale = Vector3.one;
    }
    public void InititateSwitch()
    {
        if (transitioning) return;
        animator.SetTrigger();
        transitioning = true;
    }
    public void SwitchWardrobeState()
    {
        if (!transitioning) return;
        if (wardrobeState == WardrobeState.rolling)
        {
            rollPanel.SetActive(false);
            wardrobePanel.SetActive(true);
            wardrobeState = WardrobeState.wardrobe;
        }
        else if (wardrobeState == WardrobeState.wardrobe)
        {
            wardrobePanel.SetActive(false);
            rollPanel.SetActive(true);
            wardrobeState = WardrobeState.rolling;
        }
        transitioning = false;
    }

    public List<Sprite> MatchIdToSprite(int id)
    {
        foreach (IdSpritePair i in pairs.pairs)
        {
            if (i.id == id) return i.sprites;
        }
        Debug.Log("please add the id to the list");
        return null;
    }

    public void SelectId(int id)
    {
        data.selectedId = id;
        previewSprite.sprite = MatchIdToSprite(id)[0];
    }
}
