namespace MyMacApp._1;

[Register ("AppDelegate")]
public class AppDelegate : NSApplicationDelegate
{
	private readonly MainWindowController _mainWindowController = new ();


	public override void DidFinishLaunching (NSNotification notification)
	{
		_mainWindowController.ShowWindow(this);
	}


	public override Boolean SupportsSecureRestorableState (NSApplication application)
	{
		return true;
	}


	public override void WillTerminate (NSNotification notification)
	{
		// Insert code here to tear down your application
	}
}
