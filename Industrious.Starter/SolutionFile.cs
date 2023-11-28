using System.Text;

namespace Industrious.Starter;

public class SolutionFile : TextFile
{
	public SolutionFile (String name)
		: base ($"{name}.sln", new UTF8Encoding (true))
	{
	}


	public SolutionFile AddProject (String projectPath, String identifier)
	{
		var projectName = System.IO.Path.GetFileNameWithoutExtension (projectPath);
		projectPath = projectPath.Replace ("/", "\\");

		InsertBeforeLast ("^Global", String.Join ("\r\n",
			$@"Project(""{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}"") = ""{projectName}"", ""{projectPath}"", ""{identifier}""",
			"EndProject",
			""));

		InsertBeforeLast ("\tEndGlobalSection", String.Join ("\r\n",
			$"\t\t{identifier}.Debug|Any CPU.ActiveCfg = Debug|Any CPU",
			$"\t\t{identifier}.Debug|Any CPU.Build.0 = Debug|Any CPU",
			$"\t\t{identifier}.Release|Any CPU.ActiveCfg = Release|Any CPU",
			$"\t\t{identifier}.Release|Any CPU.Build.0 = Release|Any CPU",
			""));

		return this;
	}


	public SolutionFile AddSolutionItem (String name)
	{
		InsertBeforeLast ("\tEndProjectSection", $"\t\t{name} = {name}\r\n");
		return this;
	}
}
