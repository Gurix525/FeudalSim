using Dialogues;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DialogueWindow : Window
    {
        #region Fields

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _portrait;
        [SerializeField] private Button _fullscreenContinueButton;

        private Dialogue _dialogue;
        private int _verseIndex = 0;
        private int _charIndex = 0;

        #endregion Fields

        #region Properties

        public static DialogueWindow Current { get; private set; }

        #endregion Properties

        #region Public

        public void ShowDialogue(Dialogue dialogue)
        {
            _dialogue = dialogue;
            _verseIndex = 0;
            _charIndex = 0;
            _text.text = string.Empty;
            gameObject.SetActive(true);
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            Current = this;
            gameObject.SetActive(false);
            _fullscreenContinueButton.Clicked += _fullscreenContinueButton_Clicked;
        }

        private void FixedUpdate()
        {
            UpdateWindow();
        }

        #endregion Unity

        #region Private

        private void UpdateWindow()
        {
            if (_charIndex++ < _dialogue[_verseIndex].Text.Length)
                _text.text = _dialogue[_verseIndex].Text.Substring(0, _charIndex);
            _portrait.sprite = _dialogue[_verseIndex].Sprite;
            _portrait.gameObject.SetActive(_portrait.sprite != null);
        }

        private void _fullscreenContinueButton_Clicked(object sender, System.EventArgs e)
        {
            if (_charIndex < _dialogue[_verseIndex].Text.Length)
            {
                _charIndex = _dialogue[_verseIndex].Text.Length - 1;
                UpdateWindow();
            }
            else
                ShowNextVerse();
        }

        private void ShowNextVerse()
        {
            if (++_verseIndex >= _dialogue.Length)
                EndDialogue();
            _charIndex = 0;
        }

        private void EndDialogue()
        {
            _dialogue = null;
            _verseIndex = 0;
            _charIndex = 0;
            gameObject.SetActive(false);
        }

        #endregion Private
    }
}