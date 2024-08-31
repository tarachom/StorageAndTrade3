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
    public class РеалізаціяТоварівТаПослуг : ДокументЖурнал
    {
        public РеалізаціяТоварівТаПослуг() : base()
        {
            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async void LoadRecords()
        {
            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Назва
            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(РеалізаціяТоварівТаПослуг_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask<(string Name, Func<Widget>? FuncWidget, System.Action? SetValue)> OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            РеалізаціяТоварівТаПослуг_Елемент page = new РеалізаціяТоварівТаПослуг_Елемент
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
            РеалізаціяТоварівТаПослуг_Objest РеалізаціяТоварівТаПослуг_Objest = new РеалізаціяТоварівТаПослуг_Objest();
            if (await РеалізаціяТоварівТаПослуг_Objest.Read(unigueID))
                await РеалізаціяТоварівТаПослуг_Objest.SetDeletionLabel(!РеалізаціяТоварівТаПослуг_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            РеалізаціяТоварівТаПослуг_Objest РеалізаціяТоварівТаПослуг_Objest = new РеалізаціяТоварівТаПослуг_Objest();
            if (await РеалізаціяТоварівТаПослуг_Objest.Read(unigueID))
            {
                РеалізаціяТоварівТаПослуг_Objest РеалізаціяТоварівТаПослуг_Objest_Новий = await РеалізаціяТоварівТаПослуг_Objest.Copy(true);
                await РеалізаціяТоварівТаПослуг_Objest_Новий.Save();
                await РеалізаціяТоварівТаПослуг_Objest_Новий.Товари_TablePart.Save(true);

                return РеалізаціяТоварівТаПослуг_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "Документи.РеалізаціяТоварівТаПослуг";

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
            РеалізаціяТоварівТаПослуг_Pointer РеалізаціяТоварівТаПослуг_Pointer = new РеалізаціяТоварівТаПослуг_Pointer(unigueID);
            РеалізаціяТоварівТаПослуг_Objest? РеалізаціяТоварівТаПослуг_Objest = await РеалізаціяТоварівТаПослуг_Pointer.GetDocumentObject(true);
            if (РеалізаціяТоварівТаПослуг_Objest == null) return;

            if (spendDoc)
            {
                if (!await РеалізаціяТоварівТаПослуг_Objest.SpendTheDocument(РеалізаціяТоварівТаПослуг_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(РеалізаціяТоварівТаПослуг_Objest.UnigueID);
            }
            else
                await РеалізаціяТоварівТаПослуг_Objest.ClearSpendTheDocument();
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new РеалізаціяТоварівТаПослуг_Pointer(unigueID));
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{РеалізаціяТоварівТаПослуг_Const.FULLNAME}_{unigueID}.xml");
            await РеалізаціяТоварівТаПослуг_Export.ToXmlFile(new РеалізаціяТоварівТаПослуг_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar

        protected override Menu? ToolbarNaOsnoviSubMenu()
        {
            Menu Menu = new Menu();

            {
                MenuItem doc = new MenuItem("Прихідний касовий ордер");
                doc.Activated += OnNewDocNaOsnovi_ПрихіднийКасовийОрдер;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem("Повернення товарів від клієнта");
                doc.Activated += OnNewDocNaOsnovi_ПоверненняТоварівВідКлієнта;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem("Збірка товарів на складі");
                doc.Activated += OnNewDocNaOsnovi_ЗбіркаТоварівНаСкладі;
                Menu.Append(doc);
            }

            Menu.ShowAll();

            return Menu;
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

                    РеалізаціяТоварівТаПослуг_Pointer реалізаціяТоварівТаПослуг_Pointer = new РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid));
                    РеалізаціяТоварівТаПослуг_Objest? реалізаціяТоварівТаПослуг_Objest = await реалізаціяТоварівТаПослуг_Pointer.GetDocumentObject(false);
                    if (реалізаціяТоварівТаПослуг_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    ПрихіднийКасовийОрдер_Objest прихіднийКасовийОрдер_Новий = new ПрихіднийКасовийОрдер_Objest();
                    await прихіднийКасовийОрдер_Новий.New();
                    прихіднийКасовийОрдер_Новий.ГосподарськаОперація = ГосподарськіОперації.ПоступленняОплатиВідКлієнта;
                    прихіднийКасовийОрдер_Новий.Організація = реалізаціяТоварівТаПослуг_Objest.Організація;
                    прихіднийКасовийОрдер_Новий.Валюта = реалізаціяТоварівТаПослуг_Objest.Валюта;
                    прихіднийКасовийОрдер_Новий.Каса = реалізаціяТоварівТаПослуг_Objest.Каса;
                    прихіднийКасовийОрдер_Новий.Контрагент = реалізаціяТоварівТаПослуг_Objest.Контрагент;
                    прихіднийКасовийОрдер_Новий.Договір = реалізаціяТоварівТаПослуг_Objest.Договір;
                    прихіднийКасовийОрдер_Новий.СумаДокументу = реалізаціяТоварівТаПослуг_Objest.СумаДокументу;
                    прихіднийКасовийОрдер_Новий.Основа = реалізаціяТоварівТаПослуг_Objest.GetBasis();

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

        async void OnNewDocNaOsnovi_ПоверненняТоварівВідКлієнта(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    РеалізаціяТоварівТаПослуг_Pointer реалізаціяТоварівТаПослуг_Pointer = new РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid));
                    РеалізаціяТоварівТаПослуг_Objest? реалізаціяТоварівТаПослуг_Objest = await реалізаціяТоварівТаПослуг_Pointer.GetDocumentObject(true);
                    if (реалізаціяТоварівТаПослуг_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    ПоверненняТоварівВідКлієнта_Objest поверненняТоварівВідКлієнта_Objest = new ПоверненняТоварівВідКлієнта_Objest();
                    await поверненняТоварівВідКлієнта_Objest.New();
                    поверненняТоварівВідКлієнта_Objest.Організація = реалізаціяТоварівТаПослуг_Objest.Організація;
                    поверненняТоварівВідКлієнта_Objest.Валюта = реалізаціяТоварівТаПослуг_Objest.Валюта;
                    поверненняТоварівВідКлієнта_Objest.Каса = реалізаціяТоварівТаПослуг_Objest.Каса;
                    поверненняТоварівВідКлієнта_Objest.Контрагент = реалізаціяТоварівТаПослуг_Objest.Контрагент;
                    поверненняТоварівВідКлієнта_Objest.Договір = реалізаціяТоварівТаПослуг_Objest.Договір;
                    поверненняТоварівВідКлієнта_Objest.Склад = реалізаціяТоварівТаПослуг_Objest.Склад;
                    поверненняТоварівВідКлієнта_Objest.СумаДокументу = реалізаціяТоварівТаПослуг_Objest.СумаДокументу;
                    поверненняТоварівВідКлієнта_Objest.Основа = реалізаціяТоварівТаПослуг_Objest.GetBasis();

                    if (await поверненняТоварівВідКлієнта_Objest.Save())
                    {
                        //Товари
                        foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record record_реалізаціяТоварівТаПослуг in реалізаціяТоварівТаПослуг_Objest.Товари_TablePart.Records)
                        {
                            ПоверненняТоварівВідКлієнта_Товари_TablePart.Record record_повернення = new ПоверненняТоварівВідКлієнта_Товари_TablePart.Record();
                            поверненняТоварівВідКлієнта_Objest.Товари_TablePart.Records.Add(record_повернення);

                            record_повернення.Номенклатура = record_реалізаціяТоварівТаПослуг.Номенклатура;
                            record_повернення.ХарактеристикаНоменклатури = record_реалізаціяТоварівТаПослуг.ХарактеристикаНоменклатури;
                            record_повернення.Серія = record_реалізаціяТоварівТаПослуг.Серія;
                            record_повернення.Пакування = record_реалізаціяТоварівТаПослуг.Пакування;
                            record_повернення.КількістьУпаковок = record_реалізаціяТоварівТаПослуг.КількістьУпаковок;
                            record_повернення.Кількість = record_реалізаціяТоварівТаПослуг.Кількість;
                            record_повернення.Ціна = record_реалізаціяТоварівТаПослуг.Ціна;
                            record_повернення.Сума = record_реалізаціяТоварівТаПослуг.Сума;
                            record_повернення.ДокументРеалізації = реалізаціяТоварівТаПослуг_Objest.GetDocumentPointer();
                        }

                        await поверненняТоварівВідКлієнта_Objest.Товари_TablePart.Save(false);

                        NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{поверненняТоварівВідКлієнта_Objest.Назва}", () =>
                        {
                            ПоверненняТоварівВідКлієнта_Елемент page = new ПоверненняТоварівВідКлієнта_Елемент
                            {
                                IsNew = false,
                                Елемент = поверненняТоварівВідКлієнта_Objest
                            };

                            page.SetValue();

                            return page;
                        });
                    }
                }
            }
        }

        async void OnNewDocNaOsnovi_ЗбіркаТоварівНаСкладі(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    РеалізаціяТоварівТаПослуг_Pointer реалізаціяТоварівТаПослуг_Pointer = new РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid));
                    РеалізаціяТоварівТаПослуг_Objest? реалізаціяТоварівТаПослуг_Objest = await реалізаціяТоварівТаПослуг_Pointer.GetDocumentObject(true);
                    if (реалізаціяТоварівТаПослуг_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    ЗбіркаТоварівНаСкладі_Objest збіркаТоварівНаСкладі_Objest = new ЗбіркаТоварівНаСкладі_Objest();
                    await збіркаТоварівНаСкладі_Objest.New();
                    збіркаТоварівНаСкладі_Objest.Організація = реалізаціяТоварівТаПослуг_Objest.Організація;
                    збіркаТоварівНаСкладі_Objest.Склад = реалізаціяТоварівТаПослуг_Objest.Склад;
                    збіркаТоварівНаСкладі_Objest.Основа = реалізаціяТоварівТаПослуг_Objest.GetBasis();
                    збіркаТоварівНаСкладі_Objest.ДокументРеалізації = реалізаціяТоварівТаПослуг_Pointer;

                    if (await збіркаТоварівНаСкладі_Objest.Save())
                    {
                        //Товари
                        foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record record_реалізаціяТоварівТаПослуг in реалізаціяТоварівТаПослуг_Objest.Товари_TablePart.Records)
                        {
                            ЗбіркаТоварівНаСкладі_Товари_TablePart.Record record = new ЗбіркаТоварівНаСкладі_Товари_TablePart.Record();
                            збіркаТоварівНаСкладі_Objest.Товари_TablePart.Records.Add(record);

                            record.Номенклатура = record_реалізаціяТоварівТаПослуг.Номенклатура;
                            record.ХарактеристикаНоменклатури = record_реалізаціяТоварівТаПослуг.ХарактеристикаНоменклатури;
                            record.Серія = record_реалізаціяТоварівТаПослуг.Серія;
                            record.Пакування = record_реалізаціяТоварівТаПослуг.Пакування;
                            record.КількістьУпаковок = record_реалізаціяТоварівТаПослуг.КількістьУпаковок;
                            record.Кількість = record_реалізаціяТоварівТаПослуг.Кількість;
                        }

                        await збіркаТоварівНаСкладі_Objest.Товари_TablePart.Save(false);

                        NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{збіркаТоварівНаСкладі_Objest.Назва}", () =>
                        {
                            ЗбіркаТоварівНаСкладі_Елемент page = new ЗбіркаТоварівНаСкладі_Елемент
                            {
                                IsNew = false,
                                Елемент = збіркаТоварівНаСкладі_Objest
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