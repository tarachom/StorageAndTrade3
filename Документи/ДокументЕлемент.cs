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
    public abstract class ДокументЕлемент : ФормаЕлемент
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
        /// Функція зворотнього виклику для вибору елементу
        /// Використовується коли потрібно новий елемент зразу вибрати
        /// </summary>
        public Action<UnigueID>? CallBack_OnSelectPointer { get; set; }

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
        /// Горизонтальний бокс для назви
        /// </summary>
        protected Box HBoxName = new Box(Orientation.Horizontal, 0);

        /// <summary>
        /// Горизонтальний бокс для коментаря
        /// </summary>
        protected Box HBoxComment = new Box(Orientation.Horizontal, 0);

        /// <summary>
        /// Панель з двох колонок
        /// </summary>
        protected Paned HPanedTop = new Paned(Orientation.Vertical) { BorderWidth = 5 };

        /// <summary>
        /// Блокнот для табличних частин і додаткових реквізитів
        /// </summary>
        protected Notebook NotebookTablePart = new Notebook() { Scrollable = true, EnablePopup = true, BorderWidth = 0, ShowBorder = false, TabPos = PositionType.Top };

        public ДокументЕлемент() : base()
        {
            Button bSaveAndSpend = new Button("Провести та закрити");
            bSaveAndSpend.Clicked += (object? sender, EventArgs args) => { BeforeAndAfterSave(true, true); };
            HBoxTop.PackStart(bSaveAndSpend, false, false, 10);

            Button bSpend = new Button("Провести");
            bSpend.Clicked += (object? sender, EventArgs args) => { BeforeAndAfterSave(true); };
            HBoxTop.PackStart(bSpend, false, false, 10);

            Button bSave = new Button("Зберегти");
            bSave.Clicked += (object? sender, EventArgs args) => { BeforeAndAfterSave(false); };
            HBoxTop.PackStart(bSave, false, false, 10);

            //Проводки
            {
                LinkButton linkNew = new LinkButton("Проводки") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkNew.Clicked += (object? sender, EventArgs args) =>
                {
                    if (UnigueID != null)
                    {
                        DocumentPointer? documentPointer = ReportSpendTheDocument(UnigueID);

                        if (documentPointer != null)
                        {
                            Program.GeneralForm?.CreateNotebookPage($"Проводки", () =>
                            {
                                Звіт_РухДокументівПоРегістрах page = new Звіт_РухДокументівПоРегістрах();
                                page.CreateReport(documentPointer);
                                return page;
                            });
                        }
                    }
                };

                HBoxTop.PackStart(linkNew, false, false, 0);
            }

            PackStart(HBoxTop, false, false, 10);

            //Pack1
            Box vBox1 = new Box(Orientation.Vertical, 0);
            HPanedTop.Pack1(vBox1, false, false);
            CreatePack1(vBox1);

            //Pack2
            Box vBox2 = new Box(Orientation.Vertical, 0);
            HPanedTop.Pack2(vBox2, true, false);
            CreatePack2(vBox2);

            PackStart(HPanedTop, true, true, 0);

            ShowAll();
        }

        /// <summary>
        /// Верхній Блок
        /// </summary>
        protected virtual void CreatePack1(Box vBox)
        {
            vBox.PackStart(HBoxName, false, false, 5);

            //Два блоки для полів -->
            Box hBoxContainer = new Box(Orientation.Horizontal, 0);

            Expander expanderHead = new Expander("Реквізити шапки") { Expanded = true };
            expanderHead.Add(hBoxContainer);

            vBox.PackStart(expanderHead, false, false, 5);

            //Container1
            Box vBoxContainer1 = new Box(Orientation.Vertical, 0) { WidthRequest = 500 };
            hBoxContainer.PackStart(vBoxContainer1, false, false, 5);

            CreateContainer1(vBoxContainer1);

            //Container2
            Box vBoxContainer2 = new Box(Orientation.Vertical, 0) { WidthRequest = 500 };
            hBoxContainer.PackStart(vBoxContainer2, false, false, 5);

            CreateContainer2(vBoxContainer2);
            // <--

            vBox.PackStart(HBoxComment, false, false, 5);
        }

        /// <summary>
        /// Нижній Блок
        /// </summary>
        protected virtual void CreatePack2(Box vBox)
        {
            vBox.PackStart(NotebookTablePart, true, true, 0);

            Box vBoxPage = new Box(Orientation.Vertical, 0);
            NotebookTablePart.AppendPage(vBoxPage, new Label("Додаткові реквізити"));

            //Два блоки для полів -->
            Box hBoxContainer = new Box(Orientation.Horizontal, 0);
            vBoxPage.PackStart(hBoxContainer, false, false, 5);

            Box vBoxContainer1 = new Box(Orientation.Vertical, 0) { WidthRequest = 500 };
            hBoxContainer.PackStart(vBoxContainer1, false, false, 5);

            CreateContainer3(vBoxContainer1);

            Box vBoxContainer2 = new Box(Orientation.Vertical, 0) { WidthRequest = 500 };
            hBoxContainer.PackStart(vBoxContainer2, false, false, 5);

            CreateContainer4(vBoxContainer2);
            // <--
        }

        protected virtual void CreateContainer1(Box vBox) { }
        protected virtual void CreateContainer2(Box vBox) { }
        protected virtual void CreateContainer3(Box vBox) { }
        protected virtual void CreateContainer4(Box vBox) { }

        #region Create Field

        /// <summary>
        /// Назва документу
        /// </summary>
        /// <param name="НазваДок">Назва</param>
        /// <param name="НомерДок">Номер</param>
        /// <param name="ДатаДок">Дата</param>
        protected void CreateDocName(string НазваДок, Widget НомерДок, Widget ДатаДок)
        {
            HBoxName.PackStart(new Label($"{НазваДок} №:"), false, false, 5);
            HBoxName.PackStart(НомерДок, false, false, 5);
            HBoxName.PackStart(new Label("від:"), false, false, 5);
            HBoxName.PackStart(ДатаДок, false, false, 5);
        }

        #endregion

        /// <summary>
        /// Присвоєння значень
        /// </summary>
        public abstract void SetValue();

        /// <summary>
        /// Зчитування значень
        /// </summary>
        protected abstract void GetValue();

        /// <summary>
        /// Функція обробки перед збереження та після збереження
        /// </summary>
        /// <param name="closePage"></param>
        async void BeforeAndAfterSave(bool spendDoc, bool closePage = false)
        {
            GetValue();

            Program.GeneralForm?.SensitiveNotebookPageToCode(this.Name, false);

            bool isSave = await Save();
            bool isSpend = await SpendTheDocument(isSave && spendDoc);

            Program.GeneralForm?.SensitiveNotebookPageToCode(this.Name, true);

            if (CallBack_OnSelectPointer != null && UnigueID != null)
                CallBack_OnSelectPointer.Invoke(UnigueID);

            if (CallBack_LoadRecords != null)
                CallBack_LoadRecords.Invoke(UnigueID);

            if (closePage && isSpend)
                Program.GeneralForm?.CloseNotebookPageToCode(this.Name);
            else
                Program.GeneralForm?.RenameNotebookPageToCode(Caption, this.Name);
        }

        /// <summary>
        /// Збереження
        /// </summary>
        protected abstract ValueTask<bool> Save();

        /// <summary>
        /// Проведення
        /// </summary>
        /// <param name="spendDoc">Провести</param>
        protected abstract ValueTask<bool> SpendTheDocument(bool spendDoc);

        /// <summary>
        /// Записати повідомлення про помилку і вивести меседж
        /// </summary>
        /// <param name="ex">Помилка</param>
        protected async void MsgError(Exception ex)
        {
            await ФункціїДляПовідомлень.ДодатиПовідомленняПроПомилку(DateTime.Now, "Запис", UnigueID?.UGuid, "Документи", Caption, ex.Message + "\n" + ex.StackTrace + "\n" + ex.Source);
            ФункціїДляПовідомлень.ПоказатиПовідомлення();

            Message.Info(Program.GeneralForm, "Не вдалось записати");
        }

        /// <summary>
        /// Для звіту Проводки
        /// </summary>
        /// <param name="unigueID"></param>
        /// <returns></returns>
        protected abstract DocumentPointer? ReportSpendTheDocument(UnigueID unigueID);
    }
}