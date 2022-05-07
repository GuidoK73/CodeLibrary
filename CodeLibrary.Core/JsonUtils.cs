using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace CodeLibrary.Core
{
    internal static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            foreach (var i in ie)
            {
                action(i);
            }
        }
    }

    public static class JsonUtils
    {
        private const string INDENT_STRING = "    ";

        public static string JsonToCSharp(string json)
        {
            NJsonSchema.Generation.SampleJsonSchemaGenerator _sampler = new NJsonSchema.Generation.SampleJsonSchemaGenerator();

            var schema = _sampler.Generate(json);
            var schemaJson = schema.ToJson();

            var settings = new NJsonSchema.CodeGeneration.CSharp.CSharpGeneratorSettings();
            var generator = new NJsonSchema.CodeGeneration.CSharp.CSharpGenerator(schema, settings);
            var code = generator.GenerateFile();

            return code;
        }

        public static string FormatJson(string str)
        {
            var indent = 0;
            var quoted = false;
            var sb = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                var ch = str[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;

                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        sb.Append(ch);
                        break;

                    case '"':
                        sb.Append(ch);
                        bool escaped = false;
                        var index = i;
                        while (index > 0 && str[--index] == '\\')
                            escaped = !escaped;
                        if (!escaped)
                            quoted = !quoted;
                        break;

                    case ',':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;

                    case ':':
                        sb.Append(ch);
                        if (!quoted)
                            sb.Append(" ");
                        break;

                    default:
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }

        public static string ConvertJson(string inpput)
        {
            var obj = Newtonsoft.Json.Linq.JToken.Parse(inpput);

            return null;
        }

        public static bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = Newtonsoft.Json.Linq.JToken.Parse(strInput);
                    return true;
                }
                catch (Newtonsoft.Json.JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public static string ConvertXmlToJson(string xml)
        {
            // To convert an XML node contained in string xml into a JSON string   
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string jsonText = JsonConvert.SerializeXmlNode(doc);

            return jsonText;
            // To convert JSON text contained in string json into an XML node
            // XmlDocument doc = JsonConvert.DeserializeXmlNode(json);
        }

        public static string ConvertJsonToXml(string json)
        {
            string _result = string.Empty;

            using (MemoryStream mStream = new MemoryStream())
            {
                using (XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.Unicode))
                {
                    XmlDocument document = JsonConvert.DeserializeXmlNode(json);

                    try
                    {
                        // Load the XmlDocument with the XML.
                        writer.Formatting = System.Xml.Formatting.Indented;

                        // Write the XML into a formatting XmlTextWriter
                        document.WriteContentTo(writer);
                        writer.Flush();
                        mStream.Flush();

                        // Have to rewind the MemoryStream in order to read
                        // its contents.
                        mStream.Position = 0;

                        // Read MemoryStream contents into a StreamReader.
                        StreamReader sReader = new StreamReader(mStream);

                        // Extract the text from the StreamReader.
                        string formattedXml = sReader.ReadToEnd();

                        _result = formattedXml;
                    }
                    catch (XmlException)
                    {
                        // Handle the exception
                    }
                    return _result;
                }
            }
        }
    }
}