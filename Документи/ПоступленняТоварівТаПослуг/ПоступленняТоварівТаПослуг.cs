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
        public ПоступленняТоварівТаПослуг()
        {
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            //Назва
            ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(ПоступленняТоварівТаПослуг_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.ПоступленняТоварівТаПослуг_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ПоступленняТоварівТаПослуг_Елемент page = new ПоступленняТоварівТаПослуг_Елемент
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

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await LoadRecords();
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

            {
                MenuItem doc = new MenuItem($"{РозхіднийКасовийОрдер_Const.FULLNAME}");
                doc.Activated += НаОснові_РозхіднийКасовийОрдер;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem($"{ПоверненняТоварівПостачальнику_Const.FULLNAME}");
                doc.Activated += НаОснові_ПоверненняТоварівПостачальнику;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem($"{РозміщенняТоварівНаСкладі_Const.FULLNAME}");
                doc.Activated += НаОснові_РозміщенняТоварівНаСкладі;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem($"{ВнутрішнєСпоживанняТоварів_Const.FULLNAME}");
                doc.Activated += НаОснові_ВнутрішнєСпоживанняТоварів;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem($"{ПереміщенняТоварів_Const.FULLNAME}");
                doc.Activated += НаОснові_ПереміщенняТоварів;
                Menu.Append(doc);
            }

            Menu.ShowAll();
            return Menu;
        }

        async void НаОснові_РозхіднийКасовийОрдер(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
                foreach (TreePath itemPath in TreeViewGrid.Selection.GetSelectedRows())
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ПоступленняТоварівТаПослуг_Objest? Обєкт = await new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid)).GetDocumentObject(false);
                    if (Обєкт == null) continue;

                    //
                    //Новий документ
                    //

                    РозхіднийКасовийОрдер_Objest Новий = new РозхіднийКасовийОрдер_Objest();
                    await Новий.New();
                    Новий.Організація = Обєкт.Організація;
                    Новий.Валюта = Обєкт.Валюта;
                    Новий.Каса = Обєкт.Каса;
                    Новий.Контрагент = Обєкт.Контрагент;
                    Новий.Договір = Обєкт.Договір;
                    Новий.СумаДокументу = Обєкт.СумаДокументу;
                    Новий.Основа = Обєкт.GetBasis();
                    Новий.ГосподарськаОперація = ГосподарськіОперації.ОплатаПостачальнику;

                    if (await Новий.Save())
                    {
                        NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{Новий.Назва}", () =>
                        {
                            РозхіднийКасовийОрдер_Елемент page = new РозхіднийКасовийОрдер_Елемент
                            {
                                IsNew = false,
                                Елемент = Новий,
                            };

                            page.SetValue();
                            return page;
                        });
                    }
                }
        }

        async void НаОснові_ПоверненняТоварівПостачальнику(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
                foreach (TreePath itemPath in TreeViewGrid.Selection.GetSelectedRows())
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ПоступленняТоварівТаПослуг_Objest? Обєкт = await new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid)).GetDocumentObject(true);
                    if (Обєкт == null) continue;

                    //
                    //Новий документ
                    //

                    ПоверненняТоварівПостачальнику_Objest Новий = new ПоверненняТоварівПостачальнику_Objest();
                    await Новий.New();
                    Новий.Організація = Обєкт.Організація;
                    Новий.Валюта = Обєкт.Валюта;
                    Новий.Каса = Обєкт.Каса;
                    Новий.Контрагент = Обєкт.Контрагент;
                    Новий.Договір = Обєкт.Договір;
                    Новий.Склад = Обєкт.Склад;
                    Новий.СумаДокументу = Обєкт.СумаДокументу;
                    Новий.Основа = Обєкт.GetBasis();

                    if (await Новий.Save())
                    {
                        //Товари
                        foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
                        {
                            Новий.Товари_TablePart.Records.Add(new ПоверненняТоварівПостачальнику_Товари_TablePart.Record()
                            {
                                Номенклатура = record.Номенклатура,
                                ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                                Серія = record.Серія,
                                Пакування = record.Пакування,
                                КількістьУпаковок = record.КількістьУпаковок,
                                Кількість = record.Кількість,
                                Ціна = record.Ціна,
                                Сума = record.Сума,
                                ДокументПоступлення = Обєкт.GetDocumentPointer()
                            });
                        }

                        await Новий.Товари_TablePart.Save(false);

                        NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{Новий.Назва}", () =>
                        {
                            ПоверненняТоварівПостачальнику_Елемент page = new ПоверненняТоварівПостачальнику_Елемент
                            {
                                IsNew = false,
                                Елемент = Новий
                            };

                            page.SetValue();
                            return page;
                        });
                    }
                }
        }

        async void НаОснові_РозміщенняТоварівНаСкладі(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
                foreach (TreePath itemPath in TreeViewGrid.Selection.GetSelectedRows())
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ПоступленняТоварівТаПослуг_Objest? Обєкт = await new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid)).GetDocumentObject(true);
                    if (Обєкт == null) continue;

                    //
                    //Новий документ
                    //

                    РозміщенняТоварівНаСкладі_Objest Новий = new РозміщенняТоварівНаСкладі_Objest();
                    await Новий.New();
                    Новий.Організація = Обєкт.Організація;
                    Новий.Склад = Обєкт.Склад;
                    Новий.Автор = Обєкт.Автор;
                    Новий.Підрозділ = Обєкт.Підрозділ;
                    Новий.Основа = Обєкт.GetBasis();
                    Новий.ДокументПоступлення = Обєкт.GetDocumentPointer();

                    if (await Новий.Save())
                    {
                        //Товари
                        foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
                        {
                            Новий.Товари_TablePart.Records.Add(new РозміщенняТоварівНаСкладі_Товари_TablePart.Record()
                            {
                                Номенклатура = record.Номенклатура,
                                ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                                Серія = record.Серія,
                                Пакування = record.Пакування,
                                КількістьУпаковок = record.КількістьУпаковок,
                                Кількість = record.Кількість
                            });
                        }

                        await Новий.Товари_TablePart.Save(false);

                        NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{Новий.Назва}", () =>
                        {
                            РозміщенняТоварівНаСкладі_Елемент page = new РозміщенняТоварівНаСкладі_Елемент
                            {
                                IsNew = false,
                                Елемент = Новий
                            };

                            page.SetValue();
                            return page;
                        });
                    }
                }
        }

        async void НаОснові_ВнутрішнєСпоживанняТоварів(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
                foreach (TreePath itemPath in TreeViewGrid.Selection.GetSelectedRows())
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ПоступленняТоварівТаПослуг_Objest? Обєкт = await new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid)).GetDocumentObject(true);

                    if (Обєкт == null)
                        continue;

                    //
                    //Новий документ
                    //

                    ВнутрішнєСпоживанняТоварів_Objest Новий = new ВнутрішнєСпоживанняТоварів_Objest();

                    await Новий.New();
                    Новий.Організація = Обєкт.Організація;
                    Новий.Склад = Обєкт.Склад;
                    Новий.Валюта = Обєкт.Валюта;
                    Новий.Автор = Обєкт.Автор;
                    Новий.Підрозділ = Обєкт.Підрозділ;
                    Новий.Основа = Обєкт.GetBasis();

                    if (await Новий.Save())
                    {
                        //Товари
                        foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
                        {
                            Новий.Товари_TablePart.Records.Add(new ВнутрішнєСпоживанняТоварів_Товари_TablePart.Record()
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

                        await Новий.Товари_TablePart.Save(false);

                        NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{Новий.Назва}", () =>
                        {
                            ВнутрішнєСпоживанняТоварів_Елемент page = new ВнутрішнєСпоживанняТоварів_Елемент()
                            {
                                IsNew = false,
                                Елемент = Новий
                            };

                            page.SetValue();
                            return page;
                        });
                    }
                }
        }

        async void НаОснові_ПереміщенняТоварів(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
                foreach (TreePath itemPath in TreeViewGrid.Selection.GetSelectedRows())
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    ПоступленняТоварівТаПослуг_Objest? Обєкт = await new ПоступленняТоварівТаПослуг_Pointer(new UnigueID(uid)).GetDocumentObject(true);
                    if (Обєкт == null) continue;

                    //
                    //Новий документ
                    //

                    ПереміщенняТоварів_Objest Новий = new ПереміщенняТоварів_Objest();
                    await Новий.New();
                    Новий.Організація = Обєкт.Організація;
                    Новий.СкладВідправник = Обєкт.Склад;
                    Новий.Автор = Обєкт.Автор;
                    Новий.Підрозділ = Обєкт.Підрозділ;
                    Новий.Основа = Обєкт.GetBasis();

                    if (await Новий.Save())
                    {
                        //Товари
                        foreach (ПоступленняТоварівТаПослуг_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
                        {
                            Новий.Товари_TablePart.Records.Add(new ПереміщенняТоварів_Товари_TablePart.Record()
                            {
                                Номенклатура = record.Номенклатура,
                                ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                                Серія = record.Серія,
                                Пакування = record.Пакування,
                                КількістьУпаковок = record.КількістьУпаковок,
                                Кількість = record.Кількість
                            });
                        }

                        await Новий.Товари_TablePart.Save(false);

                        NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{Новий.Назва}", () =>
                        {
                            ПереміщенняТоварів_Елемент page = new ПереміщенняТоварів_Елемент
                            {
                                IsNew = false,
                                Елемент = Новий
                            };

                            page.SetValue();
                            return page;
                        });
                    }
                }
        }

        #endregion
    }
}