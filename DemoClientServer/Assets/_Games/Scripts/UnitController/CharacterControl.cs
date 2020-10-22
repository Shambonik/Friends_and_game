using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CreatorKitCode;
using CreatorKitCodeInternal;
using UnitSystem;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = System.Random;

namespace UnitController
{
    public class CharacterControl : MonoBehaviour
    {
        public static CharacterControl Instance { get; protected set; }
        
        public UnitData Data => m_CharacterData;
        public UnitData CurrentTarget => m_CurrentTargetCharacterData;
        
        public DynamicJoystick Joystic;
        public Transform WeaponLocator;
        
        public float Speed = 10.0f;

        public enum State
        {
            DEFAULT,
            HIT,
            ATTACKING
        }

        private GameObject m_WeaponAttackEffect;
        public State m_CurrentState;


        private NavMeshAgent m_Agent;

        private Vector3 m_PlayerSpawn;
        private bool pressedAttackButtom;
        bool m_IsKO = false;
        float m_KOTimer = 0.0f;
        public static CharacterData m_CharacterData;
        private UnitData m_CurrentTargetCharacterData = null;
        private UnitData m_LastTegetCharacterData = null;
        
        private int m_WalkParamID = Animator.StringToHash("isWalking");
        private int m_AttackParamID = Animator.StringToHash("Attack");
        private int m_HitParamID = Animator.StringToHash("Hit");
        private int m_FaintParamID = Animator.StringToHash("Death");
        private int m_RespawnParamID = Animator.StringToHash("Respawn");
        float turnSmoothVelocity, turnSmoothTime = 0.01f;

        private List<UnitData> enemieDatas = new List<UnitData>();
        private UIManager _uiManager;
        private Animator m_Animator;
        
        private void Awake()
        {
            Instance = this;
            
            m_CharacterData = GetComponent<CharacterData>();
            
            m_CharacterData.Equipment.OnEquiped += item =>
            {
                if (item.Slot == EquipmentItem.EquipmentSlot.Weapon)
                    Instantiate(item.WorldObjectPrefab, WeaponLocator);
            };
        
            m_CharacterData.Equipment.OnUnequip += item =>
            {
                if (item.Slot == EquipmentItem.EquipmentSlot.Weapon)
                {
                    foreach(Transform t in WeaponLocator)
                        Destroy(t.gameObject);
                }
            };

            AttackState._characterControl = this;
            
            m_CharacterData.Init();
        }
        
        private void Start()
        {
            m_Animator = GetComponentInChildren<Animator>();

            m_Agent = GetComponent<NavMeshAgent>();

            m_Agent.speed = Speed;

            m_PlayerSpawn = transform.position;
            _uiManager = UIManager.Instance;
            
            m_CurrentState = State.DEFAULT;
            
            m_CharacterData.OnDamage += () => { m_Animator.SetTrigger(m_HitParamID); };
            UIManager.Instance.CharacterPanel.Avatar.sprite = m_CharacterData.Avatar;
        }

        private void Update()
        {
            if (m_IsKO)
            {
                m_KOTimer += Time.deltaTime;
                if (m_KOTimer > 3.0f)
                {
                    GoToRespawn();
                }

                return;
            }

            if (m_CharacterData.Stats.CurrentHealth == 0)
            {
                m_Animator.SetTrigger(m_FaintParamID);

                m_IsKO = true;
                m_KOTimer = 0.0f;
            }

             float horizontal = Joystic.Horizontal;
            float vertical = Joystic.Vertical;
            Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

            if (direction.magnitude >= 0.1f && m_CurrentState != State.ATTACKING)
            {
                pressedAttackButtom = false;
                Move(direction);
                m_Animator.SetBool(m_WalkParamID, true);
            }
            else if (pressedAttackButtom && m_CurrentTargetCharacterData)
            {
                if (!m_CharacterData.TargetIsLive(m_CurrentTargetCharacterData))
                {
                    m_CharacterData.TakeExp(m_CurrentTargetCharacterData.Exp());
                    enemieDatas.Remove(m_CurrentTargetCharacterData);
                    m_CurrentTargetCharacterData = null;
                    _uiManager.EnemyPanel.Panel.SetActive(false);
                    pressedAttackButtom = false;
                }
                else 
                {
                    if (m_CharacterData.CanAttackReach(m_CurrentTargetCharacterData))
                    {
                        StopAgent();
                        
                        Vector3 forward = (m_CurrentTargetCharacterData.transform.position - transform.position);
                        forward.y = 0;
                        forward.Normalize();
                    
                        transform.forward = forward;
                        
                        if (m_CharacterData.CanAttackTarget(m_CurrentTargetCharacterData))
                        {
                            m_CurrentState = State.ATTACKING;
                            m_Animator.SetTrigger(m_AttackParamID);
                            m_CharacterData.Attack(m_CurrentTargetCharacterData);
                            m_CharacterData.AttackTriggered();
                            pressedAttackButtom = false;
                        }
                    }
                    else
                    { 
                        m_Animator.SetBool(m_WalkParamID, true);
                        m_Agent.SetDestination(m_CurrentTargetCharacterData.transform.position);
                    }
                }
            }
            else
                StopAgent();
        }

        void GoToRespawn()
        {
            m_Agent.Warp(m_PlayerSpawn);
            m_Agent.isStopped = true;
            m_Agent.ResetPath();
            m_IsKO = false;

            m_CurrentTargetCharacterData = null;

            m_CurrentState = State.DEFAULT;
            m_Animator.SetTrigger(m_RespawnParamID);
        
            m_CharacterData.Stats.ChangeHealth(m_CharacterData.Stats.stats.health);
        }
        
        private void Move(Vector3 direction)
        {
            Vector3 newPosition = transform.position + direction * (Time.deltaTime * Speed);
            NavMeshHit hit;
            
            bool isValid = NavMesh.SamplePosition(newPosition, out hit, .3f, NavMesh.AllAreas);
            
            if (isValid)
            {
                m_Agent.ResetPath();
                transform.position = hit.position;
                Rotate(direction);
            }
        }
        
        private void Rotate(Vector3 direction)
        {
            transform.localRotation = Quaternion.RotateTowards(from: transform.rotation,
                    to: Quaternion.LookRotation(direction), maxDegreesDelta: Time.deltaTime * 720);
        }

        private void StopAgent()
        {
            m_Agent.velocity = Vector3.zero;
            m_Animator.SetBool(m_WalkParamID, false);
        }
        
        public void CheckAttack()
        {
            if(m_CurrentState == State.ATTACKING)
                return;
            
            TryFoundEnemy();
            pressedAttackButtom = true;
            
        }

        private void TryFoundEnemy()
        {
            if (enemieDatas.Count == 0 ||
                m_CurrentTargetCharacterData && enemieDatas.Contains(m_CurrentTargetCharacterData))
            {
                if (m_CurrentTargetCharacterData)
                    SwitchTarget();
                return;
            }
            
            
            float minDistance = 10000;

            foreach (var enemieData in enemieDatas)
            {
                if (!enemieData) continue;
                
                float enemyDistance = Vector3.SqrMagnitude(enemieData.transform.position - transform.position);

                if (!(enemyDistance < minDistance)) continue;
                
                minDistance = enemyDistance;
                m_CurrentTargetCharacterData = enemieData;
                SwitchTarget();
            }
        }

        private void SwitchTarget()
        {
            _uiManager.EnemyPanel.Panel.SetActive(true);
            _uiManager.EnemyPanel.Avatar.sprite = m_CurrentTargetCharacterData.Avatar;
            _uiManager.EnemyPanel.Title.text = $"{m_CurrentTargetCharacterData.CharacterName}";
            _uiManager.EnemyPanel.HealthPoint.text = $"{m_CurrentTargetCharacterData.Stats.CurrentHealth}/{m_CurrentTargetCharacterData.Stats.stats.health}";
            _uiManager.EnemyPanel.HealthBar.fillAmount = (float) m_CurrentTargetCharacterData.Stats.CurrentHealth /
                                                         m_CurrentTargetCharacterData.Stats.stats.health;
            _uiManager.EnemyPanel.Lvl.text = $"Ур: {m_CurrentTargetCharacterData.UnitLvl}";
        }
        
        private void OnTriggerEnter(Collider other)
        {
            UnitData target = other.GetComponent<UnitData>();
            if (target)
            {
                enemieDatas.Add(target);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            UnitData target = other.GetComponent<UnitData>();
            if (target)
            {
                enemieDatas.Remove(target);
            }
        }
    }
}
