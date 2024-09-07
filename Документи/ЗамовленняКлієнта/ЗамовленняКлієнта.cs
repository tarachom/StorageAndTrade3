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
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    public class ЗамовленняКлієнта : ДокументЖурнал
    {
        public ЗамовленняКлієнта() : base()
        {
            ТабличніСписки.ЗамовленняКлієнта_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async void LoadRecords()
        {
            ТабличніСписки.ЗамовленняКлієнта_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ЗамовленняКлієнта_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ЗамовленняКлієнта_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ЗамовленняКлієнта_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            //Назва
            ТабличніСписки.ЗамовленняКлієнта_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(ЗамовленняКлієнта_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.ЗамовленняКлієнта_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.ЗамовленняКлієнта_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask<(string Name, Func<Widget>? FuncWidget, System.Action? SetValue)> OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ЗамовленняКлієнта_Елемент page = new ЗамовленняКлієнта_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (IsNew)
                await page.Елемент.New();
            else if (unigueID == null || !await page.Елемент.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return ("", null, null);
            }

            return (page.Caption, () => page, page.SetValue);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest = new ЗамовленняКлієнта_Objest();
            if (await ЗамовленняКлієнта_Objest.Read(unigueID))
                await ЗамовленняКлієнта_Objest.SetDeletionLabel(!ЗамовленняКлієнта_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest = new ЗамовленняКлієнта_Objest();
            if (await ЗамовленняКлієнта_Objest.Read(unigueID))
            {
                ЗамовленняКлієнта_Objest ЗамовленняКлієнта_Objest_Новий = await ЗамовленняКлієнта_Objest.Copy(true);
                await ЗамовленняКлієнта_Objest_Новий.Save();
                await ЗамовленняКлієнта_Objest_Новий.Товари_TablePart.Save(true);

                return ЗамовленняКлієнта_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "Документи.ЗамовленняКлієнта";

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
            ЗамовленняКлієнта_Pointer ЗамовленняКлієнта_Pointer = new ЗамовленняКлієнта_Pointer(unigueID);
            ЗамовленняКлієнта_Objest? ЗамовленняКлієнта_Objest = await ЗамовленняКлієнта_Pointer.GetDocumentObject(true);
            if (ЗамовленняКлієнта_Objest == null) return;

            if (spendDoc)
            {
                if (!await ЗамовленняКлієнта_Objest.SpendTheDocument(ЗамовленняКлієнта_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(ЗамовленняКлієнта_Objest.UnigueID);
            }
            else
                await ЗамовленняКлієнта_Objest.ClearSpendTheDocument();
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ЗамовленняКлієнта_Pointer(unigueID));
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ЗамовленняКлієнта_Const.FULLNAME}_{unigueID}.xml");
            await ЗамовленняКлієнта_Export.ToXmlFile(new ЗамовленняКлієнта_Pointer(unigueID), pathToSave);
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

        async void OnNewDocNaOsnovi_РеалізаціяТоварівТаПослуг(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ЗамовленняКлієнта_Pointer замовленняКлієнта_Pointer = new ЗамовленняКлієнта_Pointer(new UnigueID(uid));
                    ЗамовленняКлієнта_Objest? замовленняКлієнта_Objest = await замовленняКлієнта_Pointer.GetDocumentObject(true);
                    if (замовленняКлієнта_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Новий = new РеалізаціяТоварівТаПослуг_Objest();
                    await реалізаціяТоварівТаПослуг_Новий.New();
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

                    if (await реалізаціяТоварівТаПослуг_Новий.Save())
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

                        await реалізаціяТоварівТаПослуг_Новий.Товари_TablePart.Save(false);

                        NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{реалізаціяТоварівТаПослуг_Новий.Назва}", () =>
                        {
                            РеалізаціяТоварівТаПослуг_Елемент page = new РеалізаціяТоварівТаПослуг_Елемент
                            {
                                IsNew = false,
                                Елемент = реалізаціяТоварівТаПослуг_Новий,
                            };

                            page.SetValue();

                            return page;
                        });
                    }
                }
            }
        }

        async void OnNewDocNaOsnovi_ЗамовленняПостачальнику(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ЗамовленняКлієнта_Pointer замовленняКлієнта_Pointer = new ЗамовленняКлієнта_Pointer(new UnigueID(uid));
                    ЗамовленняКлієнта_Objest? замовленняКлієнта_Objest = await замовленняКлієнта_Pointer.GetDocumentObject(true);
                    if (замовленняКлієнта_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    ЗамовленняПостачальнику_Objest замовленняПостачальнику_Новий = new ЗамовленняПостачальнику_Objest();
                    await замовленняПостачальнику_Новий.New();
                    замовленняПостачальнику_Новий.Організація = замовленняКлієнта_Objest.Організація;
                    замовленняПостачальнику_Новий.Валюта = замовленняКлієнта_Objest.Валюта;
                    замовленняПостачальнику_Новий.Каса = замовленняКлієнта_Objest.Каса;
                    замовленняПостачальнику_Новий.Контрагент = замовленняКлієнта_Objest.Контрагент;
                    замовленняПостачальнику_Новий.Договір = await ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(замовленняКлієнта_Objest.Контрагент, ТипДоговорів.ЗПостачальниками) ?? замовленняКлієнта_Objest.Договір;
                    замовленняПостачальнику_Новий.Склад = замовленняКлієнта_Objest.Склад;
                    замовленняПостачальнику_Новий.СумаДокументу = замовленняКлієнта_Objest.СумаДокументу;
                    замовленняПостачальнику_Новий.Статус = СтатусиЗамовленьПостачальникам.Підтверджений;
                    замовленняПостачальнику_Новий.ФормаОплати = замовленняКлієнта_Objest.ФормаОплати;
                    замовленняПостачальнику_Новий.Основа = замовленняКлієнта_Objest.GetBasis();

                    if (await замовленняПостачальнику_Новий.Save())
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

                        await замовленняПостачальнику_Новий.Товари_TablePart.Save(false);

                        NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{замовленняПостачальнику_Новий.Назва}", () =>
                        {
                            ЗамовленняПостачальнику_Елемент page = new ЗамовленняПостачальнику_Елемент
                            {
                                IsNew = false,
                                Елемент = замовленняПостачальнику_Новий,
                            };

                            page.SetValue();

                            return page;
                        });
                    }
                }
            }
        }

        async void OnNewDocNaOsnovi_ПоступленняТоварівТаПослуг(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ЗамовленняКлієнта_Pointer замовленняКлієнта_Pointer = new ЗамовленняКлієнта_Pointer(new UnigueID(uid));
                    ЗамовленняКлієнта_Objest? замовленняКлієнта_Objest = await замовленняКлієнта_Pointer.GetDocumentObject(true);
                    if (замовленняКлієнта_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    ПоступленняТоварівТаПослуг_Objest поступленняТоварівТаПослуг_Новий = new ПоступленняТоварівТаПослуг_Objest();
                    await поступленняТоварівТаПослуг_Новий.New();
                    поступленняТоварівТаПослуг_Новий.Організація = замовленняКлієнта_Objest.Організація;
                    поступленняТоварівТаПослуг_Новий.Валюта = замовленняКлієнта_Objest.Валюта;
                    поступленняТоварівТаПослуг_Новий.Каса = замовленняКлієнта_Objest.Каса;
                    поступленняТоварівТаПослуг_Новий.Контрагент = замовленняКлієнта_Objest.Контрагент;
                    поступленняТоварівТаПослуг_Новий.Договір = await ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(замовленняКлієнта_Objest.Контрагент, ТипДоговорів.ЗПостачальниками) ?? замовленняКлієнта_Objest.Договір;
                    поступленняТоварівТаПослуг_Новий.Склад = замовленняКлієнта_Objest.Склад;
                    поступленняТоварівТаПослуг_Новий.СумаДокументу = замовленняКлієнта_Objest.СумаДокументу;
                    поступленняТоварівТаПослуг_Новий.ФормаОплати = замовленняКлієнта_Objest.ФормаОплати;
                    поступленняТоварівТаПослуг_Новий.Основа = замовленняКлієнта_Objest.GetBasis();

                    if (await поступленняТоварівТаПослуг_Новий.Save())
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

                        await поступленняТоварівТаПослуг_Новий.Товари_TablePart.Save(false);

                        NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{поступленняТоварівТаПослуг_Новий.Назва}", () =>
                        {
                            ПоступленняТоварівТаПослуг_Елемент page = new ПоступленняТоварівТаПослуг_Елемент
                            {
                                IsNew = false,
                                Елемент = поступленняТоварівТаПослуг_Новий,
                            };

                            page.SetValue();

                            return page;
                        });
                    }
                }
            }
        }

        async void OnNewDocNaOsnovi_ПрихіднийКасовийОрдер(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ЗамовленняКлієнта_Pointer замовленняКлієнта_Pointer = new ЗамовленняКлієнта_Pointer(new UnigueID(uid));
                    ЗамовленняКлієнта_Objest? замовленняКлієнта_Objest = await замовленняКлієнта_Pointer.GetDocumentObject(true);
                    if (замовленняКлієнта_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    ПрихіднийКасовийОрдер_Objest прихіднийКасовийОрдер_Новий = new ПрихіднийКасовийОрдер_Objest();
                    await прихіднийКасовийОрдер_Новий.New();
                    прихіднийКасовийОрдер_Новий.ГосподарськаОперація = ГосподарськіОперації.ПоступленняОплатиВідКлієнта;
                    прихіднийКасовийОрдер_Новий.Організація = замовленняКлієнта_Objest.Організація;
                    прихіднийКасовийОрдер_Новий.Валюта = замовленняКлієнта_Objest.Валюта;
                    прихіднийКасовийОрдер_Новий.Каса = замовленняКлієнта_Objest.Каса;
                    прихіднийКасовийОрдер_Новий.Контрагент = замовленняКлієнта_Objest.Контрагент;
                    прихіднийКасовийОрдер_Новий.Договір = замовленняКлієнта_Objest.Договір;
                    прихіднийКасовийОрдер_Новий.СумаДокументу = замовленняКлієнта_Objest.СумаДокументу;
                    прихіднийКасовийОрдер_Новий.Основа = замовленняКлієнта_Objest.GetBasis();

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