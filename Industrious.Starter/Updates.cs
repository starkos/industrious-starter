namespace Industrious.Starter;

///////////////////////////////////////////////////////////////////////////////////////////
/// <summary>
///   The set of updates that make up a fully configuration Industrious solution.
/// </summary>
///////////////////////////////////////////////////////////////////////////////////////////
public static class Updates
{
	private static readonly Action<Workspace>[] Versions = {
		CreateEmptySolution,
		CreateTopLevelSupportFiles,
		CreateMacOsProject,
		CreateConsoleExecutableProject
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
		wks.Solution.LoadFromResource ("Solution");
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

		wks.Solution
			.AddSolutionItem (".editorconfig")
			.AddSolutionItem (".gitattributes")
			.AddSolutionItem (".gitignore")
			.AddSolutionItem ("LICENSE.txt")
			.AddSolutionItem ("README.md");
	}


	private static void CreateMacOsProject (Workspace wks)
	{
		Console.WriteLine ("Creating macOS project");

		wks.MacOs.Project.LoadFromResource ("macOS/Project")
			.Replace ("{Name}", wks.Name)
			.Replace ("{Title}", wks.ApplicationTitle)
			.Replace ("{Company}", wks.CompanyName);

		wks.MacOs.AppDelegate.LoadFromResource ("macOS/AppDelegate")
			.Replace ("{Name}", wks.Name);

		wks.MacOs.Entitlements.LoadFromResource ("macOS/Entitlements.plist");

		wks.MacOs.Info.LoadFromResource ("macOS/Info.plist")
			.Replace ("{Name}", wks.Name)
			.Replace ("{Title}", wks.ApplicationTitle)
			.Replace ("{Company}", wks.CompanyName)
			.Replace ("{Identifier}", wks.CompanyIdentifier)
			.Replace ("{Year}", DateTime.Now.Year.ToString ());

		wks.MacOs.Main.LoadFromResource ("macOS/Main")
			.Replace ("{Name}", wks.Name);

		wks.MacOs.MainStoryboard.LoadFromResource ("macOS/Main.storyboard")
			.Replace ("{Title}", wks.ApplicationTitle);

		wks.MacOs.MainWindowController.LoadFromResource ("macOS/MainWindowController")
			.Replace ("{Name}", wks.Name)
			.Replace ("{Title}", wks.ApplicationTitle);

		foreach (var asset in wks.MacOs.Assets)
		{
			var resourcePath = asset.Path.AsSpan (wks.MacOs.ProjectFolder.Length);
			asset.CopyFromResource (String.Concat ("macOS", resourcePath));
		}

		wks.Solution.AddProject (wks.MacOs.ProjectPath, "{40B768D8-DCF3-4353-A813-089E779F2E0E}");
	}


	private static void CreateConsoleExecutableProject (Workspace wks)
	{
		Console.WriteLine ("Creating console executable project");

		wks.Console.Project.LoadFromResource ("Console/Project")
			.Replace ("{Name}", wks.Name)
			.Replace ("{Title}", wks.ApplicationTitle)
			.Replace ("{Company}", wks.CompanyName);

		wks.Console.Program.LoadFromResource ("Console/Program");

		wks.Solution.AddProject (wks.Console.ProjectPath, "{548B54F6-AF78-4582-A875-75BE2F0BBA07}");
	}
}
