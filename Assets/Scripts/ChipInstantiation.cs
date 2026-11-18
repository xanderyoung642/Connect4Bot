using UnityEngine;

public class ChipInstantiation : MonoBehaviour
{
    public GameObject redChipPrefab;
    public GameObject yellowChipPrefab;
    public Vector3[] columnPositions;
    public bool readyToInstantiate;
    public bool redTurnCI;

    public void InstantiatePickColor(int whichColumn)
    {
        if (readyToInstantiate)
        {
            if (redTurnCI)
            {
                InstantiateRed(whichColumn);
            }
            else
            {
                InstantiateYellow(whichColumn);
            }
        }
    }

    public void InstantiateRed(int whichColumn)
    {
        GameObject newChip = Instantiate(redChipPrefab, columnPositions[whichColumn], transform.rotation);
        readyToInstantiate = false;
    }
    public void InstantiateYellow(int whichColumn)
    {
        GameObject newChip = Instantiate(yellowChipPrefab, columnPositions[whichColumn], transform.rotation);
        readyToInstantiate = false;
    }
}