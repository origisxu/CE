using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITaskHandler 
{
    void OnPlayerMessage(string message);
    bool IsGameOver();
}

