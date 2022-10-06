using Gtk;
using System;

using AccountingSoftware;
using Конфа = StorageAndTrade_1_0;

class FormConfigurationSelectionParam : Window
{
    Entry ConfName;
    Entry Server;
    Entry Port;
    Entry Login;
    Entry Password;
    Entry Basename;

    public FormConfigurationSelectionParam() : base("Параметри підключення PostgreSQL")
    {
        SetDefaultSize(420, 0);
        SetPosition(WindowPosition.Center);
        SetDefaultIconFromFile("configuration.png");
        BorderWidth = 5;

        VBox vbox = new VBox(false, 2);

        ConfName = new Entry();
        Server = new Entry();
        Port = new Entry();
        Login = new Entry();
        Password = new Entry();
        Basename = new Entry();

        AddNameAndField(vbox, "Назва", ConfName);
        AddNameAndField(vbox, "Сервер", Server);
        AddNameAndField(vbox, "Порт", Port);
        AddNameAndField(vbox, "Логін", Login);
        AddNameAndField(vbox, "Пароль", Password);
        AddNameAndField(vbox, "База даних", Basename);

        HBox hBoxButton = new HBox();

        Button buttonSave = new Button("Зберегти");
        buttonSave.SetSizeRequest(0, 35);
        buttonSave.Clicked += OnButtonSaveClicked;

        Button buttonCreateBase = new Button("Створити базу даних");
        buttonCreateBase.SetSizeRequest(0, 35);
        buttonCreateBase.Clicked += OnButtonCreateBaseClicked;

        Button buttonClose = new Button("Закрити");
        buttonClose.SetSizeRequest(0, 35);
        buttonClose.Clicked += OnButtonCloseClicked;

        hBoxButton.PackStart(buttonSave, false, false, 5);
        hBoxButton.PackStart(buttonCreateBase, false, false, 5);
        hBoxButton.PackStart(buttonClose, false, false, 5);

        vbox.PackStart(hBoxButton, false, false, 5);

        Add(vbox);
        ShowAll();
    }

    public System.Action<ConfigurationParam>? CallBackUpdate { get; set; }

    private ConfigurationParam? mOpenConfigurationParam;
    public ConfigurationParam? OpenConfigurationParam
    {
        get { return mOpenConfigurationParam; }
        set
        {
            mOpenConfigurationParam = value;

            if (mOpenConfigurationParam != null)
            {
                ConfName.Text = mOpenConfigurationParam.ConfigurationName;
                Server.Text = mOpenConfigurationParam.DataBaseServer;
                Port.Text = mOpenConfigurationParam.DataBasePort.ToString();
                Login.Text = mOpenConfigurationParam.DataBaseLogin;
                Password.Text = mOpenConfigurationParam.DataBasePassword;
                Basename.Text = mOpenConfigurationParam.DataBaseBaseName;
            }
        }
    }

    private void AddNameAndField(VBox vbox, string name, Entry field)
    {
        Fixed fix = new Fixed();

        Label label = new Label(name);

        field.SetSizeRequest(300, 0);

        fix.Put(label, 5, 18);
        fix.Put(field, 100, 10);

        vbox.PackStart(fix, false, false, 0);
    }

    bool SaveConfParam()
    {
        int rezult;
        if (!int.TryParse(Port.Text, out rezult))
        {
            MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Close,
                "Порт має бути цілим числом!");
            md.Run();
            md.Destroy();
            return false;
        }

        if (OpenConfigurationParam != null && CallBackUpdate != null)
        {
            OpenConfigurationParam.ConfigurationName = ConfName.Text;
            OpenConfigurationParam.DataBaseServer = Server.Text;
            OpenConfigurationParam.DataBaseLogin = Login.Text;
            OpenConfigurationParam.DataBasePassword = Password.Text;
            OpenConfigurationParam.DataBasePort = int.Parse(Port.Text);
            OpenConfigurationParam.DataBaseBaseName = Basename.Text;

            CallBackUpdate.Invoke(OpenConfigurationParam);

            return true;
        }
        else
            return false;
    }

    void OnButtonSaveClicked(object? sender, EventArgs args)
    {
        if (SaveConfParam())
        {
            Close();
        }
    }

    void OnButtonCreateBaseClicked(object? sender, EventArgs args)
    {
        if (SaveConfParam())
        {
            Конфа.Config.Kernel = new Kernel();

            Exception exception;
            bool IsExistsDatabase = false;

            bool flag = Конфа.Config.Kernel.CreateDatabaseIfNotExist(
                OpenConfigurationParam.DataBaseServer,
                OpenConfigurationParam.DataBaseLogin,
                OpenConfigurationParam.DataBasePassword,
                OpenConfigurationParam.DataBasePort,
                OpenConfigurationParam.DataBaseBaseName, out exception, out IsExistsDatabase);

            if (flag)
            {
                MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Close,
                "OK. База даних створена або вже існує");
                md.Run();
                md.Destroy();
            }
            else
            {
                MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Close,
                "Error: " + exception.Message);
                md.Run();
                md.Destroy();
            }
        }
    }

    void OnButtonCloseClicked(object? sender, EventArgs args)
    {
        Close();
    }
}