namespace Little_Choice_Based_RPG.Types
{
    public class SanitizedString
    {
        string output;
        public SanitizedString(string input)
        {
            Value = Sanitize(input); 
        }

        private protected string Sanitize(string input) => output = input;

        public string Concatenate(string input) => output += Sanitize(input);

        public string Value { get { return output; } set { Sanitize(value); } }
    }
}
