using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Werewolf : EnemyController
{
    void KickOff()
    {
        var targetStats = attackTarget.GetComponent<CharacterStates>();
        targetStats.TakeDamage(characterStats, targetStats);

        GameObject tm = Instantiate(hitReport, (transform.position + new Vector3(0.0f, 1.6f, 0.0f)), Quaternion.identity);//在被攻击的英雄头上加1.6m的位置生成此预制体。
        tm.gameObject.SetActive(true);
        Hit tmHit = tm.GetComponent<Hit>();
        tm.transform.forward = tmHit.cam.forward;
        tmHit.text = characterStats.damageShow.ToString();
    }
}
