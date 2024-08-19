/*
Copyright (C) 2019-2024 TARAKHOMYN YURIY IVANOVYCH
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
using InterfaceGtk;

using AccountingSoftware;

namespace StorageAndTrade
{
    public abstract class РегістриЕлемент : ФормаЕлемент
    {
        /// <summary>
        /// Чи це новий елемент
        /// </summary>
        public bool IsNew { get; set; } = true;

        /// <summary>
        /// Функція зворотнього виклику для перевантаження списку
        /// </summary>
        public Action<UnigueID?>? CallBack_LoadRecords { get; set; }

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
        protected Box HBoxTop = new Box(Orientation.Horizontal, 0);

        /// <summary>
        /// Панель з двох колонок
        /// </summary>
        protected Paned HPanedTop = new Paned(Orientation.Horizontal) { BorderWidth = 5, Position = 500 };

        public РегістриЕлемент() : base()
        {
            Button bSaveAndClose = new Button("Зберегти та закрити");
            bSaveAndClose.Clicked += (object? sender, EventArgs args) => { BeforeAndAfterSave(true); };
            HBoxTop.PackStart(bSaveAndClose, false, false, 10);

            Button bSave = new Button("Зберегти");
            bSave.Clicked += (object? sender, EventArgs args) => { BeforeAndAfterSave(); };
            HBoxTop.PackStart(bSave, false, false, 10);

            PackStart(HBoxTop, false, false, 10);

            //Pack1
            Box vBox1 = new Box(Orientation.Vertical, 0);
            HPanedTop.Pack1(vBox1, false, false);
            CreatePack1(vBox1);

            //Pack2
            Box vBox2 = new Box(Orientation.Horizontal, 0);
            HPanedTop.Pack2(vBox2, false, false);
            CreatePack2(vBox2);

            PackStart(HPanedTop, false, false, 5);

            ShowAll();
        }

        /// <summary>
        /// Лівий Блок
        /// </summary>
        protected virtual void CreatePack1(Box vBox) { }

        /// <summary>
        /// Правий Блок
        /// </summary>
        protected virtual void CreatePack2(Box vBox) { }

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
        async void BeforeAndAfterSave(bool closePage = false)
        {
            GetValue();

            await Save();

            CallBack_LoadRecords?.Invoke(UnigueID);

            if (closePage)
                NotebookFunction.CloseNotebookPageToCode(Program.GeneralNotebook, this.Name);
            else
                NotebookFunction.RenameNotebookPageToCode(Program.GeneralNotebook, Caption, this.Name);
        }

        /// <summary>
        /// Збереження
        /// </summary>
        protected virtual ValueTask Save() { return new ValueTask(); }

        /// <summary>
        /// Записати повідомлення про помилку і вивести меседж
        /// </summary>
        /// <param name="ex">Помилка</param>
        protected async void MsgError(Exception ex)
        {
            await new ФункціїДляПовідомлень().ДодатиПовідомленняПроПомилку("Запис", UnigueID?.UGuid, "Регістри Відомостей", Caption, ex.Message);
            new ФункціїДляПовідомлень().ПоказатиПовідомлення(UnigueID);

            Message.Info(Program.GeneralForm, "Не вдалось записати");
        }
    }
}