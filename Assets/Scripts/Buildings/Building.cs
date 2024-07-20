using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.Enemies;
using DefaultNamespace.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace.Buildings
{
    public class Building : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] float attackSpeed;
        [SerializeField] float attackDamage;
        [SerializeField] float attackRange;
        [SerializeField] LayerMask enemyLayer;
        [SerializeField] Projectile projectile;

        // [SerializeField] GameObject buildingWeapon;
        // [SerializeField] List<Sprite> bodies;
        // [SerializeField] List<Sprite> heads;

        SpriteRenderer spriteRenderer;
        SpriteRenderer weaponSpriteRenderer;

        public float AttackSpeed => attackSpeed;
        public float AttackDamage => attackDamage;
        public float AttackRange => attackRange;
        public int UpgradeLevel = 1;
        public int MaxUpgradeLevel = 3;


        public ETargetOption targetOption;

        readonly List<Enemy> enemiesInRange = new();

        bool isShooting;
        Enemy target;

        void Awake()
        {
            StartCoroutine(Shoot());
            // spriteRenderer = GetComponent<SpriteRenderer>();
            // weaponSpriteRenderer = buildingWeapon.GetComponent<SpriteRenderer>();
            // spriteRenderer.sprite = bodies[UpgradeLevel - 1];
            // weaponSpriteRenderer.sprite = heads[UpgradeLevel - 1];
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
                while (isShooting)
                {
                    var position = target.transform.position;
                    Vector3 direction = target.transform.position - transform.position;
                    transform.up = direction;
                    // buildingWeapon.transform.up = direction;
                    var spawnedProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
                    spawnedProjectile.transform.LookAt(position);
                    spawnedProjectile.GetComponent<Rigidbody2D>().velocity = transform.up * 2f;
                    spawnedProjectile.SetDamage(attackDamage);
                    spawnedProjectile.SetDestination(target);
                    spawnedProjectile.transform.up = direction;
                    yield return new WaitForSeconds(1 / attackSpeed);
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
                BuildingManager.Instance.selectedBuilding = this;
                UIManager.Instance.ShowBuildingUI();
            }
        }

        public void Upgrade()
        {
            UpgradeLevel++;
            attackDamage += 5;
            attackSpeed += 0.5f;
            attackRange += 0.5f;
            // spriteRenderer.sprite = bodies[UpgradeLevel - 1];
            // weaponSpriteRenderer.sprite = heads[UpgradeLevel - 1];
        }
    }
}