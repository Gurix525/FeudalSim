using System.IO;
using TMPro;
using UnityEngine;

public class NameInput : MonoBehaviour
{
    #region Fields

    [SerializeField] private TextMeshProUGUI _alert;

    private TMP_InputField _nameInput;
    private bool _isNameAlreadyTaken;

    #endregion Fields

    #region Properties

    public bool IsNameAllowed => !IsNameEmpty() && !_isNameAlreadyTaken;

    public string Text => _nameInput.text;

    #endregion Properties

    #region Unity

    private void Awake()
    {
        _nameInput = GetComponent<TMP_InputField>();
        _nameInput.characterValidation =
            TMP_InputField.CharacterValidation.Alphanumeric;
        _nameInput.characterLimit = 50;
    }

    private void OnEnable()
    {
        _nameInput.ActivateInputField();
        _nameInput.onValueChanged.AddListener(OnInputTextChange);
        OnInputTextChange(_nameInput.text);
    }

    private void OnDisable()
    {
        _nameInput.onValueChanged.RemoveListener(OnInputTextChange);
    }

    #endregion Unity

    #region Private

    private bool IsNameEmpty()
    {
        return _nameInput.text.Length == 0;
    }

    private void OnInputTextChange(string input)
    {
        _isNameAlreadyTaken = false;
        DirectoryInfo savesFolder = new DirectoryInfo(
            Path.Combine(Application.persistentDataPath, "Saves"));
        var files = savesFolder.GetFiles("*.zip");
        foreach (var file in files)
        {
            if (file.Name == $"{_nameInput.text}.zip")
            {
                {
                    _isNameAlreadyTaken = true;
                    break;
                }
            }
        }
        _alert.text = _isNameAlreadyTaken
            ? "Name already taken"
            : IsNameEmpty()
                ? "Name must not be empty"
                : string.Empty;
    }

    #endregion Private
}