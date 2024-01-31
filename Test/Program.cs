using AetherUtils.Core;
using AetherUtils.Core.Configuration;
using AetherUtils.Core.Logging;
using NLog;

var manager = new YamlConfigManager("F:\\GitHub\\AetherUtils\\Test\\configTest\\config.yaml");
if (!manager.ConfigExists())
    manager.CreateConfig(); //create and save the configuration if the file does not exist.

manager.Load(); //load the configuration

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

CLogger.Initialize((LogOptions) manager.Get("logOptions"));
Logger log = CLogger.GetCurrentLogger<YamlConfigManager>();
Logger log2 = CLogger.GetCurrentLogger("FooMethod1::GetObject()");

log.Info("Test log message");
log2.Fatal("This is a fatal message! GetObject() returned nothing...");

try
{
    Console.WriteLine("Enter 0: ");
    int num = int.Parse(Console.ReadLine());

    double div = 1 / num;
} catch (Exception ex) { log2.Fatal(ex); }