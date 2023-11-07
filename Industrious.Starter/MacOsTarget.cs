namespace Industrious.Starter;

public class MacOsTarget
{
	public MacOsTarget (String name)
	{
		Project = new TextFile ($"{name}.macOS/{name}.macOS.csproj");
		AppDelegate = new TextFile ($"{name}.macOS/AppDelegate.cs");
		Entitlements = new TextFile ($"{name}.macOS/Entitlements.plist");
		Info = new TextFile ($"{name}.macOS/Info.plist");
		Main = new TextFile ($"{name}.macOS/Main.cs");
		MainStoryboard = new TextFile ($"{name}.macOS/Main.storyboard");
		MainWindowController = new TextFile ($"{name}.macOS/MainWindowController.cs");

		Assets = new[] {
			new BinaryFile ($"{name}.macOS/Assets.xcassets/Contents.json"),
			new BinaryFile ($"{name}.macOS/Assets.xcassets/AppIcon.appiconset/Contents.json"),
			new BinaryFile ($"{name}.macOS/Assets.xcassets/AppIcon.appiconset/Icon16.png"),
			new BinaryFile ($"{name}.macOS/Assets.xcassets/AppIcon.appiconset/Icon32.png"),
			new BinaryFile ($"{name}.macOS/Assets.xcassets/AppIcon.appiconset/Icon64.png"),
			new BinaryFile ($"{name}.macOS/Assets.xcassets/AppIcon.appiconset/Icon128.png"),
			new BinaryFile ($"{name}.macOS/Assets.xcassets/AppIcon.appiconset/Icon256.png"),
			new BinaryFile ($"{name}.macOS/Assets.xcassets/AppIcon.appiconset/Icon512.png"),
			new BinaryFile ($"{name}.macOS/Assets.xcassets/AppIcon.appiconset/Icon1024.png"),
		};
	}


	public TextFile Project { get; }
	public TextFile AppDelegate { get; }
	public TextFile Entitlements { get; }
	public TextFile Info { get; }
	public TextFile Main { get; }
	public TextFile MainStoryboard { get; }
	public TextFile MainWindowController { get; }

	public IList<BinaryFile> Assets { get; }


	public void Save ()
	{
		Project.Save ();
		AppDelegate.Save ();
		Entitlements.Save ();
		Info.Save ();
		Main.Save ();
		MainStoryboard.Save ();
		MainWindowController.Save ();
	}
}
