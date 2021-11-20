using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    float timerToSpawn = 0;

    public float minTimeToSpawn = 5f, maxTimeToSpawn = 10f;

    public GameObject[] spawnPoints;
    public GameObject[] Enemies;

    public int maxCount = 10;
    public int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        timerToSpawn = Random.Range((int)minTimeToSpawn, (int)maxTimeToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameIsEnded)
            return;

        timerToSpawn -= Time.deltaTime;

        if(timerToSpawn < 0 && counter < maxCount)
        {
            int point = Random.Range(0, spawnPoints.Length);

            IEnumerator enumerator = Spawner(point);
            StartCoroutine(enumerator);

            timerToSpawn = Random.Range((int)minTimeToSpawn, (int)maxTimeToSpawn);
            counter++;
        }
    }

    IEnumerator Spawner(int spawnIndex)
    {
        spawnPoints[spawnIndex].GetComponent<Animator>().SetTrigger("Big");
        yield return new WaitForSeconds(1f);
        

        var enemy = Instantiate(Enemies[ Random.Range(0,Enemies.Length) ]);
        var lol = spawnPoints[spawnIndex].transform.position;
        enemy.transform.position = new Vector3( lol.x,0,lol.z);
        //enemy.transform.LookAt( GameObject.FindGameObjectWithTag("Player").transform);

        spawnPoints[spawnIndex].GetComponent<Animator>().SetTrigger("Small");
        yield return new WaitForSeconds(1f);
        spawnPoints[spawnIndex].GetComponent<Animator>().ResetTrigger("Big");
        spawnPoints[spawnIndex].GetComponent<Animator>().ResetTrigger("Small");

    }

}
