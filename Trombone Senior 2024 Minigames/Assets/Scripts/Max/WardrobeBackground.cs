using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WardrobeBackground : MonoBehaviour
{
    [Serializable]
    public class ListOfImages
    {
        public List<Image> images;
    }

    //choose random index, choose random image from that list, set that ones layer to 1, others to zero, play animation!
    [SerializeField] List<ListOfImages> images;
    List<List<Image>> imagesList = new();

    [SerializeField] float timer;
    [SerializeField] float interval;
    [SerializeField] UnityEngine.Color color1;
    [SerializeField] UnityEngine.Color color2;

    void Start()
    {
        foreach (ListOfImages i in images) imagesList.Add(i.images);
        UpdateImageStart();
    }

    void Update()
    {
        if (timer >= interval)
        {
            timer = 0;
            UpdateImage();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    public void UpdateImage()
    {
        int index = Random.Range(0, imagesList.Count);
        List<Image> im = imagesList[index];
        Image i = im[0].transform.parent.GetChild(0).GetComponent<Image>();

        i.transform.SetAsLastSibling();
        i.GetComponent<AnimatorSetTrigger>().SetTrigger();
        i.color = UnityEngine.Color.Lerp(color1, color2, Random.value);
    }

    public void UpdateImageStart()
    {
        for (int i = 0; i < imagesList.Count; i++)
        {
            List<Image> img = imagesList[i];
            Image im = img[0];

            im.transform.SetAsLastSibling();
            im.GetComponent<AnimatorSetTrigger>().SetTrigger();
            im.color = UnityEngine.Color.Lerp(color1, color2, Random.value);
        }
    }
}
