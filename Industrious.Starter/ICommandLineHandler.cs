namespace Industrious.Starter;

public interface ICommandLineHandler
{
	public void OnNewSolution (String name, String? applicationTitle, String companyName, String companyIdentifier);

	public void OnUpdateSolution ();
}
