namespace Industrious.Starter;

public interface ICommandLineHandler
{
	public void OnNewSolution (String name, String? title, String company, String identifier);

	public void OnUpdateSolution ();
}
