//using System.Collections.Generic;
//using Extensions;
//
//using Items;
//using Misc;
//using UnityEngine;
//using UnityEngine.UI;
//using System.Linq;
//using static UnityEngine.InputSystem.InputAction;
//using Cursor = Controls.Cursor;
//using UnityEngine.Events;

//namespace UI
//{
//    public class QuickMenu : MonoBehaviour
//    {
//        #region Fields

//        [SerializeField] private GameObject _quickMenu;
//        [SerializeField] private GameObject _cursorReplacement;
//        [SerializeField] private Image _item;

//        private GameObject _slotPrefab;
//        private QuickMenuSlot[] _slots;
//        private Queue<Vector2> _mouseDeltas = new();
//        private int _resetCounter;
//        private int? _slotNumber;

//        #endregion Fields

//        #region Properties

//        public static UnityEvent<ItemAction, int> Closed { get; } = new();

//        #endregion Properties

//        #region Unity

//        private void Awake()
//        {
//            _slotPrefab = Resources.Load<GameObject>("Prefabs/UI/QuickMenuSlot");
//        }

//        private void Update()
//        {
//            if (!_quickMenu.activeInHierarchy || _slots == null)
//                return;
//            Vector2 mouseDelta = PlayerController.QuickMenuMouseDelta.ReadValue<Vector2>().normalized;
//            if (mouseDelta.magnitude < 1F)
//            {
//                _resetCounter++;
//                if (_resetCounter == 20)
//                {
//                    _mouseDeltas.Clear();
//                    _resetCounter = 0;
//                }
//                return;
//            }
//            _mouseDeltas.Enqueue(mouseDelta);
//            if (_mouseDeltas.Count > 5)
//                _mouseDeltas.Dequeue();
//            Vector2 average = _mouseDeltas.GetAverage();
//            Vector2 direction = average.normalized;
//            _cursorReplacement.transform.localPosition = average * 100F;
//            float angle = Vector2.SignedAngle(direction, Vector2.down);
//            angle = angle < 0F ? angle + 360F : angle;
//            int slotNumber = (int)(angle / (360F / _slots.Length));
//            for (int i = 0; i < _slots.Length; i++)
//            {
//                if (i == slotNumber)
//                    _slots[i].Background.color = Color.gray;
//                else
//                    _slots[i].Background.color = Color.white;
//            }
//            _slotNumber = slotNumber;
//        }

//        private void OnEnable()
//        {
//            PlayerController.MainQuickMenu.AddListener(ActionType.Started, OpenQuickMenu);
//        }

//        private void OnDisable()
//        {
//            PlayerController.MainQuickMenu.RemoveListener(ActionType.Started, OpenQuickMenu);
//        }

//        #endregion Unity

//        #region Private

//        private void OpenQuickMenu(CallbackContext context)
//        {
//            if (Cursor.Item == null)
//                return;
//            //UnityEngine.Cursor.visible = false;
//            _quickMenu.SetActive(true);
//            Initialize();
//            PlayerController.Main.Disable();
//            PlayerController.QuickMenuQuickMenu.AddListener(ActionType.Canceled, CloseQuickMenu);
//        }

//        private void CloseQuickMenu(CallbackContext obj)
//        {
//            if (_slotNumber != null)
//            {
//                Closed.Invoke(_slots[_slotNumber.Value].Action, _slotNumber.Value);
//                Cursor.Container.CollectionUpdated.Invoke();
//            }
//            //UnityEngine.Cursor.visible = true;
//            _quickMenu.SetActive(false);
//            _mouseDeltas.Clear();
//            ClearSlots();
//            _slotNumber = null;
//            PlayerController.Main.Enable();
//            PlayerController.QuickMenuQuickMenu.RemoveListener(ActionType.Canceled, CloseQuickMenu);
//        }

//        private void ClearSlots()
//        {
//            if (_slots == null)
//                return;
//            for (int i = 0; i < _slots.Length; i++)
//                Destroy(_slots[i].gameObject);
//            _slots = null;
//        }

//        private void Initialize()
//        {
//            _item.sprite = Cursor.Item.Sprite;
//            ItemAction[] actions = Cursor.Item.Actions;
//            QuickMenuSlot[] slots = new QuickMenuSlot[actions.Length];
//            for (int i = 0; i < actions.Length; i++)
//            {
//                slots[i] = GameObject.Instantiate(_slotPrefab, _quickMenu.transform)
//                    .GetComponent<QuickMenuSlot>();
//                slots[i].Action = actions[i];
//                (slots[i].Action as BuildAction)?.SetBuildingMode(
//                    i - actions.ToList().FindIndex(action => action is BuildAction));
//                (slots[i].Action as ShovelAction)?.SetShovelMode(
//                    i - actions.ToList().FindIndex(action => action is ShovelAction));
//                slots[i].Background.fillAmount = 1F / actions.Length;
//                slots[i].transform.rotation = Quaternion.Euler(0F, 0F, i * (-1F / actions.Length * 360F));
//                slots[i].ItemImage.transform.localPosition = new Vector3(0F, -250F, 0F)
//                    .RotateAroundPivot(Vector3.zero, -0.5F / actions.Length * 360F);
//                slots[i].ItemImage.transform.rotation = Quaternion.identity;
//                slots[i].ItemImage.sprite = actions[i].Sprite;
//            }
//            _slots = slots;
//        }

//        #endregion Private
//    }
//}