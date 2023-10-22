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
                return new System.Random().Next(0, 9999999);
            return parsed;
        }
    }

    #endregion Properties

    #region Unity

    private void Awake()
    {
        _seedInput = GetComponent<TMP_InputField>();
        _seedInput.characterValidation =
            TMP_InputField.CharacterValidation.Digit;
        _seedInput.characterLimit = 7;
    }

    private void OnEnable()
    {
        _seedInput.ActivateInputField();
    }

    #endregion Unity
}