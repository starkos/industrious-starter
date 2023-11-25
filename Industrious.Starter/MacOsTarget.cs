namespace Industrious.Starter;

public class MacOsTarget
{
	public MacOsTarget (String name)
	{
		ProjectFolder = $"Code/{name}.macOS";
		ProjectPath = $"{ProjectFolder}/{name}.macOS.csproj";

		Project = new TextFile (ProjectPath);
		AppDelegate = new TextFile ($"{ProjectFolder}/AppDelegate.cs");
		Entitlements = new TextFile ($"{ProjectFolder}/Entitlements.plist");
		Info = new TextFile ($"{ProjectFolder}/Info.plist");
		Main = new TextFile ($"{ProjectFolder}/Main.cs");
		MainStoryboard = new TextFile ($"{ProjectFolder}/Main.storyboard");
		MainWindowController = new TextFile ($"{ProjectFolder}/MainWindowController.cs");

		Assets = new[] {
			new BinaryFile ($"{ProjectFolder}/Assets.xcassets/Contents.json"),
			new BinaryFile ($"{ProjectFolder}/Assets.xcassets/AppIcon.appiconset/Contents.json"),
			new BinaryFile ($"{ProjectFolder}/Assets.xcassets/AppIcon.appiconset/Icon16.png"),
			new BinaryFile ($"{ProjectFolder}/Assets.xcassets/AppIcon.appiconset/Icon32.png"),
			new BinaryFile ($"{ProjectFolder}/Assets.xcassets/AppIcon.appiconset/Icon64.png"),
			new BinaryFile ($"{ProjectFolder}/Assets.xcassets/AppIcon.appiconset/Icon128.png"),
			new BinaryFile ($"{ProjectFolder}/Assets.xcassets/AppIcon.appiconset/Icon256.png"),
			new BinaryFile ($"{ProjectFolder}/Assets.xcassets/AppIcon.appiconset/Icon512.png"),
			new BinaryFile ($"{ProjectFolder}/Assets.xcassets/AppIcon.appiconset/Icon1024.png"),
		};
	}


	public String ProjectFolder { get; }
	public String ProjectPath { get; }

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
