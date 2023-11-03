using System.Text.Json;

namespace Industrious.Starter;

///////////////////////////////////////////////////////////////////////////////////////////
/// <summary>
/// Configuration and state information about the currently generated solution.
/// </summary>
///////////////////////////////////////////////////////////////////////////////////////////
public class Configuration
{
	public Configuration(String name, String title)
	{
		Name = name;
		Title = title;
		Version = 0;
	}


	public String Name { get; set; }
	public String Title { get; set; }
	public Int32 Version { get; set; }

	public static Configuration? Load(String path)
	{
		try
		{
			var json = File.ReadAllText(path);
			return JsonSerializer.Deserialize<Configuration>(json);
		}
		catch (FileNotFoundException)
		{
			return null;
		}
	}


	public void Save(String path)
	{
		var options = new JsonSerializerOptions { WriteIndented = true };
		var json = JsonSerializer.Serialize(this, options);
		File.WriteAllText(path, json);
	}
}
