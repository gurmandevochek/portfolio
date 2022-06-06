using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedPlayer : MonoBehaviour
{
    public static int savedPlayer;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
