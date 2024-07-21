using DefaultNamespace.Enemies;
using LL_Unity_Utils.Timers;
using UnityEngine;

namespace DefaultNamespace
{
    public class AOEProjectile : Projectile
    {

        [SerializeField] ParticleSystem explosionEffect;

        void Awake()
        {
            timer = new Timer(5);
            timer.StartTimer();
        }

        void FixedUpdate()
        {
            if (timer.CheckTimer()) Destroy(gameObject);
        }

        public void SetDestination(Vector3 _targetPosition)
        {
            targetPosition = _targetPosition;
        }


        protected override void OnTriggerEnter2D(Collider2D _other)
        {
            if ((targetPosition - transform.position).magnitude < 0.6)
            {
                DoDamage();
            }
        }

        protected override void DoDamage()
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            var enemysInRange = Physics2D.OverlapCircleAll(transform.position, 1.5f, enemy);
            foreach (var enemyCollider in enemysInRange)
            {
                enemyCollider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}