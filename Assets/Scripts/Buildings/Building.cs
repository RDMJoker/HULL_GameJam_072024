using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Enemies;
using DefaultNamespace.UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace.Buildings
{
    public class Building : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] float attackSpeed;
        [SerializeField] float attackDamage;
        [SerializeField] float attackRange;
        [SerializeField] LayerMask enemyLayer;
        [SerializeField] Projectile projectile;
        [SerializeField] float projectileSpeed;

        [SerializeField] GameObject buildingWeapon;
        [SerializeField] List<Sprite> bodies;
        [SerializeField] List<Sprite> heads;

        [SerializeField] bool useBurstFire;
        [SerializeField] int amountOfBursts;

        [SerializeField] Image highlighter;

        [SerializeField] AudioClip shootSound;
        AudioSource source;

        SpriteRenderer spriteRenderer;
        SpriteRenderer weaponSpriteRenderer;

        public float AttackSpeed => attackSpeed;
        public float AttackDamage => attackDamage;
        public float AttackRange => attackRange;
        public int UpgradeLevel = 1;
        public int MaxUpgradeLevel = 3;
        public string Name;

        public int CurrencyCost;

        public ETargetOption targetOption;

        readonly List<Enemy> enemiesInRange = new();

        bool isShooting;
        Enemy target;

        void Awake()
        {
            StartCoroutine(Shoot());
            spriteRenderer = GetComponent<SpriteRenderer>();
            weaponSpriteRenderer = buildingWeapon.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = bodies[UpgradeLevel - 1];
            weaponSpriteRenderer.sprite = heads[UpgradeLevel - 1];
            source = GetComponent<AudioSource>();
            source.clip = shootSound;
        }

        void FixedUpdate()
        {
            enemiesInRange.Clear();
            var overlapArray = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
            foreach (var enemyCollider in overlapArray)
            {
                var enemy = enemyCollider.GetComponent<Enemy>();
                enemiesInRange.Add(enemy);
            }

            if (enemiesInRange.Count == 0)
            {
                isShooting = false;
                return;
            }

            enemiesInRange.Sort(((_a, _b) => _a.TilesPassed.CompareTo(_b.TilesPassed)));
            switch (targetOption)
            {
                case ETargetOption.First:
                    target = enemiesInRange.Last();
                    break;
                case ETargetOption.Closest:
                    enemiesInRange.Sort(((_a, _b) => Vector3.Distance(_a.transform.position, transform.position).CompareTo(Vector3.Distance(_b.transform.position, transform.position))));
                    target = enemiesInRange.First();
                    break;
                case ETargetOption.Last:
                    target = enemiesInRange.First();
                    break;
                case ETargetOption.Strongest:
                    enemiesInRange.Sort(((_a, _b) => _a.MaxHP.CompareTo(_b.MaxHP)));
                    target = enemiesInRange.Last();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            isShooting = true;
        }

        IEnumerator Shoot()
        {
            while (true)
            {
                while (isShooting && target != null)
                {
                    if (useBurstFire)
                    {
                        source.clip = shootSound;
                        source.Play();
                        for (int i = 0; i < amountOfBursts; i++)
                        {
                            if (target == null) break;
                            var position = target.transform.position;
                            var direction = target.transform.position - transform.position;
                            // transform.up = direction;
                            buildingWeapon.transform.up = direction;
                            var spawnedProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
                            spawnedProjectile.GetComponent<Rigidbody2D>().velocity = buildingWeapon.transform.up * projectileSpeed;
                            spawnedProjectile.SetDamage(attackDamage);
                            if (spawnedProjectile is AOEProjectile aoeProjectile)
                            {
                                aoeProjectile.SetDestination(target.transform.position, source);
                            }
                            else
                            {
                                spawnedProjectile.SetDestination(target);
                            }

                            spawnedProjectile.transform.up = direction;
                            yield return new WaitForSeconds(0.075f);
                        }

                        yield return new WaitForSeconds(1 / attackSpeed);
                    }
                    else
                    {
                        source.clip = shootSound;
                        source.Play();
                        var position = target.transform.position;
                        var direction = target.transform.position - transform.position;
                        // transform.up = direction;
                        buildingWeapon.transform.up = direction;
                        var spawnedProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
                        spawnedProjectile.GetComponent<Rigidbody2D>().velocity = buildingWeapon.transform.up * projectileSpeed;
                        spawnedProjectile.SetDamage(attackDamage);
                        if (spawnedProjectile is AOEProjectile aoeProjectile)
                        {
                            aoeProjectile.SetDestination(target.transform.position, source);
                        }
                        else
                        {
                            spawnedProjectile.SetDestination(target);
                        }

                        spawnedProjectile.transform.up = direction;
                        yield return new WaitForSeconds(1 / attackSpeed);
                    }
                }

                yield return new WaitForFixedUpdate();
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, attackRange);
        }

        public void OnPointerClick(PointerEventData _eventData)
        {
            if (_eventData.button == PointerEventData.InputButton.Left)
            {
                if (BuildingManager.Instance.selectedBuilding != null)
                {
                    BuildingManager.Instance.selectedBuilding.SetHighlighter(false);
                    BuildingManager.Instance.selectedBuilding = null;
                }
                BuildingManager.Instance.selectedBuilding = this;
                UIManager.Instance.UpdateBuildingUI();
                UIManager.Instance.ShowBuildingUI();
                SetHighlighter(true);
            }
        }

        public void Upgrade()
        {
            UpgradeLevel++;
            attackDamage += attackDamage * 0.25f;
            attackSpeed += attackSpeed * 0.25f;
            attackRange += attackRange * 0.25f;
        }

        public void SetHighlighter(bool _isOn)
        {
            highlighter.color = _isOn switch
            {
                true => new Color(highlighter.color.r, highlighter.color.g, highlighter.color.b, 0.30f),
                false => new Color(highlighter.color.r, highlighter.color.g, highlighter.color.b, 0),
            };
        }
    }
}