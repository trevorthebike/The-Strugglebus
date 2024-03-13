using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// Last edited - Norman Zhu 3:32PM 2/21/24 

// Handles enemy spawning, and group enemy behavior.
// 
public class EnemyManager : MonoBehaviour
{
    public GameObject spawnerPrefab;
    public List<GameObject> spawnerList; // Each spawner handles its own local list of enemies.
                                         //         And its own unique list of Waypoint references to map a route.
    private int spawnersPerWave = 1;     // can increment this based on currentScaling in GameManager.
    public GameObject[] enemySpritePrefab;

    // Instantiated, not object pooled.
    //         Only need one or a few at level start.

    private static EnemyManager _instance;

    public static EnemyManager Instance { get { return _instance; } }


    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        // TEMP CODE. REPLACE WITH SUBSCRIBE TO GAMEMANAGER EVENT INSTEAD FOR PREP / ENEMY PHASES.
        // Subscribe to the startbuildphaseevent in gamemanager
        GameManager.StartBuildPhaseEvent += spawnSpawner;
        
        spawnerList = new List<GameObject>();
    }


    private void spawnSpawner()
    {
        //if (scene.name == "GameScene1")
        //{
        if (GameManager.DEBUG_MODE)
            Debug.Log("EnemyManager: Spawning spawner.");
        for (int i = 0; i < spawnersPerWave; ++i)
        {
            float xLoc = Mathf.RoundToInt(Random.Range(PathingMap.Instance.x_lower_bound, PathingMap.Instance.y_upper_bound));
            xLoc += 0.5f;
            float yLoc = Mathf.RoundToInt(Random.Range(PathingMap.Instance.y_upper_bound - 1, PathingMap.Instance.y_upper_bound));
            yLoc += 0.5f;

            Vector3 spawnLoc = new Vector3(xLoc, yLoc, 0);
            Vector3Int spawnerCell = PathingMap.Instance.tm.WorldToCell(spawnLoc);
            PathingMap.Instance.tm.SetTile(spawnerCell, PathingMap.Instance.pathable_invis_tile);
            GameObject newSpawner = Instantiate(spawnerPrefab, spawnLoc, Quaternion.identity);
            newSpawner.SetActive(true);
            newSpawner.transform.SetParent(this.transform);

            spawnerList.Add(newSpawner);
        }
    }

    private void despawnSpawners()
    {

    }

    public int getSpawnerCount()
    {
        return spawnerList.Count;
    }

    public bool vector3_matches_one_spawner(Vector3Int coordinate)
    {
        bool matches = false;
        foreach (GameObject spawner in spawnerList)
        {
            Vector3 spawnerLoc = spawner.transform.position;
            if (PathingMap.Instance.tm.WorldToCell(spawnerLoc) == coordinate)
            {
                Debug.Log("provided coordinate matched with a spawner - EnemyManager()");
                matches = true;
                break;
            }
        }
        return matches;
    }



    private void Update()
    {

    }
}
