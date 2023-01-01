using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Transform enemiesParent;
    private int totalEnemies;

    private void Start()
    {
        enemiesParent = GameObject.FindGameObjectWithTag("Enemies Parent").transform;

        if (enemiesParent != null)
        {
            Enemy[] enemies = enemiesParent.GetComponentsInChildren<Enemy>();

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
