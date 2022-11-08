using Gtk;

using AccountingSoftware;
using Конфа = StorageAndTrade_1_0;

namespace StorageAndTrade
{
    class FormConfigurationSelection : Window
    {
        ListBox listBoxDataBase;
        ScrolledWindow scrolledWindowListBox;

        public FormConfigurationSelection() : base("Зберігання та Торгівля для України | Вибір бази даних")
        {
            SetDefaultSize(660, 320);
            SetPosition(WindowPosition.Center);
            SetDefaultIconFromFile("images/form.ico");

            DeleteEvent += delegate { Application.Quit(); };

            Fixed fix = new Fixed();

            scrolledWindowListBox = new ScrolledWindow();
            scrolledWindowListBox.SetSizeRequest(500, 300);
            scrolledWindowListBox.ShadowType = ShadowType.In;
            scrolledWindowListBox.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            listBoxDataBase = new ListBox();
            listBoxDataBase.SetSizeRequest(500, 300);
            listBoxDataBase.SelectionMode = SelectionMode.Single;
            listBoxDataBase.ButtonPressEvent += OnListBoxDataBaseButtonPress;
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

            Button buttonEdit = new Button("Редагувати");
            buttonEdit.SetSizeRequest(130, 35);
            buttonEdit.Clicked += OnButtonEditClicked;

            Button buttonCopy = new Button("Копіювати");
            buttonCopy.SetSizeRequest(130, 35);
            buttonCopy.Clicked += OnButtonCopyClicked;

            Button buttonDelete = new Button("Видалити");
            buttonDelete.SetSizeRequest(130, 35);
            buttonDelete.Clicked += OnButtonDeleteClicked;

            int buttonAddVPosition = 135;
            int buttonAddHPosition = scrolledWindowListBox.WidthRequest + 20;

            fix.Put(scrolledWindowListBox, 10, 10);
            fix.Put(buttonOpen, buttonAddHPosition, 10);
            fix.Put(buttonConfigurator, buttonAddHPosition, buttonOpen.HeightRequest + 20);
            fix.Put(buttonAdd, buttonAddHPosition, buttonAddVPosition);
            fix.Put(buttonEdit, buttonAddHPosition, buttonAddVPosition += buttonAdd.HeightRequest + 10);
            fix.Put(buttonCopy, buttonAddHPosition, buttonAddVPosition += buttonEdit.HeightRequest + 10);
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

                Label itemLabel = new Label(itemConfigurationParam.ToString());
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
        }

        public void CallBackUpdate(ConfigurationParam itemConfigurationParam)
        {
            ConfigurationParamCollection.UpdateConfigurationParam(itemConfigurationParam);
            ConfigurationParamCollection.SaveConfigurationParamFromXML(ConfigurationParamCollection.PathToXML);

            FillListBoxDataBase(itemConfigurationParam.ConfigurationKey);
        }

        void OnButtonOpenClicked(object? sender, EventArgs args)
        {
            ListBoxRow[] selectedRows = listBoxDataBase.SelectedRows;

            if (selectedRows.Length != 0)
            {
                ConfigurationParam? OpenConfigurationParam = ConfigurationParamCollection.GetConfigurationParam(selectedRows[0].Name);

                if (OpenConfigurationParam == null)
                    return;

                ConfigurationParamCollection.SelectConfigurationParam(selectedRows[0].Name);
                ConfigurationParamCollection.SaveConfigurationParamFromXML(ConfigurationParamCollection.PathToXML);

                string PathToConfXML = System.IO.Path.Combine(AppContext.BaseDirectory, "Confa.xml");

                Exception exception;

                Конфа.Config.Kernel = new Kernel();
                Конфа.Config.KernelBackgroundTask = new Kernel();
                //Конфа.Config.KernelParalelWork = new Kernel();

                //Підключення до бази даних та завантаження конфігурації
                bool flagOpen = Конфа.Config.Kernel.Open(
                        PathToConfXML,
                        OpenConfigurationParam.DataBaseServer,
                        OpenConfigurationParam.DataBaseLogin,
                        OpenConfigurationParam.DataBasePassword,
                        OpenConfigurationParam.DataBasePort,
                        OpenConfigurationParam.DataBaseBaseName, out exception);

                if (!flagOpen)
                {
                    MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.Close,
                        "Error: " + exception.Message);

                    md.Run();
                    md.Destroy();
                    return;
                }

                //Підключення до бази даних для фонових завдань
                bool flagOpenBackgroundTask = Конфа.Config.KernelBackgroundTask.OpenOnlyDataBase(
                        OpenConfigurationParam.DataBaseServer,
                        OpenConfigurationParam.DataBaseLogin,
                        OpenConfigurationParam.DataBasePassword,
                        OpenConfigurationParam.DataBasePort,
                        OpenConfigurationParam.DataBaseBaseName, out exception);

                if (!flagOpenBackgroundTask)
                {
                    MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.Close,
                        "Error: " + exception.Message);

                    md.Run();
                    md.Destroy();
                    return;
                }

                //Підключення до бази даних для паралельної роботи
                // bool flagOpenParalelWork = Конфа.Config.KernelParalelWork.OpenOnlyDataBase(
                // 		OpenConfigurationParam.DataBaseServer,
                // 		OpenConfigurationParam.DataBaseLogin,
                // 		OpenConfigurationParam.DataBasePassword,
                // 		OpenConfigurationParam.DataBasePort,
                // 		OpenConfigurationParam.DataBaseBaseName, out exception);

                // if (!flagOpenParalelWork)
                // {
                // 	//MessageBox.Show(exception.Message);
                // 	return;
                // }

                Конфа.Config.ReadAllConstants();

                Program.GeneralForm = new FormStorageAndTrade();
                Program.GeneralForm.OpenConfigurationParam = ConfigurationParamCollection.GetConfigurationParam(selectedRows[0].Name);
                Program.GeneralForm.Show();

                Hide();
            }
        }

        void OnListBoxDataBaseButtonPress(object? sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress)
                OnButtonEditClicked(null, new EventArgs());
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

        void OnButtonEditClicked(object? sender, EventArgs args)
        {
            ListBoxRow[] selectedRows = listBoxDataBase.SelectedRows;

            if (selectedRows.Length != 0)
            {
                FormConfigurationSelectionParam configurationSelectionParam = new FormConfigurationSelectionParam();
                configurationSelectionParam.OpenConfigurationParam = ConfigurationParamCollection.GetConfigurationParam(selectedRows[0].Name);
                configurationSelectionParam.CallBackUpdate = CallBackUpdate;
                configurationSelectionParam.Show();
            }
        }
    }
}