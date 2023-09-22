using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomPosition
{
    // Affects how far the position is out of screen.
    // Percentage calculated with screen width and height.
    public float PercentageOffscreen { get; set; } = 0.1f;

    // Out of screen position using screen or circular shape.
    public bool Circular { get; set; } = false;

    // Out of screen position on axis or on a point in the given direction 
    public bool SinglePoint { get; set; } = false;


    private ScreenBorders screenBorder;

    public RandomPosition()
    {
        screenBorder = new ScreenBorders();
    }

    /// <summary>
    /// Get a random position around the target inside a circle .
    /// </summary>
    public Vector2 AroundTargetInCircle(GameObject target, float radius)
    {
        return (Random.insideUnitCircle * radius) + (Vector2)target.transform.position;
    }

    /// <summary>
    /// Get a random position around a target on a given radius.
    /// </summary>
    public Vector2 AroundTargetOnCircle(GameObject target, float radius)
    {
        return Random.insideUnitCircle.normalized * radius + (Vector2)target.transform.position;
    }

    /// <summary>
    /// Get a random position around the target within a given area defined by the inner and outer radius.
    /// </summary>
    public Vector2 AroundTargetInDonut(GameObject target, float innerRadius, float outerRadius)
    {
        Vector2 position = AroundTargetInCircle(target, outerRadius - innerRadius);
        Vector2 direction = (position - (Vector2)target.transform.position).normalized;
        return position + direction * innerRadius;
    }

    public Vector2 OnScreen()
    {
        screenBorder.CalculateScreenBorders();
        return new Vector2(Random.Range(screenBorder.bottomLeft.x, screenBorder.bottomRight.x), Random.Range(screenBorder.bottomLeft.y, screenBorder.topLeft.y));
    }

    /// <summary>
    /// Get a random position out of screen. Distance is defined by percentageOutOffScreen.
    /// </summary>
    public Vector2 OutOfScreen()
    {
        ScreenDirection[] directions = (ScreenDirection[])Enum.GetValues(typeof(ScreenDirection));
        ScreenDirection randomDirection = directions[Random.Range(0, directions.Length)];
        return Circular ? OutOfScreenDirectionCircular(randomDirection) : OutOfScreenDirection(randomDirection);
    }

    /// <summary>
    /// Get a random position out of screen defined by the input directions.
    /// When using singlePoint == false, only TOP, BOTTOM, LEFT, RIGHT are needed.
    /// </summary>
    /// <param name="screenDirections">Values of ScreenDirection enum</param>
    public Vector2 OutOfScreenDirection(params ScreenDirection[] screenDirections)
    {
        ScreenDirection randomSpawnDirection = screenDirections[Random.Range(0, screenDirections.Length)];
        return Circular ? OutOfScreenDirectionCircular(randomSpawnDirection) : OutOfScreenDirection(randomSpawnDirection);
    }

    private Vector2 OutOfScreenDirection(ScreenDirection direction)
    {
        screenBorder.CalculateScreenBorders();
        float x = screenBorder.bottomLeft.x + screenBorder.width * 0.5f;
        float y = screenBorder.bottomLeft.y + screenBorder.height * 0.5f;

        float additionalWidth = screenBorder.width * PercentageOffscreen;
        float additionalHeight = screenBorder.height * PercentageOffscreen;

        if (!SinglePoint)
        {
            x = Random.Range(screenBorder.bottomLeft.x - additionalWidth, screenBorder.bottomRight.x + additionalWidth);
            y = Random.Range(screenBorder.bottomLeft.y - additionalHeight, screenBorder.topLeft.y + additionalHeight);
        }

        switch (direction)
        {
            case ScreenDirection.TOP:
                y = screenBorder.topLeft.y + additionalHeight;
                break;
            case ScreenDirection.BOTTOM:
                y = screenBorder.bottomLeft.y - additionalHeight;
                break;
            case ScreenDirection.LEFT:
                x = screenBorder.bottomLeft.x - additionalWidth;
                break;
            case ScreenDirection.RIGHT:
                x = screenBorder.bottomRight.x + additionalWidth;
                break;
            case ScreenDirection.TOP_LEFT:
                x = screenBorder.bottomLeft.x - additionalWidth;
                y = screenBorder.topLeft.y + additionalHeight;
                break;
            case ScreenDirection.TOP_RIGHT:
                x = screenBorder.bottomRight.x + additionalWidth;
                y = screenBorder.topRight.y + additionalHeight;
                break;
            case ScreenDirection.BOTTOM_LEFT:
                x = screenBorder.bottomLeft.x - additionalWidth;
                y = screenBorder.bottomLeft.y - additionalHeight;
                break;
            case ScreenDirection.BOTTOM_RIGHT:
                x = screenBorder.bottomRight.x + additionalWidth;
                y = screenBorder.bottomRight.y - additionalHeight;
                break;
        }
        return new Vector2(x, y);
    }

    private Vector2 OutOfScreenDirectionCircular(ScreenDirection direction)
    {
        screenBorder.CalculateScreenBorders();
        float x = screenBorder.bottomLeft.x + screenBorder.width * 0.5f;
        float y = screenBorder.bottomLeft.y + screenBorder.height * 0.5f;

        Vector2 middle = (screenBorder.bottomLeft + screenBorder.topRight) / 2;
        float radius = Vector2.Distance(middle, screenBorder.topLeft);
        radius += radius * PercentageOffscreen;

        float directionAngle = Vector2.SignedAngle(Vector2.right, new Vector2(1, 1) - middle);
        float angle = 90;

        switch (direction)
        {
            case ScreenDirection.TOP:
                angle = Random.Range(-directionAngle + 90, directionAngle + 90);
                y = (middle + (Vector2.up - middle).normalized * radius).y;
                break;
            case ScreenDirection.BOTTOM:
                angle = Random.Range(-directionAngle - 90, directionAngle - 90);
                y = (middle + Vector2.down * radius).y;
                break;
            case ScreenDirection.LEFT:
                angle = Random.Range(-directionAngle - 180, directionAngle - 180);
                x = (middle + Vector2.left * radius).x;
                break;
            case ScreenDirection.RIGHT:
                angle = Random.Range(-directionAngle, directionAngle);
                x = (middle + Vector2.right * radius).x;
                break;
            case ScreenDirection.TOP_LEFT:
                Vector2 topLeft = middle + (new Vector2(-1, 1) - middle).normalized * radius;
                x = topLeft.x;
                y = topLeft.y;
                break;
            case ScreenDirection.TOP_RIGHT:
                Vector2 topRight = middle + (new Vector2(1, 1) - middle).normalized * radius;
                x = topRight.x;
                y = topRight.y;
                break;
            case ScreenDirection.BOTTOM_LEFT:
                Vector2 bottomLeft = middle + (new Vector2(-1, -1) - middle).normalized * radius;
                x = bottomLeft.x;
                y = bottomLeft.y;
                break;
            case ScreenDirection.BOTTOM_RIGHT:
                Vector2 bottomRight = middle + (new Vector2(1, -1) - middle).normalized * radius;
                x = bottomRight.x;
                y = bottomRight.y;
                break;
        }

        if (!SinglePoint)
        {
            float angleInRadians = angle * Mathf.Deg2Rad;
            Vector2 onCircle = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)) * radius;
            x = onCircle.x;
            y = onCircle.y;
        }
        return new Vector2(x, y);
    }

}
