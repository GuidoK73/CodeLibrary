using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace CodeLibrary.Core
{
    public class CSharpUtils
    {

         Regex _RegexProperty = new Regex("public(\\s+)((\\w+|\\d+)|(\\w+|\\d+)\\?)(\\s+)(\\w+|\\d+)(\\s+|(\r\n|\r|\n))");

        //Regex _RegexProperty = new Regex("(public(\\s+)|public(\\s+)virtual|public(\\s+)override)(\\s+)((\\w+|\\d+)|(\\w+|\\d+)\\?)(\\s+)(\\w+|\\d+)(\\s+|(\r\n|\r|\n))");

        Regex _IsEnum = new Regex("public(\\s+)enum(\\s+)(\\w+|\\d+)");

        Regex _EnumValues = new Regex("(\\w+|\\d+)(\\s+)\\=(\\s+)(\\d+)");

        Regex _IsSwitchCase = new Regex("switch(\\s+)\\((\\w+|\\d+)\\)");

        Regex _CaseValues = new Regex("case(\\s+)((\\w+|\\d+)|(\\w+|\\d+)\\.(\\w+|\\d+)):");

        public List<string[]> GetProperties(string code, out bool IsClass)
        {
            code = code.Replace(" override ", " ");
            code = code.Replace(" virtual ", " ");
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

        public List<string[]> GetEnumValues(string code, out bool IsEnum)
        {
            IsEnum = _IsEnum.IsMatch(code);
            if (IsEnum == false)
            {
                return null;
            }

            var _matches = _EnumValues.Matches(code);

            if (_matches.Count == 0)
            {
                return null;
            }
            if (_matches[0].Groups.Count < 2)
            {
                return null;
            }

            List<string[]> _result = new List<string[]>();

            foreach (Match match in _matches)
            {
                string _name = match.Groups[1].Value;
                string _value = match.Groups[4].Value;
                _result.Add(new string[] { _name, _value });
            }

            return _result;
        }

        public List<string> GetSwitchCaseValues(string code, out bool isCase)
        {
            isCase = _IsSwitchCase.IsMatch(code);
            if (isCase == false)
            {
                return null;
            }

            var _matches = _CaseValues.Matches(code);

            if (_matches.Count == 0)
            {
                return null;
            }
            if (_matches[0].Groups.Count < 2)
            {
                return null;
            }

            List<string> _result = new List<string>();

            foreach (Match match in _matches)
            {
                string _value = match.Groups[2].Value;
                _result.Add(_value);
            }

            return _result;
        }

        public List<string[]> GetConnectionStringValues(string code, out bool IsConnectionString)
        {
            List<string[]> _result = new List<string[]>();

            string[] _items = code.Split(';').Where(p => !string.IsNullOrEmpty(p)).ToArray();
            foreach (string _item in _items)
            {
                string[] _keyValue = SplitFirst(_item, '=');
                if (_keyValue == null)
                {
                    IsConnectionString = false;
                    return null;
                }
                _result.Add(new string[] { _keyValue[0], _keyValue[1] });
            }
            IsConnectionString = true;
            return _result;
        }

        private string[] SplitFirst(string text, char splitter)
        {
            int _index = text.IndexOf(splitter);
            if (_index == -1)
            {
                return null;
            }
            return new string[] { text.Substring(0, _index), text.Substring(_index + 1, text.Length - _index - 1) };
        }

    }
}
