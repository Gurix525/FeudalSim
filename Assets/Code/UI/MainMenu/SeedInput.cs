using System.IO;
using TMPro;
using UnityEngine;

public class SeedInput : MonoBehaviour
{
    #region Fields

    private TMP_InputField _seedInput;

    #endregion Fields

    #region Properties

    public int Seed
    {
        get
        {
            int.TryParse(_seedInput.text, out int parsed);
            if (parsed == 0)
                return new System.Random().Next();
            return parsed;
        }
    }

    #endregion Properties

    #region Unity

    private void Awake()
    {
        _seedInput = GetComponent<TMP_InputField>();
        _seedInput.characterValidation =
            TMP_InputField.CharacterValidation.Integer;
        _seedInput.characterLimit = 9;
    }

    private void OnEnable()
    {
        _seedInput.ActivateInputField();
    }

    #endregion Unity
}