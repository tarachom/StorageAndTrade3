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

using AccountingSoftware;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    public class ЗбіркаТоварівНаСкладі : ДокументЖурнал
    {
        public ЗбіркаТоварівНаСкладі() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.Store;
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.LoadRecords();

            if (ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.CurrentPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.Where.Clear();

            //Назва
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.Where.Add(
                new Where(ЗбіркаТоварівНаСкладі_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.LoadRecords();

            if (ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.FirstPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{ЗбіркаТоварівНаСкладі_Const.FULLNAME} *", () =>
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
                    Program.GeneralForm?.CreateNotebookPage($"{ЗбіркаТоварівНаСкладі_Objest.Назва}", () =>
                    {
                        ЗбіркаТоварівНаСкладі_Елемент page = new ЗбіркаТоварівНаСкладі_Елемент
                        {
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
                ЗбіркаТоварівНаСкладі_Objest ЗбіркаТоварівНаСкладі_Objest_Новий = ЗбіркаТоварівНаСкладі_Objest.Copy(true);
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

        protected override void PeriodWhereChanged()
        {
            ТабличніСписки.ЗбіркаТоварівНаСкладі_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ЗбіркаТоварівНаСкладі_Pointer ЗбіркаТоварівНаСкладі_Pointer = new ЗбіркаТоварівНаСкладі_Pointer(unigueID);
            ЗбіркаТоварівНаСкладі_Objest? ЗбіркаТоварівНаСкладі_Objest = await ЗбіркаТоварівНаСкладі_Pointer.GetDocumentObject(true);
            if (ЗбіркаТоварівНаСкладі_Objest == null) return;

            if (spendDoc)
            {
                if (!await ЗбіркаТоварівНаСкладі_Objest.SpendTheDocument(ЗбіркаТоварівНаСкладі_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
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