using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Room room;

	// Use this for initialization
	void Awake () {
        room = GetComponent<Room>();
        InitGame();
	}

    void InitGame()
    {
        room.SetupScene();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
