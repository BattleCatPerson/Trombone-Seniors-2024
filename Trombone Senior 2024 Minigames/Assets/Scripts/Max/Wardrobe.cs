using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
public enum WardrobeState
{
    menu, rolling, wardrobe, compendium
}
public enum SortState
{
    Newest, Oldest, Alphabetical, AlphaRev, Rarity, RarityRev
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
    [SerializeField] List<CosmeticData.Upgrade> unlockedUpgrades;
    [SerializeField] Cosmetic initialCosmetic;
    [SerializeField] Transform collectionPanel;
    [SerializeField] Image previewSprite;
    [SerializeField] TextMeshProUGUI previewName;
    [SerializeField] TextMeshProUGUI previewRarity;
    [SerializeField] RectTransform scrollPanel;
    [SerializeField] TextMeshProUGUI commonCount;
    [SerializeField] TextMeshProUGUI rareCount;
    [SerializeField] TextMeshProUGUI superRareCount;
    [SerializeField] CosmeticShop cosmeticShop;
    [Header("State Transitions")]
    [SerializeField] WardrobeState wardrobeState;
    [SerializeField] GameObject rollCanvas;
    [SerializeField] GameObject wardrobeCanvas;
    [SerializeField] GameObject menuCanvas;
    [SerializeField] AnimatorSetTrigger animator;
    [SerializeField] bool transitioning;
    [SerializeField] ListOfIdsToSprites pairs;
    [SerializeField] CostumeButton button;
    [SerializeField] CinemachineVirtualCamera menuCam;
    [SerializeField] CinemachineVirtualCamera lootboxCam;
    [SerializeField] CinemachineVirtualCamera wardrobeCam;
    [SerializeField] CinemachineVirtualCamera compendiumCam;
    [Header("Costume Sorting")]
    [SerializeField] List<SortState> sortStates;
    [SerializeField] SortState currentState;
    [SerializeField] TextMeshProUGUI sortText;
    [Header("Compendium")]
    [SerializeField] List<CompendiumButton> compendiumButtons;
    Dictionary<int, CompendiumButton> idCompendiumButtonPairs = new();
    void Start()
    {
        fileHandler = new FileHandler(Application.persistentDataPath, fileName);
        data = fileHandler.Load();
        if (data == null) data = new CosmeticData(initialCosmetic);

        crates = Initialize();

        SortPanel(true);
        foreach (var v in crates) v.Load(data);

        previewSprite.sprite = MatchIdToSprite(data.selectedId)[0];
        previewName.text = MatchIdToName(data.selectedId);
        previewRarity.text = MatchIdToRarity(data.selectedId);

        costumes.AddRange(data.costumes);
        SwitchWardrobeState(WardrobeState.menu);
        UpdatePanel();

        EnableCompendiumButtons();

        var r = cosmeticShop.ReturnRarityNumbers();
        SetCompendiumTexts(r[0], r[1], r[2]);

        foreach (var v in data.upgrades)
        {
            if (v.id == 2)
            {
                cosmeticShop.EnableLuckUpgrade();
                break;
            }
        }
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;
    }

    void Update()
    {
        costumes = data.costumes;
        trails = data.trails;
    }
    public void UpdatePanel()
    {
        int rows = (int)(Math.Ceiling(costumes.Count / 4f));
        float posY = 20 - (20 * (rows - 1));
        float height = 40 * rows;
        scrollPanel.sizeDelta = new Vector2(scrollPanel.sizeDelta.x, height);
        scrollPanel.anchoredPosition = Vector2.up * posY;
        SortPanel(currentState);
    }
    public void SetCompendiumTexts(int cCount, int rCount, int sCount)
    {
        int c = 0;
        int r = 0;
        int s = 0;

        foreach (var v in data.costumes)
        {
            if (v.rarity == Rarity.Common) c++;
            else if (v.rarity == Rarity.Rare) r++;
            else s++;
        }

        commonCount.text = $"{c}/{cCount}";
        rareCount.text = $"{r}/{rCount}";
        superRareCount.text = $"{s}/{sCount}";
    }
    private void OnApplicationQuit()
    {
        fileHandler.Save(data);
    }
    public void SaveData()
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
    public void SwitchWardrobeState(WardrobeState state)
    {
        //if (!transitioning) return;
        if (state == WardrobeState.rolling)
        {
            //rollCanvas.SetActive(true);
            //wardrobeCanvas.SetActive(false);
            //menuCanvas.SetActive(false);

            lootboxCam.Priority = 2;
            wardrobeCam.Priority = 1;
            menuCam.Priority = 0;
            compendiumCam.Priority = 0;
        }
        else if (state == WardrobeState.wardrobe)
        {
            //rollCanvas.SetActive(false);
            //wardrobeCanvas.SetActive(true);
            //menuCanvas.SetActive(false);

            lootboxCam.Priority = 1;
            wardrobeCam.Priority = 2;
            menuCam.Priority = 0;
            compendiumCam.Priority = 0;
        }
        else if (state == WardrobeState.menu)
        {
            //rollCanvas.SetActive(false);
            //wardrobeCanvas.SetActive(false);
            //menuCanvas.SetActive(true);

            lootboxCam.Priority = 0;
            wardrobeCam.Priority = 1;
            menuCam.Priority = 2;
            compendiumCam.Priority = 0;
        }
        else if (state == WardrobeState.compendium)
        {
            compendiumCam.Priority = 2;
            lootboxCam.Priority = 0;
            wardrobeCam.Priority = 0;
            menuCam.Priority = 0;
        }
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

    public string MatchIdToName(int id)
    {
        foreach (Cosmetic c in data.costumes) if (id == c.id) return c.name;
        return "ERROR NOT GOOD";
    }

    public string MatchIdToRarity(int id)
    {
        string r = "";
        foreach (Cosmetic c in data.costumes) if (id == c.id) r = c.rarity.ToString();
        if (r == "SuperRare") return "Super Rare";
        else if (r.Length > 0) return r;
        return "ERROR NOT GOOD";
    }

    public void SelectId(int id)
    {
        data.selectedId = id;
        previewSprite.sprite = MatchIdToSprite(id)[0];
        previewName.text = MatchIdToName(data.selectedId);
        previewRarity.text = MatchIdToRarity(data.selectedId);
    }

    public void SortAlphabetically(bool reverse)
    {
        //delete all current buttons
        //dictionary: string to cosmetic. one list of names.
        //go through all obtained cosmetics, add names to list of names.
        //sort list of names.
        //add cosmetic buttons in the order of list of names.
        ResetPanel();

        Dictionary<string, int> dict = new();
        List<string> nameOrder = new();

        foreach (Cosmetic c in data.costumes)
        {
            dict[c.name] = c.id;
            nameOrder.Add(c.name);
        }
        nameOrder.Sort();

        if (!reverse)
        {
            foreach (string s in nameOrder) AddToPanel(dict[s]);
            return;
        }

        nameOrder.Reverse();
        foreach (string s in nameOrder) AddToPanel(dict[s]);
    }

    public void SortByTime(bool reverse)
    {
        ResetPanel();
        if (!reverse)
        {
            foreach (Cosmetic c in data.costumes)
            {
                AddToPanel(c.id);
            }
            return;
        }

        List<Cosmetic> temp = new();
        foreach (Cosmetic c in data.costumes) temp.Add(c);
        temp.Reverse();
        foreach (Cosmetic c in temp) AddToPanel(c.id);
    }

    public void SortByRarity(bool reverse)
    {
        ResetPanel();
        Dictionary<string, int> nameToId = new();
        List<string> commons = new();
        List<string> rares = new();
        List<string> superRares = new();
        foreach (Cosmetic c in data.costumes)
        {
            if (c.rarity == Rarity.Common) commons.Add(c.name);
            if (c.rarity == Rarity.Rare) rares.Add(c.name);
            if (c.rarity == Rarity.SuperRare) superRares.Add(c.name);

            nameToId[c.name] = c.id;
        }

        commons.Sort();
        rares.Sort();
        superRares.Sort();
        
        if (reverse)
        {
            commons.Reverse();
            rares.Reverse();
            superRares.Reverse();
            
            foreach (string name in commons) AddToPanel(nameToId[name]);
            foreach (string name in rares) AddToPanel(nameToId[name]);
            foreach (string name in superRares) AddToPanel(nameToId[name]);

            return;
        }
        foreach (string name in superRares) AddToPanel(nameToId[name]); 
        foreach (string name in rares) AddToPanel(nameToId[name]); 
        foreach (string name in commons) AddToPanel(nameToId[name]); 
    }

    public void ResetPanel()
    {
        for (int i = 0; i < collectionPanel.childCount; i++)
        {
            Destroy(collectionPanel.GetChild(i).gameObject);
        }
    }

    public void SortPanel(bool initial = false)
    {
        if (!initial)
        {
            data.sortId++;
            data.sortId = data.sortId % sortStates.Count;
        }
        if (sortStates[data.sortId] == SortState.Newest) SortByTime(true);
        else if (sortStates[data.sortId] == SortState.Oldest) SortByTime(false);
        else if (sortStates[data.sortId] == SortState.Alphabetical) SortAlphabetically(false);
        else if (sortStates[data.sortId] == SortState.AlphaRev) SortAlphabetically(true);
        else if (sortStates[data.sortId] == SortState.Rarity) SortByRarity(false);
        else if (sortStates[data.sortId] == SortState.RarityRev) SortByRarity(true);

        currentState = sortStates[data.sortId];

        sortText.text = "Sort By: " + ReturnStringSort(sortStates[data.sortId]);
    }
    public void SortPanel(SortState state)
    {
        if (state == SortState.Newest) SortByTime(true);
        else if (state == SortState.Oldest) SortByTime(false);
        else if (state == SortState.Alphabetical) SortAlphabetically(false);
        else if (state == SortState.AlphaRev) SortAlphabetically(true);
        else if (state == SortState.Rarity) SortByRarity(false);
        else if (state == SortState.RarityRev) SortByRarity(true);
    }

    public string ReturnStringSort(SortState sortState)
    {
        if (sortState == SortState.AlphaRev) return "Alphabetical (Reverse)";
        if (sortState == SortState.RarityRev) return "Rarity (Reverse)";
        return sortState.ToString();
    }

    public void EnableCompendiumButtons()
    {
        foreach (var button in compendiumButtons) idCompendiumButtonPairs[button.id] = button;
        foreach (Cosmetic c in data.costumes)
        {
            if (idCompendiumButtonPairs.ContainsKey(c.id)) idCompendiumButtonPairs[c.id].EnableButton();
        }
    }

    public void EnableCompendiumButton(int id)
    {
        idCompendiumButtonPairs[id].EnableButton();
    }
}
