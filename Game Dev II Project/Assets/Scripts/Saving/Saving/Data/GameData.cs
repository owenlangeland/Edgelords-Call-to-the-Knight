using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //public bool DoubleJumpItem;

    public SerializableDictionary<string, bool> DoubleJumpItem;

    public bool doubleJumpAcquired;

    public int numberOfJumps;
    public int currentHealth;

    public GameData()
    {
            //this.DoubleJumpItem = false;
            DoubleJumpItem = new SerializableDictionary<string, bool>();
            doubleJumpAcquired = false;
            numberOfJumps = 1;
            currentHealth = 100;
    }
}