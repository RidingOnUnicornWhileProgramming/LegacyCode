using System;
using System.IO;
using System.Reflection;
using DE.API;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
namespace DE.PluginManagement
{
	/// <summary>
	/// Summary description for PluginServices.
	/// </summary>
	public class PanelPluginServices
	{
		/// <summary>
		/// Constructor of the Class
		/// </summary>
		public PanelPluginServices()
		{
		}
		private Types.AvailablePanelPlugins colAvailablePlugins = new Types.AvailablePanelPlugins();

		/// <summary>
		/// A Collection of all Plugins Found and Loaded by the FindPlugins() Method
		/// </summary>
		public Types.AvailablePanelPlugins AvailablePlugins {
			get { return colAvailablePlugins; }
			set { colAvailablePlugins = value; }
		}

		/// <summary>
		/// Searches the Application's Startup Directory for Plugins
		/// </summary>
		public void FindPlugins()
		{
			FindPlugins(AppDomain.CurrentDomain.BaseDirectory + "\\rsc\\Plugins");
		}
		/// <summary>
		/// Searches the passed Path for Plugins
		/// </summary>
		/// <param name="Path">Directory to search for Plugins in</param>
		public void FindPlugins(string Path)
		{
			//First empty the collection, we're reloading them all
			colAvailablePlugins.Clear();

			//Go through all the files in the plugin directory
			foreach (string fileOn in Directory.GetFiles(Path)) {
				FileInfo file = new FileInfo(fileOn);

				//Preliminary check, must be .dll
				if (file.Extension.Equals(".dll")) {
					//Add the 'plugin'
					this.AddPlugin(fileOn);
				}
			}
		}
		public void RegisterInternalPLugin(Type type)
		{
			Types.AvailablePlugin newPlugin = new Types.AvailablePlugin(type);

			//Add the new plugin to our collection here
			this.colAvailablePlugins.Add(newPlugin);
		}
		/*/// <summary>
		/// Unloads and Closes all AvailablePlugins
		/// </summary>
		public void ClosePlugins()
		{
			foreach (Types.AvailablePlugin pluginOn in colAvailablePlugins)
			{
				//Close all plugin instances
				//We call the plugins Dispose sub first incase it has to do
				//Its own cleanup stuff

				//After we give the plugin a chance to tidy up, get rid of it
				pluginOn.Instance = null;
			}

			//Finally, clear our collection of available plugins
			colAvailablePlugins.Clear();
		}*/
			private void AddPlugin(string FileName)
		{

			//Create a new assembly from the plugin file we're adding..
			Assembly pluginAssembly = Assembly.LoadFrom(FileName);
			try {
				//Next we'll loop through all the Types found in the assembly
				foreach (Type pluginType in pluginAssembly.GetTypes()) {
					if (pluginType.IsPublic) { //Only look at public types
						if (!pluginType.IsAbstract) {  //Only look at non-abstract types
							//Gets a type object of the interface we need the plugins to match
							Type typeInterface = pluginType.GetInterface("DE.API.PanelElement", true);

							//Make sure the interface we want to use actually exists
							if (typeInterface != null) {
								//Create a new available plugin since the type implements the PanelElement interface
								Types.AvailablePlugin newPlugin = new Types.AvailablePlugin(pluginAssembly.GetType(pluginType.ToString()));

								//Add the new plugin to our collection here
								this.colAvailablePlugins.Add(newPlugin);

								return;
							}
						}
					}
				}
			} catch (ReflectionTypeLoadException ex) {
				MessageBox.Show(ex.LoaderExceptions[0].ToString());
			}
			MessageBox.Show("Not a plugin: " + FileName);
		}
	}
	namespace Types
	{
		/// <summary>
		/// Collection for AvailablePlugin Type
		/// </summary>
		public class AvailablePanelPlugins : System.Collections.CollectionBase
		{
			//A Simple Home-brew class to hold some info about our Available Plugins

			/// <summary>
			/// Add a Plugin to the collection of Available plugins
			/// </summary>
			/// <param name="pluginToAdd">The Plugin to Add</param>
			public void Add(Types.AvailablePlugin pluginToAdd)
			{
				this.List.Add(pluginToAdd);
			}
			
			/// <summary>
			/// Remove a Plugin to the collection of Available plugins
			/// </summary>
			/// <param name="pluginToRemove">The Plugin to Remove</param>
			public void Remove(Types.AvailablePlugin pluginToRemove)
			{
				this.List.Remove(pluginToRemove);
			}

		}

		/// <summary>
		/// Data Class for Available Plugin.  Holds and instance of the loaded Plugin, as well as the Plugin's Assembly Path
		/// </summary>
		public class AvailablePlugin
		{
			//This is the actual AvailablePlugin object..

			//public List<PanelElement> Instances = new List<PanelElement>();
			private Type type;
			public Type Type { get { return type; } }

			public AvailablePlugin(Type t)
			{
				type = t;
			}

			public PanelElement MakeInstance()
			{
				return (PanelElement)Activator.CreateInstance(type);
			}
		}
	}
}

