using UnityEngine;

namespace PlayerControls
{
    public class RightHandItemHook : MonoBehaviour
    {
        [SerializeField] private GameObject _assignedItem;

        public void SetItemActive(string name, bool state)
        {
            foreach (Transform child in transform)
            {
                if (child.name == name)
                    child.gameObject.SetActive(state);
            }
        }
    }
}