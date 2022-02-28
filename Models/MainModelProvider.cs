using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace PathFinder.Models
{
	public static class MainModelProvider
	{
		private static string SettingsPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings.json");

		public static MainModel Load()
		{
			MainModel model;
			if(!File.Exists(SettingsPath))
			{
				model = new MainModel();
				model.AddPage();
				Save(model);
			}
			else
			{
				model = JsonConvert.DeserializeObject<MainModel>(File.ReadAllText(SettingsPath));
				if(!model.Pages.Any())
					model.AddPage();
			}
			return model;
		}

		public static void Save(MainModel model)
		{
			File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(model, Formatting.Indented));
		}
	}
}