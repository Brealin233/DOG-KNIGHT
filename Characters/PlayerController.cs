using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;

    private CharacterStates characterStates;

    private Animator anim;

    private Rigidbody rb;

    private GameObject attackTarget;

    private float lastAttackTime;

    private bool isDead;

    private float stopDistance;
    public GameObject hitReport;
    public GameObject suckHitReport;
    public GameObject criReport;
    bool isShow = false;

    public float sectorDistance;

    [Header("ThrowWeapon")]
    public GameObject dartPerfab;
    public Transform handPos;
    public ItemUI itemUI;
    public SlotHolder itemSlot;

    [Header("ItemBuff")]
    public ItemDate_SO yellowMushroomBuff;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        characterStates = GetComponent<CharacterStates>();

        stopDistance = agent.stoppingDistance;
    }

    void OnEnable()
    {
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
        MouseManager.Instance.onTargetChange += MoveToTagets;
        GameManager.Instance.RigisterPlayer(characterStates);
    }

    void Start()
    {  
        SaveManager.Instance.LoadPlayerDate();
    }

    void OnDisable()
    {
        if (!MouseManager.IsInitialized) return;
        MouseManager.Instance.OnMouseClicked -= MoveToTarget;
        MouseManager.Instance.OnEnemyClicked -= EventAttack;
        MouseManager.Instance.onTargetChange -= MoveToTagets;
    }

    void Update()
    {
        isDead = characterStates.CurrentHealth <= 0;

        //todo
        if (isDead)
            GameManager.Instance.NotifyObserver();

        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.S))
        {
            agent.isStopped = true;
        }

        if (Input.GetMouseButtonDown(0) && characterStates.xiaolifeidao == true)
        {
            Instantiate(dartPerfab, handPos.position, Quaternion.identity);
            SoundManager.instance.XiaolifeidaoAudio();

            itemUI.Bag.items[itemUI.Index].amount -= 1;
            itemSlot.UpdateItem();
            if(itemUI.Bag.items[itemUI.Index].amount <= 0)
            {
                characterStates.xiaolifeidao = false;
            }
        } 

        foreach (var item in InventoryManager.Instance.inventoryUI.slotHolders)
        {
            if (item.isComplete == true)
            {
                characterStates.AppledeBuff(yellowMushroomBuff.useableDate.buffAttack, yellowMushroomBuff.useableDate.buffCriticalChance);
                item.isComplete = false;
            }
        }

        foreach (var item in InventoryManager.Instance.actionUI.slotHolders)
        {
            if (item.isComplete == true)
            {
                characterStates.AppledeBuff(yellowMushroomBuff.useableDate.buffAttack, yellowMushroomBuff.useableDate.buffCriticalChance);
                item.isComplete = false;
            }
        }

    }

    private void SwitchAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude); //最后一个是方法,转化为浮点数
        anim.SetBool("Death", isDead);
    }

    public void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        if (isDead) return;

        //agent.stoppingDistance = 1;
        agent.isStopped = false;

        agent.destination = target;
    }

    public void MoveToTagets(GameObject target,Vector3 place)
    {
        StopAllCoroutines();
        if (isDead) return;

        if (target.CompareTag("Enemy"))
        {
            agent.stoppingDistance = stopDistance;
        }
        else
            agent.stoppingDistance = 0.1f;

        agent.isStopped = false;
        agent.destination = place;
    }

    private void EventAttack(GameObject target)
    {
        if (isDead) return;
        if (target != null)
        {
            attackTarget = target;
            characterStates.isCritical = UnityEngine.Random.value < characterStates.attackDate.criticalChance;
            StartCoroutine(MoveToAttackTarget());
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        transform.LookAt(attackTarget.transform);
        //stopDistance = characterStates.attackDate.attackRange;

        while(Vector3.Distance(attackTarget.transform.position,transform.position) > characterStates.attackDate.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }
        agent.isStopped = true;

        if(lastAttackTime < 0)
        {
            anim.SetBool("Critical", characterStates.isCritical);
            anim.SetTrigger("Attack");

            lastAttackTime = characterStates.attackDate.coolDown;
        }
    }

    void Hit()
    {
        if (attackTarget.CompareTag("Attackable"))
        {
            if (attackTarget.GetComponent<Rock>() && attackTarget.GetComponent<Rock>().rockStates == Rock.RockStates.HitNothing)
            {
                stopDistance = characterStates.attackDate.attackRange;
                attackTarget.GetComponent<Rock>().rockStates = Rock.RockStates.HitEnemy;
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
            }
        }
        else
        {
            SoundManager.instance.FiteAudio();
            var targetStats = attackTarget.GetComponent<CharacterStates>();
            targetStats.TakeDamage(characterStates, targetStats);

            stopDistance = characterStates.attackDate.attackRange;

            isShow = true;
            DamageShow();
        }
    }

    void sectorHit()
    {
        Collider[] enemy_collider = Physics.OverlapBox(transform.position + Vector3.forward, transform.localScale * 2, Quaternion.identity); ;

        foreach (var item in enemy_collider)
        {
            if (item.CompareTag("Enemy"))
            {
                SoundManager.instance.DakandaoAudio();
                var targetStats = item.GetComponent<CharacterStates>();
                targetStats.TakeDamage(characterStates, targetStats);

                stopDistance = characterStates.attackDate.attackRange;

                isShow = true;
                DamageShow();
            }
        }
    }

    void SuckBloodHit()
    {
        SoundManager.instance.KexuezhanfuAudio();
        SuckDamageShow();
        isShow = true;
        DamageShow();
    }

    public void DamageShow()
    {
        if (isShow == true)
        {
            if (characterStates.isCritical == true)
            {
                GameObject critical = Instantiate(criReport, (attackTarget.transform.position + new Vector3(0.0f, 1.6f, 0.0f)), Quaternion.identity);//在被攻击的英雄头上加1.6m的位置生成此预制体。
                critical.gameObject.SetActive(true);
                Hit cirHit = critical.GetComponent<Hit>();
                cirHit.transform.forward = cirHit.cam.forward;
                cirHit.text = "- " + characterStates.damageShow.ToString(); //设置显示信息，伤害值取绝对值整数
            }
            else
            {
                GameObject tm = Instantiate(hitReport, (attackTarget.transform.position + new Vector3(0.0f, 1.6f, 0.0f)), Quaternion.identity);
                tm.gameObject.SetActive(true);//显示预制体
                Hit tmHit = tm.GetComponent<Hit>();//获取预制体父物体的脚本
                tm.transform.forward = tmHit.cam.forward;
                tmHit.text = "- " + characterStates.damageShow.ToString();
            }
        }
        isShow = false;
    }

    public void SuckDamageShow()
    {
        var targetStats = attackTarget.GetComponent<CharacterStates>();
        targetStats.TakeDamage(characterStates, targetStats);

        stopDistance = characterStates.attackDate.attackRange;
        GameObject tm = Instantiate(suckHitReport, (transform.position + new Vector3(0.0f, 1.6f, 0.0f)), Quaternion.identity);
        tm.gameObject.SetActive(true);
        Hit tmHit = tm.GetComponent<Hit>();
        tm.transform.forward = tmHit.cam.forward;
        tmHit.text = "+ " + (characterStates.damageShow * 0.3).ToString();
        int a = Convert.ToInt32(characterStates.damageShow * 0.3);
        characterStates.CurrentHealth += a;

        isShow = false;
    }

    public void SoundRun()
    {
        SoundManager.instance.RunAudio();
    }
}
