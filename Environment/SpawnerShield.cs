using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerShield : MonoBehaviour
{
    public List<GameObject> EnemyKinds = new List<GameObject>();

    public float spawnerTime;
    float countTime;
    float newCount;
    Vector3 spawnerPos;

    void Update()
    {
        if (GameManager.Instance.playStats.characterDate.currentLevel >= 2)
            SpawnerEnemyCount();
    }

    public void SpawnerEnemyCount()
    {
        countTime += Time.deltaTime;
        spawnerPos = transform.position;
        spawnerPos.z = Random.Range(3f, 98f);
        spawnerPos.x = Random.Range(3f, 96f);

        if(countTime >= spawnerTime)
        {
            CreateEnemy();
            countTime = 0;
        }
    }

    public void CreateEnemy()
    {
        int index = Random.Range(0, EnemyKinds.Count);
        GameObject enemy =  Instantiate(EnemyKinds[index], spawnerPos, Quaternion.identity);
        enemy.transform.SetParent(this.gameObject.transform);
    }
}
