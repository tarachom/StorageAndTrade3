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
    public class ЗамовленняПостачальнику : ДокументЖурнал
    {
        public ЗамовленняПостачальнику() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.ЗамовленняПостачальнику_Записи.Store;
            ТабличніСписки.ЗамовленняПостачальнику_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.ЗамовленняПостачальнику_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ЗамовленняПостачальнику_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ЗамовленняПостачальнику_Записи.LoadRecords();

            if (ТабличніСписки.ЗамовленняПостачальнику_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЗамовленняПостачальнику_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ЗамовленняПостачальнику_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЗамовленняПостачальнику_Записи.CurrentPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ЗамовленняПостачальнику_Записи.Where.Clear();

            //Назва
            ТабличніСписки.ЗамовленняПостачальнику_Записи.Where.Add(
                new Where(ЗамовленняПостачальнику_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.ЗамовленняПостачальнику_Записи.LoadRecords();

            if (ТабличніСписки.ЗамовленняПостачальнику_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЗамовленняПостачальнику_Записи.FirstPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{ЗамовленняПостачальнику_Const.FULLNAME} *", () =>
                {
                    ЗамовленняПостачальнику_Елемент page = new ЗамовленняПостачальнику_Елемент
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
                ЗамовленняПостачальнику_Objest ЗамовленняПостачальнику_Objest = new ЗамовленняПостачальнику_Objest();
                if (await ЗамовленняПостачальнику_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ЗамовленняПостачальнику_Objest.Назва}", () =>
                    {
                        ЗамовленняПостачальнику_Елемент page = new ЗамовленняПостачальнику_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ЗамовленняПостачальнику_Objest = ЗамовленняПостачальнику_Objest,
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
            ЗамовленняПостачальнику_Objest ЗамовленняПостачальнику_Objest = new ЗамовленняПостачальнику_Objest();
            if (await ЗамовленняПостачальнику_Objest.Read(unigueID))
                await ЗамовленняПостачальнику_Objest.SetDeletionLabel(!ЗамовленняПостачальнику_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ЗамовленняПостачальнику_Objest ЗамовленняПостачальнику_Objest = new ЗамовленняПостачальнику_Objest();
            if (await ЗамовленняПостачальнику_Objest.Read(unigueID))
            {
                ЗамовленняПостачальнику_Objest ЗамовленняПостачальнику_Objest_Новий = ЗамовленняПостачальнику_Objest.Copy(true);
                await ЗамовленняПостачальнику_Objest_Новий.Save();
                await ЗамовленняПостачальнику_Objest_Новий.Товари_TablePart.Save(true);

                return ЗамовленняПостачальнику_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        protected override void PeriodWhereChanged()
        {
            ТабличніСписки.ЗамовленняПостачальнику_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ЗамовленняПостачальнику_Pointer ЗамовленняПостачальнику_Pointer = new ЗамовленняПостачальнику_Pointer(unigueID);
            ЗамовленняПостачальнику_Objest? ЗамовленняПостачальнику_Objest = await ЗамовленняПостачальнику_Pointer.GetDocumentObject(true);
            if (ЗамовленняПостачальнику_Objest == null) return;

            if (spendDoc)
            {
                if (!await ЗамовленняПостачальнику_Objest.SpendTheDocument(ЗамовленняПостачальнику_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
            }
            else
                await ЗамовленняПостачальнику_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ЗамовленняПостачальнику_Pointer(unigueID);
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ЗамовленняПостачальнику_Const.FULLNAME}_{unigueID}.xml");
            await ЗамовленняПостачальнику_Export.ToXmlFile(new ЗамовленняПостачальнику_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar

        protected override Menu? ToolbarNaOsnoviSubMenu()
        {
            Menu Menu = new Menu();

            MenuItem newDocPryhydnaNakladnaButton = new MenuItem("Поступлення товарів та послуг");
            newDocPryhydnaNakladnaButton.Activated += OnNewDocNaOsnovi_PryhydnaNakladna;
            Menu.Append(newDocPryhydnaNakladnaButton);

            MenuItem newDocKasovyiOrderButton = new MenuItem("Розхідний касовий ордер");
            newDocKasovyiOrderButton.Activated += OnNewDocNaOsnovi_KasovyiOrder;
            Menu.Append(newDocKasovyiOrderButton);

            Menu.ShowAll();

            return Menu;
        }

        async void OnNewDocNaOsnovi_PryhydnaNakladna(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ЗамовленняПостачальнику_Pointer замовленняПостачальнику_Pointer = new ЗамовленняПостачальнику_Pointer(new UnigueID(uid));
                    ЗамовленняПостачальнику_Objest? замовленняПостачальнику_Objest = await замовленняПостачальнику_Pointer.GetDocumentObject(true);
                    if (замовленняПостачальнику_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Новий = new ПоступленняТоварівТаПослуг_Objest();
                    поступленняТоварівТаПослуг_Новий.New();
                    поступленняТоварівТаПослуг_Новий.Організація = замовленняПостачальнику_Objest.Організація;
                    поступленняТоварівТаПослуг_Новий.Валюта = замовленняПостачальнику_Objest.Валюта;
                    поступленняТоварівТаПослуг_Новий.Каса = замовленняПостачальнику_Objest.Каса;
                    поступленняТоварівТаПослуг_Новий.Контрагент = замовленняПостачальнику_Objest.Контрагент;
                    поступленняТоварівТаПослуг_Новий.Договір = замовленняПостачальнику_Objest.Договір;
                    поступленняТоварівТаПослуг_Новий.Склад = замовленняПостачальнику_Objest.Склад;
                    поступленняТоварівТаПослуг_Новий.СумаДокументу = замовленняПостачальнику_Objest.СумаДокументу;
                    поступленняТоварівТаПослуг_Новий.ФормаОплати = замовленняПостачальнику_Objest.ФормаОплати;
                    поступленняТоварівТаПослуг_Новий.ЗамовленняПостачальнику = замовленняПостачальнику_Objest.GetDocumentPointer();
                    поступленняТоварівТаПослуг_Новий.Основа = замовленняПостачальнику_Objest.GetBasis();

                    if (await поступленняТоварівТаПослуг_Новий.Save())
                    {
                        //Товари
                        foreach (ЗамовленняПостачальнику_Товари_TablePart.Record record_замовлення in замовленняПостачальнику_Objest.Товари_TablePart.Records)
                        {
                            ПоступленняТоварівТаПослуг_Товари_TablePart.Record record_поступлення = new ПоступленняТоварівТаПослуг_Товари_TablePart.Record();
                            поступленняТоварівТаПослуг_Новий.Товари_TablePart.Records.Add(record_поступлення);

                            record_поступлення.Номенклатура = record_замовлення.Номенклатура;
                            record_поступлення.ХарактеристикаНоменклатури = record_замовлення.ХарактеристикаНоменклатури;
                            record_поступлення.Пакування = record_замовлення.Пакування;
                            record_поступлення.КількістьУпаковок = record_замовлення.КількістьУпаковок;
                            record_поступлення.Кількість = record_замовлення.Кількість;
                            record_поступлення.Ціна = record_замовлення.Ціна;
                            record_поступлення.Сума = record_замовлення.Сума;
                            record_поступлення.Скидка = record_замовлення.Скидка;
                            record_поступлення.ЗамовленняПостачальнику = замовленняПостачальнику_Objest.GetDocumentPointer();
                            record_поступлення.Склад = замовленняПостачальнику_Objest.Склад;
                        }

                        await поступленняТоварівТаПослуг_Новий.Товари_TablePart.Save(false);

                        Program.GeneralForm?.CreateNotebookPage($"{поступленняТоварівТаПослуг_Новий.Назва}", () =>
                        {
                            ПоступленняТоварівТаПослуг_Елемент page = new ПоступленняТоварівТаПослуг_Елемент
                            {
                                IsNew = false,
                                ПоступленняТоварівТаПослуг_Objest = поступленняТоварівТаПослуг_Новий,
                            };

                            page.SetValue();

                            return page;
                        });
                    }
                }
            }
        }

        async void OnNewDocNaOsnovi_KasovyiOrder(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ЗамовленняПостачальнику_Pointer замовленняПостачальнику_Pointer = new ЗамовленняПостачальнику_Pointer(new UnigueID(uid));
                    ЗамовленняПостачальнику_Objest? замовленняПостачальнику_Objest = await замовленняПостачальнику_Pointer.GetDocumentObject(true);
                    if (замовленняПостачальнику_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    РозхіднийКасовийОрдер_Objest розхіднийКасовийОрдер_Новий = new РозхіднийКасовийОрдер_Objest();
                    розхіднийКасовийОрдер_Новий.New();
                    розхіднийКасовийОрдер_Новий.Організація = замовленняПостачальнику_Objest.Організація;
                    розхіднийКасовийОрдер_Новий.Валюта = замовленняПостачальнику_Objest.Валюта;
                    розхіднийКасовийОрдер_Новий.Каса = замовленняПостачальнику_Objest.Каса;
                    розхіднийКасовийОрдер_Новий.Контрагент = замовленняПостачальнику_Objest.Контрагент;
                    розхіднийКасовийОрдер_Новий.Договір = замовленняПостачальнику_Objest.Договір;
                    розхіднийКасовийОрдер_Новий.СумаДокументу = замовленняПостачальнику_Objest.СумаДокументу;
                    розхіднийКасовийОрдер_Новий.Основа = замовленняПостачальнику_Objest.GetBasis();

                    if (await розхіднийКасовийОрдер_Новий.Save())
                    {
                        Program.GeneralForm?.CreateNotebookPage($"{розхіднийКасовийОрдер_Новий.Назва}", () =>
                        {
                            РозхіднийКасовийОрдер_Елемент page = new РозхіднийКасовийОрдер_Елемент
                            {
                                IsNew = false,
                                РозхіднийКасовийОрдер_Objest = розхіднийКасовийОрдер_Новий,
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