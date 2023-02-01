using GK.Template.Attributes;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace GK.Template.Methods
{
    [Category("Casting")]
    [FormatMethod(Name = "SqlToDotNetType",
        Example = "{0:SqlToDotNetType()} {0:SqlToDotNetType} ", Aliasses = "SqlToNetType")]
    [Description("Converts a Dot Net Value type to SQL equivalent")]
   
    public sealed class MethodSqlToNetTypeConvertor : MethodBase
    {
        private Regex _regexVarchar = new Regex("varchar\\(\\d+\\)");
        private Regex _regexNVarchar = new Regex("nvarchar\\(\\d+\\)");

        public override string Apply(string value)
        {
            return netType(value);
        }

        private string netType(string type)
        {
            type = type.ToLower().Trim();

            if (Utils.IsOneOfRegex(type, _regexVarchar,  _regexNVarchar))
            {
                return "string";
            }
            if (Utils.IsOneOfStrings(type, StringComparison.OrdinalIgnoreCase, "nvarchar(max)", "nvarchar()", "nvarchar", "varchar", "varchar(max)", "text", "ntext"))
                return "string";

            if (Utils.IsOneOfStrings(type, StringComparison.Ordinal, "char", "nchar(1)", "nchar"))
                return "char";

            if (Utils.IsOneOfStrings(type, StringComparison.Ordinal, "binary"))
                return "byte[]";

            if (Utils.IsOneOfStrings(type, StringComparison.Ordinal, "bit"))
                return "bool";

            if (Utils.IsOneOfStrings(type, StringComparison.Ordinal, "tinyint"))
                return "byte";

            if (Utils.IsOneOfStrings(type, StringComparison.Ordinal, "smallint"))
                return "short";

            if (Utils.IsOneOfStrings(type, StringComparison.Ordinal, "int"))
                return "int";

            if (Utils.IsOneOfStrings(type, StringComparison.Ordinal, "real"))
                return "float";

            if (Utils.IsOneOfStrings(type, StringComparison.Ordinal, "bigint"))
                return "long";

            if (Utils.IsOneOfStrings(type, StringComparison.Ordinal, "float"))
                return "double";

            if (Utils.IsOneOfStrings(type, StringComparison.Ordinal, "decimal"))
                return "decimal";

            if (Utils.IsOneOfStrings(type, StringComparison.Ordinal, "datetime"))
                return "DateTime";

            if (Utils.IsOneOfStrings(type, StringComparison.Ordinal, "datetimeoffset"))
                return "DateTimeOffset";
            

            if (Utils.IsOneOfStrings(type, StringComparison.Ordinal, "time"))
                return "TimeSpan";

            if (Utils.IsOneOfStrings(type, StringComparison.Ordinal, "uniqueidentifier"))
                return "Guid";

            return string.Empty; // type not supported.
        }
    }
}