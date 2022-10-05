using Gtk;
using System;
using System.IO;

class FormConfigurationSelection : Window
{
    ListBox listBoxDataBase;
    ScrolledWindow scrolledWindowListBox;

    public FormConfigurationSelection() : base("Зберігання та Торгівля для України | Вибір бази даних")
    {
        SetDefaultSize(660, 320);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };

        Fixed fix = new Fixed();

        scrolledWindowListBox = new ScrolledWindow();
        scrolledWindowListBox.SetSizeRequest(500, 300);
        scrolledWindowListBox.ShadowType = ShadowType.In;
        scrolledWindowListBox.SetPolicy(PolicyType.Never, PolicyType.Automatic);

        listBoxDataBase = new ListBox();
        listBoxDataBase.SetSizeRequest(500, 300);
        listBoxDataBase.SelectionMode = SelectionMode.Single;
        listBoxDataBase.SelectedRowsChanged += OnListBoxDataBaseChanged;
        scrolledWindowListBox.Add(listBoxDataBase);

        Button buttonOpen = new Button("Відкрити");
        buttonOpen.SetSizeRequest(130, 35);
        buttonOpen.Clicked += OnButtonOpenClicked;

        Button buttonConfigurator = new Button("Конфігуратор");
        buttonConfigurator.SetSizeRequest(130, 35);
        buttonConfigurator.Clicked += OnButtonConfiguratorClicked;

        Button buttonAdd = new Button("Додати");
        buttonAdd.SetSizeRequest(130, 35);
        buttonAdd.Clicked += OnButtonAddClicked;

        Button buttonCopy = new Button("Копіювати");
        buttonCopy.SetSizeRequest(130, 35);
        buttonCopy.Clicked += OnButtonCopyClicked;

        Button buttonDelete = new Button("Видалити");
        buttonDelete.SetSizeRequest(130, 35);
        buttonDelete.Clicked += OnButtonDeleteClicked;

        int buttonAddVPosition = 180;
        int buttonAddHPosition = scrolledWindowListBox.WidthRequest + 20;

        fix.Put(scrolledWindowListBox, 10, 10);
        fix.Put(buttonOpen, buttonAddHPosition, 10);
        fix.Put(buttonConfigurator, buttonAddHPosition, buttonOpen.HeightRequest + 20);
        fix.Put(buttonAdd, buttonAddHPosition, buttonAddVPosition);
        fix.Put(buttonCopy, buttonAddHPosition, buttonAddVPosition += buttonAdd.HeightRequest + 10);
        fix.Put(buttonDelete, buttonAddHPosition, buttonAddVPosition += buttonCopy.HeightRequest + 10);
        Add(fix);

        ShowAll();

        LoadConfigurationParam();
        FillListBoxDataBase();
    }

    private void LoadConfigurationParam()
    {
        ConfigurationParamCollection.PathToXML = System.IO.Path.Combine(AppContext.BaseDirectory, "ConfigurationParam.xml");
        ConfigurationParamCollection.LoadConfigurationParamFromXML(ConfigurationParamCollection.PathToXML);
    }

    private void FillListBoxDataBase(string selectConfKey = "")
    {
        foreach (Widget child in listBoxDataBase.Children)
            listBoxDataBase.Remove(child);

        foreach (ConfigurationParam itemConfigurationParam in ConfigurationParamCollection.ListConfigurationParam!)
        {
            ListBoxRow row = new ListBoxRow();
            row.Name = itemConfigurationParam.ConfigurationKey;

            Label itemLabel = new Label(itemConfigurationParam.ConfigurationName);
            itemLabel.Halign = Align.Start;

            row.Add(itemLabel);
            listBoxDataBase.Add(row);

            if (!String.IsNullOrEmpty(selectConfKey))
            {
                if (itemConfigurationParam.ConfigurationKey == selectConfKey)
                    listBoxDataBase.SelectRow(row);
            }
            else
            {
                if (itemConfigurationParam.Select)
                    listBoxDataBase.SelectRow(row);
            }
        }

        listBoxDataBase.ShowAll();

        if (listBoxDataBase.Children.Length != 0 && listBoxDataBase.SelectedRow == null)
        {
            ListBoxRow row = (ListBoxRow)listBoxDataBase.Children[0];
            listBoxDataBase.SelectRow(row);
        }

        //scrolledWindowListBox.Vadjustment.Value = scrolledWindowListBox.Vadjustment.Upper;
    }

    void OnButtonOpenClicked(object? sender, EventArgs args)
    {
        ListBoxRow[] selectedRows = listBoxDataBase.SelectedRows;

        if (selectedRows.Length != 0)
        {
            Hide();
 
            ConfigurationParamCollection.SelectConfigurationParam(selectedRows[0].Name);
            ConfigurationParamCollection.SaveConfigurationParamFromXML(ConfigurationParamCollection.PathToXML);

            FormStorageAndTrade storageAndTrade = new FormStorageAndTrade();
            storageAndTrade.OpenConfigurationParam = ConfigurationParamCollection.GetConfigurationParam(selectedRows[0].Name);
            storageAndTrade.Show();
        }
    }

    void OnButtonConfiguratorClicked(object? sender, EventArgs args)
    {
        ListBoxRow[] selectedRows = listBoxDataBase.SelectedRows;

        if (selectedRows.Length != 0)
        {
            //selectedRows[0].Name
        }
    }

    void OnButtonAddClicked(object? sender, EventArgs args)
    {
        ConfigurationParam itemConfigurationParam = ConfigurationParam.New();
        ConfigurationParamCollection.ListConfigurationParam?.Add(itemConfigurationParam);

        ConfigurationParamCollection.SaveConfigurationParamFromXML(ConfigurationParamCollection.PathToXML);
        FillListBoxDataBase(itemConfigurationParam.ConfigurationKey);
    }

    void OnButtonCopyClicked(object? sender, EventArgs args)
    {
        ListBoxRow[] selectedRows = listBoxDataBase.SelectedRows;

        if (selectedRows.Length != 0)
        {
            ConfigurationParam? itemConfigurationParam = ConfigurationParamCollection.GetConfigurationParam(selectedRows[0].Name);
            if (itemConfigurationParam != null)
            {
                ConfigurationParam copyConfigurationParam = itemConfigurationParam.Clone();
                ConfigurationParamCollection.ListConfigurationParam?.Add(copyConfigurationParam);

                ConfigurationParamCollection.SaveConfigurationParamFromXML(ConfigurationParamCollection.PathToXML);
                FillListBoxDataBase(itemConfigurationParam.ConfigurationKey);
            }
        }
    }

    void OnButtonDeleteClicked(object? sender, EventArgs args)
    {
        ListBoxRow[] selectedRows = listBoxDataBase.SelectedRows;

        if (selectedRows.Length != 0)
        {
            if (ConfigurationParamCollection.RemoveConfigurationParam(selectedRows[0].Name))
            {
                ConfigurationParamCollection.SaveConfigurationParamFromXML(ConfigurationParamCollection.PathToXML);
                FillListBoxDataBase();
            }
        }
    }

    void OnListBoxDataBaseChanged(object? sender, EventArgs args)
    {
        if (sender != null)
        {
            ListBox lb = (ListBox)sender;
            ListBoxRow[] selectedRows = lb.SelectedRows;

            if (selectedRows.Length != 0)
            {
                //label.Text = selectedRows[0].Name;
            }
        }
    }


}