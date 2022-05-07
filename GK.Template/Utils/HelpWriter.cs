using GK.Template.Attributes;
using GK.Template.DataCommands;
using GK.Template.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.Template
{
    public class HelpWriter
    {
        private string BuildItemHelp(FormatMethodAttribute at)
        {
            string tmp = @"

# {0}
**{1}**

**Aliasses:**
{2}

**Description:**
{3}

**Parameters:**
{5}

**Example:**
{4}

---

";
            StringBuilder sb = new StringBuilder();
            if (at.Parameters.Count == 0)
            {
                sb.Append("- None");
            }
            foreach (FormatMethodParameterAttribute prm in at.Parameters)
            {
                sb.Append("- ");
                sb.AppendFormat("**{0}** ", prm.Name);
                sb.AppendFormat("{0}", prm.Optional == true ? "***optional***" : "");
                sb.AppendFormat("{0}", prm.IsParams == true ? "***params***" : "");
                if (!string.IsNullOrEmpty(prm.Description))
                    sb.AppendFormat("{0}", prm.Description);
                sb.Append("\r\n");

                if (prm.PropertyReference.PropertyType.BaseType == typeof(Enum))
                {


                    foreach (int val in EnumUtility.GetValues(prm.PropertyReference.PropertyType))
                    {
                        string name = Enum.GetName(prm.PropertyReference.PropertyType, val);
                        string description = EnumUtility.GetDescription(EnumUtility.GetValueByName(prm.PropertyReference.PropertyType, name));
                        sb.AppendFormat("- **{0}:** {1}\r\n", name, description);
                    }
                }
            }

            return string.Format(tmp, at.Name, at.ToString(), at.Aliasses, at.Description, at.Example, sb.ToString());
        }

        private string BuildItemHelp(DataCommandAttribute at, StringBuilder sb)
        {
            string tmp = @"
<html>
    <head>
    <style type=""text/css"">
{0}
    </style>
    </head>

    <body>
<h1>{1}</h1>
<h2>{2}</h2>
<h3>Aliasses</h3>
{3}
<h3>Description</h3>
{4}
<h3>Example</h3>
{5}
<h3>Parameters</h3>
<ul>
{6}
</ul>
    </body>
</html>
";

            foreach (DataCommandParameterAttribute prm in at.Parameters)
            {
                sb.Append("<li>");
                sb.AppendFormat("{0} ", prm.Name);

                sb.AppendFormat("{0}", prm.IsParams == true ? "<i>params</i>" : "");
                if (!string.IsNullOrEmpty(prm.Description))
                    sb.AppendFormat("<br/>{0}", prm.Description);
                sb.Append("</li>");
            }

            return string.Format(tmp, "", at.Name, at.ToString(), at.Aliasses.Replace("\r\n", "<br/>"), at.Description.Replace("\r\n", "<br/>"), at.Example.Replace("\r\n", "<br/>"), sb.ToString());
        }

        public void ListCommands(StringBuilder sb)
        {
            List<Type> methodtTypes = Utils.GetObjectsWithBaseType(typeof(DataCommandBase), true);
            List<DataCommandAttribute> attribs = new List<DataCommandAttribute>();
            foreach (Type t in methodtTypes)
            {
                DataCommandAttribute att = DataCommandAttribute.GetAttribute(t);
                if (!string.IsNullOrEmpty(att.Name))
                {
                    attribs.Add(att);
                }
            }
            attribs = (from t in attribs
                       orderby t.Name ascending
                       select t).ToList();

            foreach (DataCommandAttribute att in attribs)
            {
                BuildItemHelp(att, sb);
            }
        }

        public void ListFormatters(StringBuilder sb)
        {
            List<Type> methodtTypes = Utils.GetObjectsWithBaseType(typeof(MethodBase), true);
            List<FormatMethodAttribute> attribs = new List<FormatMethodAttribute>();
            foreach (Type t in methodtTypes)
            {
                FormatMethodAttribute att = FormatMethodAttribute.GetAttribute(t);
                if (!string.IsNullOrEmpty(att.Name))
                {
                    attribs.Add(att);
                }
            }
            attribs = (from t in attribs
                       orderby t.Name ascending
                       select t).ToList();

            foreach (FormatMethodAttribute att in attribs)
            {
                sb.Append(BuildItemHelp(att));
            }
        }
    }
}