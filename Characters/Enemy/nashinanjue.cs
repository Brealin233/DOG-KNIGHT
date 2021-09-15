using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nashinanjue : EnemyController
{
    public bool isFire;
    public GameObject laser;
    public bool isEnd = false;

    private void Update()
    {
        if (FoundPlayer() == false)
        {
            transform.GetChild(3).GetComponent<Laser>().attackPlayer = null;
            laser.SetActive(false);
        }

        if (FoundPlayer() == true || attackTarget != null)
        {
            transform.GetChild(3).GetComponent<Laser>().attackPlayer = attackTarget;
            laser.SetActive(true);
        }

        
    }

    private void OnDisable()
    {
        if (characterStats.CurrentHealth <= 0)
            isEnd = true;
    }

    public void FireLaser()
    {
        if (attackTarget != null)
        {
            //var laser = Instantiate(laserPrefab);
            //laserPrefab.attackPlayer = attackTarget;
            isFire = true;

            attackTarget.GetComponent<CharacterStates>().TakeDamage(10, attackTarget.GetComponent<CharacterStates>());

            GameObject tm = Instantiate(hitReport, (attackTarget.transform.position + new Vector3(0.0f, 1.6f, 0.0f)), Quaternion.identity);
            tm.gameObject.SetActive(true);
            Hit tmHit = tm.GetComponent<Hit>();
            tm.transform.forward = tmHit.cam.forward;
            tmHit.text = "- " + GameManager.Instance.playStats.damageShow.ToString();
        }
       
    }

    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRedius);

        foreach (var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }

}
