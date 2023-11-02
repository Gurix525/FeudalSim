using System;
using UnityEngine;

namespace Dialogues
{
    [Serializable]
    public class Verse
    {
        #region Properties

        [field: TextArea(1, 10)][field: SerializeField] public string Text { get; set; } = string.Empty;

        [field: SerializeField] public VerseRuleScriptableObject[] VerseRules { get; set; }
        public static Verse Empty { get; } = new();

        #endregion Properties

        #region Constructors

        public Verse()
        {
        }

        public Verse(string text)
        {
            Text = text;
        }

        #endregion Constructors

        #region Public

        public override string ToString()
        {
            return Text;
        }

        #endregion Public
    }
}