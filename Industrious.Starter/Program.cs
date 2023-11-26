namespace Industrious.Starter;

internal class Program : ICommandLineHandler
{
	// Name of file used to store last-run configuration about the solution
	private const String ConfigFileName = ".starter.json";


	///////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	///   Program entry point.
	/// </summary>
	///////////////////////////////////////////////////////////////////////////////////

	public static Int32 Main (String[] args)
	{
		var program = new Program ();
		var parser = new CommandLineParser (program);
		return parser.Parse (args);
	}

	///////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	///   Generate a new solution.
	/// </summary>
	/// <param name="name">
	///   A name for the new solution.
	/// </param>
	/// <param name="applicationTitle">
	///   An optional application title.
	/// </param>
	/// <param name="companyName">
	///    A company name, to use in templates where applicable.
	/// </param>
	/// <param name="companyIdentifier">
	///    A company identifier, to use in templates where applicable.
	/// </param>
	///////////////////////////////////////////////////////////////////////////////////

	public void OnNewSolution (String name, String? applicationTitle, String companyName, String companyIdentifier)
	{
		var workspace = new Workspace (name, applicationTitle ?? name, companyName, companyIdentifier);
		workspace.CurrentVersion = Updates.Apply (workspace);
		workspace.Save (ConfigFileName);
		Console.WriteLine ("Done");
	}


	///////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	///   Update an existing solution.
	/// </summary>
	///////////////////////////////////////////////////////////////////////////////////

	public void OnUpdateSolution ()
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
			Console.WriteLine ("Done");
		}
		else
		{
			Console.WriteLine ("Workspace is up to date.");
		}
	}
}
