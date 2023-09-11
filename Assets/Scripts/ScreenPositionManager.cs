using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class ScreenPositionManager
{
    public float percentageOutOffScreen { get; set; } = 0.1f;
    public bool spawnOnAxis { get; set; } = false;
    public bool spawnOnCircle { get; set; } = false;

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
        return (Random.insideUnitCircle * radius) + (Vector2)target.transform.position;
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

    private Vector2 GetPostionOnCircleOutOfScreen(SpawnDirection direction)
    {
        screenBorder.CalculateScreenBorders();
        float x = screenBorder.bottomLeft.x + screenBorder.width * 0.5f;
        float y = screenBorder.bottomLeft.y + screenBorder.height * 0.5f;

        Vector2 middle = (screenBorder.bottomLeft + screenBorder.topRight) / 2;
        float radius = Vector2.Distance(middle, screenBorder.topLeft);
        radius += radius * percentageOutOffScreen;

        float directionAngle = Vector2.SignedAngle(Vector2.right, new Vector2(1, 1) - middle);
        float angle = 90;

        switch (direction)
        {
            case SpawnDirection.TOP:
                angle = Random.Range(-directionAngle + 90, directionAngle + 90);
                y = (middle + (Vector2.up - middle).normalized * radius).y;
                break;
            case SpawnDirection.BOTTOM:
                angle = Random.Range(-directionAngle - 90, directionAngle - 90);
                y = (middle + Vector2.down * radius).y;
                break;
            case SpawnDirection.LEFT:
                angle = Random.Range(-directionAngle - 180, directionAngle - 180);
                x = (middle + Vector2.left * radius).x;
                break;
            case SpawnDirection.RIGHT:
                angle = Random.Range(-directionAngle, directionAngle);
                x = (middle + Vector2.right * radius).x;
                break;
            case SpawnDirection.TOP_LEFT:
                Vector2 topLeft = middle + (new Vector2(-1, 1) - middle).normalized * radius;
                x = topLeft.x;
                y = topLeft.y;
                break;
            case SpawnDirection.TOP_RIGHT:
                Debug.Log(((Vector2)screenBorder.topRight - middle).normalized);
                Vector2 topRight = middle + (new Vector2(1, 1) - middle).normalized * radius;
                x = topRight.x;
                y = topRight.y;
                break;
            case SpawnDirection.BOTTOM_LEFT:
                Vector2 bottomLeft = middle + (new Vector2(-1, -1) - middle).normalized * radius;
                x = bottomLeft.x;
                y = bottomLeft.y;
                break;
            case SpawnDirection.BOTTOM_RIGHT:
                Vector2 bottomRight = middle + (new Vector2(1, -1) - middle).normalized * radius;
                x = bottomRight.x;
                y = bottomRight.y;
                break;
        }

        if (spawnOnCircle)
        {
            float angleInRadians = angle * Mathf.Deg2Rad;
            Vector2 onCircle = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)) * radius;
            x = onCircle.x;
            y = onCircle.y;
        }
        return new Vector2(x, y);
    }

}
