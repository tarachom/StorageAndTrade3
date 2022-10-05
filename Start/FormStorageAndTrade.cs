using Gtk;
using System;
using System.IO;

class FormStorageAndTrade : Window
{
    public ConfigurationParam? OpenConfigurationParam { get; set; }

    public FormStorageAndTrade() : base("Зберігання та Торгівля для України")
    {
        SetDefaultSize(660, 320);
        SetPosition(WindowPosition.Center);

        DeleteEvent += delegate { Application.Quit(); };

        ShowAll();
    }
}