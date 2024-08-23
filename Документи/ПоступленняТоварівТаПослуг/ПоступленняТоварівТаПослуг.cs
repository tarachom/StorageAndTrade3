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
    public class ПоступленняТоварівТаПослуг : ДокументЖурнал
    {
        public ПоступленняТоварівТаПослуг() : base()
        {
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async void LoadRecords()
        {
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Назва
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(ПоступленняТоварівТаПослуг_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{ПоступленняТоварівТаПослуг_Const.FULLNAME} *", () =>
                {
                    ПоступленняТоварівТаПослуг_Елемент page = new ПоступленняТоварівТаПослуг_Елемент
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
                ПоступленняТоварівТаПослуг_Objest ПоступленняТоварівТаПослуг_Objest = new ПоступленняТоварівТаПослуг_Objest();
                if (await ПоступленняТоварівТаПослуг_Objest.Read(unigueID))
                {
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{ПоступленняТоварівТаПослуг_Objest.Назва}", () =>
                    {
                        ПоступленняТоварівТаПослуг_Елемент page = new ПоступленняТоварівТаПослуг_Елемент
                        {
                            UnigueID = unigueID,
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ПоступленняТоварівТаПослуг_Objest = ПоступленняТоварівТаПослуг_Objest,
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
            ПоступленняТоварівТаПослуг_Objest ПоступленняТоварівТаПослуг_Objest = new ПоступленняТоварівТаПослуг_Objest();
            if (await ПоступленняТоварівТаПослуг_Objest.Read(unigueID))
                await ПоступленняТоварівТаПослуг_Objest.SetDeletionLabel(!ПоступленняТоварівТаПослуг_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ПоступленняТоварівТаПослуг_Objest ПоступленняТоварівТаПослуг_Objest = new ПоступленняТоварівТаПослуг_Objest();
            if (await ПоступленняТоварівТаПослуг_Objest.Read(unigueID))
            {
                ПоступленняТоварівТаПослуг_Objest ПоступленняТоварівТаПослуг_Objest_Новий = await ПоступленняТоварівТаПослуг_Objest.Copy(true);
                await ПоступленняТоварівТаПослуг_Objest_Новий.Save();
                await ПоступленняТоварівТаПослуг_Objest_Новий.Товари_TablePart.Save(true);

                return ПоступленняТоварівТаПослуг_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "Документи.ПоступленняТоварівТаПослуг";

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
            ПоступленняТоварівТаПослуг_Pointer ПоступленняТоварівТаПослуг_Pointer = new ПоступленняТоварівТаПослуг_Pointer(unigueID);
            ПоступленняТоварівТаПослуг_Objest? ПоступленняТоварівТаПослуг_Objest = await ПоступленняТоварівТаПослуг_Pointer.GetDocumentObject(true);
            if (ПоступленняТоварівТаПослуг_Objest == null) return;

            if (spendDoc)
            {
                if (!await ПоступленняТоварівТаПослуг_Objest.SpendTheDocument(ПоступленняТоварівТаПослуг_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(ПоступленняТоварівТаПослуг_Objest.UnigueID);
            }
            else
                await ПоступленняТоварівТаПослуг_Objest.ClearSpendTheDocument();
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new ПоступленняТоварівТаПослуг_Pointer(unigueID));
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ПоступленняТоварівТаПослуг_Const.FULLNAME}_{unigueID}.xml");
            await ПоступленняТоварівТаПослуг_Export.ToXmlFile(new ПоступленняТоварівТаПослуг_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar

        protected override Menu? ToolbarNaOsnoviSubMenu()
        {
            Menu Menu = new Menu();

            MenuItem newDocKasovyiOrderButton = new MenuItem("Розхідний касовий ордер");
            newDocKasovyiOrderButton.Activated += OnNewDocNaOsnovi_РозхіднийКасовийОрдер;
            Menu.Append(newDocKasovyiOrderButton);

            MenuItem newDocPovernenjaPostachalnykuButton = new MenuItem("Повернення товарів постачальнику");
            newDocPovernenjaPostachalnykuButton.Activated += OnNewDocNaOsnovi_ПоверненняТоварівПостачальнику;
            Menu.Append(newDocPovernenjaPostachalnykuButton);

            MenuItem newDocRozmisctenjaNaSkaldyButton = new MenuItem("Розміщення товарів на складі");
            newDocRozmisctenjaNaSkaldyButton.Activated += OnNewDocNaOsnovi_РозміщенняТоварівНаСкладі;
            Menu.Append(newDocRozmisctenjaNaSkaldyButton);

            MenuItem newDocVnSpozivButton = new MenuItem("Внутрішнє споживання товарів");
            newDocVnSpozivButton.Activated += OnNewDocNaOsnovi_ВнутрішнєСпоживанняТоварів;
            Menu.Append(newDocVnSpozivButton);

            MenuItem newDocPeremischennyaTovariv = new MenuItem("Переміщення товарів");
            newDocPeremischennyaTovariv.Activated += OnNewDocNaOsnovi_ПереміщенняТоварів;
            Menu.Append(newDocPeremischennyaTovariv);

            Menu.ShowAll();

            return Menu;
        }

        async void OnNewDocNaOsnovi_РозхіднийКасовийОрдер(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ПоступленняТоварівТаПослуг_Pointer поступленняТоварівТаПослуг_Pointer = new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid));
                    ПоступленняТоварівТаПослуг_Objest? поступленняТоварівТаПослуг_Objest = await поступленняТоварівТаПослуг_Pointer.GetDocumentObject(false);
                    if (поступленняТоварівТаПослуг_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    РозхіднийКасовийОрдер_Objest розхіднийКасовийОрдер_Новий = new РозхіднийКасовийОрдер_Objest();
                    await розхіднийКасовийОрдер_Новий.New();
                    розхіднийКасовийОрдер_Новий.Організація = поступленняТоварівТаПослуг_Objest.Організація;
                    розхіднийКасовийОрдер_Новий.Валюта = поступленняТоварівТаПослуг_Objest.Валюта;
                    розхіднийКасовийОрдер_Новий.Каса = поступленняТоварівТаПослуг_Objest.Каса;
                    розхіднийКасовийОрдер_Новий.Контрагент = поступленняТоварівТаПослуг_Objest.Контрагент;
                    розхіднийКасовийОрдер_Новий.Договір = поступленняТоварівТаПослуг_Objest.Договір;
                    розхіднийКасовийОрдер_Новий.СумаДокументу = поступленняТоварівТаПослуг_Objest.СумаДокументу;
                    розхіднийКасовийОрдер_Новий.Основа = поступленняТоварівТаПослуг_Objest.GetBasis();
                    розхіднийКасовийОрдер_Новий.ГосподарськаОперація = ГосподарськіОперації.ОплатаПостачальнику;

                    if (await розхіднийКасовийОрдер_Новий.Save())
                    {
                        NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{розхіднийКасовийОрдер_Новий.Назва}", () =>
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

        async void OnNewDocNaOsnovi_ПоверненняТоварівПостачальнику(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ПоступленняТоварівТаПослуг_Pointer поступленняТоварівТаПослуг_Pointer = new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid));
                    ПоступленняТоварівТаПослуг_Objest? поступленняТоварівТаПослуг_Objest = await поступленняТоварівТаПослуг_Pointer.GetDocumentObject(true);
                    if (поступленняТоварівТаПослуг_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    ПоверненняТоварівПостачальнику_Objest поверненняТоварівПостачальнику_Objest = new ПоверненняТоварівПостачальнику_Objest();
                    await поверненняТоварівПостачальнику_Objest.New();
                    поверненняТоварівПостачальнику_Objest.Організація = поступленняТоварівТаПослуг_Objest.Організація;
                    поверненняТоварівПостачальнику_Objest.Валюта = поступленняТоварівТаПослуг_Objest.Валюта;
                    поверненняТоварівПостачальнику_Objest.Каса = поступленняТоварівТаПослуг_Objest.Каса;
                    поверненняТоварівПостачальнику_Objest.Контрагент = поступленняТоварівТаПослуг_Objest.Контрагент;
                    поверненняТоварівПостачальнику_Objest.Договір = поступленняТоварівТаПослуг_Objest.Договір;
                    поверненняТоварівПостачальнику_Objest.Склад = поступленняТоварівТаПослуг_Objest.Склад;
                    поверненняТоварівПостачальнику_Objest.СумаДокументу = поступленняТоварівТаПослуг_Objest.СумаДокументу;
                    поверненняТоварівПостачальнику_Objest.Основа = поступленняТоварівТаПослуг_Objest.GetBasis();

                    if (await поверненняТоварівПостачальнику_Objest.Save())
                    {
                        //Товари
                        foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record record in поступленняТоварівТаПослуг_Objest.Товари_TablePart.Records)
                        {
                            поверненняТоварівПостачальнику_Objest.Товари_TablePart.Records.Add(new ПоверненняТоварівПостачальнику_Товари_TablePart.Record()
                            {
                                Номенклатура = record.Номенклатура,
                                ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                                Серія = record.Серія,
                                Пакування = record.Пакування,
                                КількістьУпаковок = record.КількістьУпаковок,
                                Кількість = record.Кількість,
                                Ціна = record.Ціна,
                                Сума = record.Сума,
                                ДокументПоступлення = поступленняТоварівТаПослуг_Objest.GetDocumentPointer()
                            });
                        }

                        await поверненняТоварівПостачальнику_Objest.Товари_TablePart.Save(false);

                        NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{поверненняТоварівПостачальнику_Objest.Назва}", () =>
                        {
                            ПоверненняТоварівПостачальнику_Елемент page = new ПоверненняТоварівПостачальнику_Елемент
                            {
                                IsNew = false,
                                ПоверненняТоварівПостачальнику_Objest = поверненняТоварівПостачальнику_Objest
                            };

                            page.SetValue();

                            return page;
                        });
                    }
                }
            }
        }

        async void OnNewDocNaOsnovi_РозміщенняТоварівНаСкладі(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ПоступленняТоварівТаПослуг_Pointer поступленняТоварівТаПослуг_Pointer = new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid));
                    ПоступленняТоварівТаПослуг_Objest? поступленняТоварівТаПослуг_Objest = await поступленняТоварівТаПослуг_Pointer.GetDocumentObject(true);
                    if (поступленняТоварівТаПослуг_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    РозміщенняТоварівНаСкладі_Objest розміщенняТоварівНаСкладі_Objest = new РозміщенняТоварівНаСкладі_Objest();
                    await розміщенняТоварівНаСкладі_Objest.New();
                    розміщенняТоварівНаСкладі_Objest.Організація = поступленняТоварівТаПослуг_Objest.Організація;
                    розміщенняТоварівНаСкладі_Objest.Склад = поступленняТоварівТаПослуг_Objest.Склад;
                    розміщенняТоварівНаСкладі_Objest.Автор = поступленняТоварівТаПослуг_Objest.Автор;
                    розміщенняТоварівНаСкладі_Objest.Підрозділ = поступленняТоварівТаПослуг_Objest.Підрозділ;
                    розміщенняТоварівНаСкладі_Objest.Основа = поступленняТоварівТаПослуг_Objest.GetBasis();
                    розміщенняТоварівНаСкладі_Objest.ДокументПоступлення = поступленняТоварівТаПослуг_Pointer;

                    if (await розміщенняТоварівНаСкладі_Objest.Save())
                    {
                        //Товари
                        foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record record in поступленняТоварівТаПослуг_Objest.Товари_TablePart.Records)
                        {
                            розміщенняТоварівНаСкладі_Objest.Товари_TablePart.Records.Add(new РозміщенняТоварівНаСкладі_Товари_TablePart.Record()
                            {
                                Номенклатура = record.Номенклатура,
                                ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                                Серія = record.Серія,
                                Пакування = record.Пакування,
                                КількістьУпаковок = record.КількістьУпаковок,
                                Кількість = record.Кількість
                            });
                        }

                        await розміщенняТоварівНаСкладі_Objest.Товари_TablePart.Save(true);

                        NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{розміщенняТоварівНаСкладі_Objest.Назва}", () =>
                        {
                            РозміщенняТоварівНаСкладі_Елемент page = new РозміщенняТоварівНаСкладі_Елемент
                            {
                                IsNew = false,
                                РозміщенняТоварівНаСкладі_Objest = розміщенняТоварівНаСкладі_Objest
                            };

                            page.SetValue();

                            return page;
                        });
                    }
                }
            }
        }

        async void OnNewDocNaOsnovi_ВнутрішнєСпоживанняТоварів(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ПоступленняТоварівТаПослуг_Pointer поступленняТоварівТаПослуг_Pointer = new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid));
                    ПоступленняТоварівТаПослуг_Objest? поступленняТоварівТаПослуг_Objest = await поступленняТоварівТаПослуг_Pointer.GetDocumentObject(true);
                    if (поступленняТоварівТаПослуг_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    ВнутрішнєСпоживанняТоварів_Objest внутрішнєСпоживанняТоварів_Objest = new ВнутрішнєСпоживанняТоварів_Objest();
                    await внутрішнєСпоживанняТоварів_Objest.New();
                    внутрішнєСпоживанняТоварів_Objest.Організація = поступленняТоварівТаПослуг_Objest.Організація;
                    внутрішнєСпоживанняТоварів_Objest.Склад = поступленняТоварівТаПослуг_Objest.Склад;
                    внутрішнєСпоживанняТоварів_Objest.Валюта = поступленняТоварівТаПослуг_Objest.Валюта;
                    внутрішнєСпоживанняТоварів_Objest.Автор = поступленняТоварівТаПослуг_Objest.Автор;
                    внутрішнєСпоживанняТоварів_Objest.Підрозділ = поступленняТоварівТаПослуг_Objest.Підрозділ;
                    внутрішнєСпоживанняТоварів_Objest.Основа = поступленняТоварівТаПослуг_Objest.GetBasis();

                    if (await внутрішнєСпоживанняТоварів_Objest.Save())
                    {
                        //Товари
                        foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record record in поступленняТоварівТаПослуг_Objest.Товари_TablePart.Records)
                        {
                            внутрішнєСпоживанняТоварів_Objest.Товари_TablePart.Records.Add(new ВнутрішнєСпоживанняТоварів_Товари_TablePart.Record()
                            {
                                Номенклатура = record.Номенклатура,
                                ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                                Серія = record.Серія,
                                Пакування = record.Пакування,
                                КількістьУпаковок = record.КількістьУпаковок,
                                Кількість = record.Кількість,
                                Ціна = record.Ціна,
                                Сума = record.Сума
                            });
                        }

                        await внутрішнєСпоживанняТоварів_Objest.Товари_TablePart.Save(true);

                        NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{внутрішнєСпоживанняТоварів_Objest.Назва}", () =>
                        {
                            ВнутрішнєСпоживанняТоварів_Елемент page = new ВнутрішнєСпоживанняТоварів_Елемент
                            {
                                IsNew = false,
                                ВнутрішнєСпоживанняТоварів_Objest = внутрішнєСпоживанняТоварів_Objest
                            };

                            page.SetValue();

                            return page;
                        });
                    }
                }
            }
        }

        async void OnNewDocNaOsnovi_ПереміщенняТоварів(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ПоступленняТоварівТаПослуг_Pointer поступленняТоварівТаПослуг_Pointer = new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid));
                    ПоступленняТоварівТаПослуг_Objest? поступленняТоварівТаПослуг_Objest = await поступленняТоварівТаПослуг_Pointer.GetDocumentObject(true);
                    if (поступленняТоварівТаПослуг_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    ПереміщенняТоварів_Objest переміщенняТоварів_Objest = new ПереміщенняТоварів_Objest();
                    await переміщенняТоварів_Objest.New();
                    переміщенняТоварів_Objest.Організація = поступленняТоварівТаПослуг_Objest.Організація;
                    переміщенняТоварів_Objest.СкладВідправник = поступленняТоварівТаПослуг_Objest.Склад;
                    переміщенняТоварів_Objest.Автор = поступленняТоварівТаПослуг_Objest.Автор;
                    переміщенняТоварів_Objest.Підрозділ = поступленняТоварівТаПослуг_Objest.Підрозділ;
                    переміщенняТоварів_Objest.Основа = поступленняТоварівТаПослуг_Objest.GetBasis();

                    if (await переміщенняТоварів_Objest.Save())
                    {
                        //Товари
                        foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record record in поступленняТоварівТаПослуг_Objest.Товари_TablePart.Records)
                        {
                            переміщенняТоварів_Objest.Товари_TablePart.Records.Add(new ПереміщенняТоварів_Товари_TablePart.Record()
                            {
                                Номенклатура = record.Номенклатура,
                                ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                                Серія = record.Серія,
                                Пакування = record.Пакування,
                                КількістьУпаковок = record.КількістьУпаковок,
                                Кількість = record.Кількість
                            });
                        }

                        await переміщенняТоварів_Objest.Товари_TablePart.Save(true);

                        NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{переміщенняТоварів_Objest.Назва}", () =>
                        {
                            ПереміщенняТоварів_Елемент page = new ПереміщенняТоварів_Елемент
                            {
                                IsNew = false,
                                ПереміщенняТоварів_Objest = переміщенняТоварів_Objest
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