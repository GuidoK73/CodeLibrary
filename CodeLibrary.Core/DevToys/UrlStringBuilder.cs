using System;
using System.Collections.Generic;
using System.Text;

namespace DevToys
{
    public class UrlStringBuilder
    {
        public Dictionary<string, string> Query { get; } = new Dictionary<string, string>();
        public List<string> Path { get; } = new List<string>();
        private string _Url;
        private string _Base;

        public string BaseUrl
        {
            get
            {
                return _Base;
            }
            set
            {
                _Base = value;
            }
        }

        public string PathString() => string.Join("/", Path);

        public UrlStringBuilder(string url)
        {
            var _uri = new Uri(url);

            if (!string.IsNullOrEmpty(_uri.Query))
            {
                _Url = url.Replace(_uri.Query, string.Empty).TrimEnd('/');

                var _items = _uri.Query.TrimStart('?').Split(new char[] { '&' });
                foreach (string _item in _items)
                {
                    if (string.IsNullOrWhiteSpace(_item))
                    {
                        continue;
                    }
                    var _keyandValue = _item.Split('=');
                    if (_keyandValue.Length == 2)
                    {
                        if (!Query.ContainsKey(_keyandValue[0]))
                        {
                            Query.Add(_keyandValue[0], _keyandValue[1]);
                        }
                    }
                    else if (_keyandValue.Length == 1)
                    {
                        if (!Query.ContainsKey(_keyandValue[0]))
                        {
                            Query.Add(_keyandValue[0], string.Empty);
                        }
                    }
                }
            }
            else
            {
                _Url = url.TrimEnd('/');
            }

            int _index = _Url.IndexOf("//", 0);
            _index = _Url.IndexOf("/", _index + 2);
            if (_index > -1)
            {
                _Base = _Url.Substring(0, _index);
                var _pathString = _Url.Substring(_index, _Url.Length - _index).Trim('/');
                var _pathItems = _pathString.Split('/');
                foreach (var pathItem in _pathItems)
                {
                    if (!string.IsNullOrWhiteSpace(pathItem))
                    {
                        Path.Add(pathItem);
                    }
                }
            }
            else
            {
                _Base = _Url;
            }
        }

        public override string ToString()
        {
            var _result = new StringBuilder(_Base);

            foreach (var path in Path)
            {
                _result.Append($"/{path}");
            }

            if (Query.Count == 0)
            {
                return _result.ToString();
            }

            _result.Append('?');

            foreach (var key in Query.Keys)
            {
                _result.Append($"{key}={Query[key]}&");
            }
            _result.Length--;
            return _result.ToString();
        }
    }
}
