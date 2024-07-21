using DefaultNamespace.Enemies;
using LL_Unity_Utils.Timers;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(AudioSource))]
    public class AOEProjectile : Projectile
    {

        [SerializeField] ParticleSystem explosionEffect;
        [SerializeField] AudioClip explosionSound;

        AudioSource source;
        AudioSource parentSource;

        void Awake()
        {
            timer = new Timer(5);
            timer.StartTimer();
            source = GetComponent<AudioSource>();
            source.clip = explosionSound;
        }

        void FixedUpdate()
        {
            if (timer.CheckTimer()) Destroy(gameObject);
        }

        public void SetDestination(Vector3 _targetPosition, AudioSource _parentSource)
        {
            targetPosition = _targetPosition;
            parentSource = _parentSource;
        }


        protected override void OnTriggerEnter2D(Collider2D _other)
        {
            if ((targetPosition - transform.position).magnitude < 1)
            {
                DoDamage();
            }
        }

        protected override void DoDamage()
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            parentSource.Stop();
            parentSource.clip = explosionSound;
            parentSource.Play();
            var enemysInRange = Physics2D.OverlapCircleAll(transform.position, 1f, enemy);
            foreach (var enemyCollider in enemysInRange)
            {
                if (enemyCollider == null || enemyCollider.gameObject == null) continue;
                enemyCollider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}