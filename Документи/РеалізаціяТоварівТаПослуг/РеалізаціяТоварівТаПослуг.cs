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
        public РеалізаціяТоварівТаПослуг()
        {
            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.ДодатиВідбір(TreeViewGrid, РеалізаціяТоварівТаПослуг_Функції.Відбори(searchText));

            await ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.LoadRecords(TreeViewGrid);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.РеалізаціяТоварівТаПослуг_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await РеалізаціяТоварівТаПослуг_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await РеалізаціяТоварівТаПослуг_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await РеалізаціяТоварівТаПослуг_Функції.Copy(unigueID);
        }

        const string КлючНалаштуванняКористувача = "Документи.РеалізаціяТоварівТаПослуг";

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
            РеалізаціяТоварівТаПослуг_Objest? Обєкт = await new РеалізаціяТоварівТаПослуг_Pointer(unigueID).GetDocumentObject(true);
            if (Обєкт == null) return;

            if (spendDoc)
            {
                if (!await Обєкт.SpendTheDocument(Обєкт.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(Обєкт.UnigueID);
            }
            else
                await Обєкт.ClearSpendTheDocument();
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new РеалізаціяТоварівТаПослуг_Pointer(unigueID));
        }

        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            РеалізаціяТоварівТаПослуг_Pointer Вказівник = new РеалізаціяТоварівТаПослуг_Pointer(unigueID);
            await Вказівник.GetPresentation();

            await РеалізаціяТоварівТаПослуг_Export.ToXmlFile(Вказівник, System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml"));
        }

        protected override async ValueTask PrintingDoc(UnigueID unigueID)
        {
            await РеалізаціяТоварівТаПослуг_Друк.PDF(unigueID);
        }

        #endregion

        #region ToolBar

        protected override Menu? ToolbarNaOsnoviSubMenu()
        {
            Menu Menu = new Menu();

            {
                MenuItem doc = new MenuItem(ПрихіднийКасовийОрдер_Const.FULLNAME);
                doc.Activated += НаОснові_ПрихіднийКасовийОрдер;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem(ПоверненняТоварівВідКлієнта_Const.FULLNAME);
                doc.Activated += НаОснові_ПоверненняТоварівВідКлієнта;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem(ЗбіркаТоварівНаСкладі_Const.FULLNAME);
                doc.Activated += НаОснові_ЗбіркаТоварівНаСкладі;
                Menu.Append(doc);
            }

            Menu.ShowAll();

            return Menu;
        }

        async void НаОснові_ПрихіднийКасовийОрдер(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
                foreach (TreePath itemPath in TreeViewGrid.Selection.GetSelectedRows())
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    РеалізаціяТоварівТаПослуг_Objest? Обєкт = await new РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid)).GetDocumentObject(false);
                    if (Обєкт == null) continue;

                    //
                    //Новий документ
                    //

                    ПрихіднийКасовийОрдер_Objest Новий = new ПрихіднийКасовийОрдер_Objest();
                    await Новий.New();
                    Новий.ГосподарськаОперація = ГосподарськіОперації.ПоступленняОплатиВідКлієнта;
                    Новий.Організація = Обєкт.Організація;
                    Новий.Валюта = Обєкт.Валюта;
                    Новий.Каса = Обєкт.Каса;
                    Новий.Контрагент = Обєкт.Контрагент;
                    Новий.Договір = Обєкт.Договір;
                    Новий.СумаДокументу = Обєкт.СумаДокументу;
                    Новий.Основа = Обєкт.GetBasis();

                    await Новий.Save();

                    ПрихіднийКасовийОрдер_Елемент page = new ПрихіднийКасовийОрдер_Елемент();
                    await page.Елемент.Read(Новий.UnigueID);
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
                    page.SetValue();
                }
        }

        async void НаОснові_ПоверненняТоварівВідКлієнта(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
                foreach (TreePath itemPath in TreeViewGrid.Selection.GetSelectedRows())
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    РеалізаціяТоварівТаПослуг_Objest? Обєкт = await new РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid)).GetDocumentObject(true);
                    if (Обєкт == null) continue;

                    //
                    //Новий документ
                    //

                    ПоверненняТоварівВідКлієнта_Objest Новий = new ПоверненняТоварівВідКлієнта_Objest();
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
                        foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
                        {
                            Новий.Товари_TablePart.Records.Add(new ПоверненняТоварівВідКлієнта_Товари_TablePart.Record()
                            {
                                Номенклатура = record.Номенклатура,
                                ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                                Серія = record.Серія,
                                Пакування = record.Пакування,
                                КількістьУпаковок = record.КількістьУпаковок,
                                Кількість = record.Кількість,
                                Ціна = record.Ціна,
                                Сума = record.Сума,
                                ДокументРеалізації = Обєкт.GetDocumentPointer()
                            });
                        }

                        await Новий.Товари_TablePart.Save(false);
                    }

                    ПоверненняТоварівВідКлієнта_Елемент page = new ПоверненняТоварівВідКлієнта_Елемент();
                    await page.Елемент.Read(Новий.UnigueID);
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
                    page.SetValue();
                }
        }

        async void НаОснові_ЗбіркаТоварівНаСкладі(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
                foreach (TreePath itemPath in TreeViewGrid.Selection.GetSelectedRows())
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    РеалізаціяТоварівТаПослуг_Objest? Обєкт = await new РеалізаціяТоварівТаПослуг_Pointer(new UnigueID(uid)).GetDocumentObject(true);
                    if (Обєкт == null) continue;

                    //
                    //Новий документ
                    //

                    ЗбіркаТоварівНаСкладі_Objest Новий = new ЗбіркаТоварівНаСкладі_Objest();
                    await Новий.New();
                    Новий.Організація = Обєкт.Організація;
                    Новий.Склад = Обєкт.Склад;
                    Новий.Основа = Обєкт.GetBasis();
                    Новий.ДокументРеалізації = Обєкт.GetDocumentPointer();

                    if (await Новий.Save())
                    {
                        //Товари
                        foreach (РеалізаціяТоварівТаПослуг_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
                        {
                            Новий.Товари_TablePart.Records.Add(new ЗбіркаТоварівНаСкладі_Товари_TablePart.Record()
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
                    }

                    ЗбіркаТоварівНаСкладі_Елемент page = new ЗбіркаТоварівНаСкладі_Елемент();
                    await page.Елемент.Read(Новий.UnigueID);
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
                    page.SetValue();
                }
        }

        #endregion
    }
}