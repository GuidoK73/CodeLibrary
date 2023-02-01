using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace CodeLibrary.Core
{
    public class CSharpUtils
    {
        //Regex _RegexProperty = new Regex("public(\\s+)(\\w+|\\d+)(\\s+)(\\w+|\\d+)(\\s+|(\r\n|\r|\n))");

        Regex _RegexProperty = new Regex("public(\\s+)((\\w+|\\d+)|(\\w+|\\d+)\\?)(\\s+)(\\w+|\\d+)(\\s+|(\r\n|\r|\n))");

        public List<string[]> GetProperties(string code, out bool IsClass)
        {
            var _matches = _RegexProperty.Matches(code);

            if (_matches.Count == 0)
            {
                IsClass = false;
                return null;
            }
            if (_matches[0].Groups.Count < 2)
            {
                IsClass = false;
                return null;  
            }

            IsClass = _matches[0].Groups[2].Value == "class";
            if (IsClass == false)
            {
                return null;
            }

            List<string[]> _result = new List<string[]>();

            foreach (Match match in _matches)
            {
                string _type = match.Groups[2].Value;
                string _name = match.Groups[6].Value;
                _result.Add(new string[] { _type, _name });
            }

            return _result;

        }
    }
}
