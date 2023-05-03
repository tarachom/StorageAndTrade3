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

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    public class РозміщенняНоменклатуриПоКоміркам : ДокументЖурнал
    {
        public РозміщенняНоменклатуриПоКоміркам() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.Store;
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.LoadRecords();

            if (ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.CurrentPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.Where.Clear();

            //Назва
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.Where.Add(
                new Where(РозміщенняНоменклатуриПоКоміркам_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.LoadRecords();

            if (ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.FirstPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{РозміщенняНоменклатуриПоКоміркам_Const.FULLNAME} *", () =>
                {
                    РозміщенняНоменклатуриПоКоміркам_Елемент page = new РозміщенняНоменклатуриПоКоміркам_Елемент
                    {
                        PageList = this,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else if (unigueID != null)
            {
                РозміщенняНоменклатуриПоКоміркам_Objest РозміщенняНоменклатуриПоКоміркам_Objest = new РозміщенняНоменклатуриПоКоміркам_Objest();
                if (РозміщенняНоменклатуриПоКоміркам_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{РозміщенняНоменклатуриПоКоміркам_Objest.Назва}", () =>
                    {
                        РозміщенняНоменклатуриПоКоміркам_Елемент page = new РозміщенняНоменклатуриПоКоміркам_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            РозміщенняНоменклатуриПоКоміркам_Objest = РозміщенняНоменклатуриПоКоміркам_Objest,
                        };

                        page.SetValue();

                        return page;
                    }, true);
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override void SetDeletionLabel(UnigueID unigueID)
        {
            РозміщенняНоменклатуриПоКоміркам_Objest РозміщенняНоменклатуриПоКоміркам_Objest = new РозміщенняНоменклатуриПоКоміркам_Objest();
            if (РозміщенняНоменклатуриПоКоміркам_Objest.Read(unigueID))
                РозміщенняНоменклатуриПоКоміркам_Objest.SetDeletionLabel(!РозміщенняНоменклатуриПоКоміркам_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            РозміщенняНоменклатуриПоКоміркам_Objest РозміщенняНоменклатуриПоКоміркам_Objest = new РозміщенняНоменклатуриПоКоміркам_Objest();
            if (РозміщенняНоменклатуриПоКоміркам_Objest.Read(unigueID))
            {
                РозміщенняНоменклатуриПоКоміркам_Objest РозміщенняНоменклатуриПоКоміркам_Objest_Новий = РозміщенняНоменклатуриПоКоміркам_Objest.Copy(true);
                РозміщенняНоменклатуриПоКоміркам_Objest_Новий.Save();
                РозміщенняНоменклатуриПоКоміркам_Objest_Новий.Товари_TablePart.Save(true);

                return РозміщенняНоменклатуриПоКоміркам_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        protected override void PeriodWhereChanged()
        {
            ТабличніСписки.РозміщенняНоменклатуриПоКоміркам_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        protected override void SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            РозміщенняНоменклатуриПоКоміркам_Pointer РозміщенняНоменклатуриПоКоміркам_Pointer = new РозміщенняНоменклатуриПоКоміркам_Pointer(unigueID);
            РозміщенняНоменклатуриПоКоміркам_Objest? РозміщенняНоменклатуриПоКоміркам_Objest = РозміщенняНоменклатуриПоКоміркам_Pointer.GetDocumentObject(true);
            if (РозміщенняНоменклатуриПоКоміркам_Objest == null) return;

            if (spendDoc)
            {
                if (!РозміщенняНоменклатуриПоКоміркам_Objest.SpendTheDocument(РозміщенняНоменклатуриПоКоміркам_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
            }
            else
                РозміщенняНоменклатуриПоКоміркам_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new РозміщенняНоменклатуриПоКоміркам_Pointer(unigueID);
        }

        protected override void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{РозміщенняНоменклатуриПоКоміркам_Const.FULLNAME}_{unigueID}.xml");
            РозміщенняНоменклатуриПоКоміркам_Export.ToXmlFile(new РозміщенняНоменклатуриПоКоміркам_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar

        #endregion
    }
}