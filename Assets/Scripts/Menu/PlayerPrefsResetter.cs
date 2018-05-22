using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsResetter : MonoBehaviour {
    public bool DeleteAllFromPlayerPrefs = false;

	void Awake () 
    {
        if (DeleteAllFromPlayerPrefs)
        {
            PlayerPrefs.DeleteAll();
            Debug.LogWarning("PlayerPrefs have been reset in script PlayerPrefsResetter");
        }
	}	
}
