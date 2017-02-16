namespace Solum.Models
{
    public class DisplayItems
    {
        public DisplayItems(string displayText, float value)
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
}