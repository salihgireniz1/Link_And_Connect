using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelButtonHandler : MonoBehaviour
{
    public void NextLevel()
    {
        LevelManager.Instance.LoadNextLevel();
    }
}
