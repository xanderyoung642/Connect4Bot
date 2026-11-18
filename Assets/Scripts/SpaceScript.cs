using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class SpaceScript : MonoBehaviour
{
    [Header("Positioning References")]
    public int row;
    public int column;

    [Header("Diagonal Positionings")]
    public bool inLRDiag;
    public int lRDiagIndex;
    public bool inRLDiag;
    public int rLDiagIndex;

    [Header("Chip State")]
    public string state;
    public bool filled;
    public GameObject chipFilling;

    [Header("Space Above For Collider Stuff")]
    public bool topRow;
    public GameObject spaceAboveMe;

    [Header("Win Logic")]
    public GameObject winLogicObject;
    public WinLogic winLogicScript;

    void Start()
    {
        state = "e";

        filled = false;

        winLogicScript = winLogicObject.GetComponent<WinLogic>();
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        Debug.Log("landed");

        GameObject other = collider.gameObject;

        if (!topRow)
        {
            spaceAboveMe.GetComponent<BoxCollider2D>().enabled = true;
        }

        other.transform.SetParent(gameObject.transform);
        other.transform.localPosition = UnityEngine.Vector3.zero;
        other.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        other.GetComponent<ChipScript>().filling = true;


        other.GetComponent<ChipScript>().NowOccupyingSomething(gameObject);
    }

    public void ChipUpdated()
    {
        winLogicScript.UpdateChip(row, column, state, inLRDiag, lRDiagIndex, inRLDiag, rLDiagIndex);
    }
}