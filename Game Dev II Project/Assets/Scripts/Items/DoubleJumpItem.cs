using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpItem : MonoBehaviour, IDataPersistence
{
    public bool item = false;
    [SerializeField] private string id;
    public GameObject visual; // You need to assign this in the Unity editor

    // Method to generate a unique ID for the item
    [ContextMenu("Generate GUID for ID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    // Ensure you use Start() instead of start()
    private void Start()
    {
        if (item)
        {
            Destroy(gameObject);
        }
    }

    // Load data for this item
    public void loadData(GameData data)
    {
        // Check if the item exists in saved data, if yes, set its state
        if (data.DoubleJumpItem.TryGetValue(id, out item) && item)
        {
            visual.SetActive(false);
        }
    }

    // Save data for this item
    public void SaveData(ref GameData data)
    {
        // If the item already exists in the saved data, remove it first
        if (data.DoubleJumpItem.ContainsKey(id))
        {
            data.DoubleJumpItem.Remove(id);
        }
        // Add or update the item's state in the saved data
        data.DoubleJumpItem[id] = item;
    }

    // Method called when a collider enters the trigger area of this object
    private void OnTriggerEnter2D(Collider2D collider)
    {
        item = true; // Set item state to true when triggered
        visual.SetActive(false);
    }
}
