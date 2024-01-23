namespace Industrious.Starter.Projects;

public class IosProject : ISaveable
{
	private readonly TextFile _appDelegate;
	private readonly TextFile _entitlements;
	private readonly TextFile _infoPlist;
	private readonly TextFile _main;
	private readonly TextFile _launchStoryboard;
	private readonly IList<BinaryFile> _assets;


	public IosProject (String name)
	{
		Project = new ProjectFile ($"Code/{name}/{name}.csproj");
		_appDelegate = new TextFile ($"Code/{name}/AppDelegate.cs");
		_entitlements = new TextFile ($"Code/{name}/Entitlements.plist");
		_infoPlist = new TextFile ($"Code/{name}/Info.plist");
		_main = new TextFile ($"Code/{name}/Main.cs");
		_launchStoryboard = new TextFile ($"Code/{name}/Main.storyboard");
		_assets = new[] {
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Contents.json"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon20.png"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon29.png"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon40.png"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon58.png"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon60.png"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon76.png"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon80.png"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon87.png"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon120.png"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon152.png"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon167.png"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon180.png"),
			new BinaryFile ($"Code/{name}/Assets.xcassets/AppIcon.appiconset/Icon1024.png")
		};
	}


	public readonly ProjectFile Project;


	public void Init (Configuration cfg)
	{
		Project.LoadFromResource ("iOS/Project")
			.Replace ("{Name}", cfg.Name)
			.Replace ("{Title}", cfg.Title)
			.Replace ("{Company}", cfg.Company);

		_appDelegate.LoadFromResource ("iOS/AppDelegate")
			.Replace ("{Name}", cfg.Name);

		_entitlements.LoadFromResource ("iOS/Entitlements.plist");

		_infoPlist.LoadFromResource ("iOS/Info.plist")
			.Replace ("{Name}", cfg.Name)
			.Replace ("{Title}", cfg.Title)
			.Replace ("{Company}", cfg.Company)
			.Replace ("{Identifier}", cfg.Identifier)
			.Replace ("{Year}", DateTime.Now.Year.ToString ());

		_main.LoadFromResource ("iOS/Main")
			.Replace ("{Name}", cfg.Name);

		_launchStoryboard.LoadFromResource ("iOS/LaunchScreen.storyboard")
			.Replace ("{Title}", cfg.Title);

		var projectFolder = Path.GetDirectoryName (Project.Path)!;
		foreach (var asset in _assets)
		{
			var resourcePath = asset.Path.AsSpan (projectFolder.Length);
			asset.CopyFromResource (String.Concat ("iOS", resourcePath));
		}
	}


	public void Save ()
	{
		Project.Save ();
		_appDelegate.Save ();
		_entitlements.Save ();
		_infoPlist.Save ();
		_main.Save ();
		_launchStoryboard.Save ();
	}
}
