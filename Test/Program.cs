using AetherUtils.Core;
using AetherUtils.Core.Configuration;

var manager = new YamlConfigManager("F:\\GitHub\\AetherUtils\\Test\\configTest\\config.yaml");
manager.CreateConfig();
manager.Load();
//manager.Set("connectionString", "This is a test!");
manager.Set("licenseFile", "This is another test!!!");
Console.WriteLine(manager.Get("licenseFile") as string);
Console.WriteLine("test");
manager.Save();

public class YamlConfigManager(string configFilePath) : ConfigManager<DefaultConfig>(configFilePath)
{
    public void CreateConfig()
    {
        CurrentConfig = new DefaultConfig();
    }
}