using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevToys
{
    public sealed class Url : IEnumerable<string>, IComparable, IComparable<string>, IComparable<Uri>, ICloneable, IEquatable<object>, IEquatable<string>, IEquatable<Uri>
    {
        private string _Url = string.Empty;
        private string _Base = string.Empty;

        public Url(string url) => SetUrl(url);

        public Url(Uri uri) => SetUrl(uri.ToString());

        public Dictionary<string, string> Query { get; } = new Dictionary<string, string>();

        public List<string> Path { get; } = new List<string>();

        public string PathString => string.Join("/", Path);

        public string QueryString => string.Join("&", Query.Select(p => $"{p.Key}={p.Value}"));
            
        public string Base => _Base;

      
        private void SetUrl(string url)
        {
            var _uri = new Uri(url);

            if (!string.IsNullOrEmpty(_uri.Query))
            {
                _Url = url.Replace(_uri.Query, string.Empty).TrimEnd('/');

                var _items = _uri.Query.TrimStart('?').Split(new char[] { '&' });
                foreach (string _item in _items)
                {
                    if (string.IsNullOrWhiteSpace(_item))
                        continue;

                    var _keyandValue = _item.Split('=');
                    if (_keyandValue.Length == 2)
                    {
                        if (!Query.ContainsKey(_keyandValue[0]))
                            Query.Add(_keyandValue[0], _keyandValue[1]);
                    }
                    else if (_keyandValue.Length == 1)
                    {
                        if (!Query.ContainsKey(_keyandValue[0]))
                            Query.Add(_keyandValue[0], string.Empty);
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
                    string _item = pathItem;
                    if (!string.IsNullOrWhiteSpace(_item))
                    {
                        if (_item.IndexOf("?") > -1)
                        {
                            _item = _item.Substring(0, _item.IndexOf("?"));
                        }

                        Path.Add(_item);
                    }
                }
            }
            else
            {
                _Base = _Url;
            }
        }

        private string GetUrl()
        {
            var _result = new StringBuilder(_Base);

            foreach (var path in Path)
                _result.Append($"/{path}");

            if (Query.Count == 0)
                return _result.ToString();

            _result.Append('?');

            foreach (var key in Query.Keys)
                _result.Append($"{key}={Query[key]}&");

            _result.Length--;
            return _result.ToString();
        }

        public override string ToString() => GetUrl();

        public static implicit operator Url(string url) => new Url(url);

        public static implicit operator string(Url url) => url.GetUrl();

        public static implicit operator Uri(Url url) => new Uri(url.GetUrl());

        public static implicit operator Url(Uri url) => new Url(url);

        public static bool operator !=(Url url1, Url url2) => url1.CompareTo(url2) != 0;

        public static bool operator ==(Url url1, Url url2) => url1.CompareTo(url2) == 0;

        public static bool operator !=(Url url1, Uri url2) => url1.CompareTo(url2) != 0;

        public static bool operator ==(Url url1, Uri url2) => url1.CompareTo(url2) == 0;

        public static bool operator !=(Url url1, string url2) => url1.CompareTo(url2) != 0;

        public static bool operator !=(string url1, Url url2) => url1.CompareTo(url2) != 0;

        public static bool operator ==(Url url1, string url2) => url1.CompareTo(url2) == 0;

        public static bool operator ==(string url1, Url url2) => url1.CompareTo(url2) == 0;

        public bool Equals(string url) => (CompareTo(url) == 0);

        public bool Equals(Url url) => (CompareTo(url) == 0);

        public bool Equals(Uri uri) => (CompareTo(uri) == 0);

        public int CompareTo(object obj)
        {
            if (obj == null)
                return -1;

            return CompareTo(obj.ToString());
        }

        public int CompareTo(Url url) => CompareTo(url.ToString());

        public int CompareTo(Uri uri) => CompareTo(uri.ToString());

        public int CompareTo(string url)
        {
            if (url == null)
                return -1;

            string _fullUrl = GetUrl().ToLower();
            string _url = url.ToLower();

            return (_fullUrl.CompareTo(_url));
        }

        public override int GetHashCode() => GetUrl().GetHashCode();

        public object Clone() => new Url(GetUrl());

        public IEnumerator<string> GetEnumerator()
        {
            yield return _Base;

            foreach (string path in Path)
                yield return path;

            foreach (var key in Query.Keys)
                yield return $"{key}={Query[key]}";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return _Base;

            foreach (string path in Path)
                yield return path;

            foreach (var key in Query.Keys)
                yield return $"{key}={Query[key]}";
        }
    }
}