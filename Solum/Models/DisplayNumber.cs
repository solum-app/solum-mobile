using System;

namespace Solum.Models
{
    public class DisplayNumber
    {
        public DisplayNumber(string displayText, float value)
        {
            DisplayText = displayText;
            Value = value;
        }

        public string DisplayText { get; set; }
        public float Value { get; set; }

        public override string ToString()
        {
            return DisplayText;
        }
    }

    public class DisplayEnum
    {
        public DisplayEnum(string text, Enum item)
        {
            Text = text;
            Item = item;
        }

        public string Text { get; set; }
        public Enum Item { get; set; }
        public override string ToString()
        {
            return Text;
        }
    }
}