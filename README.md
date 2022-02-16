# MSU Dissertation Validator - G3

A piece of software for validating electronic dissertations and theses to ensure that they are in the proper format. More information on [these documents here](https://www.montana.edu/etd/etd_format.html)

Authors: Silas Almgren, Marnie Manning, Patrick O'Connor
Scrum Artifacts: https://docs.google.com/spreadsheets/d/1vMsgJqcq-de_TvijcnDFhtC3tjhCkrVmlte_-1rYX8c/edit#gid=1410066615

## Using the application

For now the in-devlopment application can be found at our [development heroku app](https://etdvalidator-dev.herokuapp.com/)

## Development  Instructions

### Source Code Projects File Structure

*Source code files (such as .cs, .js, .html, etc.) have been omitted, just the necessary project directories and files are displayed

```
.
+-- G3
|   +-- SourceCode
|   |   +-- ETDValidator
|   |   |   +-- ETDValidator.sln
|   |   |   +-- ETDValidator
|   |   |   |   +-- ETDValidator.csproj
|   |   |   |   +-- Models
|   |   |   |   +-- Controllers
|   |   |   |   +-- Views
|   |   |   |   +-- wwroot
|   +-- UnitTests
|   |   +-- ETDValidatorUnitTests
|   |   |   +-- ETDValidatorUnitTests.csproj
```

### Building the Project

#### Necessary Tools/Software

- Visual Studio IDE (available at [here](https://visualstudio.microsoft.com/downloads/))
- .NET SDK 5 (available at [here](https://dotnet.microsoft.com/en-us/download/dotnet/5.0))
- Git (<https://git-scm.com/>)
- Terminal on Mac/Linux, PowerShell on Windows

#### Get it on your machine

Clone the repo by running
`git clone git@github.com:423s22/G3.git`

#### Running

There are two ways to build and run the .NET application, with Visual Studio and with the dotnet CLI tools (which should be automatically installed upon downloading the .NET SDK)

With Visual Studio:

1. Open Visual Studio
2. Select "Open Exisiting Project"
3. Open ETDValidator.sln (at ./G3/SourceCode/ETDValidator/)
4. Click the run button and the application should build and open in your browser

With dotnet CLI:

- Still in the terminal/PowerShell after cloning the repo, navigate into the the repository with `cd G3`
- To run the application:
  - Move to the project directory: `cd SourceCode/ETDValidator/ETDValidator`
  - Then simply run the project with `dotnet run` or `dotnet run watch` to use hot reload
- To build the entire solution (will output executables for application and unit tests)
  - Move to the solution directory `cd SourceCode/ETDValidator`
  - Run the build command `dotnet build`
- To run the unit tests
  - Move to the unit tests project `cd UnitTests/ETDValidatorUnitTests`
  - Run the test command `dotnet test`. This command can also be ran from the solution directory

Additionally, the application can be ran by downloading the executable artifacts

- Download the folder `dotnet-results` from the latest successful workflow, which are viewable in the [github repository actions](https://github.com/423s22/G3/actions)
- Unzip the download folder
- In terminal/PowerShell, navigate to wherever you unzipped the download folder. Then navigate to the executable folder: `cd Release/net5.0`
- Run the executable artifact with dotnet: `dotnet ETDValidator.dll`

## Collaborating

### Cloning

The project can simply be cloned with `git clone git@github.com:423s22/G3.git`

### Branching

Always develop on a branch besides main. The ideal formatting for a branch name is `initals_changedescription`. For example if John Deer is doing some refactors, his branch would be something like `jd_refactors`. However, as long as the branch name is descriptive of what is being changed, it's all good. A new branch can be created and switched to with `git checkout -b "branch name"`

### Adding and committing during development

During development it's best commit early and often. Commits should have descriptive messages. It's also best to have a small number of changes in each commit. The flow for adding changes and committing is:

- `git add .` to add all files or `git add file_or_directory` to add specific files/folders
- `git commit -m "message"` to commit changes

### Pushing new changes

Once you're ready to push changes, make sure you push your development branch using `git push origin "branch_name"`.

After pushing changes you're ready to open a pull request.

- Go to the pull requests tab
- Select "New Pull Request"
- Select "main" as the base branch and the branch you pushed as the compare branch
- Follow the format of [this template](https://github.com/423s22/G3/blob/main/.github/pull_request_template.md) to begin describing the changes in your pull request
- Open your request and wait for it to be merged!
  - Note: in order to be merged, your changes must build successfully in the CI/CD pipeline. This pipeline can be viewed under ["Actions" in github](https://github.com/423s22/G3/actions).

### Issues

Issues should simply be tracked through the [repository issue page](https://github.com/423s22/G3/issues)
