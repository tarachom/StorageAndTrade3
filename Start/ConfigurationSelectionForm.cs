using Gtk;
using System;
using System.IO;

class ConfigurationSelectionForm : Window
{
    ListBox listBoxDataBase;
    Label label;

    public ConfigurationSelectionForm() : base("Зберігання та Торгівля для України")
    {
        SetDefaultSize(650, 350);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        listBoxDataBase = new ListBox();
        listBoxDataBase.SetSizeRequest(500, 300);
        listBoxDataBase.SelectionMode = SelectionMode.Single;
        listBoxDataBase.SelectedRowsChanged += OnChanged;

        label = new Label();

        FillListBoxDataBase();

        fix.Put(listBoxDataBase, 10, 10);
        fix.Put(label, 10, 310);
        Add(fix);

        ShowAll();
    }

    private void FillListBoxDataBase()
    {
        string pathToXML = System.IO.Path.Combine(AppContext.BaseDirectory, "ConfigurationParam.xml");

        ConfigurationParamCollection.LoadConfigurationParamFromXML(pathToXML);

        foreach (ConfigurationParam itemConfigurationParam in ConfigurationParamCollection.ListConfigurationParam!)
        {
            ListBoxRow row = new ListBoxRow();
            row.Name = itemConfigurationParam.ConfigurationKey;

            Label itemLabel = new Label(itemConfigurationParam.ConfigurationName);
            itemLabel.Halign = Align.Start;

            row.Add(itemLabel);
            listBoxDataBase.Add(row);
        }
    }

    void OnChanged(object? sender, EventArgs args)
    {
        if (sender != null)
        {
            ListBox lb = (ListBox)sender;
            ListBoxRow[] selectedRows = lb.SelectedRows;

            if (selectedRows.Length != 0)
            {
                label.Text = selectedRows[0].Name;
            }
        }
    }
}