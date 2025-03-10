using CodeLibrary.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void JsonTest()
        {

            string _text = "4464.26000000";

            object _typedvalue = TypeUtils.GetTypedValue(_text);

            //HelpWriter helpWriter = new HelpWriter();
            //StringBuilder sb = new StringBuilder();
            //helpWriter.ListFormatters(sb);


        }



    }
}
