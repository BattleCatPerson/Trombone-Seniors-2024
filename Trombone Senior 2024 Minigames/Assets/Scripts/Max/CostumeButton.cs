using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostumeButton : MonoBehaviour
{
    public int id;
    public Wardrobe wardrobe;
    [SerializeField] Image image;

    public void Initiate(int id, Wardrobe wardrobe, Sprite sprite)
    {
        this.id = id;
        this.wardrobe = wardrobe;
        image.sprite = sprite;
    }
    public void AssignCosmetic() => wardrobe.SelectId(id);
}
