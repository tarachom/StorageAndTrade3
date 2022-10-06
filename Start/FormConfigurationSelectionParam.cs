using Gtk;
using System;

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
        SetDefaultSize(500, 320);
        SetPosition(WindowPosition.Center);
        SetDefaultIconFromFile("configuration.png");
        BorderWidth = 10;

        VBox vbox = new VBox(false, 2);

        ConfName = new Entry();
        Server = new Entry();
        Port = new Entry();
        Login = new Entry();
        Password = new Entry();
        Basename = new Entry();

        AddNameAndField(vbox, "Назва:", ConfName);
        AddNameAndField(vbox, "Сервер:", Server);
        AddNameAndField(vbox, "Порт:", Port);
        AddNameAndField(vbox, "Логін:", Login);
        AddNameAndField(vbox, "Пароль:", Password);
        AddNameAndField(vbox, "База даних:", Basename);

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

        fix.Put(label, 10, 18);
        fix.Put(field, 120, 10);

        vbox.PackStart(fix, false, false, 0);
    }

    void OnButtonSaveClicked(object? sender, EventArgs args)
    {
        int rezult;
        if (!int.TryParse(Port.Text, out rezult))
        {
            MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Close, 
                "Порт має бути цілим числом!");
            md.Run();
            md.Destroy();
            return;
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