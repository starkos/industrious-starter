﻿using Industrious.Starter.Commands;

namespace Industrious.Starter;

internal class Program : ICommandLineHandler
{
	// Name of file used to store last-run configuration about the solution
	private const String ConfigFileName = ".starter.json";


	public static Int32 Main (String[] args)
	{
		// Process the command line; calls one of the `On...` methods below
		var program = new Program ();
		var parser = new CommandLineParser (program);
		return parser.Parse (args);
	}


	public void OnNewSolution (String name, String? title, String company, String identifier)
	{
		var configuration = new Configuration (name, title ?? name, company, identifier);

		var builder = new SolutionBuilder (configuration);
		var updatedConfiguration = builder.ApplyUpdates ();

		updatedConfiguration.Save (ConfigFileName);
		Console.WriteLine ("Done");
	}


	public void OnRenameSolution (String newName)
	{
		var configuration = LoadConfiguration ();

		var command = new RenameCommand (configuration, newName);
		command.Apply ();

		configuration.WithName (newName).Save (ConfigFileName);

		Console.WriteLine ("Rename complete; please search for remaining replacements");
	}


	public void OnUpdateSolution ()
	{
		var configuration = LoadConfiguration ();

		var builder = new SolutionBuilder (configuration);
		var updatedConfiguration = builder.ApplyUpdates ();

		if (configuration.Version == updatedConfiguration.Version)
		{
			Console.WriteLine ("Solution is up to date.");
		}
		else
		{
			updatedConfiguration.Save (ConfigFileName);
			Console.WriteLine ("Done");
		}
	}


	private static Configuration LoadConfiguration ()
	{
		var configuration = Configuration.Load (ConfigFileName);
		if (configuration == null)
		{
			Console.WriteLine ($"Error: `{ConfigFileName}` not found");
			Environment.Exit (1);
		}

		return configuration;
	}
}
