using UnityEngine;

public class ChipInstantiation : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject redChipPrefab;
    public GameObject yellowChipPrefab;

    [Header("Instantiation Positions")]
    public Vector3[] columnPositions;

    [Header("Instantiation Bools")]
    public bool readyToInstantiate;
    public bool redTurnCI;
    public bool[] columnsFilled;

    [Header("Arduino Connector")]
    public GameObject arduinoConnector;
    public ArduinoConnector arduinoConnectorScript;

    void Start()
    {
        //arduinoConnectorScript = arduinoConnector.GetComponent<ArduinoConnector>();
    }

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
        if (!columnsFilled[whichColumn])
        {
            GameObject newChip = Instantiate(redChipPrefab, columnPositions[whichColumn], transform.rotation);
            readyToInstantiate = false;
        }
    }
    public void InstantiateYellow(int whichColumn)
    {
        if (!columnsFilled[whichColumn])
        {
            GameObject newChip = Instantiate(yellowChipPrefab, columnPositions[whichColumn], transform.rotation);
            readyToInstantiate = false;
        }
    }

    public void ColumnSelectedToArduinoConnector(int column)
    {
        arduinoConnectorScript.SendColumnToArduino(column + 1);
        InstantiatePickColor(column);
    }

    public void ColumnSelectedUnityOrArduino(int column)
    {
        if (arduinoConnector.activeSelf)
        {
            ColumnSelectedToArduinoConnector(column);
        }
        else
        {
            InstantiatePickColor(column);
        }
    }
}