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

            CreatePack1();
            CreatePack2();

            PackStart(HPanedTop, false, false, 5);

            ShowAll();
        }

        protected virtual void CreatePack1()
        {
            VBox vBox = new VBox();

            HPanedTop.Pack1(vBox, false, false);
        }

        protected virtual void CreatePack2()
        {
            VBox vBox = new VBox();

            HPanedTop.Pack2(vBox, false, false);
        }

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
                Program.GeneralForm?.CloseCurrentPageNotebook();
            else
                Program.GeneralForm?.RenameCurrentPageNotebook(Caption);
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