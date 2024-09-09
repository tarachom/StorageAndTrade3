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

using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.РегістриВідомостей;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class ХарактеристикиНоменклатури : ДовідникЖурнал
    {
        public Номенклатура_PointerControl НоменклатураВласник = new Номенклатура_PointerControl();

        public ХарактеристикиНоменклатури()
        {
            //Власник
            HBoxTop.PackStart(НоменклатураВласник, false, false, 2);
            НоменклатураВласник.Caption = $"{Номенклатура_Const.FULLNAME}:";
            НоменклатураВласник.AfterSelectFunc = async () => await LoadRecords();

            CreateLink(HBoxTop, ШтрихкодиНоменклатури_Const.FULLNAME, async () =>
            {
                ШтрихкодиНоменклатури page = new ШтрихкодиНоменклатури();
                page.НоменклатураВласник.Pointer = НоменклатураВласник.Pointer;

                if (SelectPointerItem != null)
                    page.ХарактеристикиНоменклатуриВласник.Pointer = new ХарактеристикиНоменклатури_Pointer(SelectPointerItem);

                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{ШтрихкодиНоменклатури_Const.FULLNAME}", () => page);

                await page.SetValue();
            });

            ТабличніСписки.ХарактеристикиНоменклатури_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ХарактеристикиНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!НоменклатураВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ХарактеристикиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ХарактеристикиНоменклатури_Const.Номенклатура, Comparison.EQ, НоменклатураВласник.Pointer.UnigueID.UGuid));
            }

            await ТабличніСписки.ХарактеристикиНоменклатури_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!НоменклатураВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ХарактеристикиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ХарактеристикиНоменклатури_Const.Номенклатура, Comparison.EQ, НоменклатураВласник.Pointer.UnigueID.UGuid));
            }

            //Відбори
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid, ХарактеристикиНоменклатури_ВідбориДляПошуку.Відбори(searchText));

            await ТабличніСписки.ХарактеристикиНоменклатури_Записи.LoadRecords(TreeViewGrid);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.ХарактеристикиНоменклатури_Записи.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ХарактеристикиНоменклатури_Елемент page = new ХарактеристикиНоменклатури_Елемент
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
            ХарактеристикиНоменклатури_Objest ХарактеристикиНоменклатури_Objest = new ХарактеристикиНоменклатури_Objest();
            if (await ХарактеристикиНоменклатури_Objest.Read(unigueID))
                await ХарактеристикиНоменклатури_Objest.SetDeletionLabel(!ХарактеристикиНоменклатури_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ХарактеристикиНоменклатури_Objest ХарактеристикиНоменклатури_Objest = new ХарактеристикиНоменклатури_Objest();
            if (await ХарактеристикиНоменклатури_Objest.Read(unigueID))
            {
                ХарактеристикиНоменклатури_Objest ХарактеристикиНоменклатури_Objest_Новий = await ХарактеристикиНоменклатури_Objest.Copy(true);
                await ХарактеристикиНоменклатури_Objest_Новий.Save();

                return ХарактеристикиНоменклатури_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        #endregion
    }
}