using System;
using System.IO.Ports;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class ArduinoConnector : MonoBehaviour
{
    SerialPort serial = new SerialPort("COM7", 9600);
    public GameObject chipInstantiatorAc;
    public ChipInstantiation chipInstantiationScriptAc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        serial.Open();
        serial.ReadTimeout = 10;

        chipInstantiationScriptAc = chipInstantiatorAc.GetComponent<ChipInstantiation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (serial == null || !serial.IsOpen) return;

        try
        {
            string data = serial.ReadLine().Trim();

            if (int.TryParse(data, out int column) && !(column > 7 || column == 0))
            {
                chipInstantiationScriptAc.InstantiatePickColor(column - 1);
            }
        }
        catch (TimeoutException)
        {
            //nothing
        }
    }

    public void SendColumnToArduino(int column)
    {
        serial.Write(column.ToString());
    }

    void OnApplicationQuit()
    {
        serial.Close();
        serial.Dispose();
    }
    void OnDisable()
    {
        serial.Close();
        serial.Dispose();
    }
    void OnDestroy()
    {
        serial.Close();
        serial.Dispose();
    }
}
