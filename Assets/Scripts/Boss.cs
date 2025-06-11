using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject bosslaserPrefab;
    [SerializeField] private float fireRate = 2.0f;
    private float _canFire = -1f;

    void Update()
    {
        if (Time.time > _canFire)
        {
            _canFire = Time.time + fireRate;
            Instantiate(bosslaserPrefab, transform.position, Quaternion.identity);
        }
    }

}