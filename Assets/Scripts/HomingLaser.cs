using UnityEngine;

public class HomingLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    float _distanceToClosestEnemy = Mathf.Infinity;

    private Collider2D[] _enemies;
    Transform _target;
    
   
    
    void Update()
    {
        _enemies = Physics2D.OverlapCircleAll(transform.position, 20f, LayerMask.GetMask("Enemy"));

        if (_enemies.Length == 0)
        {
            transform.Translate(Vector2.up * _speed * Time.deltaTime);
        }
        else
        {
            foreach (var enemy in _enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);

                if (distance < _distanceToClosestEnemy)
                {
                    _distanceToClosestEnemy = distance;
                    _target = enemy.transform;
                }
                
                transform.position = Vector2.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime );
            }
        }

        if (transform.position.y > 6f)
        {
            Destroy(this.gameObject);
        }
    }
}
