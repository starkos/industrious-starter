using System.Text.Json;

namespace Industrious.Starter;

///////////////////////////////////////////////////////////////////////////////////////////
/// <summary>
///  Workspace settings which are serialized and persisted between runs.
/// </summary>
///////////////////////////////////////////////////////////////////////////////////////////
public class Configuration
{
	public Configuration (Workspace workspace)
	{
		Name = workspace.Name;
		Title = workspace.ApplicationTitle;
		Company = workspace.CompanyName;
		Identifier = workspace.CompanyIdentifier;
		Version = workspace.CurrentVersion;
	}

	public String Name { get; set; }
	public String Title { get; set; }
	public String Company { get; set; }
	public String Identifier { get; set; }
	public Int32 Version { get; set; }


	public static Configuration? Load (String fileName)
	{
		try
		{
			var json = File.ReadAllText (fileName);
			return JsonSerializer.Deserialize<Configuration> (json);
		}
		catch (FileNotFoundException)
		{
			return null;
		}
	}


	public void Save (String fileName)
	{
		var options = new JsonSerializerOptions { WriteIndented = true };
		var json = JsonSerializer.Serialize (this, options);
		File.WriteAllText (fileName, json);
	}
}
