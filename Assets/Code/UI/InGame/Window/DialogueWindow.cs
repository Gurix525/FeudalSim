using Dialogues;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DialogueWindow : Window
    {
        #region Fields

        [SerializeField] private TextMeshProUGUI _text;

        private Dialogue _dialogue;
        private int _verseIndex = 0;

        #endregion Fields

        #region Properties

        public static DialogueWindow Current { get; private set; }

        #endregion Properties

        #region Public

        public void ShowDialogue(Dialogue dialogue)
        {
            _dialogue = dialogue;
            _verseIndex = 0;
            _text.text = string.Empty;
            gameObject.SetActive(true);
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            Current = this;
            gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            _text.text = _dialogue[_verseIndex].Text;
        }

        #endregion Unity
    }
}