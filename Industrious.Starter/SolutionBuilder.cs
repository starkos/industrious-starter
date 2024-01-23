using Industrious.Starter.Projects;

namespace Industrious.Starter;

public class SolutionBuilder
{
	private readonly Configuration _cfg;
	private readonly Solution _solution;

	private readonly ClassLibraryProject _common;
	private readonly ConsoleProject _console;
	private readonly MacOsProject _macOs;
	private readonly IosProject _ios;

	private readonly ISaveable[] _files;


	public SolutionBuilder (Configuration cfg)
	{
		_cfg = cfg;
		_files = new ISaveable[] {
			_solution = new Solution (cfg.Name),
			_common = new ClassLibraryProject ($"{cfg.Name}.Common"),
			_console = new ConsoleProject ($"{cfg.Name}.Console"),
			_macOs = new MacOsProject ($"{cfg.Name}.macOS"),
			_ios = new IosProject ($"{cfg.Name}.iOS")
		};
	}


	public Configuration ApplyUpdates ()
	{
		var versions = new[] {
			() => {
				Console.WriteLine ("Creating solution");
				_solution.Init (_cfg);
			},
			() => {
				Console.WriteLine ("Creating common code library");
				_common.Init ();
				_solution.AddProject (_common.Project.Path, "{E4DCEF92-4272-4329-B946-6BA46249C619}");
				_solution.AddProject (_common.TestProject.Path, "{95FE6095-9FC7-4802-987F-A59B06346BB1}");
			},
			() => {
				Console.WriteLine ("Creating console executable project");
				_console.Init (_cfg);
				_console.Project.AddLocalProjectReference (_common.Project);
				_solution.AddProject (_console.Project.Path, "{548B54F6-AF78-4582-A875-75BE2F0BBA07}");
			},
			() => {
				Console.WriteLine ("Creating macOS project");
				_macOs.Init (_cfg);
				_macOs.Project.AddLocalProjectReference (_common.Project);
				_solution.AddProject (_macOs.Project.Path, "{40B768D8-DCF3-4353-A813-089E779F2E0E}");
			},
			() => {
				Console.WriteLine ("Creating iOS project");
				_ios.Init (_cfg);
				_ios.Project.AddLocalProjectReference (_common.Project);
				_solution.AddProject (_ios.Project.Path, "{3006724E-A8E3-4411-BF05-BB6432A88424}");
			}
		};

		for (var i = _cfg.Version; i < versions.Length; ++i)
			versions[i].Invoke ();

		foreach (var file in _files)
			file.Save ();

		return _cfg.WithVersion (versions.Length);
	}
}
