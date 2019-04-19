using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public List<GameObject> Enemies = new List<GameObject>();
    public GameObject EnemyPre;

    public Transform SpawnPoint;
    Transform[] spawnPoints;

    public int count;
    public int nowCount = 0;

    public static EnemyManager instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;

        for(int i = 0; i < SpawnPoint.childCount; i++)
        {
            GameObject newEnemy = Instantiate(EnemyPre, transform.position, Quaternion.identity);
            newEnemy.transform.SetParent(transform);
            Enemies.Add(newEnemy);
        }

        spawnPoints = new Transform[SpawnPoint.childCount];
        for (int i = 0; i < SpawnPoint.childCount; i++)
        {
            spawnPoints[i] = SpawnPoint.GetChild(i);
        }

        for (int i = 0; i < 3; i++)
        {
            EnemyAdd();
        }
    }

    public void EnemyAdd() //적 추가
    {
        //SpawnPoints중 하나의 위치에 소환
        Vector3 genPos = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        int i = 0;
        while (i < Enemies.Count)
        {
            if (!Enemies[i].activeSelf)
            {
                Enemies[i].transform.position = genPos;
                Enemies[i].GetComponent<EnemyAI>().index = count;
                Enemies[i].GetComponent<EnemyAI>().originPos = genPos;
                Enemies[i].GetComponent<EnemyAI>().ResetAI();
                Enemies[i].SetActive(true);
                break;
            }
            else if (i == Enemies.Count - 1)
            {
                GameObject newEnemy = Instantiate(EnemyPre, transform.position, Quaternion.identity);
                newEnemy.transform.SetParent(transform);
                Enemies.Add(newEnemy);
                Enemies[i].transform.position = genPos;
                Enemies[i].GetComponent<EnemyAI>().index = count;
                Enemies[i].GetComponent<EnemyAI>().originPos = genPos;
                Enemies[i].GetComponent<EnemyAI>().ResetAI();
                Enemies[i].SetActive(true);
                break;
            }
            i++;
        }
        nowCount++;
    }

    public void EnemyDie(GameObject obj)
    {
        //죽었을때는 EnemyDie(gameObject) 이런식으로 해주길 바람
        int i = 0;
        while (i < Enemies.Count)
        {
            if (Enemies[i] == obj)
            {
                Enemies[i].SetActive(false);
                Enemies[i].transform.position = transform.position;
                break;
            }
            i++;
        }

        nowCount--;

        if (nowCount < count)
        {
            Invoke("EnemyAdd",Random.Range(1,3));
        }
    }
}
