using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject onPoint;
    [SerializeField] private GameObject onAxis;
    [SerializeField] private GameObject onScreen;

    private ScreenPositionManager screenPositionManager;
    private int spawnCount = 1000;
    private float timeBetweenSpawns = 0.005f;

    private void Awake()
    {
        screenPositionManager = new ScreenPositionManager();
    }

    private IEnumerator Start()
    {
        GameObject gameObject = new GameObject();
        gameObject.transform.position = new Vector2(0, 0);
        //yield return StartCoroutine(ArroundTargetWithSpace(gameObject, 5, 2));
        //yield return StartCoroutine(SpawnOutOfScreen());
        screenPositionManager.spawnOnAxis = true;
        //yield return StartCoroutine(SpawnOutOfScreen());
        //yield return StartCoroutine(SpawnOutOfScreen(SpawnDirection.LEFT, SpawnDirection.RIGHT, SpawnDirection.BOTTOM));
        //yield return StartCoroutine(ArroundTargetWithRadius(gameObject, 5));
        yield return StartCoroutine(CircleScreen());

    }

    private IEnumerator SpawnOutOfScreen()
    {
        onPoint.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(onPoint, screenPositionManager.GetRandomPositionOutOfScreen(), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private IEnumerator SpawnOutOfScreen(params SpawnDirection[] spawnDirections)
    {
        onPoint.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(onPoint, screenPositionManager.GetPositionOutOfScreen(spawnDirections), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private IEnumerator SpawnOnScreen()
    {
        onPoint.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(onPoint, screenPositionManager.GetRandomPositionOnScreen(), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private IEnumerator ArroundCircle(GameObject gameObject, float radius)
    {
        onPoint.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(onPoint, screenPositionManager.GetRandomPositionArroundTarget(gameObject, radius), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
    private IEnumerator ArroundTargetWithSpace(GameObject gameObject, float radius, float space)
    {
        onPoint.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(onPoint, screenPositionManager.ArroundTargetWithSpace(gameObject, radius, space), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private IEnumerator ArroundTargetWithRadius(GameObject gameObject, float radius)
    {
        onPoint.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(onPoint, screenPositionManager.GetRandomPositionOnCircleArroundTarget(gameObject, radius), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private IEnumerator CircleScreen()
    {
        onPoint.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        GameObject gameObject = new GameObject();
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(onPoint, screenPositionManager.GetPostionOnCircleOutOfScreen(gameObject), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

}
