using System.Collections;
using UnityEngine;

/// <summary>
/// Class to play test out the RandomPosition class.
/// </summary>
public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private GameObject targetPrefab;

    [SerializeField] private bool singlePoint;
    [SerializeField] private bool spawnCircular;

    [SerializeField] private bool onScreen = false;
    [SerializeField] private bool outOfScreen = false;
    [SerializeField] private bool outOfScreenLeftRight = false;

    [SerializeField] private bool arroundTarget = false;
    [SerializeField] private bool arroundTargetOnRadius = false;
    [SerializeField] private bool arroundTargetInDonut = false;
    
    private RandomPosition randomSP;
    private int spawnCount = 500;
    private float timeBetweenSpawns = 0.005f;

    private void Awake()
    {
        randomSP = new RandomPosition();
    }

    private IEnumerator Start()
    {
        GameObject target = Instantiate(targetPrefab);
        target.transform.position = new Vector2(0, 0);
        randomSP.SinglePoint = singlePoint;
        randomSP.Circular = spawnCircular;

        if(onScreen) yield return StartCoroutine(SpawnOnScreen());
        if(outOfScreen) yield return StartCoroutine(SpawnOutOfScreen());
        if(outOfScreenLeftRight) yield return StartCoroutine(SpawnOutOfScreen(ScreenDirection.LEFT, ScreenDirection.RIGHT));
        
        if(arroundTarget) yield return StartCoroutine(ArroundCircle(target, 5));
        if(arroundTargetOnRadius) yield return StartCoroutine(ArroundTargetWithRadius(target, 5));
        if(arroundTargetInDonut) yield return StartCoroutine(ArroundTargetWithSpace(target, 5, 2));
    }

    private IEnumerator SpawnOutOfScreen()
    {
        objectToSpawn.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(objectToSpawn, randomSP.OutOfScreen(), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private IEnumerator SpawnOutOfScreen(params ScreenDirection[] spawnDirections)
    {
        objectToSpawn.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(objectToSpawn, randomSP.OutOfScreenDirection(spawnDirections), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private IEnumerator SpawnOnScreen()
    {
        objectToSpawn.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(objectToSpawn, randomSP.OnScreen(), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private IEnumerator ArroundCircle(GameObject gameObject, float radius)
    {
        objectToSpawn.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(objectToSpawn, randomSP.AroundTargetInCircle(gameObject, radius), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private IEnumerator ArroundTargetWithSpace(GameObject gameObject, float radius, float space)
    {
        objectToSpawn.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(objectToSpawn, randomSP.AroundTargetInDonut(gameObject, space, radius), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private IEnumerator ArroundTargetWithRadius(GameObject gameObject, float radius)
    {
        objectToSpawn.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(objectToSpawn, randomSP.AroundTargetOnCircle(gameObject, radius), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

}
