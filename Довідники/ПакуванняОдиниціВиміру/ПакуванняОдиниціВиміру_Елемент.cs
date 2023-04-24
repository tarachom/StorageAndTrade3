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
    class ПакуванняОдиниціВиміру_Елемент : VBox
    {
        public ПакуванняОдиниціВиміру? PageList { get; set; }
        public System.Action<ПакуванняОдиниціВиміру_Pointer>? CallBack_OnSelectPointer { get; set; }

        public bool IsNew { get; set; } = true;

        public ПакуванняОдиниціВиміру_Objest ПакуванняОдиниціВиміру_Objest { get; set; } = new ПакуванняОдиниціВиміру_Objest();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Entry НазваПовна = new Entry() { WidthRequest = 500 };
        IntegerControl КількістьУпаковок = new IntegerControl();

        public ПакуванняОдиниціВиміру_Елемент() : base()
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

            //НазваПовна
            HBox hBoxNameFull = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxNameFull, false, false, 5);

            hBoxNameFull.PackStart(new Label("Назва повна:"), false, false, 5);
            hBoxNameFull.PackStart(НазваПовна, false, false, 5);

            //КількістьУпаковок
            HBox hBoxKvoPack = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxKvoPack, false, false, 5);

            hBoxKvoPack.PackStart(new Label("Кількість упаковок:"), false, false, 5);
            hBoxKvoPack.PackStart(КількістьУпаковок, false, false, 5);

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
            {
                ПакуванняОдиниціВиміру_Objest.New();
                ПакуванняОдиниціВиміру_Objest.КількістьУпаковок = 1;
            }

            Код.Text = ПакуванняОдиниціВиміру_Objest.Код;
            Назва.Text = ПакуванняОдиниціВиміру_Objest.Назва;
            НазваПовна.Text = ПакуванняОдиниціВиміру_Objest.НазваПовна;
            КількістьУпаковок.Value = ПакуванняОдиниціВиміру_Objest.КількістьУпаковок;
        }

        void GetValue()
        {
            ПакуванняОдиниціВиміру_Objest.Код = Код.Text;
            ПакуванняОдиниціВиміру_Objest.Назва = Назва.Text;
            ПакуванняОдиниціВиміру_Objest.НазваПовна = НазваПовна.Text;
            ПакуванняОдиниціВиміру_Objest.КількістьУпаковок = КількістьУпаковок.Value;
        }

        #endregion

        void Save(bool closePage = false)
        {
            GetValue();

            bool isSave = false;

            try
            {
                isSave = ПакуванняОдиниціВиміру_Objest.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомленняПроПомилку(DateTime.Now, "Запис",
                    ПакуванняОдиниціВиміру_Objest.UnigueID.UGuid, "Довідники", ПакуванняОдиниціВиміру_Objest.Назва, ex.Message);

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
                Program.GeneralForm?.RenameCurrentPageNotebook($"Пакування: {ПакуванняОдиниціВиміру_Objest.Назва}");

            if (CallBack_OnSelectPointer != null)
                CallBack_OnSelectPointer.Invoke(ПакуванняОдиниціВиміру_Objest.GetDirectoryPointer());

            if (PageList != null)
            {
                PageList.SelectPointerItem = ПакуванняОдиниціВиміру_Objest.GetDirectoryPointer();
                PageList.LoadRecords();
            }
        }
    }
}