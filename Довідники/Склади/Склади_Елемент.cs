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
using Перелічення = StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class Склади_Елемент : VBox
    {
        public Склади? PageList { get; set; }
        public System.Action<Склади_Pointer>? CallBack_OnSelectPointer { get; set; }

        public bool IsNew { get; set; } = true;

        public Склади_Папки_Pointer РодичДляНового { get; set; } = new Склади_Папки_Pointer();

        public Склади_Objest Склади_Objest { get; set; } = new Склади_Objest();

        #region Field

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Склади_Папки_PointerControl Родич = new Склади_Папки_PointerControl() { Caption = "Папка:" };
        ComboBoxText ТипСкладу = new ComboBoxText();
        ВидиЦін_PointerControl ВидЦін = new ВидиЦін_PointerControl();
        ComboBoxText НалаштуванняАдресногоЗберігання = new ComboBoxText();
        Склади_ТабличнаЧастина_Контакти Контакти = new Склади_ТабличнаЧастина_Контакти();

        #endregion

        public Склади_Елемент() : base()
        {
            HBox hBox = new HBox();

            Button bSaveAndClose = new Button("Зберегти та закрити");
            bSaveAndClose.Clicked += (object? sender, EventArgs args) => { Save(true); };
            hBox.PackStart(bSaveAndClose, false, false, 10);

            Button bSave = new Button("Зберегти");
            bSave.Clicked += (object? sender, EventArgs args) => { Save(); };
            hBox.PackStart(bSave, false, false, 10);

            PackStart(hBox, false, false, 10);

            HPaned hPaned = new HPaned() { BorderWidth = 5, Position = 500 };

            CreatePack1(hPaned);
            CreatePack2(hPaned);

            PackStart(hPaned, false, false, 5);

            ShowAll();
        }

        void CreatePack1(HPaned hPaned)
        {
            VBox vBox = new VBox();

            //Код
            HBox hBoxCode = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxCode, false, false, 5);

            hBoxCode.PackStart(new Label("Код:"), false, false, 5);
            hBoxCode.PackStart(Код, false, false, 5);

            //Назва
            HBox hBoxName = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxName, false, false, 5);

            hBoxName.PackStart(new Label("Назва:"), false, false, 5);
            hBoxName.PackStart(Назва, false, false, 5);

            //Родич
            HBox hBoxParent = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxParent, false, false, 5);

            hBoxParent.PackStart(Родич, false, false, 5);

            hPaned.Pack1(vBox, false, false);

            //ВидЦін
            HBox hBoxVidCen = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxVidCen, false, false, 5);

            hBoxVidCen.PackStart(ВидЦін, false, false, 5);

            //Тип складу
            HBox hBoxType = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxType, false, false, 5);

            foreach (var field in Перелічення.ПсевдонімиПерелічення.ТипиСкладів_Array())
                ТипСкладу.Append(field.Value.ToString(), field.Name);

            hBoxType.PackStart(new Label("Тип складу:"), false, false, 5);
            hBoxType.PackStart(ТипСкладу, false, false, 5);

            //НалаштуванняАдресногоЗберігання
            HBox hBoxAdressSave = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxAdressSave, false, false, 5);

            foreach (var field in Перелічення.ПсевдонімиПерелічення.НалаштуванняАдресногоЗберігання_Array())
                НалаштуванняАдресногоЗберігання.Append(field.Value.ToString(), field.Name);

            hBoxAdressSave.PackStart(new Label("Адресне зберігання:"), false, false, 5);
            hBoxAdressSave.PackStart(НалаштуванняАдресногоЗберігання, false, false, 5);

            hPaned.Pack1(vBox, false, false);
        }

        void CreatePack2(HPaned hPaned)
        {
            VBox vBox = new VBox();

            HBox hBox = new HBox();
            hBox.PackStart(new Label("Контакти:"), false, false, 5);
            vBox.PackStart(hBox, false, false, 5);

            HBox hBoxContakty = new HBox();
            hBoxContakty.PackStart(Контакти, true, true, 5);

            vBox.PackStart(hBoxContakty, false, false, 0);
            hPaned.Pack2(vBox, false, false);
        }

        #region Присвоєння / зчитування значень

        public void SetValue()
        {
            if (IsNew)
            {
                Склади_Objest.New();
                Склади_Objest.Папка = РодичДляНового;
            }

            Код.Text = Склади_Objest.Код;
            Назва.Text = Склади_Objest.Назва;
            Родич.Pointer = Склади_Objest.Папка;
            ВидЦін.Pointer = Склади_Objest.ВидЦін;
            ТипСкладу.ActiveId = Склади_Objest.ТипСкладу.ToString();
            НалаштуванняАдресногоЗберігання.ActiveId = Склади_Objest.НалаштуванняАдресногоЗберігання.ToString();

            if (ТипСкладу.Active == -1)
                ТипСкладу.ActiveId = Перелічення.ТипиСкладів.Гуртовий.ToString();

            if (НалаштуванняАдресногоЗберігання.Active == -1)
                НалаштуванняАдресногоЗберігання.ActiveId = Перелічення.НалаштуванняАдресногоЗберігання.НеВикористовувати.ToString();

            Контакти.Склади_Objest = Склади_Objest;
            Контакти.LoadRecords();
        }

        void GetValue()
        {
            Склади_Objest.Код = Код.Text;
            Склади_Objest.Назва = Назва.Text;
            Склади_Objest.Папка = Родич.Pointer;
            Склади_Objest.ВидЦін = ВидЦін.Pointer;
            Склади_Objest.ТипСкладу = Enum.Parse<Перелічення.ТипиСкладів>(ТипСкладу.ActiveId);
            Склади_Objest.НалаштуванняАдресногоЗберігання = Enum.Parse<Перелічення.НалаштуванняАдресногоЗберігання>(НалаштуванняАдресногоЗберігання.ActiveId);
        }

        #endregion

        void Save(bool closePage = false)
        {
            GetValue();

            bool isSave = false;

            try
            {
                isSave = Склади_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомленняПроПомилку(DateTime.Now, "Запис",
                    Склади_Objest.UnigueID.UGuid, "Довідники", Склади_Objest.Назва, ex.Message);

                ФункціїДляПовідомлень.ВідкритиТермінал();
            }

            if (!isSave)
            {
                Message.Info(Program.GeneralForm, "Не вдалось записати");
                return;
            }

            Контакти.SaveRecords();

            if (closePage)
                Program.GeneralForm?.CloseCurrentPageNotebook();
            else
                Program.GeneralForm?.RenameCurrentPageNotebook($"{Склади_Objest.Назва}");

            if (CallBack_OnSelectPointer != null)
                CallBack_OnSelectPointer.Invoke(Склади_Objest.GetDirectoryPointer());

            if (PageList != null)
            {
                PageList.SelectPointerItem = Склади_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}