using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace $namespace-prefix$$safeprojectname$
{
    public class SampleTests : RevitTest
    {
        [Test]
        [Property("TestRevitDocument", "Test Model.rvt")]
        [Property("LeaveRevitDocumentOpen", 0)]
        public void TestDocumentOpened()
        {
            Assert.IsNotNull(this.TestDocument);
        }

        [Test]
        public void TestDocumentNotOpened()
        {
            Assert.IsNull(this.TestDocument);
        }

        [Test]
        [Property("TestRevitDocument", "Test Model.rvt")]
        [Property("LeaveRevitDocumentOpen", 0)]
        public void CheckWallTypeNames()
        {
            Assert.IsNotNull(this.TestDocument, "There is no active document");

            Autodesk.Revit.DB.FilteredElementCollector col = new Autodesk.Revit.DB.FilteredElementCollector(TestDocument).WhereElementIsElementType().OfClass(typeof(WallType));
            var wallTypes = col.ToElements().Cast<WallType>();

            string wallTypeRegex = @"\(\w\d\)\s-\s.+?\s-\s((\dHR)|(NR))";
            List<string> failingNames = new List<string>();

            foreach (var t in wallTypes)
            {
                var n = t.Name;

                if (!System.Text.RegularExpressions.Regex.IsMatch(t.Name, wallTypeRegex))
                {
                    failingNames.Add(n);
                }
            }

            if (failingNames.Count > 0)
            {
                Assert.Fail("One or more wall type names do not match the standard:" + String.Join(", ", failingNames.ToArray()));
            }
        }

        [Test]
        public void PassTest()
        {
            Assert.Pass("This will always pass");
        }

        [Test]
        public void FailTest()
        {
            Assert.Fail("This will always fail");
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void ExpectedExceptionTest()
        {
            throw new Exception();
        }
    }
}

