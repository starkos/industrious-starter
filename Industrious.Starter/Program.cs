using System.CommandLine;

namespace Industrious.Starter;

internal static class Program
{
	// Default company information
	private const String CompanyName = "Industrious One";
	private const String CompanyId = "com.industriousone";

	// Name of file used to store last-run configuration about the solution
	private const String ConfigFileName = ".starter.json";


	///////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	///   Program entry point.
	/// </summary>
	///////////////////////////////////////////////////////////////////////////////////

	public static Int32 Main (String[] args)
	{
		// New solution command

		var nameArgument = new Argument<String> (
			name: "name",
			description: "name of the solution to be generated");

		var titleOption = new Option<String?> (
			aliases: new[] { "--title" },
			description: "application title; defaults to solution name");

		var companyOption = new Option<String> (
			aliases: new[] { "--company" },
			description: $@"company name; defaults to {CompanyName}",
			getDefaultValue: () => CompanyName);

		var identifierOption = new Option<String> (
			aliases: new[] { "--id" },
			description: $@"company identifier; defaults to {CompanyId}",
			getDefaultValue: () => CompanyId);

		var newCommand = new Command ("new", "Generate a new solution") {
			nameArgument, titleOption, companyOption, identifierOption
		};

		newCommand.SetHandler (OnNewCommand, nameArgument, titleOption, companyOption, identifierOption);

		// Update solution command

		var updateCommand = new Command ("update", "Update the current solution");
		updateCommand.SetHandler (OnUpdateCommand);

		// Put it all together; make it go brr

		var rootCommand = new RootCommand ("Generate an Industrious standard .NET solution");

		rootCommand.AddCommand (newCommand);
		rootCommand.AddCommand (updateCommand);

		return rootCommand.Invoke (args);
	}

	///////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	///   Generate a new solution.
	/// </summary>
	/// <param name="name">
	///   A name for the new solution.
	/// </param>
	/// <param name="title">
	///   An optional application title.
	/// </param>
	/// <param name="company">
	///    A company name, to use in templates where applicable.
	/// </param>
	/// <param name="identifier">
	///    A company identifier, to use in templates where applicable.
	/// </param>
	///////////////////////////////////////////////////////////////////////////////////

	private static void OnNewCommand (String name, String? title, String company, String identifier)
	{
		var workspace = new Workspace (name, title ?? name, company, identifier);
		workspace.CurrentVersion = Updates.Apply (workspace);
		workspace.Save (ConfigFileName);
	}


	///////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	///   Update an existing solution.
	/// </summary>
	///////////////////////////////////////////////////////////////////////////////////

	private static void OnUpdateCommand ()
	{
		var workspace = Workspace.Load (ConfigFileName);
		if (workspace == null)
		{
			Console.WriteLine ($"Error: `{ConfigFileName}` not found");
			Environment.Exit (1);
		}

		if (workspace.CurrentVersion < Updates.LatestVersion)
		{
			workspace.CurrentVersion = Updates.Apply (workspace);
			workspace.Save (ConfigFileName);
		}
		else
		{
			Console.WriteLine ("Workspace is up to date.");
		}
	}
}
