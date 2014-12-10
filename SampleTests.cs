using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace $namespace-prefix$$safeprojectname$
{
    [TestFixture]
    [Description("Sample Tests")]
    public class SampleTests
    {
	
		public Document CurrentDocument { get; set; }
        public UIApplication Application { get; set; }
	
        [TestFixtureSetUp]
        public void Setup()
        {
		    this.CurrentDocument = RunUnitTestsCommand.CurrentDocument;
            this.Application = RunUnitTestsCommand.Application;
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
			
        }
	
	
        [Test]
        [Category("Walls")]
        [Description("Check wall type names follow standard format")]
        public void CheckWallTypeNames()
        {

            Assert.IsNotNull(CurrentDocument,"There is no active document");

            Autodesk.Revit.DB.FilteredElementCollector col = new Autodesk.Revit.DB.FilteredElementCollector(CurrentDocument).WhereElementIsElementType().OfClass(typeof(WallType));
            var wallTypes = col.ToElements().Cast<WallType>();

            string wallTypeRegex = @"\(\w\d\)\s-\s.+?\s-\s((\dHR)|(NR))";
            List<string> failingNames = new List<string>();

            foreach (var t in wallTypes)
            {
                var n = t.Name;

                if(!System.Text.RegularExpressions.Regex.IsMatch(t.Name,wallTypeRegex))
                {
                    failingNames.Add(n);
                }
            }

            if(failingNames.Count>0)
            {
                Assert.Fail("One or more wall type names do not match the standard:"+String.Join(", ",failingNames.ToArray()));
            }
        }

        [Test]
        [Category("Sample Tests")]
        [Description("Always Pass")]
        public void PassTest()
        {
            Assert.Pass("This will always pass");
        }

        [Test]
        [Category("Sample Tests")]
        [Description("Always Fail")]
        public void FailTest()
        {
            Assert.Fail("This will always fail");
        }

        [Test]
        [Category("Sample Tests")]
        [Description("Test if a document is currently opened")]
        public void DocumentIsOpen()
        {

            if (RunUnitTestsCommand.CurrentDocument != null)
            {
                Assert.Pass("A document is opened in Revit");
            }
            else
            {
                Assert.Fail("A document is not opened in Revit");
            }
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        [Category("Sample Tests")]
        [Description("Shows the use of the ExcpectedException attribute")]
        public void ExceptionTest()
        {
            throw new Exception();
        }
    }
}
