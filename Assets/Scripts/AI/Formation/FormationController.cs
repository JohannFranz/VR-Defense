using UnityEngine;

public class FormationController : MonoBehaviour
{
    public Formation formation;
    public Slot[] slots;

    public void Start()
    {
        formation = GetComponent<LineFormation>();

        slots = formation.GetSlots();
        int activeMembers = formation.GetActiveMembers();
    }

    public void RemoveFromFormation(GameObject agent)
    {
        formation.RemoveFromFormation(agent);
    }

    public void UpdateFormation()
    {
        formation.MoveUp();
    }

    public void AddFormationMember(GameObject member)
    {
        formation.AddToFormation(member);
    }

    public bool IsInFront(GameObject agent)
    {
        return formation.IsInFront(agent);
    }

    public void TransferFormationTo(FormationController other)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            other.slots[i].character = slots[i].character;
            other.slots[i].seekPosition = slots[i].seekPosition;
        }
        other.formation.activeMembers = formation.activeMembers;
    }

}
