using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerSprite : MonoBehaviour
{
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] SpriteRenderer boardSprite;
    [SerializeField] Image gameOverImage;
    [SerializeField] ListOfIdsToSprites pairs;
    [SerializeField] int currentId;
    [SerializeField] string fileName;
    void Start()
    {
        FileHandler fileHandler = new FileHandler(Application.persistentDataPath, fileName);
        CosmeticData data = fileHandler.Load();
        if (data != null) currentId = data.selectedId;
        else currentId = -1;

        foreach (IdSpritePair i in pairs.pairs)
        {
            if (i.id == currentId)
            {
                playerSprite.sprite = i.sprites[0];
                gameOverImage.sprite = i.sprites[0];
                boardSprite.sprite = i.sprites[1];
                break;
            }
        }
    }
}
