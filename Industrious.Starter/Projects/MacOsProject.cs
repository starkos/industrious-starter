namespace Industrious.Starter.Projects;

public class MacOsProject : ISaveable
{
	private readonly TextFile _appDelegate;
	private readonly TextFile _entitlements;
	private readonly TextFile _infoPlist;
	private readonly TextFile _main;
	private readonly TextFile _storyboard;
	private readonly TextFile _windowController;
	private readonly IList<BinaryFile> _assets;


	public MacOsProject (String name)
	{
		Project = new ProjectFile ($"Code/{name}/{name}.csproj");
		_appDelegate = new TextFile ($"Code/{name}/AppDelegate.cs");
		_entitlements = new TextFile ($"Code/{name}/Entitlements.plist");
		_infoPlist = new TextFile ($"Code/{name}/Info.plist");
		_main = new TextFile ($"Code/{name}/Main.cs");
		_storyboard = new TextFile ($"Code/{name}/Main.storyboard");
		_windowController = new TextFile ($"Code/{name}/MainWindowController.cs");
		_assets = new[] {
			new BinaryFile ($"Code/{name}/Assets.xcassets/Contents.json"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Contents.json"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon16.png"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon32.png"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon64.png"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon128.png"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon256.png"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon512.png"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon1024.png")
		};
	}


	public readonly ProjectFile Project;


	public void Init (Configuration cfg)
	{
		Project.LoadFromResource ("macOS/Project")
			.Replace ("{Name}", cfg.Name)
			.Replace ("{Title}", cfg.Title)
			.Replace ("{Company}", cfg.Company);

		_appDelegate.LoadFromResource ("macOS/AppDelegate")
			.Replace ("{Name}", cfg.Name);

		_entitlements.LoadFromResource ("macOS/Entitlements.plist");

		_infoPlist.LoadFromResource ("macOS/Info.plist")
			.Replace ("{Name}", cfg.Name)
			.Replace ("{Title}", cfg.Title)
			.Replace ("{Company}", cfg.Company)
			.Replace ("{Identifier}", cfg.Identifier)
			.Replace ("{Year}", DateTime.Now.Year.ToString ());

		_main.LoadFromResource ("macOS/Main")
			.Replace ("{Name}", cfg.Name);

		_storyboard.LoadFromResource ("macOS/Main.storyboard")
			.Replace ("{Title}", cfg.Title);

		_windowController.LoadFromResource ("macOS/MainWindowController")
			.Replace ("{Name}", cfg.Name)
			.Replace ("{Title}", cfg.Title);

		var projectFolder = Path.GetDirectoryName (Project.Path)!;
		foreach (var asset in _assets)
		{
			var resourcePath = asset.Path.AsSpan (projectFolder.Length);
			asset.CopyFromResource (String.Concat ("macOS", resourcePath));
		}
	}


	public void Save ()
	{
		Project.Save ();
		_appDelegate.Save ();
		_entitlements.Save ();
		_infoPlist.Save ();
		_main.Save ();
		_storyboard.Save ();
		_windowController.Save ();
	}
}
