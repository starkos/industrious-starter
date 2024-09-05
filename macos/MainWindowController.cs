namespace MyMacApp._1;

public sealed class MainWindowController : NSWindowController
{
	public MainWindowController ()
	{
		const NSWindowStyle style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;
		Window = new NSWindow (new CGRect (100, 100, 800, 600), style, NSBackingStore.Buffered, true)
		{
			Title = "My Application"
		};

		WindowFrameAutosaveName = "MainWindow";
	}
}
