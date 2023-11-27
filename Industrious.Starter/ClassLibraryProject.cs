namespace Industrious.Starter;

public class ClassLibraryProject
{
	private readonly String _name;
	private readonly TextFile _class1;
	private readonly TextFile _tests;


	public ClassLibraryProject (String name)
	{
		_name = name;

		Project = new TextFile ($"Code/{name}/{name}.csproj");
		_class1 = new TextFile ($"Code/{name}/Class1.cs");

		TestProject = new TextFile ($"Code/{name}.Tests/{name}.Tests.csproj");
		_tests = new TextFile ($"Code/{name}.Tests/Class1Tests.cs");
	}


	public TextFile Project { get; }
	public TextFile TestProject { get; }


	public void LoadFromResources ()
	{
		Project.LoadFromResource ("ClassLib/Project");
		TestProject.LoadFromResource ("ClassLib/TestProject");

		_class1.LoadFromResource ("ClassLib/Class1")
			.Replace ("{Name}", _name);

		_tests.LoadFromResource ("ClassLib/Tests")
			.Replace ("{Name}", _name);
	}


	public void Save ()
	{
		Project.Save ();
		_class1.Save ();

		TestProject.Save ();
		_tests.Save ();
	}
}
