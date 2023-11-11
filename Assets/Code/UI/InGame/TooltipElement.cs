using System;
using UnityEngine;

namespace UI
{
    public class TooltipElement : IEquatable<TooltipElement>
    {
        private readonly int _normalFontSize = 16;
        private readonly int _headerFontSize = 20;
        private readonly int _titleFontSize = 24;

        private string _text;
        private FontType _fontType;
        private Sprite _sprite;

        public TooltipElement(string text, FontType fontType = FontType.Normal, Sprite sprite = null)
        {
            _text = text;
            _fontType = fontType;
            _sprite = sprite;
        }

        public int FontSize => _fontType switch
        {
            FontType.Normal => _normalFontSize,
            FontType.Header => _headerFontSize,
            FontType.Title => _titleFontSize,
            _ => _normalFontSize,
        };

        public string Text => _text;

        public Sprite Sprite => _sprite;

        public override int GetHashCode()
        {
            return HashCode.Combine(_text, _fontType, _sprite);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is not TooltipElement)
                return false;
            return Equals((TooltipElement)obj);
        }

        public bool Equals(TooltipElement other)
        {
            if (other == null)
                return false;
            return _text == other._text
                && _fontType == other._fontType
                && _sprite == other._sprite;
        }

        public override string ToString()
        {
            return Text;
        }

        public static bool operator ==(TooltipElement lhs, TooltipElement rhs)
        {
            if (lhs is null)
                return rhs is null;
            if (rhs is null)
                return lhs is null;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(TooltipElement lhs, TooltipElement rhs)
        {
            return !(rhs == lhs);
        }

        public enum FontType
        {
            Normal,
            Header,
            Title
        }
    }
}