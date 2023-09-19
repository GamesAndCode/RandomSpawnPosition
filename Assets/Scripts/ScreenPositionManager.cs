using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScreenPositionManager
{
    public float percentageOutOffScreen { get; set; } = 0.1f;

    /// <summary>
    /// Defines if the position is on the axis or on a point from the given direction 
    /// </summary>
    public bool spawnOnAxis { get; set; } = false;

    /// <summary>
    /// Defines if the position is on the circle or on a point from the given direction 
    /// </summary>
    public bool spawnOnCircle { get; set; } = false;

    public bool spawnCircular { get; set; } = false;

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

    /// <summary>
    /// Get a random position inside a circle arround the target.
    /// </summary>
    public Vector2 GetRandomPositionArroundTarget(GameObject target, float radius)
    {
        return (Random.insideUnitCircle * radius) + (Vector2)target.transform.position;
    }

    /// <summary>
    /// Get a random position arround a target with a given radius.
    /// </summary>
    public Vector2 GetRandomPositionOnCircleArroundTarget(GameObject target, float radius)
    {
        return Random.insideUnitCircle.normalized * radius + (Vector2)target.transform.position;
    }

    /// <summary>
    /// Get 
    /// </summary>
    public Vector2 GetRandomPositionInsideDonutArroundTarget(GameObject target, float outerRadius, float innerRadius)
    {
        Vector2 position = GetRandomPositionArroundTarget(target, outerRadius - innerRadius);
        Vector2 direction = (position - (Vector2)target.transform.position).normalized;
        return position + direction * innerRadius;
    }

    /// <summary>
    /// Get a random position out of screen. Distance is defined by percentageOutOffScreen.
    /// </summary>
    public Vector2 GetRandomPositionOutOfScreen()
    {
        SpawnDirection[] directions = (SpawnDirection[])Enum.GetValues(typeof(SpawnDirection));
        SpawnDirection randomDirection = directions[Random.Range(0, directions.Length)];
        return spawnCircular ? GetPostionOnCircleOutOfScreen(randomDirection) : GetPostionOnCircleOutOfScreen(randomDirection);
    }

    /// <summary>
    /// Get a random position out of screen defined the input directions.
    /// </summary>
    /// <param name="spawnDirections">TOP, BOTTOM, LEFT, RIGHT,</param>
    public Vector2 GetPositionOutOfScreen(params SpawnDirection[] spawnDirections)
    {
        SpawnDirection randomSpawnDirection = spawnDirections[Random.Range(0, spawnDirections.Length)];
        return spawnCircular ? GetPostionOnCircleOutOfScreen(randomSpawnDirection) : GetPositionOutOfScreen(randomSpawnDirection);
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
