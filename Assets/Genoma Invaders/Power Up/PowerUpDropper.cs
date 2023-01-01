using System;
using System.Linq;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class PowerUpDropper : MonoBehaviour
{
    [Serializable]
    private class PowerUpDrop
    {
        public GameObject powerUp;
        public int probability;
    }

    [SerializeField]
    private int dropRatio = 25;
    [SerializeField]
    private int dropAmmount = 1;
    [SerializeField]
    private List<PowerUpDrop> drops = new List<PowerUpDrop>();

    private Transform powerUpsParent;
    private int totalProbability;

    public bool TryDrop()
    {
        bool didDrop = false;

        for (int i = 0; i < dropAmmount; i++)
        {
            int roll = Random.Range(0, 100);

            Debug.Log($"Roll: {roll}");

            bool canDrop = dropRatio > roll;

            if (canDrop)
            {
                PowerUpDrop drop = GetRandomDrop();

                Instantiate(drop.powerUp, transform.position, Quaternion.identity, powerUpsParent);

                didDrop = true;
            }
        }

        return didDrop;
    }

    private void Awake()
    {
        totalProbability = drops.Sum(drop => drop.probability);
    }

    private void Start()
    {
        powerUpsParent = GameObject.FindGameObjectWithTag("Power Ups Parent").transform;
    }

    private PowerUpDrop GetRandomDrop()
    {
        int roll = Random.Range(0, totalProbability);

        foreach (PowerUpDrop drop in drops)
        {
            if (drop.probability > roll)
            {
                return drop;
            }
            else
            {
                roll -= drop.probability;
            }
        }

        throw new UnityException("GetRandomDrop Failed");
    }
}
