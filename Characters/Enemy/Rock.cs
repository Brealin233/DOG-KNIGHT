using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Rock : MonoBehaviour
{
    public enum RockStates { HitPlayer,HitEnemy,HitNothing}
    private Rigidbody rb;

    public RockStates rockStates;

    [Header("Basic Settings")]
    public float force;
    public GameObject target;
    public int damage;
    private Vector3 direction;

    public GameObject braekEffect;
    public GameObject hitReport;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one;

        rockStates = RockStates.HitPlayer;
        FlyToTarget();
    }

    void FixedUpdate()
    {
        if(rb.velocity.sqrMagnitude < 1f)
        {
            rockStates = RockStates.HitNothing;
        }
    }

    void FlyToTarget()
    {
        if (target == null)
            target = FindObjectOfType<PlayerController>().gameObject;

        direction = (target.transform.position - transform.position + Vector3.up).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other)
    {
        switch(rockStates)
        {
            case RockStates.HitPlayer:
                if(other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;
                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    other.gameObject.GetComponent<CharacterStates>().TakeDamage(damage, other.gameObject.GetComponent<CharacterStates>());

                    GameObject tm = Instantiate(hitReport, (transform.position + new Vector3(0.0f, 1.6f, 0.0f)), Quaternion.identity);//在被攻击的英雄头上加1.6m的位置生成此预制体。
                    tm.gameObject.SetActive(true);
                    Hit tmHit = tm.GetComponent<Hit>();
                    tm.transform.forward = tmHit.cam.forward;
                    tmHit.text = "- " + GameManager.Instance.playStats.damageShow.ToString();

                    rockStates = RockStates.HitNothing;
                }
                break;
            case RockStates.HitEnemy:
                if(other.gameObject.GetComponent<Golem>())
                {
                    var otherStats = other.gameObject.GetComponent<CharacterStates>();
                    otherStats.TakeDamage(damage, otherStats);
                    Instantiate(braekEffect, transform.position, Quaternion.identity);

                    GameObject tm = Instantiate(hitReport, (transform.position + new Vector3(0.0f, 1.6f, 0.0f)), Quaternion.identity);//在被攻击的英雄头上加1.6m的位置生成此预制体。
                    tm.gameObject.SetActive(true);
                    Hit tmHit = tm.GetComponent<Hit>();
                    tm.transform.forward = tmHit.cam.forward;
                    tmHit.text = "- " +GameManager.Instance.playStats.damageShow.ToString();

                    Destroy(gameObject);
                }
                break;
        }
    }
}
