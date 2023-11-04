using System.CommandLine;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Industrious.Starter;

internal static class Program
{
	// You'll want to change these if you aren't me; should they be env. vars.?
	private const String CompanyName = "Industrious One";
	private const String CompanyId = "com.industriousone";

	// Name of file used to store last-run configuration about the solution
	private const String ConfigFileName = "Starter.config.json";

	// All the ingredients for an Industrious-worthy solution
	private static readonly Action<Configuration>[] Versions = {
		CreateSupportFiles,
		CreateSolution,
		CreateMacOsProject
	};

	///////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	///   Program entry point.
	/// </summary>
	///////////////////////////////////////////////////////////////////////////////////
	public static Int32 Main (String[] args)
	{
		// Command arguments; reused across commands
		var slnNameArgument = new Argument<String> ("sln_name", "name of the solution to be generated");

		// Command options; reused across commands
		var titleOption = new Option<String?> (new[] { "-t", "--title" }, "application title; defaults to solution name");

		// New solution command
		var newCommand = new Command ("new", "Generate a new solution") {
			slnNameArgument,
			titleOption
		};

		newCommand.SetHandler (OnNew, slnNameArgument, titleOption);

		// Put it all together; make it go brr
		var rootCommand = new RootCommand ("Generate an Industrious standard .NET solution");
		rootCommand.AddCommand (newCommand);
		return rootCommand.Invoke (args);
	}

	///////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	///   Generate a new solution; called in response to `new` command.
	/// </summary>
	/// <param name="name">
	///   A name for the new solution.
	/// </param>
	/// <param name="title">
	///   An optional application title.
	/// </param>
	///////////////////////////////////////////////////////////////////////////////////
	private static void OnNew (String name, String? title)
	{
		var cfg = new Configuration (name, title ?? name);
		ApplyUpdates (cfg);
		cfg.Save (ConfigFileName);
	}

	///////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	///   Run the update steps, starting from a specific version.
	/// </summary>
	/// <param name="cfg">
	///   The current solution's configuration, which includes its current version.
	/// </param>
	///////////////////////////////////////////////////////////////////////////////////
	private static void ApplyUpdates (Configuration cfg)
	{
		for (var i = cfg.Version; i < Versions.Length; ++i)
			Versions[i].Invoke (cfg);
	}

	///////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	///   Create the supporting configuration files required by all projects.
	/// </summary>
	///////////////////////////////////////////////////////////////////////////////////
	private static void CreateSupportFiles (Configuration cfg)
	{
		Console.WriteLine ("Creating support files");
		File.WriteAllText (".editorconfig", ReadResource (".editorconfig"));
		File.WriteAllText (".gitattributes", ReadResource (".gitattributes"));
		File.WriteAllText (".gitignore", ReadResource (".gitignore"));
		File.WriteAllText ("LICENSE.txt", ReadResource ("LICENSE.txt")
			.Replace ("{DateTime.Now.Year}", DateTime.Now.Year.ToString ())
			.Replace ("{CompanyName}", CompanyName));
		File.WriteAllText ("README.md", ReadResource ("README.md")
			.Replace ("{Name}", cfg.Name));
	}

	///////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	///   Create an empty Visual Studio solution.
	/// </summary>
	///////////////////////////////////////////////////////////////////////////////////
	private static void CreateSolution (Configuration cfg)
	{
		// Use `dotnet` to create an empty solution
		Console.WriteLine ("Creating solution");
		RunCommand ("dotnet", $"new sln --name {cfg.Name} --force");

		// Add the global configurations generated above as solution items
		ModifyFile ($"{cfg.Name}.sln", new[] {
			(
				Pattern: "^Global",
				Replacement: String.Join ("\r\n",
					"Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \"Support Files\", \"Support Files\", \"{6B0FD701-B9D2-44C1-BD08-C9E8AE7318DE}\"",
					"\tProjectSection(SolutionItems) = preProject",
					"\t\t.editorconfig = .editorconfig",
					"\t\t.gitattributes = .gitattributes",
					"\t\t.gitignore = .gitignore",
					"\t\tLICENSE.txt = LICENSE.txt",
					"\t\tREADME.md = README.md",
					"\tEndProjectSection",
					"EndProject",
					"Global"))
		});
	}

	///////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	///   Create the macOS project and add it to the solution.
	/// </summary>
	///////////////////////////////////////////////////////////////////////////////////
	private static void CreateMacOsProject (Configuration cfg)
	{
		var projectName = cfg.Name + ".macOS";

		// Use `dotnet` to create a new project from the "macOS" template, and then
		// add it to the previously generated solution

		RunCommand ("dotnet", $"new macos --output {projectName} --force");
		RunCommand ("dotnet", $"sln add {projectName}/{projectName}.csproj");

		// Patch up `MyProject.csproj`

		ModifyFile ($"{projectName}/{projectName}.csproj", new[] {
			( // add some additional properties to the project
				Pattern: "  </PropertyGroup>",
				Replacement: String.Join ("\n",
					$"    <RootNamespace>{cfg.Name}</RootNamespace>",
					$"    <AssemblyName>{cfg.Title}</AssemblyName>",
					$"    <Company>{CompanyName}</Company>",
					$"    <Product>{cfg.Name}</Product>",
					"  </PropertyGroup>"))
		});

		// Patch up `Info.plist`

		ModifyFile ($"{projectName}/Info.plist", new[] {
			( // replace the placeholder bundle ID
				Pattern: $"com.companyname.{projectName}",
				Replacement: $"{CompanyId}.{cfg.Name}"),
			( // set the application title
				Pattern: projectName,
				Replacement: cfg.Title),
			( // set a copyright string
				Pattern: @"\$\{AuthorCopyright:HtmlEncode}",
				Replacement: $"© {DateTime.Now.Year.ToString ()} {CompanyName}")
		});

		// Patch up `AppDelegate.cs`

		ModifyFile ($"{projectName}/AppDelegate.cs", new[] {
			( // replace `MyProject.macOS` with `MyProject` for namespace, etc.
				Pattern: projectName,
				Replacement: cfg.Name),
			( // move class opening brace to new line; add fields
				Pattern: " {",
				Replacement: String.Join ("\n",
					"\n{",
					"\tprivate readonly MainWindowController _mainWindowController = new ();",
					"")),
			( // fill in `DidFinishLaunching()` implementation
				Pattern: @"\t\t// Insert code here to initialize your application.*?}",
				Replacement: String.Join ("\n",
					"\t\t_mainWindowController.ShowWindow(this);",
					"\t}",
					"",
					"\tpublic override Boolean SupportsSecureRestorableState (NSApplication application)",
					"\t{",
					"\t\treturn true;",
					"\t}"))
		});

		// Patch up `Main.cs`

		ModifyFile ($"{projectName}/Main.cs", new[] {
			( // replace `MyProject.macOS` with `MyProject` for namespace, etc.
				Pattern: projectName,
				Replacement: cfg.Name)
		});

		// Patch up `Main.storyboard`

		ModifyFile ($"{projectName}/Main.storyboard", new[] {
			( // replace `MyProject.macOS` with application title in menus & windows
				Pattern: projectName,
				Replacement: cfg.Title),
			( // detach initial view controller
				Pattern: " initialViewController=\"[a-zA-Z0-9-]+\"",
				Replacement: ""),
			( // remove the default window controller
				Pattern: @"        <!\-\-Window Controller\-\->.*?</scene>\n",
				Replacement: ""),
			( // remove the default view controller
				Pattern: @"        <!\-\-View Controller\-\->.*?</scene>\n",
				Replacement: "")
		});

		// Remove the generated view controller
		File.Delete ($"{projectName}/ViewController.cs");
		File.Delete ($"{projectName}/ViewController.designer.cs");

		// Add a main window controller
		File.WriteAllText ($"{projectName}/MainWindowController.cs", ReadResource ("MainWindowController.cs")
			.Replace ("{Name}", cfg.Name)
			.Replace ("{Title}", cfg.Title));
	}

	///////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	///   Apply modifications to the contents of a file in place.
	/// </summary>
	/// <param name="fileName">
	///   The path of the file to be modified.
	/// </param>
	/// <param name="replacements">
	///   An array of patterns to be replaced, along with the corresponding replacement
	///   values.
	/// </param>
	///////////////////////////////////////////////////////////////////////////////////
	private static void ModifyFile (String fileName, IList<(String Pattern, String Replacement)> replacements)
	{
		// Read in the contents of the file
		var contents = File.ReadAllText (fileName);

		// Apply the provided replacements
		for (var i = 0; i < replacements.Count; ++i)
		{
			var options = replacements[i].Pattern.StartsWith ("^")
				? RegexOptions.Multiline
				: RegexOptions.Singleline;
			contents = Regex.Replace (contents, replacements[i].Pattern, replacements[i].Replacement, options);
		}

		// Write it back out
		File.WriteAllText (fileName, contents);
	}

	///////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	///   Read text file embedded resource.
	/// </summary>
	/// <param name="resourceName">
	///   The name of a resource located in this assembly's `Resources` folder.
	/// </param>
	/// <returns>
	///   The contents of the specified resource.
	/// </returns>
	/// <exception cref="InvalidProgramException">
	///   The specified resource could not be found.
	/// </exception>
	///////////////////////////////////////////////////////////////////////////////////
	private static String ReadResource (String resourceName)
	{
		using var stream = Assembly
			.GetExecutingAssembly ()
			.GetManifestResourceStream ("Industrious.Starter.Resources." + resourceName);

		if (stream == null)
			throw new InvalidProgramException ($"Missing resource '{resourceName}'");

		using var reader = new StreamReader (stream);
		return reader.ReadToEnd ();
	}

	///////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	///   Run an external command.
	/// </summary>
	/// <remarks>
	///   If the command succeeded, this method returns normally. If the command
	///   fails, the contents of `stderr` are echoed to the console, the application
	///   exits, returning the failed error code.
	/// </remarks>
	///////////////////////////////////////////////////////////////////////////////////
	private static void RunCommand (String program, String arguments)
	{
		var process = new Process ();

		process.StartInfo = new ProcessStartInfo {
			FileName = program,
			Arguments = arguments,
			RedirectStandardOutput = true,
			RedirectStandardError = true
		};

		process.Start ();
		process.WaitForExit ();

		if (process.ExitCode == 0) return;

		Console.WriteLine (process.StandardError.ReadToEnd ());
		Environment.Exit (process.ExitCode);
	}
}
