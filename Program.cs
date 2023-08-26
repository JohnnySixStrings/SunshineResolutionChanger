using System.CommandLine;
using System.Runtime.InteropServices;
using SunshineResolutionChanger;

//Handling Interoping with native c librarys\    
[DllImport("user32.dll", CharSet = CharSet.Auto)]
static extern bool EnumDisplaySettings(string? deviceName, int modeNum, ref DEVMODE devMode);
[DllImport("user32.dll", CharSet = CharSet.Auto)]
static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

//Setting up commandline arguments
string[] widthAliases = new string[] { "-w", "--width" };
var widthOption = new Option<int>(
    aliases: widthAliases,
    "width resolution for screen 1920, 3840 etc..")
{
    IsRequired = true
};

string[] heightAliases = new string[] { "-h", "--height" };
var heightOption = new Option<int>(
    aliases: heightAliases,
    "height resolution for screen 1080, 2160 etc..")
{
    IsRequired = true
};


//Setting up the root command
var rootCommand = new RootCommand();
rootCommand.AddOption(widthOption);
rootCommand.AddOption(heightOption);

rootCommand.SetHandler(
    (width, height) =>
    {
        var devmode = new DEVMODE();
        EnumDisplaySettings(null, 0, ref devmode);
        devmode.dmPelsWidth = width;
        devmode.dmPelsHeight = height;
        _ = ChangeDisplaySettings(ref devmode, 0);
    },
    widthOption,
    heightOption);

return rootCommand.Invoke(args);