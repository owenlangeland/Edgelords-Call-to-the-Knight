using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnButton : MonoBehaviour
{
    public void Respawn()
    {
        SceneManager.LoadScene("StartingRoom");
    }
}
