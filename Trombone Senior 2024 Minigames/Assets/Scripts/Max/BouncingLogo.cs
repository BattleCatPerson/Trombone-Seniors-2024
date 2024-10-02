using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BouncingLogo : MonoBehaviour
{
    public enum Direction
    {
        UpRight, UpLeft, DownRight, DownLeft
    }

    [SerializeField] RectTransform border;
    [SerializeField] RectTransform logo;
    [SerializeField] Direction direction;
    [SerializeField] float speed;
    [SerializeField] Vector2 directionVector;
    [SerializeField] Vector2 maxDimensions;
    [SerializeField] Vector2 logoDimensions;
    [SerializeField] Image logoImage;
    [SerializeField] List<UnityEngine.Color> colors;
    void Start()
    {
        List<Direction> directions = new List<Direction>() { Direction.UpRight, Direction.UpLeft, Direction.DownRight, Direction.DownLeft};
        UpdateDirection(directions[Random.Range(0, 4)]);
        maxDimensions = new Vector2(border.sizeDelta.x / 2, border.sizeDelta.y / 2);
        logoDimensions = new Vector2(logo.sizeDelta.x / 2, logo.sizeDelta.y / 2);
    }

    void FixedUpdate()
    {
        logo.anchoredPosition += directionVector * speed;
        if (logo.anchoredPosition.x + logoDimensions.x >= maxDimensions.x)
        {
            if (direction == Direction.UpRight) UpdateDirection(Direction.UpLeft);
            if (direction == Direction.DownRight) UpdateDirection(Direction.DownLeft);
        }
        if (logo.anchoredPosition.y + logoDimensions.y >= maxDimensions.y)
        {
            if (direction == Direction.UpRight) UpdateDirection(Direction.DownRight);
            if (direction == Direction.UpLeft) UpdateDirection(Direction.DownLeft);
        }
        if (logo.anchoredPosition.x - logoDimensions.x <= -maxDimensions.x)
        {
            if (direction == Direction.UpLeft) UpdateDirection(Direction.UpRight);
            if (direction == Direction.DownLeft) UpdateDirection(Direction.DownRight);
        }
        if (logo.anchoredPosition.y - logoDimensions.y <= -maxDimensions.y)
        {
            if (direction == Direction.DownRight) UpdateDirection(Direction.UpRight);
            if (direction == Direction.DownLeft) UpdateDirection(Direction.UpLeft);
        }
    }

    public void UpdateDirection(Direction d)
    {
        UnityEngine.Color current = logoImage.color;
        UnityEngine.Color selected = colors[Random.Range(0, colors.Count)];
        while (current == selected) selected = colors[Random.Range(0, colors.Count)];
        logoImage.color = selected;
        direction = d;
        if (direction == Direction.UpRight) 
        {
            directionVector = (new Vector2(1, 1)).normalized;
        }
        else if (direction == Direction.UpLeft) 
        {
            directionVector = (new Vector2(-1, 1)).normalized;
        }
        else if (direction == Direction.DownLeft)
        {
            directionVector = (new Vector2(-1, -1)).normalized;
        }
        else if (direction == Direction.DownRight)
        {
            directionVector = (new Vector2(1, -1)).normalized;
        }
    }
}
