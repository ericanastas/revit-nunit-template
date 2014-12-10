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


##Usage


![Add Project to Solution](https://bitbucket.org/repo/be7b9k/images/3149626223-add%20project%20to%20solution.png)


The template should show up when creating a new project in Visual Studio. Note that the templates are filtered by the .Net Framework selected at the top of the window. So for example to see the Revit 2015 template the target framework needs to be set to 4.5.


![New Unit Tests](https://bitbucket.org/repo/be7b9k/images/761863980-New%20Unit%20Tests.png)


![set as startup project.png](https://bitbucket.org/repo/be7b9k/images/1607490496-set%20as%20startup%20project.png)


##Creating Tests


##Running Tests






