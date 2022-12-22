using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Transform enemiesPool;
    private int totalEnemies;

    private void Start()
    {
        enemiesPool = GameObject.FindGameObjectWithTag("Enemies Pool").transform;

        if (enemiesPool != null)
        {
            Enemy[] enemies = enemiesPool.GetComponentsInChildren<Enemy>();

            totalEnemies = enemies.Length;

            foreach (Enemy enemy in enemies)
            {
                enemy.OnDie += OnEnemyDie;
            }
        }
    }

    private void OnEnemyDie(Enemy enemy)
    {
        enemy.OnDie -= OnEnemyDie;

        GameManager.Instance.UpdateDiseaseLevel(-enemy.DiseaseLevel);

        totalEnemies--;

        GameManager.Instance.OnTotalEnemiesChanged(totalEnemies);
    }
}
