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
    class ВидиНоменклатури_Елемент : VBox
    {
        public ВидиНоменклатури? PageList { get; set; }
        public System.Action<ВидиНоменклатури_Pointer>? CallBack_OnSelectPointer { get; set; }

        public bool IsNew { get; set; } = true;

        public ВидиНоменклатури_Objest ВидиНоменклатури_Objest { get; set; } = new ВидиНоменклатури_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        ПакуванняОдиниціВиміру_PointerControl ОдиницяВиміру = new ПакуванняОдиниціВиміру_PointerControl();

        public ВидиНоменклатури_Елемент() : base()
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

            //ОдиницяВиміру
            HBox hBoxPackuvannja = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxPackuvannja, false, false, 5);

            hBoxPackuvannja.PackStart(ОдиницяВиміру, false, false, 5);

            hPaned.Pack1(vBox, false, false);
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
                ВидиНоменклатури_Objest.New();

            Код.Text = ВидиНоменклатури_Objest.Код;
            Назва.Text = ВидиНоменклатури_Objest.Назва;
            ОдиницяВиміру.Pointer = ВидиНоменклатури_Objest.ОдиницяВиміру;
        }

        void GetValue()
        {
            ВидиНоменклатури_Objest.Код = Код.Text;
            ВидиНоменклатури_Objest.Назва = Назва.Text;
            ВидиНоменклатури_Objest.ОдиницяВиміру = ОдиницяВиміру.Pointer;
        }

        #endregion

        void Save(bool closePage = false)
        {
            GetValue();

            bool isSave = false;

            try
            {
                isSave = ВидиНоменклатури_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомленняПроПомилку(DateTime.Now, "Запис",
                    ВидиНоменклатури_Objest.UnigueID.UGuid, "Довідники", ВидиНоменклатури_Objest.Назва, ex.Message);

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
                Program.GeneralForm?.RenameCurrentPageNotebook($"Види номенклатури: {ВидиНоменклатури_Objest.Назва}");

            if (CallBack_OnSelectPointer != null)
                CallBack_OnSelectPointer.Invoke(ВидиНоменклатури_Objest.GetDirectoryPointer());

            if (PageList != null)
            {
                PageList.SelectPointerItem = ВидиНоменклатури_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}