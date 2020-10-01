using UnityEngine;

public class Slot
{
    public GameObject character;
    public GameObject seekPosition; //the position of the slot in world coordinates
}

public class Formation : MonoBehaviour
{
    public int maxCountMembers;
    public Slot[] slots;
    public Transform center;

    public float slotRadius;
    public float slotSpacing;
    public float lineSpacing;

    public int activeMembers;
    protected float slotDistance;


    public void Awake()
    {
        slots = new Slot[maxCountMembers];
        for (int i = 0; i < maxCountMembers; i++)
        {
            slots[i] = new Slot();
            slots[i].seekPosition = new GameObject();
        }

        GameObject go = new GameObject();
        center = go.transform;
    }

    public void Start()
    {
        slotDistance = 2 * slotRadius + slotSpacing;
    }

    public virtual void AddToFormation(GameObject character) { }
    public virtual void AddToFormation(GameObject character, int slotIndex) { }
    public virtual bool RemoveFromFormation(GameObject character) { return false; }
    public virtual void PlanFormation(Vector3 newPosition) { }
    public virtual void PlanFormation() { }
    public virtual void MoveFormation() { }
    public virtual void MoveUp() { }
    public virtual bool IsInFront(GameObject agent) { return false; }

    public Vector3 GetCenter() { return center.position; }
    public Slot[] GetSlots() { return slots; }
    public int GetActiveMembers() { return activeMembers; }
    public void SetCurrentCenter(Vector3 newCenter) { center.position = newCenter; }

    protected virtual int GetReplacementIndex(int indexToReplace) { return -1; }

    //returns false if there are holes in the formation
    public virtual bool CheckFormationIntegrity() { return false; }

}
