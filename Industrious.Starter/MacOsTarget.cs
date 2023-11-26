namespace Industrious.Starter;

public class MacOsTarget
{
	private readonly TextFile _project;
	private readonly TextFile _appDelegate;
	private readonly TextFile _entitlements;
	private readonly TextFile _infoPlist;
	private readonly TextFile _main;
	private readonly TextFile _storyboard;
	private readonly TextFile _windowController;
	private readonly IList<BinaryFile> _assets;


	public MacOsTarget (String name)
	{
		var projectFolder = $"Code/{name}";
		Path = $"{projectFolder}/{name}.csproj";

		_project = new TextFile (Path);
		_appDelegate = new TextFile ($"{projectFolder}/AppDelegate.cs");
		_entitlements = new TextFile ($"{projectFolder}/Entitlements.plist");
		_infoPlist = new TextFile ($"{projectFolder}/Info.plist");
		_main = new TextFile ($"{projectFolder}/Main.cs");
		_storyboard = new TextFile ($"{projectFolder}/Main.storyboard");
		_windowController = new TextFile ($"{projectFolder}/MainWindowController.cs");

		_assets = new[] {
			new BinaryFile ($"{projectFolder}/Assets.xcassets/Contents.json"),
			new BinaryFile ($"{projectFolder}/Assets.xcassets/AppIcon.appiconset/Contents.json"),
			new BinaryFile ($"{projectFolder}/Assets.xcassets/AppIcon.appiconset/Icon16.png"),
			new BinaryFile ($"{projectFolder}/Assets.xcassets/AppIcon.appiconset/Icon32.png"),
			new BinaryFile ($"{projectFolder}/Assets.xcassets/AppIcon.appiconset/Icon64.png"),
			new BinaryFile ($"{projectFolder}/Assets.xcassets/AppIcon.appiconset/Icon128.png"),
			new BinaryFile ($"{projectFolder}/Assets.xcassets/AppIcon.appiconset/Icon256.png"),
			new BinaryFile ($"{projectFolder}/Assets.xcassets/AppIcon.appiconset/Icon512.png"),
			new BinaryFile ($"{projectFolder}/Assets.xcassets/AppIcon.appiconset/Icon1024.png"),
		};
	}


	public String Path { get; }


	public void LoadFromResources (Workspace wks)
	{
		_project.LoadFromResource ("macOS/Project")
			.Replace ("{Name}", wks.Name)
			.Replace ("{Title}", wks.ApplicationTitle)
			.Replace ("{Company}", wks.CompanyName);

		_appDelegate.LoadFromResource ("macOS/AppDelegate")
			.Replace ("{Name}", wks.Name);

		_entitlements.LoadFromResource ("macOS/Entitlements.plist");

		_infoPlist.LoadFromResource ("macOS/Info.plist")
			.Replace ("{Name}", wks.Name)
			.Replace ("{Title}", wks.ApplicationTitle)
			.Replace ("{Company}", wks.CompanyName)
			.Replace ("{Identifier}", wks.CompanyIdentifier)
			.Replace ("{Year}", DateTime.Now.Year.ToString ());

		_main.LoadFromResource ("macOS/Main")
			.Replace ("{Name}", wks.Name);

		_storyboard.LoadFromResource ("macOS/Main.storyboard")
			.Replace ("{Title}", wks.ApplicationTitle);

		_windowController.LoadFromResource ("macOS/MainWindowController")
			.Replace ("{Name}", wks.Name)
			.Replace ("{Title}", wks.ApplicationTitle);

		var projectFolder = System.IO.Path.GetDirectoryName (Path)!;
		foreach (var asset in _assets)
		{
			var resourcePath = asset.Path.AsSpan (projectFolder.Length);
			asset.CopyFromResource (String.Concat ("macOS", resourcePath));
		}
	}


	public void Save ()
	{
		_project.Save ();
		_appDelegate.Save ();
		_entitlements.Save ();
		_infoPlist.Save ();
		_main.Save ();
		_storyboard.Save ();
		_windowController.Save ();
	}
}
