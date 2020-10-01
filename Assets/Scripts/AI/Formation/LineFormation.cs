using UnityEngine;

public class LineFormation : Formation
{
    public int membersPerLine;

    //#####################################
    public GameObject[] slotChars;
    public GameObject[] slotPositions;
    //#####################################

    public new void Awake()
    {
        base.Awake();

        slotChars = new GameObject[maxCountMembers];
        slotPositions = new GameObject[maxCountMembers];

        activeMembers = 1;
    }

    public new void Start()
    {
        base.Start();
    }

    void Update()
    {
        for (int i = 0; i < maxCountMembers; i++)
        {
            slotChars[i] = slots[i].character;
            slotPositions[i] = slots[i].seekPosition;
        }
    }

    public override void AddToFormation(GameObject character)
    {
        for (int i = 0; i < maxCountMembers; i++)
        {
            if (slots[i].character == null)
            {
                slots[i].character = character;
                slotChars[i] = character;
                activeMembers += 1;
                break;
            }
        }
    }

    public override void AddToFormation(GameObject character, int slotIndex)
    {
        if (slots[slotIndex].character != null)
            return;

        slots[slotIndex].character = character;
        activeMembers += 1;
    }


    public override bool RemoveFromFormation(GameObject character)
    {
        foreach (Slot s in slots)
        {
            if (s.character == null || s.character.GetInstanceID() != character.GetInstanceID())
                continue;

            s.character = null;
            activeMembers -= 1;
            return true;
        }
        return false;
    }

    public override bool IsInFront(GameObject agent)
    {
        GameObject character = gameObject.transform.parent.parent.gameObject;
        for (int i = 0; i < membersPerLine; i ++)
        {
            if (slots[i].character == null)
                continue;

            if (slots[i].character.GetInstanceID() == agent.GetInstanceID())
                return true;
        }

        return false;
    }

    public override void PlanFormation()
    {
        PlanFormation(center.position);
    }

    //calculates the new position for the slots
    //Differs for every formation type
    public override void PlanFormation(Vector3 newPosition)
    {
        Vector3 moveDirection = newPosition - center.position;
        moveDirection.Normalize();
        Vector3 rightDir = Vector3.Cross(moveDirection, Vector3.up);

        newPosition.y += 0.5f;
        center.position = newPosition;
        int lineStart = 0;
        Vector3 lineCenter = newPosition;
        int rowIndex = 0;

        while (lineStart < maxCountMembers)
        {
            int actualMembersInLine = 0;
            for (int i = lineStart; i < lineStart + membersPerLine; i++)
            {
                if (i >= maxCountMembers)
                    break;
                if (slots[i].character == null)
                    continue;

                actualMembersInLine += 1;
            }
            int membersPerSide = GetMembersPerSide(actualMembersInLine);
            SetSeekPositionForSlots(actualMembersInLine, membersPerSide, lineStart, lineCenter, rightDir, rowIndex);

            //###################################################
            for (int i = 0; i < maxCountMembers; i++)
            {
                slotPositions[i] = slots[i].seekPosition;
            }
            //###################################################
            rowIndex += 1;
            lineStart += membersPerLine;
            lineCenter -= moveDirection * lineSpacing;
        }
    }

    int GetMembersPerSide(int activeMembers)
    {
        if (activeMembers % 2 == 1)
        {
            return (activeMembers - 1) / 2;
        }

        return activeMembers / 2;
    }

    void SetSeekPositionForSlots(int actualMembersInLine, int membersPerSide, int lineStart, Vector3 lineCenter, Vector3 rightDir, int rowIndex)
    {
        if (actualMembersInLine == 1)
        {
            slots[rowIndex * membersPerLine].seekPosition.transform.position = lineCenter;
            return;
        }

        float startSlotDistance;

        //the center position is also in the central slot
        if (actualMembersInLine % 2 == 1)
        {
            startSlotDistance = 2 * slotRadius + slotSpacing * membersPerSide + slotRadius * 2 * (membersPerSide - 1);
        }
        else //the center position is between the 2 inner slots
        {
            startSlotDistance = slotRadius * (2 * (membersPerSide - 1) + 1) + slotSpacing * (membersPerSide - 1) + slotSpacing * 0.5f;
        }

        //The formation is set from left to right, every row
        //first loop is for finding the first slot on the left side
        int j = rowIndex * membersPerLine;
        for (int i = lineStart; i < (rowIndex+1) * membersPerLine; i++)
        {
            if (i >= maxCountMembers)
                break;
            if (slots[i].character == null)
                continue;
            slots[i].seekPosition.transform.position = lineCenter - rightDir * startSlotDistance;
            j = i + 1; 
            break;
        }

        int formerValidSlotIndex = j-1;
        for (int i = j; i < (rowIndex + 1) * membersPerLine; i++)
        {
            if (i >= maxCountMembers)
                break;
            if (slots[i].character == null)
                continue;
            slots[i].seekPosition.transform.position = slots[formerValidSlotIndex].seekPosition.transform.position + rightDir * slotDistance;
            formerValidSlotIndex = i;
        }
    }

    //moves the characters to the slot position
    public override void MoveFormation()
    {
        foreach (Slot s in slots)
        {
            if (s.character == null || s.character.activeSelf == false)
                continue;

            s.character.GetComponent<AIController>().SetMoveTarget(s.seekPosition);
        }
    }

    //move a character from the back lines to the front lines
    public override void MoveUp()
    {
        if (activeMembers <= 0)
            return;

        for (int i = 0; i < maxCountMembers; i++)
        {
            if (slots[i].character != null)
                continue;

            int replacementIndex = GetReplacementIndex(i);
            if (replacementIndex <= 0)
                continue;

            slots[i].character = slots[replacementIndex].character;
            slots[replacementIndex].character = null;

            GameObject slotCharacterAI = TD_Utility.Utility.GetChildByTag(slots[i].character, Constants.AITag);
            slotCharacterAI.GetComponent<KinematicController>().SetTarget(slots[i].seekPosition);
        }
    }

    protected override int GetReplacementIndex(int indexToReplace)
    {
        //if indexToReplace is already in the last line -> no replacement exists
        if (indexToReplace + membersPerLine >= maxCountMembers) return -1;

        int replacement = -1;
        int modIndexToReplace = indexToReplace % membersPerLine;
        int modCurrentReplacement = -1;
        int spaceCurrentReplacement = -1;

        for (int i = membersPerLine; i < maxCountMembers - 1; i += membersPerLine)
        {
            if (i >= maxCountMembers)
                break;
            if (i > indexToReplace)
            {
                for (int j = i; j < i + membersPerLine; j++)
                {
                    if (j >= maxCountMembers)
                        break;
                    if (slots[j].character == null || slots[j].character.activeSelf == false)
                        continue;
                    if (replacement == -1)
                    {
                        replacement = j;
                        modCurrentReplacement = j % membersPerLine;
                        spaceCurrentReplacement = Mathf.Abs(modCurrentReplacement - modIndexToReplace);
                        continue;
                    }
                    int modCurrentIndex = j % membersPerLine;

                    int spaceCurrentIndex = Mathf.Abs(modCurrentIndex - modIndexToReplace);
                    if (spaceCurrentIndex >= spaceCurrentReplacement)
                        continue;
                    replacement = j;
                    modCurrentReplacement = j % membersPerLine;
                    spaceCurrentReplacement = Mathf.Abs(modCurrentReplacement - modIndexToReplace);
                }
            }
            if (replacement != -1)
                return replacement;
        }

        return replacement;
    }

    


//returns false if there are holes in the formation
    public override bool CheckFormationIntegrity()
    {
        return false;
    }
}
