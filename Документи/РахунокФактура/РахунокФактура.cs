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
        public РахунокФактура() : base()
        {
            ТабличніСписки.РахунокФактура_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async void LoadRecords()
        {
            ТабличніСписки.РахунокФактура_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.РахунокФактура_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.РахунокФактура_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.РахунокФактура_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.РахунокФактура_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РахунокФактура_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.РахунокФактура_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РахунокФактура_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Назва
            ТабличніСписки.РахунокФактура_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(РахунокФактура_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.РахунокФактура_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.РахунокФактура_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.РахунокФактура_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async ValueTask<(string Name, Func<Widget>? FuncWidget, System.Action? SetValue)> OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            РахунокФактура_Елемент page = new РахунокФактура_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (IsNew)
                await page.РахунокФактура_Objest.New();
            else if (unigueID == null || !await page.РахунокФактура_Objest.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return ("", null, null);
            }

            return (page.Caption, () => page, page.SetValue);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            РахунокФактура_Objest РахунокФактура_Objest = new РахунокФактура_Objest();
            if (await РахунокФактура_Objest.Read(unigueID))
                await РахунокФактура_Objest.SetDeletionLabel(!РахунокФактура_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            РахунокФактура_Objest РахунокФактура_Objest = new РахунокФактура_Objest();
            if (await РахунокФактура_Objest.Read(unigueID))
            {
                РахунокФактура_Objest РахунокФактура_Objest_Новий = await РахунокФактура_Objest.Copy(true);
                await РахунокФактура_Objest_Новий.Save();
                await РахунокФактура_Objest_Новий.Товари_TablePart.Save(true);

                return РахунокФактура_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "Документи.РахунокФактура";

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
            РахунокФактура_Pointer РахунокФактура_Pointer = new РахунокФактура_Pointer(unigueID);
            РахунокФактура_Objest? РахунокФактура_Objest = await РахунокФактура_Pointer.GetDocumentObject(true);
            if (РахунокФактура_Objest == null) return;

            if (spendDoc)
            {
                if (!await РахунокФактура_Objest.SpendTheDocument(РахунокФактура_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(РахунокФактура_Objest.UnigueID);
            }
            else
                await РахунокФактура_Objest.ClearSpendTheDocument();
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new РахунокФактура_Pointer(unigueID));
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{РахунокФактура_Const.FULLNAME}_{unigueID}.xml");
            await РахунокФактура_Export.ToXmlFile(new РахунокФактура_Pointer(unigueID), pathToSave);
        }

        #endregion

        #region ToolBar

        protected override Menu? ToolbarNaOsnoviSubMenu()
        {
            Menu Menu = new Menu();

            MenuItem newDocRoshidnaNakladnaButton = new MenuItem("Реалізація товарів та послуг");
            newDocRoshidnaNakladnaButton.Activated += OnNewDocNaOsnovi_RoshidnaNakladna;
            Menu.Append(newDocRoshidnaNakladnaButton);

            MenuItem newDocSamovlenjaPostachalnykuButton = new MenuItem("Замовлення постачальнику");
            newDocSamovlenjaPostachalnykuButton.Activated += OnNewDocNaOsnovi_SamovlenjaPostachalnyku;
            Menu.Append(newDocSamovlenjaPostachalnykuButton);

            Menu.ShowAll();

            return Menu;
        }

        async void OnNewDocNaOsnovi_RoshidnaNakladna(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    РахунокФактура_Pointer РахунокФактура_Pointer = new РахунокФактура_Pointer(new UnigueID(uid));
                    РахунокФактура_Objest? рахунокФактура_Objest = await РахунокФактура_Pointer.GetDocumentObject(true);
                    if (рахунокФактура_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    РеалізаціяТоварівТаПослуг_Objest реалізаціяТоварівТаПослуг_Новий = new РеалізаціяТоварівТаПослуг_Objest();
                    await реалізаціяТоварівТаПослуг_Новий.New();
                    реалізаціяТоварівТаПослуг_Новий.Організація = рахунокФактура_Objest.Організація;
                    реалізаціяТоварівТаПослуг_Новий.Валюта = рахунокФактура_Objest.Валюта;
                    реалізаціяТоварівТаПослуг_Новий.Каса = рахунокФактура_Objest.Каса;
                    реалізаціяТоварівТаПослуг_Новий.Контрагент = рахунокФактура_Objest.Контрагент;
                    реалізаціяТоварівТаПослуг_Новий.Договір = рахунокФактура_Objest.Договір;
                    реалізаціяТоварівТаПослуг_Новий.Склад = рахунокФактура_Objest.Склад;
                    реалізаціяТоварівТаПослуг_Новий.СумаДокументу = рахунокФактура_Objest.СумаДокументу;
                    реалізаціяТоварівТаПослуг_Новий.Статус = СтатусиРеалізаціїТоварівТаПослуг.ДоОплати;
                    реалізаціяТоварівТаПослуг_Новий.ФормаОплати = рахунокФактура_Objest.ФормаОплати;
                    реалізаціяТоварівТаПослуг_Новий.Основа = рахунокФактура_Objest.GetBasis();
                    await реалізаціяТоварівТаПослуг_Новий.Save();

                    //Товари
                    foreach (РахунокФактура_Товари_TablePart.Record record_замовлення in рахунокФактура_Objest.Товари_TablePart.Records)
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
                        record_реалізація.РахунокФактура = рахунокФактура_Objest.GetDocumentPointer();
                        record_реалізація.Склад = рахунокФактура_Objest.Склад;
                        record_реалізація.ВидЦіни = record_замовлення.ВидЦіни;
                    }

                    await реалізаціяТоварівТаПослуг_Новий.Товари_TablePart.Save(false);

                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{реалізаціяТоварівТаПослуг_Новий.Назва}", () =>
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

        async void OnNewDocNaOsnovi_SamovlenjaPostachalnyku(object? sender, EventArgs args)
        {
            if (TreeViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreePath[] selectionRows = TreeViewGrid.Selection.GetSelectedRows();

                foreach (TreePath itemPath in selectionRows)
                {
                    TreeViewGrid.Model.GetIter(out TreeIter iter, itemPath);

                    string uid = (string)TreeViewGrid.Model.GetValue(iter, 1);

                    РахунокФактура_Pointer рахунокФактура_Pointer = new РахунокФактура_Pointer(new UnigueID(uid));
                    РахунокФактура_Objest? рахунокФактура_Objest = await рахунокФактура_Pointer.GetDocumentObject(true);
                    if (рахунокФактура_Objest == null) continue;

                    //
                    //Новий документ
                    //

                    ЗамовленняПостачальнику_Objest замовленняПостачальнику_Новий = new ЗамовленняПостачальнику_Objest();
                    await замовленняПостачальнику_Новий.New();
                    замовленняПостачальнику_Новий.Організація = рахунокФактура_Objest.Організація;
                    замовленняПостачальнику_Новий.Валюта = рахунокФактура_Objest.Валюта;
                    замовленняПостачальнику_Новий.Каса = рахунокФактура_Objest.Каса;
                    замовленняПостачальнику_Новий.Контрагент = рахунокФактура_Objest.Контрагент;
                    замовленняПостачальнику_Новий.Договір = await ФункціїДляДокументів.ОсновнийДоговірДляКонтрагента(рахунокФактура_Objest.Контрагент, ТипДоговорів.ЗПостачальниками) ?? рахунокФактура_Objest.Договір;
                    замовленняПостачальнику_Новий.Склад = рахунокФактура_Objest.Склад;
                    замовленняПостачальнику_Новий.СумаДокументу = рахунокФактура_Objest.СумаДокументу;
                    замовленняПостачальнику_Новий.Статус = СтатусиЗамовленьПостачальникам.Підтверджений;
                    замовленняПостачальнику_Новий.ФормаОплати = рахунокФактура_Objest.ФормаОплати;
                    замовленняПостачальнику_Новий.Основа = рахунокФактура_Objest.GetBasis();

                    if (await замовленняПостачальнику_Новий.Save())
                    {
                        //Товари
                        foreach (РахунокФактура_Товари_TablePart.Record record_замовлення in рахунокФактура_Objest.Товари_TablePart.Records)
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
                            record_замовленняПостачальнику.Склад = рахунокФактура_Objest.Склад;
                        }

                        await замовленняПостачальнику_Новий.Товари_TablePart.Save(false);

                        NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{замовленняПостачальнику_Новий.Назва}", () =>
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

        #endregion
    }
}