using Items;
using UnityEngine;
using Cursor = Controls.Cursor;

public class CursorItemChangeDetector : MonoBehaviour
{
    private ItemAction _previousAction;
    private Item _previousItem;

    private void Update()
    {
        if (Cursor.Item != _previousItem)
        {
            _previousItem = Cursor.Item;
            Cursor.ItemChanged.Invoke(_previousItem);
        }
        //if (Cursor.Action != _previousAction)
        //{
        //    _previousAction = Cursor.Action;
        //    Cursor.Container.CollectionUpdated.Invoke();
        //}
    }
}