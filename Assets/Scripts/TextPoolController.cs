using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TextPoolController : MonoBehaviour
{
    [Tooltip("Text Prefab")]
    [SerializeField]private TextAnimator textAnimator;

    [Tooltip("range to spawn objects")]
    [SerializeField] private float spawnRange = 3.0f;

    [Tooltip("max object float distance")]
    [SerializeField] private float maxFloatDist = 1.5f;

    private IObjectPool<TextAnimator> objectPool;

    [SerializeField]
    private bool collectionCheck = true;

    [SerializeField]
    private int defaultCapacity = 20;

    [SerializeField]
    private int maxSize = 30;

    [SerializeField]
    private float floatDelay = 0.5f;

    [SerializeField]
    private bool spawning = false;

    [SerializeField]
    private WaitForSeconds delay;

    [SerializeField]
    private TextLineStorageObject linesContainer;

    private void Awake()
    {
        objectPool = new ObjectPool<TextAnimator>(CreateNewText, OnGetFromPool, OnReleaseToPool, OnDestroyObject,collectionCheck,defaultCapacity,maxSize);

        Debug.Log(objectPool);

        delay = new WaitForSeconds(floatDelay);
    }

    private void Start()
    {
        GameEventSystem.instance.onBeginScoliosisMode += EnableSpawn;
        GameEventSystem.instance.onEndScoliosisMode += DisableSpawn;
    }

    private TextAnimator CreateNewText()
    {
        TextAnimator textAnimatorInstance = Instantiate(textAnimator);
        textAnimatorInstance.ObjectPool = objectPool;
        return textAnimatorInstance;    
    }

    private void OnGetFromPool(TextAnimator pooledTextInstance)
    {
        pooledTextInstance.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(TextAnimator pooledTextInstance)
    {
        pooledTextInstance.gameObject.SetActive(false);
    }

    private void OnDestroyObject(TextAnimator pooledTextInstance)
    {
        Destroy(pooledTextInstance.gameObject);
    }

    public IEnumerator BeginSpawning()
    {

        while (spawning)
        {
            TextAnimator animatorInstance = objectPool.Get();

            if (animatorInstance == null)
            {
                Debug.Log("Nothing Found In Pool");
                spawning = false;
            }
            animatorInstance.TextComponent.text = linesContainer.lines[Random.Range(0, linesContainer.lines.Count)];

            //animatorInstance.TextComponent.fontSize = Random.Range(48, 72);

            float xRandom = Random.Range(-spawnRange * 5, spawnRange * 5);

            float yRandom = Random.Range(-spawnRange, spawnRange);

            float zRandom = Random.Range(-spawnRange * 5, spawnRange * 5);

            Vector3 positionToSpawnObject = new(transform.position.x + xRandom, transform.position.y + yRandom, transform.position.z - zRandom);

            animatorInstance.transform.SetPositionAndRotation(positionToSpawnObject, transform.rotation);

            animatorInstance.startPosition = positionToSpawnObject;

            animatorInstance.moveDistance = Random.Range(0.5f, maxFloatDist);

            animatorInstance.transform.SetParent(transform);

            animatorInstance.BeginLerp();

            yield return delay;
        }

    }

    void EnableSpawn()
    {
        spawning = true;
        StartCoroutine(BeginSpawning());
    }
    void DisableSpawn()
    {
        spawning = false;
    }


}
