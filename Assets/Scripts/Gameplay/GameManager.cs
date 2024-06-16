
using UnityEngine;


public class Game : MonoBehaviour
{

    [SerializeField] private World[] worlds;
    private LevelManager levelManager;
    public static Game Instance { get; private set; }
    
    public World[] Worlds
    {
        get
        {
            return worlds;
        }
    }

    [SerializeField] private int worldIndex;
    public int WorldIndex { get { return worldIndex; }}
    public static int TotalAmountOfLevels { get; private set; }

    
    private void Awake()
    {

        if (Instance == null)
        {

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {

            Destroy(gameObject);
        }
        SetTotalAmountOfLevels();
        levelManager = FindObjectOfType<LevelManager>();

        levelManager.StartLevel(1);
    }

    private void SetTotalAmountOfLevels()
    {
        int total = 0;
        foreach (World world in worlds)
        {
            total += world.levels.Length;
        }
        TotalAmountOfLevels = total;
    }

   








}
