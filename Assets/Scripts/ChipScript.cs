using UnityEngine;

public class ChipScript : MonoBehaviour
{
    [Header("Chip State")]
    public string color;
    public bool filling;

    [Header("Space State")]
    public GameObject spaceFilling;
    public SpaceScript spaceScript;

    public void NowOccupyingSomething(GameObject space)
    {
	    spaceFilling = space;
	
	    spaceScript = spaceFilling.GetComponent<SpaceScript>();

        spaceScript.state = color;
        spaceScript.filled = true;
        spaceScript.chipFilling = gameObject;
    }

}
