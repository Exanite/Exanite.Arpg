using System;
using DarkRift.Server;

public class SimpleGamePlugin : Plugin
{
    public SimpleGamePlugin(PluginLoadData pluginLoadData) : base(pluginLoadData) { }

    public override bool ThreadSafe
    {
        get
        {
            return false;
        }
    }

    public override Version Version
    {
        get
        {
            return new Version(1, 0, 0);
        }
    }
}
