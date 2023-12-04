using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace TestProject5
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestCaseInsensitiveComparer()
        {
            Version clrVersion = Environment.Version;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("cz-CS");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("cz-CS");
            
            var strings = new string[] {"McHale", "mchale", "MCHALE", "mChale", "mCHale", "MCHale", "mcHale" };

            var eq1 = strings.Where(s => s.Equals("mchale", StringComparison.CurrentCultureIgnoreCase));
            var expected1 = new string[] { "mchale", "MCHALE", "mChale", "mCHale", "MCHale"};
            Assert.That(eq1.SequenceEqual(expected1));

            var eq2 = strings.Where(s => s.Equals("McHale", StringComparison.CurrentCultureIgnoreCase));
            var expected2 = new string[] { "McHale", "mcHale" };
            Assert.That(eq2.SequenceEqual(expected2));
        }

        [Test]
        public void MyTest()
        {
            string a = "McHale";
            Assert.IsTrue(a.Contains("ch"));

            string b = "mchale";
            Assert.IsTrue(b.Contains("ch"));
        }
        
        [Test]
        public void MyTest1()
        {

            string a = "McHale";
            var encoding = Encoding.ASCII;
            var arrayA = a.ToCharArray();
            var indexA = a.IndexOf("ch", StringComparison.CurrentCultureIgnoreCase);
            Assert.IsFalse(a.IndexOf("ch", StringComparison.CurrentCultureIgnoreCase) >= 0);
            Assert.AreEqual(6, a.Length);

            string b = "mchale";
            var indexB = b.IndexOf("ch", StringComparison.CurrentCultureIgnoreCase);
            var arrayB = b.ToCharArray();
            byte[] byteArray = encoding.GetBytes(b);
            Assert.IsTrue(b.IndexOf("ch", StringComparison.CurrentCultureIgnoreCase) >= 0);
            Assert.AreEqual(5, a.Length);
        }

        [Test]
        public void MyTest2()
        {
            List<string> myList = new List<string> { "Čas revize", "Datum kalibrace", "Chybové kódy", "óóó", "ČČČ", "aaa", "ééé", "èèè", "êêê", "fff", "Code" };
            CultureInfo culture = new CultureInfo ("cs-CZ");
            var result = myList.OrderBy (x => x, StringComparer.Create (culture, true)).ToList ();
            
             var anotherResult = myList.OrderBy (j => j).ToList ();
        }
        
        public static Encoding DetectEncoding(String fileName, out String contents)
        {
            // open the file with the stream-reader:
            using (StreamReader reader = new StreamReader(fileName, true))
            {
                // read the contents of the file into a string
                contents = reader.ReadToEnd();

                // return the encoding.
                return reader.CurrentEncoding;
            }
        }
    }
}