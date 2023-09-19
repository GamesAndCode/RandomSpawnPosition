using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;

    [SerializeField] private bool spawnOnAxis;
    [SerializeField] private bool spawnOnCircle;
    [SerializeField] private bool spawnCircular;

    [SerializeField] private bool onScreen = false;
    [SerializeField] private bool outOfScreen = false;
    [SerializeField] private bool outOfScreenLeftRight = false;

    [SerializeField] private bool arroundTarget = false;
    [SerializeField] private bool arroundTargetOnRadius = false;
    [SerializeField] private bool arroundTargetInDonut = false;
    
    private ScreenPositionManager screenPositionManager;
    private int spawnCount = 1000;
    private float timeBetweenSpawns = 0.005f;

    private void Awake()
    {
        screenPositionManager = new ScreenPositionManager();
    }

    private IEnumerator Start()
    {
        GameObject gameObject = new GameObject("Target");
        gameObject.transform.position = new Vector2(0, 0);
        screenPositionManager.spawnOnAxis = spawnOnAxis;
        screenPositionManager.spawnOnCircle = spawnOnCircle;
        screenPositionManager.spawnCircular = spawnCircular;

        if(onScreen) yield return StartCoroutine(SpawnOnScreen());
        if(outOfScreen) yield return StartCoroutine(SpawnOutOfScreen());
        if(outOfScreenLeftRight) yield return StartCoroutine(SpawnOutOfScreen(SpawnDirection.LEFT, SpawnDirection.RIGHT));
        
        if(arroundTarget) yield return StartCoroutine(ArroundCircle(gameObject, 5));
        if(arroundTargetOnRadius) yield return StartCoroutine(ArroundTargetWithRadius(gameObject, 5));
        if(arroundTargetInDonut) yield return StartCoroutine(ArroundTargetWithSpace(gameObject, 5, 2));
    }

    private IEnumerator SpawnOutOfScreen()
    {
        objectToSpawn.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(objectToSpawn, screenPositionManager.GetRandomPositionOutOfScreen(), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private IEnumerator SpawnOutOfScreen(params SpawnDirection[] spawnDirections)
    {
        objectToSpawn.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(objectToSpawn, screenPositionManager.GetPositionOutOfScreen(spawnDirections), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private IEnumerator SpawnOnScreen()
    {
        objectToSpawn.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(objectToSpawn, screenPositionManager.GetRandomPositionOnScreen(), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private IEnumerator ArroundCircle(GameObject gameObject, float radius)
    {
        objectToSpawn.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(objectToSpawn, screenPositionManager.GetRandomPositionArroundTarget(gameObject, radius), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private IEnumerator ArroundTargetWithSpace(GameObject gameObject, float radius, float space)
    {
        objectToSpawn.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(objectToSpawn, screenPositionManager.GetRandomPositionInsideDonutArroundTarget(gameObject, radius, space), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private IEnumerator ArroundTargetWithRadius(GameObject gameObject, float radius)
    {
        objectToSpawn.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(objectToSpawn, screenPositionManager.GetRandomPositionOnCircleArroundTarget(gameObject, radius), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

}
