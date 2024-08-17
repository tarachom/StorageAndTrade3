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
    public class РозміщенняТоварівНаСкладі : ДокументЖурнал
    {
        public РозміщенняТоварівНаСкладі() : base()
        {
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async void LoadRecords()
        {
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.DocumentPointerItem = DocumentPointerItem;

            await ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Назва
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(РозміщенняТоварівНаСкладі_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{РозміщенняТоварівНаСкладі_Const.FULLNAME} *", () =>
                {
                    РозміщенняТоварівНаСкладі_Елемент page = new РозміщенняТоварівНаСкладі_Елемент
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
                РозміщенняТоварівНаСкладі_Objest РозміщенняТоварівНаСкладі_Objest = new РозміщенняТоварівНаСкладі_Objest();
                if (await РозміщенняТоварівНаСкладі_Objest.Read(unigueID))
                {
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{РозміщенняТоварівНаСкладі_Objest.Назва}", () =>
                    {
                        РозміщенняТоварівНаСкладі_Елемент page = new РозміщенняТоварівНаСкладі_Елемент
                        {
                            UnigueID = unigueID,
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            РозміщенняТоварівНаСкладі_Objest = РозміщенняТоварівНаСкладі_Objest,
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
            РозміщенняТоварівНаСкладі_Objest РозміщенняТоварівНаСкладі_Objest = new РозміщенняТоварівНаСкладі_Objest();
            if (await РозміщенняТоварівНаСкладі_Objest.Read(unigueID))
                await РозміщенняТоварівНаСкладі_Objest.SetDeletionLabel(!РозміщенняТоварівНаСкладі_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            РозміщенняТоварівНаСкладі_Objest РозміщенняТоварівНаСкладі_Objest = new РозміщенняТоварівНаСкладі_Objest();
            if (await РозміщенняТоварівНаСкладі_Objest.Read(unigueID))
            {
                РозміщенняТоварівНаСкладі_Objest РозміщенняТоварівНаСкладі_Objest_Новий = await РозміщенняТоварівНаСкладі_Objest.Copy(true);
                await РозміщенняТоварівНаСкладі_Objest_Новий.Save();
                await РозміщенняТоварівНаСкладі_Objest_Новий.Товари_TablePart.Save(true);

                return РозміщенняТоварівНаСкладі_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "Документи.РозміщенняТоварівНаСкладі";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
        }

        protected override async void PeriodChanged()
        {
            ТабличніСписки.РозміщенняТоварівНаСкладі_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);
            LoadRecords();

            await ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            РозміщенняТоварівНаСкладі_Pointer РозміщенняТоварівНаСкладі_Pointer = new РозміщенняТоварівНаСкладі_Pointer(unigueID);
            РозміщенняТоварівНаСкладі_Objest? РозміщенняТоварівНаСкладі_Objest = await РозміщенняТоварівНаСкладі_Pointer.GetDocumentObject(true);
            if (РозміщенняТоварівНаСкладі_Objest == null) return;

            if (spendDoc)
            {
                if (!await РозміщенняТоварівНаСкладі_Objest.SpendTheDocument(РозміщенняТоварівНаСкладі_Objest.ДатаДок))
                    new ФункціїДляПовідомлень().ПоказатиПовідомлення(РозміщенняТоварівНаСкладі_Objest.UnigueID);
            }
            else
                await РозміщенняТоварівНаСкладі_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new РозміщенняТоварівНаСкладі_Pointer(unigueID);
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{РозміщенняТоварівНаСкладі_Const.FULLNAME}_{unigueID}.xml");
            await РозміщенняТоварівНаСкладі_Export.ToXmlFile(new РозміщенняТоварівНаСкладі_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar

        #endregion
    }
}