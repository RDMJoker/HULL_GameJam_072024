using System;
using System.Collections.Generic;
using DefaultNamespace.Enemies;
using LL_Unity_Utils.Timers;
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class Projectile : MonoBehaviour
    {
        float damage;
        BoxCollider2D myCollider;
        LayerMask enemy;
        Enemy target;
        Timer timer;
        void Awake()
        {
            timer = new Timer(5);
            timer.StartTimer();
        }

        void FixedUpdate()
        {
            if(timer.CheckTimer()) Destroy(gameObject);
        }

        public void SetDamage(float _value)
        {
            damage = _value;
        }

        public void SetDestination( Enemy _target)
        {
            target = _target;
        }


        void OnTriggerEnter2D(Collider2D _other)
        {
            if( target == null ||target.gameObject == null) return;
            if (_other == null || target == null ||_other.gameObject != target.gameObject) return;
            DoDamage();
        }

        void DoDamage()
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}