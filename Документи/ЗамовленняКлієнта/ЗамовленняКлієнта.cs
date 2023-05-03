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
    public class ЗамовленняКлієнта : ДокументЖурнал
    {
        public ЗамовленняКлієнта() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.ЗамовленняКлієнта_Записи.Store;
            ТабличніСписки.ЗамовленняКлієнта_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.ЗамовленняКлієнта_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ЗамовленняКлієнта_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ЗамовленняКлієнта_Записи.LoadRecords();

            if (ТабличніСписки.ЗамовленняКлієнта_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЗамовленняКлієнта_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ЗамовленняКлієнта_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЗамовленняКлієнта_Записи.CurrentPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ЗамовленняКлієнта_Записи.Where.Clear();

            //Назва
            ТабличніСписки.ЗамовленняКлієнта_Записи.Where.Add(
                new Where(ЗамовленняКлієнта_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.ЗамовленняКлієнта_Записи.LoadRecords();

            if (ТабличніСписки.ЗамовленняКлієнта_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ЗамовленняКлієнта_Записи.FirstPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{ЗамовленняКлієнта_Const.FULLNAME} *", () =>
                {
                    ЗамовленняКлієнта_Елемент page = new ЗамовленняКлієнта_Елемент
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
                ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest = new ЗамовленняКлієнта_Objest();
                if (ЗамовленняКлієнта_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ЗамовленняКлієнта_Objest.Назва}", () =>
                    {
                        ЗамовленняКлієнта_Елемент page = new ЗамовленняКлієнта_Елемент
                        {
                            PageList = this,
                            IsNew = false,
                            ЗамовленняКлієнта_Objest = ЗамовленняКлієнта_Objest,
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
            ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest = new ЗамовленняКлієнта_Objest();
            if (ЗамовленняКлієнта_Objest.Read(unigueID))
                ЗамовленняКлієнта_Objest.SetDeletionLabel(!ЗамовленняКлієнта_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest = new ЗамовленняКлієнта_Objest();
            if (ЗамовленняКлієнта_Objest.Read(unigueID))
            {
                ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest_Новий = ЗамовленняКлієнта_Objest.Copy(true);
                ЗамовленняКлієнта_Objest_Новий.Save();
                ЗамовленняКлієнта_Objest_Новий.Товари_TablePart.Save(true);

                return ЗамовленняКлієнта_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        protected override void PeriodWhereChanged()
        {
            ТабличніСписки.ЗамовленняКлієнта_Записи.ДодатиВідбірПоПеріоду(Enum.Parse<ТипПеріодуДляЖурналівДокументів>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        protected override void SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ЗамовленняКлієнта_Pointer ЗамовленняКлієнта_Pointer = new ЗамовленняКлієнта_Pointer(unigueID);
            ЗамовленняКлієнта_Objest? ЗамовленняКлієнта_Objest = ЗамовленняКлієнта_Pointer.GetDocumentObject(true);
            if (ЗамовленняКлієнта_Objest == null) return;

            if (spendDoc)
            {
                if (!ЗамовленняКлієнта_Objest.SpendTheDocument(ЗамовленняКлієнта_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
            }
            else
                ЗамовленняКлієнта_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ЗамовленняКлієнта_Pointer(unigueID);
        }

        protected override void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ЗамовленняКлієнта_Const.FULLNAME}_{unigueID}.xml");
            ЗамовленняКлієнта_Export.ToXmlFile(new ЗамовленняКлієнта_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar

        protected override Menu? ToolbarNaOsnoviSubMenu()
        {
            Menu Menu = new Menu();

            {
                MenuItem doc = new MenuItem("Реалізація товарів та послуг");
                doc.Activated += OnNewDocNaOsnovi_РеалізаціяТоварівТаПослуг;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem("Замовлення постачальнику");
                doc.Activated += OnNewDocNaOsnovi_ЗамовленняПостачальнику;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem("Поступлення товарів та послуг");
                doc.Activated += OnNewDocNaOsnovi_ПоступленняТоварівТаПослуг;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem("Прихідний касовий ордер");
                doc.Activated += OnNewDocNaOsnovi_ПрихіднийКасовийОрдер;
                Menu.Append(doc);
            }

            Menu.ShowAll();

            return Menu;
        }

        void OnNewDocNaOsnovi_РеалізаціяТоварівТаПослуг(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ЗамовленняКлієнта_Pointer замовленняКлієнта_Pointer = new ЗамовленняКлієнта_Pointer(new UnigueID(uid));
                    ЗамовленняКлієнта_Objest? замовленняКлієнта_Objest = замовленняКлієнта_Pointer.GetDocumentObject(true);
                    if (замовленняКлієнта_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Новий = new РеалізаціяТоварівТаПослуг_Objest();
                    реалізаціяТоварівТаПослуг_Новий.New();
                    реалізаціяТоварівТаПослуг_Новий.Організація = замовленняКлієнта_Objest.Організація;
                    реалізаціяТоварівТаПослуг_Новий.Валюта = замовленняКлієнта_Objest.Валюта;
                    реалізаціяТоварівТаПослуг_Новий.Каса = замовленняКлієнта_Objest.Каса;
                    реалізаціяТоварівТаПослуг_Новий.Контрагент = замовленняКлієнта_Objest.Контрагент;
                    реалізаціяТоварівТаПослуг_Новий.Договір = замовленняКлієнта_Objest.Договір;
                    реалізаціяТоварівТаПослуг_Новий.Склад = замовленняКлієнта_Objest.Склад;
                    реалізаціяТоварівТаПослуг_Новий.СумаДокументу = замовленняКлієнта_Objest.СумаДокументу;
                    реалізаціяТоварівТаПослуг_Новий.Статус = СтатусиРеалізаціїТоварівТаПослуг.ДоОплати;
                    реалізаціяТоварівТаПослуг_Новий.ФормаОплати = замовленняКлієнта_Objest.ФормаОплати;
                    реалізаціяТоварівТаПослуг_Новий.Основа = замовленняКлієнта_Objest.GetBasis();

                    if (реалізаціяТоварівТаПослуг_Новий.Save())
                    {
                        //Товари
                        foreach (ЗамовленняКлієнта_Товари_TablePart.Record record_замовлення in замовленняКлієнта_Objest.Товари_TablePart.Records)
                        {
                            РеалізаціяТоварівТаПослуг_Товари_TablePart.Record record_реалізація = new РеалізаціяТоварівТаПослуг_Товари_TablePart.Record();
                            реалізаціяТоварівТаПослуг_Новий.Товари_TablePart.Records.Add(record_реалізація);

                            record_реалізація.Номенклатура = record_замовлення.Номенклатура;
                            record_реалізація.ХарактеристикаНоменклатури = record_замовлення.ХарактеристикаНоменклатури;
                            record_реалізація.Пакування = record_замовлення.Пакування;
                            record_реалізація.КількістьУпаковок = record_замовлення.КількістьУпаковок;
                            record_реалізація.Кількість = record_замовлення.Кількість;
                            record_реалізація.Ціна = record_замовлення.Ціна;
                            record_реалізація.Сума = record_замовлення.Сума;
                            record_реалізація.Скидка = record_замовлення.Скидка;
                            record_реалізація.ЗамовленняКлієнта = замовленняКлієнта_Objest.GetDocumentPointer();
                            record_реалізація.Склад = замовленняКлієнта_Objest.Склад;
                            record_реалізація.ВидЦіни = record_замовлення.ВидЦіни;
                        }

                        реалізаціяТоварівТаПослуг_Новий.Товари_TablePart.Save(false);

                        Program.GeneralForm?.CreateNotebookPage($"{реалізаціяТоварівТаПослуг_Новий.Назва}", () =>
                        {
                            РеалізаціяТоварівТаПослуг_Елемент page = new РеалізаціяТоварівТаПослуг_Елемент
                            {
                                IsNew = false,
                                РеалізаціяТоварівТаПослуг_Objest = реалізаціяТоварівТаПослуг_Новий,
                            };

                            page.SetValue();

                            return page;
                        });
                    }
                }
            }
        }

        void OnNewDocNaOsnovi_ЗамовленняПостачальнику(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ЗамовленняКлієнта_Pointer замовленняКлієнта_Pointer = new ЗамовленняКлієнта_Pointer(new UnigueID(uid));
                    ЗамовленняКлієнта_Objest? замовленняКлієнта_Objest = замовленняКлієнта_Pointer.GetDocumentObject(true);
                    if (замовленняКлієнта_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    ЗамовленняПостачальнику_Objest замовленняПостачальнику_Новий = new ЗамовленняПостачальнику_Objest();
                    замовленняПостачальнику_Новий.New();
                    замовленняПостачальнику_Новий.Організація = замовленняКлієнта_Objest.Організація;
                    замовленняПостачальнику_Новий.Валюта = замовленняКлієнта_Objest.Валюта;
                    замовленняПостачальнику_Новий.Каса = замовленняКлієнта_Objest.Каса;
                    замовленняПостачальнику_Новий.Контрагент = замовленняКлієнта_Objest.Контрагент;
                    замовленняПостачальнику_Новий.Договір = ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(замовленняКлієнта_Objest.Контрагент, ТипДоговорів.ЗПостачальниками) ?? замовленняКлієнта_Objest.Договір;
                    замовленняПостачальнику_Новий.Склад = замовленняКлієнта_Objest.Склад;
                    замовленняПостачальнику_Новий.СумаДокументу = замовленняКлієнта_Objest.СумаДокументу;
                    замовленняПостачальнику_Новий.Статус = СтатусиЗамовленьПостачальникам.Підтверджений;
                    замовленняПостачальнику_Новий.ФормаОплати = замовленняКлієнта_Objest.ФормаОплати;
                    замовленняПостачальнику_Новий.Основа = замовленняКлієнта_Objest.GetBasis();

                    if (замовленняПостачальнику_Новий.Save())
                    {
                        //Товари
                        foreach (ЗамовленняКлієнта_Товари_TablePart.Record record_замовлення in замовленняКлієнта_Objest.Товари_TablePart.Records)
                        {
                            ЗамовленняПостачальнику_Товари_TablePart.Record record_замовленняПостачальнику = new ЗамовленняПостачальнику_Товари_TablePart.Record();
                            замовленняПостачальнику_Новий.Товари_TablePart.Records.Add(record_замовленняПостачальнику);

                            record_замовленняПостачальнику.Номенклатура = record_замовлення.Номенклатура;
                            record_замовленняПостачальнику.ХарактеристикаНоменклатури = record_замовлення.ХарактеристикаНоменклатури;
                            record_замовленняПостачальнику.Пакування = record_замовлення.Пакування;
                            record_замовленняПостачальнику.КількістьУпаковок = record_замовлення.КількістьУпаковок;
                            record_замовленняПостачальнику.Кількість = record_замовлення.Кількість;
                            record_замовленняПостачальнику.Ціна = record_замовлення.Ціна;
                            record_замовленняПостачальнику.Сума = record_замовлення.Сума;
                            record_замовленняПостачальнику.Скидка = record_замовлення.Скидка;
                            record_замовленняПостачальнику.Склад = замовленняКлієнта_Objest.Склад;
                        }

                        замовленняПостачальнику_Новий.Товари_TablePart.Save(false);

                        Program.GeneralForm?.CreateNotebookPage($"{замовленняПостачальнику_Новий.Назва}", () =>
                        {
                            ЗамовленняПостачальнику_Елемент page = new ЗамовленняПостачальнику_Елемент
                            {
                                IsNew = false,
                                ЗамовленняПостачальнику_Objest = замовленняПостачальнику_Новий,
                            };

                            page.SetValue();

                            return page;
                        });
                    }
                }
            }
        }

        void OnNewDocNaOsnovi_ПоступленняТоварівТаПослуг(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ЗамовленняКлієнта_Pointer замовленняКлієнта_Pointer = new ЗамовленняКлієнта_Pointer(new UnigueID(uid));
                    ЗамовленняКлієнта_Objest? замовленняКлієнта_Objest = замовленняКлієнта_Pointer.GetDocumentObject(true);
                    if (замовленняКлієнта_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Новий = new ПоступленняТоварівТаПослуг_Objest();
                    поступленняТоварівТаПослуг_Новий.New();
                    поступленняТоварівТаПослуг_Новий.Організація = замовленняКлієнта_Objest.Організація;
                    поступленняТоварівТаПослуг_Новий.Валюта = замовленняКлієнта_Objest.Валюта;
                    поступленняТоварівТаПослуг_Новий.Каса = замовленняКлієнта_Objest.Каса;
                    поступленняТоварівТаПослуг_Новий.Контрагент = замовленняКлієнта_Objest.Контрагент;
                    поступленняТоварівТаПослуг_Новий.Договір = ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(замовленняКлієнта_Objest.Контрагент, ТипДоговорів.ЗПостачальниками) ?? замовленняКлієнта_Objest.Договір;
                    поступленняТоварівТаПослуг_Новий.Склад = замовленняКлієнта_Objest.Склад;
                    поступленняТоварівТаПослуг_Новий.СумаДокументу = замовленняКлієнта_Objest.СумаДокументу;
                    поступленняТоварівТаПослуг_Новий.ФормаОплати = замовленняКлієнта_Objest.ФормаОплати;
                    поступленняТоварівТаПослуг_Новий.Основа = замовленняКлієнта_Objest.GetBasis();

                    if (поступленняТоварівТаПослуг_Новий.Save())
                    {
                        //Товари
                        foreach (ЗамовленняКлієнта_Товари_TablePart.Record record_замовлення in замовленняКлієнта_Objest.Товари_TablePart.Records)
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
                            //record_поступлення.ЗамовленняПостачальнику = замовленняПостачальнику_Objest.GetDocumentPointer();
                            record_поступлення.Склад = замовленняКлієнта_Objest.Склад;
                        }

                        поступленняТоварівТаПослуг_Новий.Товари_TablePart.Save(false);

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

        void OnNewDocNaOsnovi_ПрихіднийКасовийОрдер(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeIter iter;
                    TreeViewGrid.Model.GetIter(out iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ЗамовленняКлієнта_Pointer замовленняКлієнта_Pointer = new ЗамовленняКлієнта_Pointer(new UnigueID(uid));
                    ЗамовленняКлієнта_Objest? замовленняКлієнта_Objest = замовленняКлієнта_Pointer.GetDocumentObject(true);
                    if (замовленняКлієнта_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    ПрихіднийКасовийОрдер_Objest прихіднийКасовийОрдер_Новий = new ПрихіднийКасовийОрдер_Objest();
                    прихіднийКасовийОрдер_Новий.New();
                    прихіднийКасовийОрдер_Новий.ГосподарськаОперація = ГосподарськіОперації.ПоступленняОплатиВідКлієнта;
                    прихіднийКасовийОрдер_Новий.Організація = замовленняКлієнта_Objest.Організація;
                    прихіднийКасовийОрдер_Новий.Валюта = замовленняКлієнта_Objest.Валюта;
                    прихіднийКасовийОрдер_Новий.Каса = замовленняКлієнта_Objest.Каса;
                    прихіднийКасовийОрдер_Новий.Контрагент = замовленняКлієнта_Objest.Контрагент;
                    прихіднийКасовийОрдер_Новий.Договір = замовленняКлієнта_Objest.Договір;
                    прихіднийКасовийОрдер_Новий.СумаДокументу = замовленняКлієнта_Objest.СумаДокументу;
                    прихіднийКасовийОрдер_Новий.Основа = замовленняКлієнта_Objest.GetBasis();

                    if (прихіднийКасовийОрдер_Новий.Save())
                    {
                        Program.GeneralForm?.CreateNotebookPage($"{прихіднийКасовийОрдер_Новий.Назва}", () =>
                        {
                            ПрихіднийКасовийОрдер_Елемент page = new ПрихіднийКасовийОрдер_Елемент
                            {
                                IsNew = false,
                                ПрихіднийКасовийОрдер_Objest = прихіднийКасовийОрдер_Новий,
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