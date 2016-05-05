using Autodesk.Revit.UI;
using NUnit.Framework;
using System;

namespace $namespace-prefix$$safeprojectname$
{

     /// <summary>
    /// A base class for units tests that depend on a Revit document or the Revit application. Test classes can 
    /// inherit from this class. The revit application can be accessed through the static RevitTest.Application property. 
    /// 
    /// The methods of a TextFixture or a TextFixture itself can be attributed with a Property attribute which sets "TestRVTDoc" 
    /// to the path of the file to open which can be access through RevitTest.TestDocument.
    /// 
    /// Adding the attribute to an entire fixture will leave the same TestDocument open for all the tests of the test fixture.
    /// On the other other hand attributing each induvidual test method will open fresh copies of the document per each test.
    /// </summary>
    
    [TestFixture]
    public abstract class RevitTest
    {
        public static string TEST_RVT_PROP = "TestRVTDoc";

        /// <summary>
        /// The current 
        /// </summary>
        public Autodesk.Revit.DB.Document TestDocument { get; private set; }
        public UIApplication Application { get; private set; }

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            //Sets reference to Revit application
            this.Application = RunUnitTestsCommand.Application;

            if (TestContext.CurrentContext.Test.Properties.Contains(TEST_RVT_PROP))
            {
                var testRvt = (string)TestContext.CurrentContext.Test.Properties[TEST_RVT_PROP];

                //Always open a fresh copy with a next test fixture
                OpenTestDocument(testRvt);
            }
        }

        /// <summary>
        /// Run before each test
        /// </summary>
        [SetUp]
        public void Setup()
        {
            //Check if current test requires a test document
            if (TestContext.CurrentContext.Test.Properties.Contains(TEST_RVT_PROP))
            {
                string testRvProp = (string)TestContext.CurrentContext.Test.Properties[TEST_RVT_PROP];
                OpenTestDocument(testRvProp);
            }
        }


        [TearDown]
        public void TearDown()
        {
            //Don't do anything after a test
            //No way to know if the next test will use the existing document
        }


        [TestFixtureTearDown]
        public void TearDownFixture()
        {
            //Check if model is currently open
            if (TestDocument != null) CloseTestDocument();


            //Deletes the temp directory
            string tempDir = GetTempDirectory();
            System.IO.Directory.Delete(tempDir, true);
        }



        /// <summary>
        /// Closes any existing temp document an opens a new one
        /// </summary>
        /// <param name="filePath"></param>
        private void OpenTestDocument(string filePath)
        {
            if (TestDocument != null) CloseTestDocument();

            //Makes a temporary copy of the file to ensure that a test does not change the file
            var tempPath = MakeTempFileCopy(filePath);

            //Opens the TestDocument
            TestDocument = Application.Application.OpenDocumentFile(tempPath);
        }

        /// <summary>
        /// Closes the opened test document and deletes any temporary files
        /// </summary>
        private void CloseTestDocument()
        {
            if (this.TestDocument != null)
            {
                string docPath = this.TestDocument.PathName;
                this.TestDocument.Close(false);
                System.IO.File.Delete(docPath);

                var ext = System.IO.Path.GetExtension(docPath);
                var backupFolderPath = docPath.Substring(0, docPath.Length - ext.Length) + "_backup";

                if (System.IO.Directory.Exists(backupFolderPath)) System.IO.Directory.Delete(backupFolderPath, true);
            }
        }

        /// <summary>
        /// Returns a directory to store temporary files for the current assembly
        /// </summary>
        /// <returns></returns>
        public static string GetTempDirectory()
        {
            var assemName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
            string assemblyName = assemName.Name;
            string tempDir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), assemblyName);
            return tempDir;
        }


        /// <summary>
        /// Makes a temporary copy of a file and returns the path to the file
        /// </summary>
        /// <remarks></remarks>
        /// <param name="path"></param>
        /// <returns>The file path to the temp copy of the file</returns>
        public static string MakeTempFileCopy(string path)
        {
            if (!System.IO.File.Exists(path)) throw new System.IO.FileNotFoundException("Could not find the file while running test: " + TestContext.CurrentContext.Test.Name, path);


            //Creates the temp dir if it does not exist
            string tempDir = GetTempDirectory();
            if (!System.IO.Directory.Exists(tempDir)) System.IO.Directory.CreateDirectory(tempDir);

            //Creates a temporary copy of the file
            //If existing file are found they are attempted to be deleted
            //if the existing temp files can not be deleted new files are created with incremented numbers
            string ext = System.IO.Path.GetExtension(path);
            string fileNameNoExt = System.IO.Path.GetFileNameWithoutExtension(path);
            int index = 0;
            string tempPath = null;

            do
            {
                //Create the temp file name
                if (index == 0) tempPath = tempPath = System.IO.Path.Combine(tempDir, fileNameNoExt + ext);
                else System.IO.Path.Combine(tempDir, fileNameNoExt + " (" + index.ToString() + ")" + ext);

                if (System.IO.File.Exists(tempPath))
                {
                    //Attempts to delete the existing file
                    try
                    {
                        System.IO.File.Delete(tempPath);
                    }
                    catch (Exception exp)
                    {
                        index++;
                    }
                }
            }
            while (System.IO.File.Exists(tempPath));

            //Coppies the file to the temporary location
            System.IO.File.Copy(path, tempPath);

            return tempPath;
        }


    }
}