using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class ScreenPositionManager
{
    public float percentageOutOffScreen { get; set; } = 0.1f;
    public bool spawnOnAxis { get; set; } = false;

    private ScreenBorder screenBorder;

    public ScreenPositionManager()
    {
        screenBorder = new ScreenBorder();
    }

    public Vector2 GetRandomPositionOnScreen()
    {
        screenBorder.CalculateScreenBorders();
        return new Vector2(Random.Range(screenBorder.bottomLeft.x, screenBorder.bottomRight.x), Random.Range(screenBorder.bottomLeft.y, screenBorder.topLeft.y));
    }

    public Vector2 GetRandomPositionArroundTarget(GameObject target, float radius)
    {
        return  (Random.insideUnitCircle * radius) + (Vector2)target.transform.position;
    }

    public Vector2 GetRandomPositionOnCircleArroundTarget(GameObject target, float radius)
    {
        return Random.insideUnitCircle.normalized * radius + (Vector2)target.transform.position;
    }

    public Vector2 ArroundTargetWithSpace(GameObject target, float radius, float space)
    {
        Vector2 position = GetRandomPositionArroundTarget(target, radius - space);
        Vector2 direction = (position - (Vector2)target.transform.position).normalized;
        return position + direction * space;
    }

    public Vector2 GetRandomPositionOutOfScreen()
    {
        SpawnDirection[] directions = (SpawnDirection[])Enum.GetValues(typeof(SpawnDirection));
        SpawnDirection randomDirection = directions[Random.Range(0, directions.Length)];
        return GetPositionOutOfScreen(randomDirection);
    }

    public Vector2 GetPositionOutOfScreen(params SpawnDirection[] spawnDirections)
    {
        return GetPositionOutOfScreen(spawnDirections[Random.Range(0, spawnDirections.Length)]);
    }

    public Vector2 GetPostionOnCircleOutOfScreen(GameObject gameObject)
    {
        screenBorder.CalculateScreenBorders();
        Vector2 middle = (screenBorder.bottomLeft + screenBorder.topRight) / 2;
        gameObject.transform.position = middle;
        return GetRandomPositionOnCircleArroundTarget(gameObject, Vector2.Distance(middle, screenBorder.topLeft));
    }

    private Vector2 GetPositionOutOfScreen(SpawnDirection direction)
    {
        screenBorder.CalculateScreenBorders();
        float x = screenBorder.bottomLeft.x + screenBorder.width * 0.5f;
        float y = screenBorder.bottomLeft.y + screenBorder.height * 0.5f;

        float additionalWidth = screenBorder.width * percentageOutOffScreen;
        float additionalHeight = screenBorder.height * percentageOutOffScreen;

        if (spawnOnAxis)
        {
            x = Random.Range(screenBorder.bottomLeft.x - additionalWidth, screenBorder.bottomRight.x + additionalWidth);
            y = Random.Range(screenBorder.bottomLeft.y - additionalHeight, screenBorder.topLeft.y + additionalHeight);
        }

        switch (direction)
        {
            case SpawnDirection.TOP:
                y = screenBorder.topLeft.y + additionalHeight;
                break;
            case SpawnDirection.BOTTOM:
                y = screenBorder.bottomLeft.y - additionalHeight;
                break;
            case SpawnDirection.LEFT:
                x = screenBorder.bottomLeft.x - additionalWidth;
                break;
            case SpawnDirection.RIGHT:
                x = screenBorder.bottomRight.x + additionalWidth;
                break;
            case SpawnDirection.TOP_LEFT:
                x = screenBorder.bottomLeft.x - additionalWidth;
                y = screenBorder.topLeft.y + additionalHeight;
                break;
            case SpawnDirection.TOP_RIGHT:
                x = screenBorder.bottomRight.x + additionalWidth;
                y = screenBorder.topRight.y + additionalHeight;
                break;
            case SpawnDirection.BOTTOM_LEFT:
                x = screenBorder.bottomLeft.x - additionalWidth;
                y = screenBorder.bottomLeft.y - additionalHeight;
                break;
            case SpawnDirection.BOTTOM_RIGHT:
                x = screenBorder.bottomRight.x + additionalWidth;
                y = screenBorder.bottomRight.y - additionalHeight;
                break;
        }
        return new Vector2(x, y);
    }

}
