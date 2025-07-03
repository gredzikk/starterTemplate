# WPF Starter Template

A clean and ready-to-use WPF project template for .NET, designed to kickstart your desktop application development. It includes essential features like logging, dependency injection, and automated release packaging.

## Features

- **File Logging**: A simple file logger ([`ILogger`](starterTemplate/Logger.cs)) is set up to log application events, warnings, and errors. Logs are stored in a `logs` directory.
- **Automatic Version Display**: The application's version number is automatically retrieved from the assembly ([`AssemblyInfo.cs`](starterTemplate/AssemblyInfo.cs)).
- **Automated Release Packaging**: When you build the project in `Release` configuration, a ZIP archive of the application is automatically created in the [`Releases`](Releases/) folder.
- **.NET Template**: Easily create new projects from this template, with automatic renaming of the project and namespaces.
- **Fluent theme**: App theme is set to Microsoft's NET9.0 built-in Fluent theme known from Windows 11 OS.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later.

## Installation

1. Clone or download this repository.
2. Open a terminal or command prompt and navigate to the root directory of the `starterTemplate` project.
3. Run the following command to install the project as a new template on your machine:

    ```sh
    dotnet new install .
    ```

## Usage

Once the template is installed, you can create a new project from it.

## Configuration

### Assembly Version

The version number is managed in [`AssemblyInfo.cs`](starterTemplate/AssemblyInfo.cs). The `AssemblyVersion` attribute supports wildcards (`*`) for automatic build and revision numbers.

````csharp
// starterTemplate/AssemblyInfo.cs
[assembly: AssemblyVersion("1.0.*")]
