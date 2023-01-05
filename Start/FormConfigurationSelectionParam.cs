/*
Copyright (C) 2019-2023 TARAKHOMYN YURIY IVANOVYCH
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
Автор:    Тарахомин Юрій Іванович
Адреса:   Україна, м. Львів
Сайт:     accounting.org.ua
*/

using Gtk;

using AccountingSoftware;

namespace StorageAndTrade
{
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
            SetDefaultIconFromFile("images/form.ico");
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

            Separator separator = new Separator(Orientation.Vertical);
            vbox.PackStart(separator, false, false, 5);

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

            fix.Put(label, 5, 8);
            fix.Put(field, 100, 0);

            vbox.PackStart(fix, false, false, 5);
        }

        bool SaveConfParam()
        {
            int rezult;
            if (!int.TryParse(Port.Text, out rezult))
            {
                Message.Error(this, "Порт має бути цілим числом!");
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
                Close();
        }

        void OnButtonCreateBaseClicked(object? sender, EventArgs args)
        {
            if (OpenConfigurationParam == null)
                return;

            if (SaveConfParam())
            {
                Kernel kernel = new Kernel();

                Exception exception;
                bool IsExistsDatabase = false;

                bool flag = kernel.CreateDatabaseIfNotExist(
                    OpenConfigurationParam.DataBaseServer,
                    OpenConfigurationParam.DataBaseLogin,
                    OpenConfigurationParam.DataBasePassword,
                    OpenConfigurationParam.DataBasePort,
                    OpenConfigurationParam.DataBaseBaseName, out exception, out IsExistsDatabase);

                if (flag)
                    Message.Info(this, "OK.\n\nБаза даних створена або вже існує");
                else
                    Message.Error(this, "Error: " + exception.Message);
            }
        }

        void OnButtonCloseClicked(object? sender, EventArgs args)
        {
            Close();
        }
    }
}