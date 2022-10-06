using Gtk;
using System;

class FormConfigurationSelectionParam : Window
{
    Entry name;
    Entry server;
    Entry port;
    Entry login;
    Entry password;
    Entry basename;

    public FormConfigurationSelectionParam() : base("Параметри підключення PostgreSQL")
    {
        SetDefaultSize(500, 320);
        SetPosition(WindowPosition.Center);
        BorderWidth = 10;

        VBox vbox = new VBox(false, 2);

        name = new Entry();
        server = new Entry();
        port = new Entry();
        login = new Entry();
        password = new Entry();
        basename = new Entry();

        AddNameAndField(vbox, "Назва: ", name);
        AddNameAndField(vbox, "Сервер:", server);
        AddNameAndField(vbox, "Порт: ", port);
        AddNameAndField(vbox, "Логін:", login);
        AddNameAndField(vbox, "Пароль: ", password);
        AddNameAndField(vbox, "База даних:", basename);

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

        vbox.PackStart(hBoxButton, false, false, 15);

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
                name.Text = mOpenConfigurationParam.ConfigurationName;
                server.Text = mOpenConfigurationParam.DataBaseServer;
                port.Text = mOpenConfigurationParam.DataBasePort.ToString();
                login.Text = mOpenConfigurationParam.DataBaseLogin;
                password.Text = mOpenConfigurationParam.DataBasePassword;
                basename.Text = mOpenConfigurationParam.DataBaseBaseName;
            }
        }
    }

    private void AddNameAndField(VBox vbox, string name, Entry field)
    {
        Fixed fix = new Fixed();

        Label label = new Label(name);

        field.SetSizeRequest(300, 0);

        fix.Put(label, 10, 18);
        fix.Put(field, 120, 10);

        vbox.PackStart(fix, false, false, 0);
    }

    void OnButtonSaveClicked(object? sender, EventArgs args)
    {
        int rezult;
        if (!int.TryParse(port.Text, out rezult))
        {
            MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Close, "Порт має бути цілим числом!");
            md.Run();
            md.Destroy();
            return;
        }

        if (OpenConfigurationParam != null && CallBackUpdate != null)
        {
            OpenConfigurationParam.ConfigurationName = name.Text;
            OpenConfigurationParam.DataBaseServer = server.Text;
            OpenConfigurationParam.DataBaseLogin = login.Text;
            OpenConfigurationParam.DataBasePassword = password.Text;
            OpenConfigurationParam.DataBasePort = int.Parse(port.Text);
            OpenConfigurationParam.DataBaseBaseName = basename.Text;

            CallBackUpdate.Invoke(OpenConfigurationParam);

            Close();
        }
    }

    void OnButtonCreateBaseClicked(object? sender, EventArgs args)
    {

    }

    void OnButtonCloseClicked(object? sender, EventArgs args)
    {
        Close();
    }
}