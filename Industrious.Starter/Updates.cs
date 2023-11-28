namespace Industrious.Starter;

///////////////////////////////////////////////////////////////////////////////////////////
/// <summary>
///     The set of updates that make up a fully configuration Industrious solution.
/// </summary>
///////////////////////////////////////////////////////////////////////////////////////////
public static class Updates
{
	private static readonly Action<Workspace>[] Versions = {
		CreateEmptySolution,
		CreateTopLevelSupportFiles,
		CreateCommonCodeLibrary,
		CreateConsoleExecutableProject,
		CreateMacOsProject
	};


	public static Int32 LatestVersion => Versions.Length;


	public static Int32 Apply (Workspace workspace)
	{
		for (var i = workspace.CurrentVersion; i < Versions.Length; ++i)
			Versions[i].Invoke (workspace);
		return Versions.Length;
	}


	private static void CreateEmptySolution (Workspace wks)
	{
		Console.WriteLine ("Creating solution");
		wks.SolutionFile.LoadFromResource ("Solution");
	}


	private static void CreateTopLevelSupportFiles (Workspace wks)
	{
		Console.WriteLine ("Creating support files");

		wks.EditorConfig.LoadFromResource ("editorconfig");
		wks.GitAttributes.LoadFromResource ("gitattributes");
		wks.GitIgnore.LoadFromResource ("gitignore");

		wks.License.LoadFromResource ("LICENSE.txt")
			.Replace ("{Year}", DateTime.Now.Year.ToString ())
			.Replace ("{CompanyName}", wks.CompanyName);

		wks.Readme.LoadFromResource ("README.md")
			.Replace ("{Name}", wks.Name);

		wks.SolutionFile
			.AddSolutionItem (".editorconfig")
			.AddSolutionItem (".gitattributes")
			.AddSolutionItem (".gitignore")
			.AddSolutionItem ("LICENSE.txt")
			.AddSolutionItem ("README.md");
	}


	private static void CreateCommonCodeLibrary (Workspace wks)
	{
		Console.WriteLine ("Creating common code library");

		wks.Common.LoadFromResources ();

		wks.SolutionFile.AddProject (wks.Common.Project.Path, "{E4DCEF92-4272-4329-B946-6BA46249C619}");
		wks.SolutionFile.AddProject (wks.Common.TestProject.Path, "{95FE6095-9FC7-4802-987F-A59B06346BB1}");
	}


	private static void CreateConsoleExecutableProject (Workspace wks)
	{
		Console.WriteLine ("Creating console executable project");

		wks.Console.LoadFromResources (wks);
		wks.Console.Project.AddLocalProjectReference (wks.Common.Project);

		wks.SolutionFile.AddProject (wks.Console.Project.Path, "{548B54F6-AF78-4582-A875-75BE2F0BBA07}");
	}


	private static void CreateMacOsProject (Workspace wks)
	{
		Console.WriteLine ("Creating macOS project");

		wks.MacOs.LoadFromResources (wks);
		wks.MacOs.Project.AddLocalProjectReference (wks.Common.Project);

		wks.SolutionFile.AddProject (wks.MacOs.Project.Path, "{40B768D8-DCF3-4353-A813-089E779F2E0E}");
	}
}
