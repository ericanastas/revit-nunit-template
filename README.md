#Revit  Unit Tests Visual Studio Template

A template for Visual Studio that creates an NUnit based unit test project which can be use to test Revit Add-ins.

##Installation

1. Open the [downloads](https://bitbucket.org/somddg/revit-unit-tests-visual-studio-template/downloads) page.

2. Find the latest download version prefixed with the desired release of Revit (*201x*.y.z) 

3. Download the ZIP file, and extract it into the following folder.

Visual Studio 2013
: `%userprofile%\Documents\Visual Studio 2013\Templates\ProjectTemplates\Visual C#\`

##Git Installation

It's also possible to setup the templates as a local Git repository. This way updates to the templates can be downloaded by running a single command rather then needing to download and replace files.

1. Open Git Bash and browse to the `Visual C#` templates directory shown above.

2. Run the following command with `201x` replaced with the desired template (e.g. 2013, 2014, 2015..)  
`git clone -b Revit201x git@bitbucket.org:somddg/revit-unit-tests-visual-studio-template.git "Revit 201x Unit Tests" `  

3. Later to update to the latest version of the template open Git Bash in the same directory and run.  
`git pull origin`  


##Creating a Unit Test Project

Unit test projects are added to Visual Studio solutions containing existing Revit add-in projects. To add a unit test project to a solution right click on the solution in the solution explorer.

![Add Project to Solution](https://bitbucket.org/repo/be7b9k/images/3149626223-add%20project%20to%20solution.png)

Next select the unit test project template which matches the release of Revit currently being targeted. Note that the templates are filtered by the .Net Framework selected at the top of the window. So for example to see the Revit 2015 template the target framework needs to be set to 4.5.

![New Unit Tests](https://bitbucket.org/repo/be7b9k/images/761863980-New%20Unit%20Tests.png)

The unit test project will be added to the current solution.

![Test Project Added](https://bitbucket.org/repo/be7b9k/images/3282741994-test%20project%20added.png)


##Creating Tests

Tests are created based n

[http://www.nunit.org/](http://www.nunit.org/)




##Running Tests

Set the unit test project as the startup project for the solution by right clicking on it in the Solution Exporer. 

![set as startup project.png](https://bitbucket.org/repo/be7b9k/images/1607490496-set%20as%20startup%20project.png)

Click `Start` on the tool bar to run the project. Revit will launch. If any of the tests depend on an active document, open the document to be used. 

From the Add-Ins tab open external tools, and click the 

![run test external tools.png](https://bitbucket.org/repo/be7b9k/images/1847103855-run%20test%20external%20tools.png)

The tests will now run. After the testing is complete a dialog will be displayed.

![test result prompt.png](https://bitbucket.org/repo/be7b9k/images/763489837-test%20result%20prompt.png)

Clicking `Yes` on the dialog will open a report of the test results in the default browser.

![test report.png](https://bitbucket.org/repo/be7b9k/images/992465089-test%20report.png)


