using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public int[] itemTypes;
    private int randomIndex;
    private GameObject childItemOfSelf;

    private void Awake()
    {
        // Generate a random index to select a random item prefab
        randomIndex = UnityEngine.Random.Range(0, itemPrefabs.Length);

        // Instantiate the random item prefab
        childItemOfSelf = Instantiate(itemPrefabs[randomIndex], transform);

        Debug.Log("Spawned random item: " + childItemOfSelf.name);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Player")) { return; }

        PlayerManager playerManager = collider.gameObject.GetComponent<PlayerManager>();

        if (playerManager != null) {
            playerManager.SetEnergyEffect(itemTypes[randomIndex]);
        }
        Destroy(childItemOfSelf, 0f);
    }
}
