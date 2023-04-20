using Items;
using UnityEngine;
using Cursor = Controls.Cursor;

public class CursorActionChangeDetector : MonoBehaviour
{
    private ItemAction _previousAction;

    private void Update()
    {
        if (Cursor.Action != _previousAction)
        {
            _previousAction = Cursor.Action;
            Cursor.Container.CollectionUpdated.Invoke();
        }
    }
}