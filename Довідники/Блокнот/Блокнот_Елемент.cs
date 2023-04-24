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

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Блокнот_Елемент : VBox
    {
        public Блокнот? PageList { get; set; }
        public System.Action<Блокнот_Pointer>? CallBack_OnSelectPointer { get; set; }

        public bool IsNew { get; set; } = true;

        public Блокнот_Objest Блокнот_Objest { get; set; } = new Блокнот_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 800 };
        DateTimeControl ДатаЗапису = new DateTimeControl();
        TextView Опис = new TextView();
        Entry Лінк = new Entry() { WidthRequest = 800 };

        public Блокнот_Елемент() : base()
        {
            HBox hBox = new HBox();

            Button bSaveAndClose = new Button("Зберегти та закрити");
            bSaveAndClose.Clicked += (object? sender, EventArgs args) => { Save(true); };
            hBox.PackStart(bSaveAndClose, false, false, 10);

            Button bSave = new Button("Зберегти");
            bSave.Clicked += (object? sender, EventArgs args) => { Save(); };
            hBox.PackStart(bSave, false, false, 10);

            PackStart(hBox, false, false, 10);

            HPaned hPaned = new HPaned() { BorderWidth = 5, Position = 800 };

            CreatePack1(hPaned);
            CreatePack2(hPaned);

            PackStart(hPaned, false, false, 5);

            ShowAll();
        }

        void CreatePack1(HPaned hPaned)
        {
            VBox vBox = new VBox();
            hPaned.Pack1(vBox, false, false);

            //Код + ДатаЗапису
            HBox hBoxCode = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxCode, false, false, 5);

            hBoxCode.PackStart(new Label("Код:"), false, false, 5);
            hBoxCode.PackStart(Код, false, false, 5);
            hBoxCode.PackStart(ДатаЗапису, false, false, 5);

            //Назва
            HBox hBoxName = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxName, false, false, 5);

            hBoxName.PackStart(new Label("Назва:"), false, false, 5);
            hBoxName.PackStart(Назва, false, false, 5);

            //Опис
            HBox hBoxOpys = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxOpys, false, false, 5);

            hBoxOpys.PackStart(new Label("Опис:") { Valign = Align.Start }, false, false, 5);

            ScrolledWindow scrollTextViewOpys = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = 800, HeightRequest = 500 };
            scrollTextViewOpys.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollTextViewOpys.Add(Опис);

            hBoxOpys.PackStart(scrollTextViewOpys, false, false, 5);

            //Лінк
            HBox hBoxLink = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxLink, false, false, 5);

            hBoxLink.PackStart(new Label("Лінк:"), false, false, 5);
            hBoxLink.PackStart(Лінк, false, false, 5);
        }

        void CreatePack2(HPaned hPaned)
        {
            VBox vBox = new VBox();



            hPaned.Pack2(vBox, false, false);
        }

        #region Присвоєння / зчитування значень

        public void SetValue()
        {
            if (IsNew)
                Блокнот_Objest.New();

            Код.Text = Блокнот_Objest.Код;
            Назва.Text = Блокнот_Objest.Назва;
            ДатаЗапису.Value = Блокнот_Objest.ДатаЗапису;
            Опис.Buffer.Text = Блокнот_Objest.Опис;
            Лінк.Text = Блокнот_Objest.Лінк;
        }

        void GetValue()
        {
            Блокнот_Objest.Код = Код.Text;
            Блокнот_Objest.Назва = Назва.Text;
            Блокнот_Objest.ДатаЗапису = ДатаЗапису.Value;
            Блокнот_Objest.Опис = Опис.Buffer.Text;
            Блокнот_Objest.Лінк = Лінк.Text;
        }

        #endregion

        void Save(bool closePage = false)
        {
            GetValue();

            bool isSave = false;

            try
            {
                isSave = Блокнот_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомленняПроПомилку(DateTime.Now, "Запис",
                    Блокнот_Objest.UnigueID.UGuid, "Довідники", Блокнот_Objest.Назва, ex.Message);

                ФункціїДляПовідомлень.ВідкритиТермінал();
            }

            if (!isSave)
            {
                Message.Info(Program.GeneralForm, "Не вдалось записати");
                return;
            }

            if (closePage)
                Program.GeneralForm?.CloseCurrentPageNotebook();
            else
                Program.GeneralForm?.RenameCurrentPageNotebook($"Блокнот: {Блокнот_Objest.Назва}");

            if (CallBack_OnSelectPointer != null)
                CallBack_OnSelectPointer.Invoke(Блокнот_Objest.GetDirectoryPointer());

            if (PageList != null)
            {
                PageList.SelectPointerItem = Блокнот_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}