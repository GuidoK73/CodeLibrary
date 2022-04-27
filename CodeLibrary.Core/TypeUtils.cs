using System;
using System.Globalization;

namespace CodeLibrary.Core
{
    public enum NetType
    {
        Unknown = 0,
        String = 1,
        Guid = 2,
        Boolean = 3,
        DateTime = 4,
        DateTimeOffset = 5,
        TimeSpan = 6,
        Byte = 7,
        SByte = 8,
        Int16 = 9,
        Int32 = 10,
        Int64 = 11,
        Single = 12,
        Decimal = 13,
        Double = 14,
        UInt16 = 15,
        UInt32 = 16,
        UInt64 = 17,
        Enum = 18,
        Null = 99
    }

    public static class TypeUtils
    {
        public static bool IsNumericType(Type type) => IsNumericType(GetNetType(type));

        public static bool IsNumericType(NetType netType)
        {
            switch (netType)
            {
                case NetType.Byte:
                case NetType.SByte:
                case NetType.Int16:
                case NetType.Int32:
                case NetType.Int64:
                case NetType.Single:
                case NetType.Decimal:
                case NetType.Double:
                case NetType.UInt16:
                case NetType.UInt32:
                case NetType.UInt64:
                    return true;
            }
            return false;
        }

        public static int GetNumericTypeLevel(NetType netType)
        {
            switch (netType)
            {
                case NetType.Byte:
                    return 1;

                case NetType.SByte:
                    return 2;

                case NetType.Int16:
                    return 3;

                case NetType.Int32:
                    return 4;

                case NetType.Int64:
                    return 5;

                case NetType.Single:
                    return 6;

                case NetType.Decimal:
                    return 7;

                case NetType.Double:
                    return 8;

                case NetType.UInt16:
                    return 9;

                case NetType.UInt32:
                    return 10;

                case NetType.UInt64:
                    return 11;
            }
            return 0;
        }

        public static Type GetType(NetType netType)
        {
            switch (netType)
            {
                case NetType.Unknown:
                    return typeof(object);

                case NetType.String:
                    return typeof(String);

                case NetType.Guid:
                    return typeof(Guid);

                case NetType.Boolean:
                    return typeof(Boolean);

                case NetType.DateTime:
                    return typeof(DateTime);

                case NetType.DateTimeOffset:
                    return typeof(DateTimeOffset);

                case NetType.TimeSpan:
                    return typeof(TimeSpan);

                case NetType.Byte:
                    return typeof(Byte);

                case NetType.SByte:
                    return typeof(SByte);

                case NetType.Int16:
                    return typeof(Int16);

                case NetType.Int32:
                    return typeof(Int32);

                case NetType.Int64:
                    return typeof(Int64);

                case NetType.Single:
                    return typeof(Single);

                case NetType.Decimal:
                    return typeof(Decimal);

                case NetType.Double:
                    return typeof(Double);

                case NetType.UInt16:
                    return typeof(UInt16);

                case NetType.UInt32:
                    return typeof(UInt32);

                case NetType.UInt64:
                    return typeof(UInt64);

                case NetType.Enum:
                    return typeof(Enum);
            }
            return typeof(object);
        }

        public static NetType GetNetType(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type.IsEnum)
                return NetType.Enum;

            if (type == typeof(String))
                return NetType.String; // Alternative: char[], char, NText, NChar, NText, NVarChar, Varchar

            if (type == typeof(Guid))
                return NetType.Guid;

            if (type == typeof(Boolean))
                return NetType.Boolean;

            if (type == typeof(DateTime))
                return NetType.DateTime; // Alternative: SmallDateTime or DateTime2

            if (type == typeof(DateTimeOffset))
                return NetType.DateTimeOffset;

            if (type == typeof(TimeSpan))
                return NetType.TimeSpan;

            if (type == typeof(Byte))
                return NetType.Byte;

            if (type == typeof(SByte))
                return NetType.SByte;

            if (type == typeof(Int16))
                return NetType.Int16;

            if (type == typeof(Int32))
                return NetType.Int32;

            if (type == typeof(Int64))
                return NetType.Int64;

            if (type == typeof(UInt16))
                return NetType.UInt16; // one larger to fit unsigned maximum

            if (type == typeof(UInt32))
                return NetType.UInt32; // one larger to fit unsigned maximum

            if (type == typeof(UInt64))
                return NetType.UInt64; // one larger to fit unsigned maximum

            if (type == typeof(Single))
                return NetType.Single;

            if (type == typeof(Decimal))
                return NetType.Decimal; // Alternative: Money, SmallMoney

            if (type == typeof(Double))
                return NetType.Double;

            return NetType.String;
        }

        /// <summary>
        /// For use in array of string values, it can be used to detect the best fitting type for a string value.
        /// </summary>
        public static NetType BestNetType(NetType currentType, string newValue)
        {
            object _value = TypeUtils.GetTypedValue(newValue);
            NetType _newType = TypeUtils.GetNetType(_value.GetType());
            if (string.IsNullOrEmpty(newValue))
            {
                return NetType.Null;
            }
            if (currentType == NetType.Unknown)
            {
                return _newType;
            }
            if (TypeUtils.IsNumericType(currentType) && TypeUtils.IsNumericType(_newType))
            {
                int _newTypeLevel = TypeUtils.GetNumericTypeLevel(_newType);
                int _currentTypeLevel = TypeUtils.GetNumericTypeLevel(currentType);
                if (_newTypeLevel >= _currentTypeLevel)
                {
                    return _newType;
                }
                return currentType;
            }
            if (_newType != currentType && currentType == NetType.Null)
            {
                return _newType;
            }
            if (_newType != currentType && _newType == NetType.Null)
            {
                return currentType;
            }


            if (_newType != currentType)
            {
                return NetType.String;
            }

            return currentType;
        }

        /// <summary>
        /// Returns a type value for a given string value.
        /// </summary>
        public static object GetTypedValue(string value)
        {      
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            CultureInfo _culture = CultureInfo.InvariantCulture;

            bool _succes = DateTime.TryParse(value.Trim(), _culture, DateTimeStyles.None, out DateTime _date);
            if (_succes)
            {
                return _date;
            }
            _succes = Guid.TryParse(value.Trim(), out Guid _guid);
            if (_succes)
            {
                return _guid;
            }
            _succes = Boolean.TryParse(value.Trim(), out bool _boolean);
            if (_succes)
            {
                return _boolean;
            }
            _succes = Byte.TryParse(value.Trim(), out byte _byte);
            if (_succes)
            {
                return _byte;
            }
            _succes = Single.TryParse(value.Trim(), out Single _single);
            if (_succes)
            {
                return _single;
            }
            _succes = Int16.TryParse(value.Trim(), out Int16 _int16); // short
            if (_succes)
            {
                return _int16;
            }
            _succes = Int32.TryParse(value.Trim(), out Int32 _int32); // int
            if (_succes)
            {
                return _int32;
            }
            _succes = Int64.TryParse(value.Trim(), out Int64 _int64); // long
            if (_succes)
            {
                return _int64;
            }
            _succes = Double.TryParse(value.Trim(), out Double _double); // long
            if (_succes)
            {
                return _double;
            }
            _succes = Decimal.TryParse(value.Trim(), NumberStyles.Any, _culture, out Decimal _decimal);
            if (_succes)
            {
                return _decimal;
            }
            _succes = TimeSpan.TryParse(value.Trim(), out TimeSpan _timespan);
            if (_succes)
            {
                return _timespan;
            }

            return value;
        }


        public static object GetTypedValue(string value, NetType nettype)
        {
            CultureInfo _culture = CultureInfo.InvariantCulture;
            bool _succes;
            switch (nettype)
            {
                case NetType.DateTime:
                    _succes = DateTime.TryParse(value.Trim(), _culture, DateTimeStyles.None, out DateTime _date);
                    if (_succes)
                    {
                        return _date;
                    }
                    if (string.IsNullOrEmpty(value))
                    {
                        return null;
                    }
                    break;
                case NetType.Guid:
                    _succes = Guid.TryParse(value.Trim(), out Guid _guid);
                    if (_succes)
                    {
                        return _guid;
                    }
                    if (string.IsNullOrEmpty(value))
                    {
                        return null;
                    }
                    break;
                case NetType.Boolean:
                    _succes = Boolean.TryParse(value.Trim(), out bool _boolean);
                    if (_succes)
                    {
                        return _boolean;
                    }
                    if (string.IsNullOrEmpty(value))
                    {
                        return null;
                    }
                    break;
                case NetType.Byte:
                    _succes = Byte.TryParse(value.Trim(), out byte _byte);
                    if (_succes)
                    {
                        return _byte;
                    }
                    if (string.IsNullOrEmpty(value))
                    {
                        return null;
                    }
                    break;
                case NetType.Single:
                    _succes = Single.TryParse(value.Trim(), out Single _single);
                    if (_succes)
                    {
                        return _single;
                    }
                    if (string.IsNullOrEmpty(value))
                    {
                        return null;
                    }
                    break;
                case NetType.Int16:
                    _succes = Int16.TryParse(value.Trim(), out Int16 _int16); // short
                    if (_succes)
                    {
                        return _int16;
                    }
                    if (string.IsNullOrEmpty(value))
                    {
                        return null;
                    }
                    break;
                case NetType.Int32:
                    _succes = Int32.TryParse(value.Trim(), out Int32 _int32); // int
                    if (_succes)
                    {
                        return _int32;
                    }
                    if (string.IsNullOrEmpty(value))
                    {
                        return null;
                    }
                    break;
                case NetType.Int64:
                    _succes = Int64.TryParse(value.Trim(), out Int64 _int64); // long
                    if (_succes)
                    {
                        return _int64;
                    }
                    if (string.IsNullOrEmpty(value))
                    {
                        return null;
                    }
                    break;
                case NetType.Double:
                    _succes = Double.TryParse(value.Trim(), out Double _double); // long
                    if (_succes)
                    {
                        return _double;
                    }
                    if (string.IsNullOrEmpty(value))
                    {
                        return null;
                    }
                    break;
                case NetType.Decimal:
                    _succes = Decimal.TryParse(value.Trim(), NumberStyles.Any, _culture, out Decimal _decimal);
                    if (_succes)
                    {
                        return _decimal;
                    }
                    if (string.IsNullOrEmpty(value))
                    {
                        return null;
                    }
                    break;
                case NetType.TimeSpan:
                    _succes = TimeSpan.TryParse(value.Trim(), out TimeSpan _timespan);
                    if (_succes)
                    {
                        return _timespan;
                    }
                    if (string.IsNullOrEmpty(value))
                    {
                        return null;
                    }
                    break;
                case NetType.String:
                    if (string.IsNullOrEmpty(value))
                    {
                        return null;
                    }
                    return value;

            }
            return value;
        }


        public static string CSharpTypeConstructorCode(object value, NetType netType)
        {          
            if (value == null)
            {
                return "null";
            }
            switch (netType)
            {
                case NetType.DateTimeOffset:
                case NetType.DateTime:
                    DateTime _datetime = (DateTime)value;
                    if (_datetime.Hour == 0 && _datetime.Minute == 0 && _datetime.Second == 0)
                    {
                        return $"new DateTime({_datetime.Year},{_datetime.Month}, {_datetime.Day})";
                    }
                    if (_datetime.Second == 0)
                    {
                        return $"new DateTime({_datetime.Year},{_datetime.Month}, {_datetime.Day}, {_datetime.Hour}, {_datetime.Minute} )";
                    }
                    return $"new DateTime({_datetime.Year},{_datetime.Month}, {_datetime.Day}, {_datetime.Hour}, {_datetime.Minute}, {_datetime.Second} )";
                case NetType.String:
                    return $"\"{ value.ToString().Replace("\"", "\\\"") }\"";
                case NetType.Boolean:
                    return value.ToString().ToLower();
                case NetType.TimeSpan:
                    TimeSpan _ts = (TimeSpan)value;
                    return $"new TimeSpan({_ts.Days},{_ts.Hours},{_ts.Minutes},{_ts.Seconds},{_ts.Milliseconds} )";
                case NetType.Int16:
                case NetType.Int32:
                case NetType.Int64:
                case NetType.Byte:
                    return value.ToString();
                case NetType.Decimal:
                    decimal _decimal = (decimal)value;
                    return _decimal.ToString(CultureInfo.InvariantCulture);
                case NetType.Double:
                    double _double = (double)value;
                    return _double.ToString(CultureInfo.InvariantCulture);
                case NetType.Single:
                    Single _single = (Single)value;
                    return _single.ToString(CultureInfo.InvariantCulture);
                case NetType.Guid:
                    Guid _guid = (Guid)value;
                    return $"Guid.Parse(\"{_guid}\")";
            }

            return value.ToString();            
        }

    }
}