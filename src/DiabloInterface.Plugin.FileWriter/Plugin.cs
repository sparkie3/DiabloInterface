using System;
using System.Reflection;
using Zutatensuppe.D2Reader;
using Zutatensuppe.DiabloInterface.Core.Logging;
using Zutatensuppe.DiabloInterface.Plugin.FileWriter.Writer;

namespace Zutatensuppe.DiabloInterface.Plugin.FileWriter
{
    public class Plugin : BasePlugin
    {
        private readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        public override string Name => "Filewriter";

        internal Config Config { get; private set; } = new Config();

        protected override Type SettingsRendererType => typeof(SettingsRenderer);

        public override void SetConfig(IPluginConfig c)
        {
            Config = c as Config;
            ApplyConfig();
        }

        public override void Initialize(DiabloInterface di)
        {
            Logger.Info("Creating character stat file writer service.");

            SetConfig(di.settings.CurrentSettings.PluginConf(Name));
            di.game.DataRead += Game_DataRead;
        }

        private void Game_DataRead(object sender, DataReadEventArgs e)
        {
            if (!Config.Enabled) return;

            var fileWriter = new TextFileWriter();
            var statWriter = new CharacterStatFileWriter(fileWriter, Config.FileFolder);
            var stats = new CharacterStats(e.Character);

            statWriter.WriteFiles(stats);
        }
    }
}
