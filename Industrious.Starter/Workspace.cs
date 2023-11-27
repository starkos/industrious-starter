namespace Industrious.Starter;

public class Workspace
{
	public Workspace (String name, String? applicationTitle, String company, String identifier, Int32 currentVersion = 0)
	{
		Name = name;
		ApplicationTitle = applicationTitle ?? name;
		CompanyName = company;
		CompanyIdentifier = identifier;
		CurrentVersion = currentVersion;

		EditorConfig = new TextFile (".editorconfig");
		GitAttributes = new TextFile (".gitattributes");
		GitIgnore = new TextFile (".gitignore");
		License = new TextFile ("LICENSE.txt");
		Readme = new TextFile ("README.md");

		Solution = new Solution (name);

		Common = new ClassLibraryProject ($"{name}");

		Console = new ConsoleProject ($"{name}.Console");
		MacOs = new MacOsProject ($"{name}.macOS");
	}


	public String Name { get; }
	public String ApplicationTitle { get; }
	public String CompanyName { get; }
	public String CompanyIdentifier { get; }
	public Int32 CurrentVersion { get; set; }

	public TextFile EditorConfig { get; }
	public TextFile GitAttributes { get; }
	public TextFile GitIgnore { get; }
	public TextFile License { get; }
	public TextFile Readme { get; }

	public Solution Solution { get; }

	public ClassLibraryProject Common { get; }

	public ConsoleProject Console { get; }
	public MacOsProject MacOs { get; }


	public static Workspace? Load (String settingsFileName)
	{
		var cfg = Configuration.Load (settingsFileName);
		return cfg != null
			? new Workspace (cfg.Name, cfg.Title, cfg.Company, cfg.Identifier, cfg.Version)
			: null;
	}


	public void Save (String settingsFileName)
	{
		EditorConfig.Save ();
		GitAttributes.Save ();
		GitIgnore.Save ();
		License.Save ();
		Readme.Save ();

		Solution.Save ();

		Console.Save ();
		Common.Save ();
		MacOs.Save ();

		var configuration = new Configuration (this);
		configuration.Save (settingsFileName);
	}
}
