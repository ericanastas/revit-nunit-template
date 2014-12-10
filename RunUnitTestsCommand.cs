#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Reflection;
using NUnit.Core;
using System.IO;
using System.Xml;
using System.Linq;
using NUnit.Util;
using System.Collections;
using System.Xml.Xsl;
#endregion

namespace $namespace-prefix$$safeprojectname$
{
    [Transaction(TransactionMode.Manual)]
    public class RunUnitTestsCommand : IExternalCommand
    {
        public static Document CurrentDocument { get; private set; }
        public static UIApplication Application { get; private set; }

        public static readonly string RESULTS_XML_FILENAME = "$safeprojectname$.xml";
        public static readonly string RESULTS_HTML_FILENAME = "$safeprojectname$.htm";
        public static readonly string XSL_TEMPLATE_FILENAME = "NUnitSummary.xsl";

        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            //Sets static application and document properties
            Application = commandData.Application;
            if (commandData.Application.ActiveUIDocument != null)
                CurrentDocument = commandData.Application.ActiveUIDocument.Document;
            else CurrentDocument = null;

            //Gets files paths
            string assemLoc = Assembly.GetExecutingAssembly().Location;
            string execDir = System.IO.Path.GetDirectoryName(assemLoc);

            //Creates the NUnit Test Suite
            TestPackage package = new TestPackage(assemLoc);
            package.Assemblies.Add(assemLoc);
            RemoteTestRunner remoteTestRunner = new RemoteTestRunner();
            remoteTestRunner.Load(package);
            TestResult result = remoteTestRunner.Run(new NullListener(), TestFilter.Empty, false, LoggingThreshold.Off);

            ////Runs the tests
            SimpleTestRunner runner = new SimpleTestRunner();
            runner.Load(package);
            var testResult = runner.Run(new NullListener(), TestFilter.Empty, false, LoggingThreshold.Off);

            ///Saves the xml results file
            string xmlResultsFilePath = System.IO.Path.Combine(execDir, RESULTS_XML_FILENAME);
            if (File.Exists(xmlResultsFilePath)) File.Delete(xmlResultsFilePath);
            XmlResultWriter resultWriter = new XmlResultWriter(xmlResultsFilePath);
            resultWriter.SaveTestResult(testResult);

            //Transforms the XML into HTML
            System.Xml.Xsl.XslCompiledTransform transform = new System.Xml.Xsl.XslCompiledTransform();
            string stylesheet = System.IO.Path.Combine(execDir, XSL_TEMPLATE_FILENAME);
            XsltSettings settings = new XsltSettings(true, true);
            transform.Load(stylesheet, settings, new XmlUrlResolver()); // compiled stylesheet


            string htmlFile = System.IO.Path.Combine(execDir, RESULTS_HTML_FILENAME);
            if (File.Exists(htmlFile)) File.Delete(htmlFile);

            using (FileStream resultsFs = new FileStream(xmlResultsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                XmlReader reasultsReader = XmlReader.Create(resultsFs);

                using (FileStream fs = new FileStream(htmlFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                {
                    XsltArgumentList args = new XsltArgumentList();
                    transform.Transform(reasultsReader, args, fs);
                }

            }

            //Prompts user to open results file
            Autodesk.Revit.UI.TaskDialog diag = new TaskDialog("Unit Test Results");
            if (result.IsFailure) diag.MainContent = "One or more unit tests failed.\n\nDo you want to open the test results?";
            else diag.MainContent = "All unit tests passed.\n\nDo you want to open the test results?";
            if (result.IsFailure) diag.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
            diag.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;
            diag.DefaultButton = TaskDialogResult.Yes;

            
            if (diag.Show() == TaskDialogResult.Yes) Process.Start(htmlFile);

            return Result.Succeeded;
        }
    }
}
