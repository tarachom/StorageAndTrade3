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
    public class ЗбіркаТоварівНаСкладі : ДокументЖурнал
    {
        public ЗбіркаТоварівНаСкладі() : base()
        {
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async void LoadRecords()
        {
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.DocumentPointerItem = DocumentPointerItem;

            await ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Назва
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(ЗбіркаТоварівНаСкладі_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{ЗбіркаТоварівНаСкладі_Const.FULLNAME} *", () =>
                {
                    ЗбіркаТоварівНаСкладі_Елемент page = new ЗбіркаТоварівНаСкладі_Елемент
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
                ЗбіркаТоварівНаСкладі_Objest ЗбіркаТоварівНаСкладі_Objest = new ЗбіркаТоварівНаСкладі_Objest();
                if (await ЗбіркаТоварівНаСкладі_Objest.Read(unigueID))
                {
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{ЗбіркаТоварівНаСкладі_Objest.Назва}", () =>
                    {
                        ЗбіркаТоварівНаСкладі_Елемент page = new ЗбіркаТоварівНаСкладі_Елемент
                        {
                            UnigueID = unigueID,
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ЗбіркаТоварівНаСкладі_Objest = ЗбіркаТоварівНаСкладі_Objest,
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
            ЗбіркаТоварівНаСкладі_Objest ЗбіркаТоварівНаСкладі_Objest = new ЗбіркаТоварівНаСкладі_Objest();
            if (await ЗбіркаТоварівНаСкладі_Objest.Read(unigueID))
                await ЗбіркаТоварівНаСкладі_Objest.SetDeletionLabel(!ЗбіркаТоварівНаСкладі_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ЗбіркаТоварівНаСкладі_Objest ЗбіркаТоварівНаСкладі_Objest = new ЗбіркаТоварівНаСкладі_Objest();
            if (await ЗбіркаТоварівНаСкладі_Objest.Read(unigueID))
            {
                ЗбіркаТоварівНаСкладі_Objest ЗбіркаТоварівНаСкладі_Objest_Новий = await ЗбіркаТоварівНаСкладі_Objest.Copy(true);
                await ЗбіркаТоварівНаСкладі_Objest_Новий.Save();
                await ЗбіркаТоварівНаСкладі_Objest_Новий.Товари_TablePart.Save(true);

                return ЗбіркаТоварівНаСкладі_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "Документи.ЗбіркаТоварівНаСкладі";

        protected override async ValueTask BeforeSetValue()
        {
            string періодДляЖурналу = await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting);
            if (!string.IsNullOrEmpty(періодДляЖурналу) && Enum.TryParse<ПеріодДляЖурналу.ТипПеріоду>(періодДляЖурналу, out ПеріодДляЖурналу.ТипПеріоду result))
                PeriodWhere = result;
        }

        protected override async void PeriodWhereChanged()
        {
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, ComboBoxPeriodWhere.ActiveId);
            LoadRecords();

            await ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, ComboBoxPeriodWhere.ActiveId);
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ЗбіркаТоварівНаСкладі_Pointer ЗбіркаТоварівНаСкладі_Pointer = new ЗбіркаТоварівНаСкладі_Pointer(unigueID);
            ЗбіркаТоварівНаСкладі_Objest? ЗбіркаТоварівНаСкладі_Objest = await ЗбіркаТоварівНаСкладі_Pointer.GetDocumentObject(true);
            if (ЗбіркаТоварівНаСкладі_Objest == null) return;

            if (spendDoc)
            {
                if (!await ЗбіркаТоварівНаСкладі_Objest.SpendTheDocument(ЗбіркаТоварівНаСкладі_Objest.ДатаДок))
                    new ФункціїДляПовідомлень().ПоказатиПовідомлення(ЗбіркаТоварівНаСкладі_Objest.UnigueID);
            }
            else
                await ЗбіркаТоварівНаСкладі_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ЗбіркаТоварівНаСкладі_Pointer(unigueID);
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ЗбіркаТоварівНаСкладі_Const.FULLNAME}_{unigueID}.xml");
            await ЗбіркаТоварівНаСкладі_Export.ToXmlFile(new ЗбіркаТоварівНаСкладі_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar

        #endregion
    }
}