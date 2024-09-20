﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Little_Choice_Based_RPG.Types
{
    internal class SanitizedString
    {
        string output;
        public SanitizedString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Invalid input!");

            Value = Sanitize(input); 
        }

        private protected string Sanitize(string input) => output = input;

        public string Concatenate(string input) => output += Sanitize(input);

        public string Value { get { return output; } set { Sanitize(value); } }
    }
}
