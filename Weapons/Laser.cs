using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Laser : MonoBehaviour
{
    public LineRenderer lr;
    public int vertextCount;

    public Transform firePos;
    public GameObject attackPlayer;

    public GameObject hitReport;

    public float dur = 3;
    public float timer;

    public nashinanjue nashinanjue;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = vertextCount;
        
    }

    private void Update()
    {
        timer += Time.deltaTime;
        StartCoroutine(Lighting());
    }
  

    IEnumerator Lighting()
    {
        while(timer < 3)
        {
            float distance = Vector3.Distance(firePos.position, attackPlayer.transform.position);
            float diestanceDir = distance / lr.positionCount;
            Vector3 dir = (attackPlayer.transform.position - firePos.position).normalized;

            for (int i = 0; i < lr.positionCount; i++)
            {
                Vector3 vectorPos = firePos.position + dir * i * diestanceDir;
                Vector3 noise = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
                vectorPos += noise;
                lr.SetPosition(i, vectorPos);
                //Debug.Log(diestanceDir);
            }
            yield return new WaitForSeconds(dur);
            timer = 0;
        }
       
    }
}
