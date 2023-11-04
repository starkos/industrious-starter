namespace Starter.Example;

public sealed class MainWindowController : NSWindowController
{
	public MainWindowController()
	{
		WindowFrameAutosaveName = "MainWindow";

		var contentRect = new CGRect(100, 100, 800, 600);
		const NSWindowStyle style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;
		base.Window = new NSWindow(contentRect, style, NSBackingStore.Buffered, true)
		{
			Title = "MyApp"
		};
	}
}
