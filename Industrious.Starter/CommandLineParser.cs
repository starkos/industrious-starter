using System.CommandLine;

namespace Industrious.Starter;

public class CommandLineParser
{
	// Default company information
	private const String CompanyName = "Industrious One";
	private const String CompanyId = "com.industriousone";

	private readonly RootCommand _rootCommand;


	public CommandLineParser (ICommandLineHandler handler)
	{
		_rootCommand = new RootCommand ("Generate an Industrious standard .NET solution");

		//---------------------------------------------------------------
		// New solution command

		var nameArgument = new Argument<String> (
			"name",
			"name of the solution to be generated");

		var titleOption = new Option<String?> (
			new[] { "--title" },
			"application title; defaults to solution name");

		var companyOption = new Option<String> (
			new[] { "--company" },
			description: $@"company name; defaults to {CompanyName}",
			getDefaultValue: () => CompanyName);

		var identifierOption = new Option<String> (
			new[] { "--id" },
			description: $@"company identifier; defaults to {CompanyId}",
			getDefaultValue: () => CompanyId);

		var newSolutionCommand = new Command ("new", "Generate a new solution") {
			nameArgument, titleOption, companyOption, identifierOption
		};

		newSolutionCommand.SetHandler (handler.OnNewSolution, nameArgument, titleOption, companyOption, identifierOption);
		_rootCommand.AddCommand (newSolutionCommand);

		//---------------------------------------------------------------
		// Rename solution command

		var newNameArgument = new Argument<String> (
			"name",
			"the new name of the solution");

		var renameSolutionCommand = new Command ("rename", "Rename the current solution") {
			newNameArgument
		};

		renameSolutionCommand.SetHandler (handler.OnRenameSolution, newNameArgument);
		_rootCommand.AddCommand (renameSolutionCommand);

		//---------------------------------------------------------------
		// Update solution command

		var updateSolutionCommand = new Command ("update", "Update the current solution");
		updateSolutionCommand.SetHandler (handler.OnUpdateSolution);
		_rootCommand.AddCommand (updateSolutionCommand);
	}


	public Int32 Parse (String[] args)
	{
		return _rootCommand.Invoke (args);
	}
}
