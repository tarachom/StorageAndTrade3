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
    public abstract class ДовідникЕлемент : VBox
    {
        /// <summary>
        /// Чи це новий елемент
        /// </summary>
        public bool IsNew { get; set; } = true;

        /// <summary>
        /// Функція зворотнього виклику для перевантаження списку
        /// </summary>
        public System.Action<UnigueID?>? CallBack_LoadRecords { get; set; }

        /// <summary>
        /// Функція зворотнього виклику для вибору елементу
        /// Використовується коли потрібно новий елемент зразу вибрати
        /// </summary>
        public System.Action<UnigueID>? CallBack_OnSelectPointer { get; set; }

        /// <summary>
        /// ІД елементу
        /// </summary>
        public UnigueID? UnigueID { get; set; }

        /// <summary>
        /// Назва         
        /// /// </summary>
        public string Caption { get; set; } = "";

        /// <summary>
        /// Горизонтальний бокс для кнопок
        /// </summary>
        protected HBox HBoxTop = new HBox();

        /// <summary>
        /// Панель з двох колонок
        /// </summary>
        protected HPaned HPanedTop = new HPaned() { BorderWidth = 5, Position = 500 };

        public ДовідникЕлемент() : base()
        {
            Button bSaveAndClose = new Button("Зберегти та закрити");
            bSaveAndClose.Clicked += (object? sender, EventArgs args) => { BeforeAndAfterSave(true); };
            HBoxTop.PackStart(bSaveAndClose, false, false, 10);

            Button bSave = new Button("Зберегти");
            bSave.Clicked += (object? sender, EventArgs args) => { BeforeAndAfterSave(); };
            HBoxTop.PackStart(bSave, false, false, 10);

            PackStart(HBoxTop, false, false, 10);

            //Pack1
            VBox vBox1 = new VBox();
            HPanedTop.Pack1(vBox1, false, false);
            CreatePack1(vBox1);

            //Pack2
            VBox vBox2 = new VBox();
            HPanedTop.Pack2(vBox2, false, false);
            CreatePack2(vBox2);

            PackStart(HPanedTop, false, false, 5);

            ShowAll();
        }

        /// <summary>
        /// Лівий Блок
        /// </summary>
        protected virtual void CreatePack1(VBox vBox) { }

        /// <summary>
        /// Правий Блок
        /// </summary>
        protected virtual void CreatePack2(VBox vBox) { }

        #region Create Field

        /// <summary>
        /// Створення поля із заголовком
        /// </summary>
        /// <param name="vBox">Контейнер</param>
        /// <param name="label">Заголовок</param>
        /// <param name="field">Поле</param>
        /// <param name="Halign">Положення</param>
        protected HBox CreateField(VBox vBox, string? label, Widget field, Align Halign = Align.End)
        {
            HBox hBox = new HBox() { Halign = Halign };
            vBox.PackStart(hBox, false, false, 5);

            if (label != null)
                hBox.PackStart(new Label(label), false, false, 5);

            hBox.PackStart(field, false, false, 5);

            return hBox;
        }

        /// <summary>
        /// Добавлення поля в HBox
        /// </summary>
        /// <param name="hBox">Контейнер</param>
        /// <param name="label">Заголовок</param>
        /// <param name="field">Поле</param>
        protected void CreateField(HBox hBox, string? label, Widget field)
        {
            if (label != null)
                hBox.PackStart(new Label(label), false, false, 5);

            hBox.PackStart(field, false, false, 5);
        }

        /// <summary>
        /// Добавлення поля з прокруткою
        /// </summary>
        /// <param name="vBox">Контейнер</param>
        /// <param name="label">Заголовок</param>
        /// <param name="field">Поле</param>
        /// <param name="Width">Висота</param>
        /// <param name="Height">Ширина</param>
        protected void CreateFieldView(VBox vBox, string? label, Widget field, int Width = 100, int Height = 100, Align Halign = Align.End)
        {
            HBox hBox = new HBox() { Halign = Halign };
            vBox.PackStart(hBox, false, false, 5);

            if (label != null)
                hBox.PackStart(new Label(label) { Valign = Align.Start }, false, false, 5);

            ScrolledWindow scrollTextView = new ScrolledWindow() { ShadowType = ShadowType.In, WidthRequest = Width, HeightRequest = Height };
            scrollTextView.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollTextView.Add(field);

            hBox.PackStart(scrollTextView, false, false, 5);
        }

        /// <summary>
        /// Добавлення табличної частини
        /// </summary>
        /// <param name="vBox">Контейнер</param>
        /// <param name="label">Заголовок</param>
        /// <param name="tablePart">Таб частина</param>
        protected void CreateTablePart(VBox vBox, string? label, Widget tablePart)
        {
            if (label != null)
            {
                HBox hBoxCaption = new HBox();
                vBox.PackStart(hBoxCaption, false, false, 5);
                hBoxCaption.PackStart(new Label(label), false, false, 5);
            }

            HBox hBox = new HBox();
            vBox.PackStart(hBox, false, false, 0);
            hBox.PackStart(tablePart, true, true, 5);
        }

        #endregion

        /// <summary>
        /// Присвоєння значень
        /// </summary>
        public virtual void SetValue() { }

        /// <summary>
        /// Зчитування значень
        /// </summary>
        protected virtual void GetValue() { }

        /// <summary>
        /// Функція обробки перед збереження та після збереження
        /// </summary>
        /// <param name="closePage"></param>
        void BeforeAndAfterSave(bool closePage = false)
        {
            GetValue();

            Save();

            if (CallBack_OnSelectPointer != null && UnigueID != null)
                CallBack_OnSelectPointer.Invoke(UnigueID);

            if (CallBack_LoadRecords != null)
                CallBack_LoadRecords.Invoke(UnigueID);

            if (closePage)
                Program.GeneralForm?.CloseNotebookPageToCode(this.Name);
            else
                Program.GeneralForm?.RenameNotebookPageToCode(Caption, this.Name);
        }

        /// <summary>
        /// Збереження
        /// </summary>
        protected virtual void Save() { }

        /// <summary>
        /// Записати повідомлення про помилку і вивести меседж
        /// </summary>
        /// <param name="ex">Помилка</param>
        protected void MsgError(Exception ex)
        {
            ФункціїДляПовідомлень.ДодатиПовідомленняПроПомилку(DateTime.Now, "Запис", UnigueID?.UGuid, "Довідники", Caption, ex.Message);
            ФункціїДляПовідомлень.ВідкритиТермінал();

            Message.Info(Program.GeneralForm, "Не вдалось записати");
        }
    }
}