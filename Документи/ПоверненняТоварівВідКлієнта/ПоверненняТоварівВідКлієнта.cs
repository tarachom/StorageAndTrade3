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
    public class ПоверненняТоварівВідКлієнта : ДокументЖурнал
    {
        public ПоверненняТоварівВідКлієнта() : base()
        {
            ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async void LoadRecords()
        {
            ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Назва
            ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(ПоверненняТоварівВідКлієнта_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПоверненняТоварівВідКлієнта_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ПоверненняТоварівВідКлієнта_Const.FULLNAME} *", () =>
                {
                    ПоверненняТоварівВідКлієнта_Елемент page = new ПоверненняТоварівВідКлієнта_Елемент
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
                ПоверненняТоварівВідКлієнта_Objest ПоверненняТоварівВідКлієнта_Objest = new ПоверненняТоварівВідКлієнта_Objest();
                if (await ПоверненняТоварівВідКлієнта_Objest.Read(unigueID))
                {
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ПоверненняТоварівВідКлієнта_Objest.Назва}", () =>
                    {
                        ПоверненняТоварівВідКлієнта_Елемент page = new ПоверненняТоварівВідКлієнта_Елемент
                        {
                            UnigueID = unigueID,
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ПоверненняТоварівВідКлієнта_Objest = ПоверненняТоварівВідКлієнта_Objest,
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
            ПоверненняТоварівВідКлієнта_Objest ПоверненняТоварівВідКлієнта_Objest = new ПоверненняТоварівВідКлієнта_Objest();
            if (await ПоверненняТоварівВідКлієнта_Objest.Read(unigueID))
                await ПоверненняТоварівВідКлієнта_Objest.SetDeletionLabel(!ПоверненняТоварівВідКлієнта_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ПоверненняТоварівВідКлієнта_Objest ПоверненняТоварівВідКлієнта_Objest = new ПоверненняТоварівВідКлієнта_Objest();
            if (await ПоверненняТоварівВідКлієнта_Objest.Read(unigueID))
            {
                ПоверненняТоварівВідКлієнта_Objest ПоверненняТоварівВідКлієнта_Objest_Новий = await ПоверненняТоварівВідКлієнта_Objest.Copy(true);
                await ПоверненняТоварівВідКлієнта_Objest_Новий.Save();
                await ПоверненняТоварівВідКлієнта_Objest_Новий.Товари_TablePart.Save(true);

                return ПоверненняТоварівВідКлієнта_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "Документи.ПоверненняТоварівВідКлієнта";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
        }

        protected override void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            LoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ПоверненняТоварівВідКлієнта_Pointer ПоверненняТоварівВідКлієнта_Pointer = new ПоверненняТоварівВідКлієнта_Pointer(unigueID);
            ПоверненняТоварівВідКлієнта_Objest? ПоверненняТоварівВідКлієнта_Objest = await ПоверненняТоварівВідКлієнта_Pointer.GetDocumentObject(true);
            if (ПоверненняТоварівВідКлієнта_Objest == null) return;

            if (spendDoc)
            {
                if (!await ПоверненняТоварівВідКлієнта_Objest.SpendTheDocument(ПоверненняТоварівВідКлієнта_Objest.ДатаДок))
                    new ФункціїДляПовідомлень().ПоказатиПовідомлення(ПоверненняТоварівВідКлієнта_Objest.UnigueID);
            }
            else
                await ПоверненняТоварівВідКлієнта_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ПоверненняТоварівВідКлієнта_Pointer(unigueID);
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ПоверненняТоварівВідКлієнта_Const.FULLNAME}_{unigueID}.xml");
            await ПоверненняТоварівВідКлієнта_Export.ToXmlFile(new ПоверненняТоварівВідКлієнта_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar


        #endregion
    }
}