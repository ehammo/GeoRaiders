using UnityEngine;
using System.Collections;

public class SpawnGameObjects : MonoBehaviour {

	public GameObject spawnPrefab;
	public float minSecondsBetweenSpawning = 3.0f;
	public float maxSecondsBetweenSpawning = 9.0f;
    private float range = 30f;
	private float savedTime;
	private float secondsBetweenSpawning;

	// Use this for initialization
	void Start () {
		savedTime = Time.time;
		secondsBetweenSpawning = Random.Range (minSecondsBetweenSpawning, maxSecondsBetweenSpawning);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - savedTime >= secondsBetweenSpawning) // is it time to spawn again?
		{
			MakeThingToSpawn();
			savedTime = Time.time; // store for next spawn
			secondsBetweenSpawning = Random.Range (minSecondsBetweenSpawning, maxSecondsBetweenSpawning);
        }
        
	}

	void MakeThingToSpawn()
	{
        float x = Random.Range(-range,range);
        float z = Random.Range(-range, range);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;
        float playerZ = player.transform.position.z;
        Vector3 pos = new Vector3(playerX+x, playerY, playerZ+z);
        GameObject clone = Instantiate(spawnPrefab, pos, player.transform.rotation) as GameObject;
               
	}
}
