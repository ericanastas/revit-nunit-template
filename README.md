#Revit Unit Tests Visual Studio Template

A template for Visual Studio that creates an NUnit based unit test project which can be use to test Revit Add-ins.

## Installation

1. Open the [downloads](https://bitbucket.org/somddg/revit-unit-tests-visual-studio-template/downloads) page.

2. Find the latest download version prefixed with the desired release of Revit (_201x_.y.z)

3. Download the ZIP file, and extract it into the following folder.

Visual Studio 2013
: `%userprofile%\Documents\Visual Studio 2013\Templates\ProjectTemplates\Visual C#\`

## Git Installation

It's also possible to setup the templates as a local Git repository. This way updates to the templates can be downloaded by running a single command rather then needing to download and replace files.

1. Open Git Bash and browse to the `Visual C#` templates directory shown above.

2. Run the following command with `201x` replaced with the desired template (e.g. 2013, 2014, 2015..)  
   `git clone -b Revit201x git@bitbucket.org:somddg/revit-unit-tests-visual-studio-template.git "Revit 201x Unit Tests" `

3. Later to update to the latest version of the template open Git Bash in the same directory and run.  
   `git pull origin`

## Creating a Unit Test Project

Unit test projects are added existing Visual Studio solutions containing Revit add-in projects. To add a unit test project to a solution right click on the solution in the solution explorer.

![Add Project to Solution](./images/add%20project%20to%20solution.png)

Next select the unit test project template which matches the release of Revit currently being targeted. Note that the templates are filtered by the .Net Framework selected at the top of the window. So for example to see the Revit 2015 Unit Tests template the target framework needs to be set to 4.5.

![New Unit Tests](./images/New%20Unit%20Tests.png)

The unit test project will be added to the current solution.

![Test Project Added](./images/test%20project%20added.png)

## Creating Tests

Tests are created based on the NUnit unit testing framework. More information about this framework and creating tests can be found at the link below.

[http://www.nunit.org/](http://www.nunit.org/)

In general test are created as public parameterless methods that have no return value. Test methods are identified by adding a `[Test]` attribute, and classes which contain test methods are identified by adding a `[TestFixture]` attribute. Throughout the test various methods on the `Assert` class can be called to confirm that certain expectations are true. If any of the assert calls fail, the entire test will fail.

```
#!c#

    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestAddtion()
        {
            int a = 1;
            int b = 1;

            int result = a + b;

            Assert.AreEqual(2, result, "Adding 1 + 1 did not return 2");
        }
}
```

The current Revit application object, and active document (if any) can be accessed through the following static properties `RunUnitTestCommand.CurrentDocument` and `RunUnitTestCommand.Application`.

##Running Tests

Set the unit test project as the startup project for the solution by right clicking on it in the Solution Explorer.

![set as startup project.png](./images/set%20as%20startup%20project.png)

Click `Start` on the tool bar to run the project. Revit will launch.

If any of the tests depend on an active document, open the document to be used during the test.

From the Add-Ins tab open external tools, and click the command for the unit tests which should be run.

![run test external tools.png](./images/run%20test%20external%20tools.png)

The tests will now run. After the testing is complete a dialog will be displayed.

![test result prompt.png](./images/test%20result%20prompt.png)

Clicking `Yes` on the dialog will open a report of the test results in the default browser.

![test report.png](./images/test%20report.png)
