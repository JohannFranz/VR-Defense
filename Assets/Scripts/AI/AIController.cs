using UnityEngine;

public class AIController : MonoBehaviour
{
    public GameObject target;

    KinematicController kinController;

    // Start is called before the first frame update
    void Start()
    {
        kinController = GetComponent<KinematicController>();
        kinController.SetTarget(target);
    }

    // Update is called once per frame
    public void Update()
    {
        
        
    }

    //the target does not have to be a character, it can also be a random position in space
    public void SetMoveTarget(GameObject target)
    {
        kinController.SetTarget(target);
    }
}
