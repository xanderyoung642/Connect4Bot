using UnityEngine;

public class BabyDifficulty : MonoBehaviour
{
    public GameObject chipInstantiation;
    public ChipInstantiation chipInstantiationScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BabyMakeMove()
    {
        int column = Random.Range(0, 7);

        chipInstantiationScript.ColumnSelectedUnityOrArduino(column);
    }
}
