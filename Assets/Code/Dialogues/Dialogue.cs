using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dialogues
{
    public class Dialogue
    {
        #region Fields

        private static Dictionary<int, Dialogue> _dialogues { get; } = new();

        #endregion Fields

        #region Properties

        public Verse[] Verses { get; set; }

        public Verse this[int i]
        {
            get
            {
                if (i >= 0 && i < Verses.Length)
                    return Verses[i];
                else
                    return Verse.Empty;
            }
        }

        public int Length => Verses.Length;

        public static Dialogue Empty { get; } = new(string.Empty);

        #endregion Properties

        #region Contructors

        public Dialogue(params string[] verses)
        {
            verses.Select(verse => new Verse(verse));
        }

        public Dialogue(DialogueScriptableObject source)
        {
            Verses = source.Verses;
        }

        #endregion Contructors

        #region Public

        public static Dialogue Get(int index)
        {
            _dialogues.TryGetValue(index, out var dialogue);
            return dialogue == null ? Empty : dialogue;
        }

        public static void LoadResources()
        {
            var resources = Resources.LoadAll<DialogueScriptableObject>("ScriptableObjects");
            foreach (var source in resources)
                _dialogues[int.Parse(source.name)] = new(source);
        }

        public override string ToString()
        {
            return string.Join("\n", Verses.ToList());
        }

        #endregion Public
    }
}