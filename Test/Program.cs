using AetherUtils.Core;
using AetherUtils.Core.Configuration;

var manager = new YamlConfigManager("F:\\GitHub\\AetherUtils\\Test\\configTest\\config.yaml");
//manager.CreateConfig();
manager.Load();
Console.WriteLine(manager.Get("testProp"));
Console.WriteLine(manager.Get("testPropListDoubles[1]"));
Console.WriteLine(manager.Get("testPropListInts[2]"));
//manager.Set("connectionString", "Yet another test2!");
//manager.Set("licenseFile", "Gobeldigook 3!");
////manager.Set("logFileDirectory", "%TEMP%\\logs2");
//manager.Set("newFileEveryLaunch", true);
////Console.WriteLine(manager.Get("includeDateOnly"));
////Console.WriteLine("test");

//manager.Set("testPropList[1]", "Changed Item 2");
//manager.Set("testPropListInts[3]", 27);
//bool success = manager.Save();
//Console.WriteLine(success);



public class YamlConfigManager(string configFilePath) : ConfigManager<DefaultConfig>(configFilePath)
{
    public void CreateConfig()
    {
        CurrentConfig = new DefaultConfig();
    }
}