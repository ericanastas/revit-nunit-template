using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace $namespace-prefix$$safeprojectname$
{

    /// <summary>
    /// A base class for units tests that depend on a Revit document or the Revit application. Test classes can inherit from this class. The revit application can be accessed through Application property. 
    /// Test methods can be attributed with a Property attribute which sets "TestRevitDocument" to the path of the file to open. If the results of a test need to be manualy verified, adding another property 
    /// attribute that sets "LeaveDocOpen" to 1 will keep the test documents open in the Revit application.
    /// </summary>
    [TestFixture]
    public class RevitTest
    {
        public static List<string> TEMP_DOCUMENT_PATHS = new List<string>();
        public Document TestDocument { get; private set; }
        public UIApplication Application { get; private set; }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            //Sets reference to Revit application
            this.Application = RunUnitTestsCommand.Application;
        }

        [SetUp]
        public void SetupTest()
        {
            if (TestContext.CurrentContext.Test.Properties.Contains("TestRevitDocument"))
            {
                string testDocPath = (string)TestContext.CurrentContext.Test.Properties["TestRevitDocument"];

                //Makes a temporary copy of the file to ensure that a test does not change the file
                var tempPath = MakeTempFileCopy(testDocPath);

                if (LeaveDocOpen)
                {
                    TestDocument = Application.OpenAndActivateDocument(tempPath).Document;
                }
                else
                {
                    TestDocument = Application.Application.OpenDocumentFile(tempPath);
                }
            }
        }

        /// <summary>
        /// Makes a temporary copy of a file. 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string MakeTempFileCopy(string path)
        {
            if (!System.IO.File.Exists(path)) throw new System.IO.FileNotFoundException("Could not find the specified Revit document specified by the test " + TestContext.CurrentContext.Test.FullName, path);

            int index = 0;
            string ext = System.IO.Path.GetExtension(path);
            string fileNameNoExt = System.IO.Path.GetFileNameWithoutExtension(path);
            string tempDir = System.IO.Path.GetTempPath();
            string testName = TestContext.CurrentContext.Test.Name;
            string tempPath = tempPath = System.IO.Path.Combine(tempDir, fileNameNoExt + " - " + testName + ext);

            if (System.IO.File.Exists(tempPath))
            {
                try
                {
                    //try to delete existing file
                    System.IO.File.Delete(tempPath);
                }
                catch (Exception exp)
                {
                    //find unique file name
                    while (System.IO.File.Exists(tempPath))
                    {
                        tempPath = System.IO.Path.Combine(tempDir, fileNameNoExt + " - " + testName + " (" + index.ToString() + ")" + ext);
                        index++;
                    }
                }
            }

            System.IO.File.Copy(path, tempPath);
            TEMP_DOCUMENT_PATHS.Add(tempPath);

            return tempPath;
        }

        [TearDown]
        public void TearDown()
        {
            if (TestDocument != null)
            {
                if (this.LeaveDocOpen == false)
                {
                    string tempPath = this.TestDocument.PathName;
                    this.TestDocument.Close(false);
                    System.IO.File.Delete(tempPath);
                }
               
                this.TestDocument = null;
            }
        }

        /// <summary>
        /// Determines if the opened document(s) should be closed at the end of the test
        /// </summary>
        public bool LeaveDocOpen
        {
            get
            {
                if (TestContext.CurrentContext.Test.Properties.Contains("LeaveRevitDocumentOpen"))
                {
                    int value = (int)TestContext.CurrentContext.Test.Properties["LeaveRevitDocumentOpen"];

                    if (value > 0) return true;
                    else return false;
                }
                else return false;
            }
        }
    }
}