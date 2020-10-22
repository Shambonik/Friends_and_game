using System;
using CreatorKitCode;
using UnitController;
using UnitSystem;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

namespace CreatorKitCodeInternal {
    public class SimpleEnemyController : MonoBehaviour
    {
        public enum State
        {
            IDLE,
            PURSUING,
            ATTACKING,
            PATROL
        }

        public float Speed = 6.0f;
        public float detectionRadius = 10.0f;
        public float ChangePositionTimer;
        public AudioClip[] SpottedAudioClip;
        public GameObject DeathEffect;

        Vector3 m_StartingAnchor;
        Animator m_Animator;
        NavMeshAgent m_Agent;
        UnitData m_CharacterData;

        private EnemySpawner _spawner;

        private UIManager _uiManager;
        //CharacterAudio m_CharacterAudio;
        int m_WalkParamID = Animator.StringToHash("Speed");
        int m_AttackAnimHash = Animator.StringToHash("Attack");
        int m_DeathAnimHash = Animator.StringToHash("Death");
        int m_HitAnimHash = Animator.StringToHash("Hit");

        bool m_Pursuing;
        float m_PursuitTimer = 0.0f;
        private GameObject enemyPanel;
        public State m_State;

        //LootSpawner m_LootSpawner;
    
        // Start is called before the first frame update
        void Start()
        {
            ChangePositionTimer = Random.Range(3, 7);
            _spawner = GetComponentInParent<EnemySpawner>();
            m_Animator = GetComponentInChildren<Animator>();
            m_Agent = GetComponent<NavMeshAgent>();

            m_CharacterData = GetComponent<UnitData>();
            m_CharacterData.Init();
            
            //m_CharacterAudio = GetComponentInChildren<CharacterAudio>();
        
            m_CharacterData.OnDamage += () =>
            {
                m_Animator.SetTrigger(m_HitAnimHash);
                //m_CharacterAudio.Hit(transform.position);
            };
            _uiManager = UIManager.Instance;

            m_Agent.speed = Speed;

            m_CharacterData.UnitLvl = Random.Range(1, 4);
            //m_LootSpawner = GetComponent<LootSpawner>();
        
            m_StartingAnchor = transform.position;
            m_CharacterData.hpBar = _uiManager.EnemyPanel.HealthBar;
            enemyPanel = _uiManager.EnemyPanel.Panel;
            

        }

        private bool block;
        // Update is called once per frame
        void Update()
        {
            if (block)
                return;
            //See the Update function of CharacterControl.cs for a comment on how we could replace
            //this (polling health) to a callback method.
            if (m_CharacterData.Stats.CurrentHealth == 0)
            {
                block = true;
                m_Animator.SetTrigger(m_DeathAnimHash);
                _spawner.CurrentEnemyCount--;
                m_Agent.isStopped = true;
                //m_CharacterAudio.Death(transform.position);
                Destroy(GetComponent<SphereCollider>());
                Destroy(m_Agent);
                Destroy(gameObject, 3);
                return;
            }
        
            //NOTE : in a full game, this would use a targetting system that would give the closest target
            //of the opposing team (e.g. multiplayer or player owned pets). Here for simplicity we just reference
            //directly the player.
            Vector3 playerPosition = CharacterControl.Instance.transform.position;
            UnitData playerData = CharacterControl.Instance.Data;
        
            switch (m_State)
            {
                case State.IDLE:
                {
                    ChangePositionTimer -= Time.deltaTime;
                    if (ChangePositionTimer <= 0)
                    {
                        ChangePositionTimer = Random.Range(3, 7);
                        m_Agent.isStopped = false;
                        m_Agent.SetDestination(_spawner.NextPosition());
                    }
                    
                    if (Vector3.SqrMagnitude(playerPosition - transform.position) < detectionRadius * detectionRadius)
                    {
                        /*if (SpottedAudioClip.Length != 0)
                        {
                            SFXManager.PlaySound(SFXManager.Use.Enemies, new SFXManager.PlayData()
                            {
                                Clip = SpottedAudioClip[Random.Range(0, SpottedAudioClip.Length)],
                                Position = transform.position
                            });
                        }*/

                        m_PursuitTimer = 2.0f;
                        m_State = State.PURSUING;
                        m_Agent.isStopped = false;
                    }
                }
                    break;
                case State.PURSUING:
                {
                    float distToPlayer = Vector3.SqrMagnitude(playerPosition - transform.position);
                    if (distToPlayer <= detectionRadius * detectionRadius)
                    {
                        m_PursuitTimer = 2.0f;

                        if (m_CharacterData.CanAttackTarget(playerData))
                        {
                            m_Agent.ResetPath();
                            m_Agent.velocity = Vector3.zero;

                            m_CharacterData.AttackTriggered();
                            m_CharacterData.Attack(playerData);
                            m_Animator.SetTrigger(m_AttackAnimHash);
                            m_State = State.ATTACKING;
                            m_Agent.isStopped = true;
                        }
                    }
                    else
                    {
                        if (m_PursuitTimer > 0.0f)
                        {
                            m_PursuitTimer -= Time.deltaTime;

                            if (m_PursuitTimer <= 0.0f)
                            {
                                m_Agent.SetDestination(m_StartingAnchor);
                                m_State = State.IDLE;
                                enemyPanel.SetActive(false);
                            }
                        }
                    }
                
                    if (m_PursuitTimer > 0)
                    {
                        m_Agent.SetDestination(playerPosition);
                    }
                }
                    break;
                case State.ATTACKING:
                {

                    if (!m_CharacterData.CanAttackReach(playerData))
                    {
                        m_State = State.PURSUING;
                        m_Agent.isStopped = false;

                    }
                    else
                    {
                        if (m_CharacterData.CanAttackTarget(playerData) && m_CharacterData.TargetIsLive(playerData))
                        {
                            m_CharacterData.Attack(playerData);
                            m_CharacterData.AttackTriggered();
                            m_Agent.ResetPath();
                            m_Animator.SetTrigger(m_AttackAnimHash);

                        }
                    }
                }
                    break;
            }
            
            m_Animator.SetFloat(m_WalkParamID, m_Agent.velocity.magnitude/Speed);
        
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}