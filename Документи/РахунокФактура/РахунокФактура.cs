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
    public class РахунокФактура : ДокументЖурнал
    {
        public РахунокФактура()
        {
            ТабличніСписки.РахунокФактура_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.РахунокФактура_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РахунокФактура_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.РахунокФактура_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.РахунокФактура_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.РахунокФактура_Записи.ОчиститиВідбір(TreeViewGrid);

            //Відбори
            ТабличніСписки.РахунокФактура_Записи.ДодатиВідбір(TreeViewGrid, РахунокФактура_Функції.Відбори(searchText));

            await ТабличніСписки.РахунокФактура_Записи.LoadRecords(TreeViewGrid);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.РахунокФактура_Записи.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await РахунокФактура_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await РахунокФактура_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            return await РахунокФактура_Функції.Copy(unigueID);
        }

        const string КлючНалаштуванняКористувача = "Документи.РахунокФактура";

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
            РахунокФактура_Objest? Обєкт = await new РахунокФактура_Pointer(unigueID).GetDocumentObject(true);
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
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new РахунокФактура_Pointer(unigueID));
        }

        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            РахунокФактура_Pointer Вказівник = new РахунокФактура_Pointer(unigueID);
            await Вказівник.GetPresentation();

            await РахунокФактура_Export.ToXmlFile(Вказівник, System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml"));
        }

        protected override async ValueTask PrintingDoc(UnigueID unigueID)
        {
            await РахунокФактура_Друк.PDF(unigueID);
        }

        #endregion

        #region ToolBar

        protected override Menu? ToolbarNaOsnoviSubMenu()
        {
            Menu Menu = new Menu();

            {
                MenuItem doc = new MenuItem(РеалізаціяТоварівТаПослуг_Const.FULLNAME);
                doc.Activated += НаОснові_РеалізаціяТоварівТаПослуг;
                Menu.Append(doc);
            }

            {
                MenuItem doc = new MenuItem(ЗамовленняПостачальнику_Const.FULLNAME);
                doc.Activated += НаОснові_ЗамовленняПостачальнику;
                Menu.Append(doc);
            }

            Menu.ShowAll();

            return Menu;
        }

        async void НаОснові_РеалізаціяТоварівТаПослуг(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
                foreach (TreePath itemPath in TreeViewGrid.Selection.GetSelectedRows())
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    РахунокФактура_Objest? Обєкт = await new РахунокФактура_Pointer(new UnigueID(uid)).GetDocumentObject(true);
                    if (Обєкт == null) continue;

                    //
                    //Новий документ
                    //

                    РеалізаціяТоварівТаПослуг_Objest Новий = new РеалізаціяТоварівТаПослуг_Objest();
                    await Новий.New();
                    Новий.Організація = Обєкт.Організація;
                    Новий.Валюта = Обєкт.Валюта;
                    Новий.Каса = Обєкт.Каса;
                    Новий.Контрагент = Обєкт.Контрагент;
                    Новий.Договір = Обєкт.Договір;
                    Новий.Склад = Обєкт.Склад;
                    Новий.СумаДокументу = Обєкт.СумаДокументу;
                    Новий.Статус = СтатусиРеалізаціїТоварівТаПослуг.ДоОплати;
                    Новий.ФормаОплати = Обєкт.ФормаОплати;
                    Новий.Основа = Обєкт.GetBasis();

                    if (await Новий.Save())
                    {
                        //Товари
                        foreach (РахунокФактура_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
                        {
                            Новий.Товари_TablePart.Records.Add(new РеалізаціяТоварівТаПослуг_Товари_TablePart.Record()
                            {
                                Номенклатура = record.Номенклатура,
                                ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                                Пакування = record.Пакування,
                                КількістьУпаковок = record.КількістьУпаковок,
                                Кількість = record.Кількість,
                                Ціна = record.Ціна,
                                Сума = record.Сума,
                                Скидка = record.Скидка,
                                РахунокФактура = Обєкт.GetDocumentPointer(),
                                Склад = Обєкт.Склад,
                                ВидЦіни = record.ВидЦіни
                            });
                        }

                        await Новий.Товари_TablePart.Save(false);
                    }

                    РеалізаціяТоварівТаПослуг_Елемент page = new РеалізаціяТоварівТаПослуг_Елемент();
                    await page.Елемент.Read(Новий.UnigueID);
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
                    page.SetValue();
                }
        }

        async void НаОснові_ЗамовленняПостачальнику(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
                foreach (TreePath itemPath in TreeViewGrid.Selection.GetSelectedRows())
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);
                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    РахунокФактура_Objest? Обєкт = await new РахунокФактура_Pointer(new UnigueID(uid)).GetDocumentObject(true);
                    if (Обєкт == null) continue;

                    //
                    //Новий документ
                    //

                    ЗамовленняПостачальнику_Objest Новий = new ЗамовленняПостачальнику_Objest();
                    await Новий.New();
                    Новий.Організація = Обєкт.Організація;
                    Новий.Валюта = Обєкт.Валюта;
                    Новий.Каса = Обєкт.Каса;
                    Новий.Контрагент = Обєкт.Контрагент;
                    Новий.Договір = await ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(Обєкт.Контрагент, ТипДоговорів.ЗПостачальниками) ?? Обєкт.Договір;
                    Новий.Склад = Обєкт.Склад;
                    Новий.СумаДокументу = Обєкт.СумаДокументу;
                    Новий.Статус = СтатусиЗамовленьПостачальникам.Підтверджений;
                    Новий.ФормаОплати = Обєкт.ФормаОплати;
                    Новий.Основа = Обєкт.GetBasis();

                    if (await Новий.Save())
                    {
                        //Товари
                        foreach (РахунокФактура_Товари_TablePart.Record record in Обєкт.Товари_TablePart.Records)
                        {
                            Новий.Товари_TablePart.Records.Add(new ЗамовленняПостачальнику_Товари_TablePart.Record()
                            {
                                Номенклатура = record.Номенклатура,
                                ХарактеристикаНоменклатури = record.ХарактеристикаНоменклатури,
                                Пакування = record.Пакування,
                                КількістьУпаковок = record.КількістьУпаковок,
                                Кількість = record.Кількість,
                                Ціна = record.Ціна,
                                Сума = record.Сума,
                                Скидка = record.Скидка,
                                Склад = Обєкт.Склад
                            });
                        }

                        await Новий.Товари_TablePart.Save(false);
                    }

                    ЗамовленняПостачальнику_Елемент page = new ЗамовленняПостачальнику_Елемент();
                    await page.Елемент.Read(Новий.UnigueID);
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
                    page.SetValue();
                }
        }
    }

    #endregion
}