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

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    public class АктВиконанихРобіт : ДокументЖурнал
    {
        public АктВиконанихРобіт() 
        {
            ТабличніСписки.АктВиконанихРобіт_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.АктВиконанихРобіт_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.АктВиконанихРобіт_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.АктВиконанихРобіт_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.АктВиконанихРобіт_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Назва
            ТабличніСписки.АктВиконанихРобіт_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(АктВиконанихРобіт_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.АктВиконанихРобіт_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.АктВиконанихРобіт_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            АктВиконанихРобіт_Елемент page = new АктВиконанихРобіт_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (IsNew)
                await page.Елемент.New();
            else if (unigueID == null || !await page.Елемент.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);

            page.SetValue();
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            АктВиконанихРобіт_Objest АктВиконанихРобіт_Objest = new АктВиконанихРобіт_Objest();
            if (await АктВиконанихРобіт_Objest.Read(unigueID))
                await АктВиконанихРобіт_Objest.SetDeletionLabel(!АктВиконанихРобіт_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            АктВиконанихРобіт_Objest АктВиконанихРобіт_Objest = new АктВиконанихРобіт_Objest();
            if (await АктВиконанихРобіт_Objest.Read(unigueID))
            {
                АктВиконанихРобіт_Objest АктВиконанихРобіт_Objest_Новий = await АктВиконанихРобіт_Objest.Copy(true);
                await АктВиконанихРобіт_Objest_Новий.Save();
                await АктВиконанихРобіт_Objest_Новий.Послуги_TablePart.Save(true);

                return АктВиконанихРобіт_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "Документи.АктВиконанихРобіт";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await LoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            АктВиконанихРобіт_Pointer АктВиконанихРобіт_Pointer = new АктВиконанихРобіт_Pointer(unigueID);
            АктВиконанихРобіт_Objest? АктВиконанихРобіт_Objest = await АктВиконанихРобіт_Pointer.GetDocumentObject(true);
            if (АктВиконанихРобіт_Objest == null) return;

            if (spendDoc)
            {
                if (!await АктВиконанихРобіт_Objest.SpendTheDocument(АктВиконанихРобіт_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(АктВиконанихРобіт_Objest.UnigueID);
            }
            else
                await АктВиконанихРобіт_Objest.ClearSpendTheDocument();
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new АктВиконанихРобіт_Pointer(unigueID));
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{АктВиконанихРобіт_Const.FULLNAME}_{unigueID}.xml");
            await АктВиконанихРобіт_Export.ToXmlFile(new АктВиконанихРобіт_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar

        protected override Menu? ToolbarNaOsnoviSubMenu()
        {
            Menu Menu = new Menu();

            {
                MenuItem doc = new MenuItem($"{ПрихіднийКасовийОрдер_Const.FULLNAME}");
                doc.Activated += OnNewDocNaOsnovi_KasovyiOrder;
                Menu.Append(doc);
            }

            Menu.ShowAll();
            return Menu;
        }

        async void OnNewDocNaOsnovi_KasovyiOrder(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    АктВиконанихРобіт_Pointer актВиконанихРобіт_Pointer = new АктВиконанихРобіт_Pointer(new UnigueID(uid));
                    АктВиконанихРобіт_Objest? актВиконанихРобіт_Objest = await актВиконанихРобіт_Pointer.GetDocumentObject(false);
                    if (актВиконанихРобіт_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    ПрихіднийКасовийОрдер_Objest прихіднийКасовийОрдер_Новий = new ПрихіднийКасовийОрдер_Objest();
                    await прихіднийКасовийОрдер_Новий.New();
                    прихіднийКасовийОрдер_Новий.Організація = актВиконанихРобіт_Objest.Організація;
                    прихіднийКасовийОрдер_Новий.Валюта = актВиконанихРобіт_Objest.Валюта;
                    прихіднийКасовийОрдер_Новий.Каса = актВиконанихРобіт_Objest.Каса;
                    прихіднийКасовийОрдер_Новий.Контрагент = актВиконанихРобіт_Objest.Контрагент;
                    прихіднийКасовийОрдер_Новий.Договір = актВиконанихРобіт_Objest.Договір;
                    прихіднийКасовийОрдер_Новий.Основа = актВиконанихРобіт_Objest.GetBasis();
                    прихіднийКасовийОрдер_Новий.СумаДокументу = актВиконанихРобіт_Objest.СумаДокументу;

                    if (await прихіднийКасовийОрдер_Новий.Save())
                    {
                        NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{прихіднийКасовийОрдер_Новий.Назва}", () =>
                        {
                            ПрихіднийКасовийОрдер_Елемент page = new ПрихіднийКасовийОрдер_Елемент
                            {
                                IsNew = false,
                                Елемент = прихіднийКасовийОрдер_Новий,
                            };

                            page.SetValue();

                            return page;
                        });
                    }
                }
            }
        }

        #endregion
    }
}