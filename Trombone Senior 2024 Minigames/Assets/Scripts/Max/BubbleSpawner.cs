using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Transform parent;
    [SerializeField] ExpandingBubble bubble;
    [SerializeField] UnityEngine.Color color1;
    [SerializeField] UnityEngine.Color color2;
    [SerializeField] float maxAlphaRate;
    [SerializeField] float minGrowthRate;
    [SerializeField] float maxGrowthRate;
    [SerializeField] float spawnRate;
    float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnRate)
        {
            SpawnBubble();
            timer = 0;
        }
    }

    public void SpawnBubble()
    {
        ExpandingBubble b = Instantiate(bubble, parent);
        b.transform.SetAsFirstSibling();
        float width = rectTransform.rect.width / 2;
        float height = rectTransform.rect.height / 2;
        float randX = Random.Range(-width, width);
        float randY = Random.Range(-height, height);

        b.GetComponent<RectTransform>().localPosition = new Vector2(randX, randY);
        b.image.color = UnityEngine.Color.Lerp(color1, color2, Random.value);
        b.alphaDecreaseRate = Mathf.Lerp(0.1f, maxAlphaRate, Random.value);

        b.growthRate = Mathf.Lerp(minGrowthRate, maxGrowthRate, Random.value);
    }
}
