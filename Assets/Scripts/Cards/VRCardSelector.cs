using UnityEngine;

namespace Card
{
    public class VRCardSelector : MonoBehaviour
    {
        [SerializeField]
        private bool inVRMode;
        [SerializeField]
        private bool selected;

        private GameObject selectionBackground;

        void Start()
        {
            selected = false;
            selectionBackground = transform.Find("Selected").gameObject;
            selectionBackground.SetActive(false);

            VR_Controller vrCon = GameObject.Find(Constants.VR_Tag).GetComponent<VR_Controller>();
            if (vrCon.IsVRMode())
            {
                inVRMode = true;
                return;
            }

            inVRMode = false;
            enabled = false;
        }

        public void OnPointerDown()
        {
            if (inVRMode == false)
                return;

            if (transform.parent.tag != Constants.HandTag)
                return;

            SelectCard(); 
            DeselectAlreadySelectedCard();
        }

        private void SelectCard()
        {
            selected = !selected;
            selectionBackground.SetActive(true);
        }

        private void DeselectAlreadySelectedCard()
        {
            Transform hand = transform.parent;
            if (hand.tag != Constants.HandTag)
                return;

            for (int i = 0; i < hand.childCount; i++)
            {
                GameObject child = hand.GetChild(i).gameObject;
                if (child == gameObject)
                    continue;

                VRCardSelector selector = child.GetComponent<VRCardSelector>();
                selector.selected = false;
                selector.selectionBackground.SetActive(false);
            }
        }
    }
}