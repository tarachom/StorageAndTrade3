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

using InterfaceGtk;
using AccountingSoftware;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    public class ПереміщенняТоварівНаСкладі : ДокументЖурнал
    {
        public ПереміщенняТоварівНаСкладі() : base()
        {
            ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async void LoadRecords()
        {
            ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.DocumentPointerItem = DocumentPointerItem;

            await ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Назва
            ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(ПереміщенняТоварівНаСкладі_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{ПереміщенняТоварівНаСкладі_Const.FULLNAME} *", () =>
                {
                    ПереміщенняТоварівНаСкладі_Елемент page = new ПереміщенняТоварівНаСкладі_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                });
            }
            else if (unigueID != null)
            {
                ПереміщенняТоварівНаСкладі_Objest ПереміщенняТоварівНаСкладі_Objest = new ПереміщенняТоварівНаСкладі_Objest();
                if (await ПереміщенняТоварівНаСкладі_Objest.Read(unigueID))
                {
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{ПереміщенняТоварівНаСкладі_Objest.Назва}", () =>
                    {
                        ПереміщенняТоварівНаСкладі_Елемент page = new ПереміщенняТоварівНаСкладі_Елемент
                        {
                            UnigueID = unigueID,
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ПереміщенняТоварівНаСкладі_Objest = ПереміщенняТоварівНаСкладі_Objest,
                        };

                        page.SetValue();

                        return page;
                    });
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            ПереміщенняТоварівНаСкладі_Objest ПереміщенняТоварівНаСкладі_Objest = new ПереміщенняТоварівНаСкладі_Objest();
            if (await ПереміщенняТоварівНаСкладі_Objest.Read(unigueID))
                await ПереміщенняТоварівНаСкладі_Objest.SetDeletionLabel(!ПереміщенняТоварівНаСкладі_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ПереміщенняТоварівНаСкладі_Objest ПереміщенняТоварівНаСкладі_Objest = new ПереміщенняТоварівНаСкладі_Objest();
            if (await ПереміщенняТоварівНаСкладі_Objest.Read(unigueID))
            {
                ПереміщенняТоварівНаСкладі_Objest ПереміщенняТоварівНаСкладі_Objest_Новий = await ПереміщенняТоварівНаСкладі_Objest.Copy(true);
                await ПереміщенняТоварівНаСкладі_Objest_Новий.Save();
                await ПереміщенняТоварівНаСкладі_Objest_Новий.Товари_TablePart.Save(true);

                return ПереміщенняТоварівНаСкладі_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "Документи.ПереміщенняТоварівНаСкладі";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
        }

        protected override async void PeriodChanged()
        {
            ТабличніСписки.ПереміщенняТоварівНаСкладі_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);
            LoadRecords();

            await ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ПереміщенняТоварівНаСкладі_Pointer ПереміщенняТоварівНаСкладі_Pointer = new ПереміщенняТоварівНаСкладі_Pointer(unigueID);
            ПереміщенняТоварівНаСкладі_Objest? ПереміщенняТоварівНаСкладі_Objest = await ПереміщенняТоварівНаСкладі_Pointer.GetDocumentObject(true);
            if (ПереміщенняТоварівНаСкладі_Objest == null) return;

            if (spendDoc)
            {
                if (!await ПереміщенняТоварівНаСкладі_Objest.SpendTheDocument(ПереміщенняТоварівНаСкладі_Objest.ДатаДок))
                    new ФункціїДляПовідомлень().ПоказатиПовідомлення(ПереміщенняТоварівНаСкладі_Objest.UnigueID);
            }
            else
                await ПереміщенняТоварівНаСкладі_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ПереміщенняТоварівНаСкладі_Pointer(unigueID);
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ПереміщенняТоварівНаСкладі_Const.FULLNAME}_{unigueID}.xml");
            await ПереміщенняТоварівНаСкладі_Export.ToXmlFile(new ПереміщенняТоварівНаСкладі_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar


        #endregion
    }
}