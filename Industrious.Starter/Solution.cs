using System.Text;

namespace Industrious.Starter;

public class Solution : ISaveable
{
	private readonly TextFile _solution;
	private readonly TextFile _editorConfig;
	private readonly TextFile _gitAttributes;
	private readonly TextFile _gitIgnore;
	private readonly TextFile _license;
	private readonly TextFile _readMe;


	public Solution (String name)
	{
		_solution = new TextFile ($"{name}.sln", new UTF8Encoding (true));
		_editorConfig = new TextFile (".editorconfig");
		_gitAttributes = new TextFile (".gitattributes");
		_gitIgnore = new TextFile (".gitignore");
		_license = new TextFile ("LICENSE.txt");
		_readMe = new TextFile ("README.md");
	}


	public void Init (Configuration cfg)
	{
		_solution.LoadFromResource ("Solution");

		_editorConfig.LoadFromResource ("editorconfig");

		_gitAttributes.LoadFromResource ("gitattributes");
		_gitIgnore.LoadFromResource ("gitignore");

		_license.LoadFromResource ("LICENSE.txt")
			.Replace ("{Year}", DateTime.Now.Year.ToString ())
			.Replace ("{CompanyName}", cfg.Company);

		_readMe.LoadFromResource ("README.md")
			.Replace ("{Name}", cfg.Name);

		AddSolutionItems (_editorConfig, _gitAttributes, _gitIgnore, _license, _readMe);
	}


	public Solution AddProject (String projectPath, String identifier)
	{
		var projectName = Path.GetFileNameWithoutExtension (projectPath);
		projectPath = projectPath.Replace ("/", "\\");

		_solution.InsertBeforeLast ("^Global", String.Join ("\r\n",
			$@"Project(""{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}"") = ""{projectName}"", ""{projectPath}"", ""{identifier}""",
			"EndProject",
			""));

		_solution.InsertBeforeLast ("\tEndGlobalSection", String.Join ("\r\n",
			$"\t\t{identifier}.Debug|Any CPU.ActiveCfg = Debug|Any CPU",
			$"\t\t{identifier}.Debug|Any CPU.Build.0 = Debug|Any CPU",
			$"\t\t{identifier}.Release|Any CPU.ActiveCfg = Release|Any CPU",
			$"\t\t{identifier}.Release|Any CPU.Build.0 = Release|Any CPU",
			""));

		return this;
	}


	public void Save ()
	{
		_editorConfig.Save ();
		_gitAttributes.Save ();
		_gitIgnore.Save ();
		_license.Save ();
		_readMe.Save ();
		_solution.Save ();
	}


	private void AddSolutionItems (params TextFile[] items)
	{
		foreach (var item in items)
			_solution.InsertBeforeLast ("\tEndProjectSection", $"\t\t{item.Path} = {item.Path}\r\n");
	}
}
