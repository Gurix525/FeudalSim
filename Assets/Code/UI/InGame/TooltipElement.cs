namespace UI
{
    public class TooltipElement
    {
        private readonly int _normalFontSize = 18;
        private readonly int _headerFontSize = 24;

        private string _text;
        private FontType _fontType;

        public TooltipElement(string text, FontType fontType = FontType.Normal)
        {
            _text = text;
            _fontType = fontType;
        }

        public int FontSize => _fontType switch
        {
            FontType.Normal => _normalFontSize,
            FontType.Header => _headerFontSize,
            _ => _normalFontSize,
        };

        public string Text => _text;

        public override string ToString()
        {
            return Text;
        }

        public enum FontType
        {
            Normal,
            Header
        }
    }
}