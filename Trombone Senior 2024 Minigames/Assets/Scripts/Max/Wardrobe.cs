using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using Cinemachine;
public enum WardrobeState
{
    menu, rolling, wardrobe
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
    [SerializeField] Cosmetic initialCosmetic;
    [SerializeField] Transform collectionPanel;
    [SerializeField] Image previewSprite;
    [SerializeField] RectTransform scrollPanel;
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

    void Start()
    {
        fileHandler = new FileHandler(Application.persistentDataPath, fileName);
        data = fileHandler.Load();
        if (data == null) data = new CosmeticData(initialCosmetic);
        crates = Initialize();
        foreach (Cosmetic c in data.costumes)
        {
            AddToPanel(c.id);
        }
        foreach (var v in crates) v.Load(data);
        previewSprite.sprite = MatchIdToSprite(data.selectedId)[0];

        costumes.AddRange(data.costumes);
        SwitchWardrobeState(WardrobeState.menu);
        UpdatePanel();
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
        }
        else if (state == WardrobeState.wardrobe)
        {
            //rollCanvas.SetActive(false);
            //wardrobeCanvas.SetActive(true);
            //menuCanvas.SetActive(false);

            lootboxCam.Priority = 1;
            wardrobeCam.Priority = 2;
            menuCam.Priority = 0;
        }
        else if (state == WardrobeState.menu)
        {
            //rollCanvas.SetActive(false);
            //wardrobeCanvas.SetActive(false);
            //menuCanvas.SetActive(true);

            lootboxCam.Priority = 0;
            wardrobeCam.Priority = 1;
            menuCam.Priority = 2;
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

    public void SelectId(int id)
    {
        data.selectedId = id;
        previewSprite.sprite = MatchIdToSprite(id)[0];
    }
}
