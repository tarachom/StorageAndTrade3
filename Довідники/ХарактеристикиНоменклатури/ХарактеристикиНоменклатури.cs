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

using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.РегістриВідомостей;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class ХарактеристикиНоменклатури : ДовідникЖурнал
    {
        public Номенклатура_PointerControl НоменклатураВласник = new Номенклатура_PointerControl();

        public ХарактеристикиНоменклатури() : base()
        {
            //Власник
            HBoxTop.PackStart(НоменклатураВласник, false, false, 2);
            НоменклатураВласник.Caption = $"{Номенклатура_Const.FULLNAME}:";
            НоменклатураВласник.AfterSelectFunc = () =>
            {
                LoadRecords();
            };

            //ШтрихКоди
            {
                LinkButton linkButtonShKody = new LinkButton($" {ШтрихкодиНоменклатури_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkButtonShKody.Clicked += (object? sender, EventArgs args) =>
                {
                    ШтрихкодиНоменклатури page = new ШтрихкодиНоменклатури();

                    page.НоменклатураВласник.Pointer = НоменклатураВласник.Pointer;

                    if (SelectPointerItem != null)
                        page.ХарактеристикиНоменклатуриВласник.Pointer = new ХарактеристикиНоменклатури_Pointer(SelectPointerItem);

                    Program.GeneralForm?.CreateNotebookPage($"{ШтрихкодиНоменклатури_Const.FULLNAME}", () => { return page; });

                    page.LoadRecords();
                };

                HBoxTop.PackStart(linkButtonShKody, false, false, 10);
            }

            TreeViewGrid.Model = ТабличніСписки.ХарактеристикиНоменклатури_Записи.Store;
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.ХарактеристикиНоменклатури_Записи.Where.Clear();

            if (!НоменклатураВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.ХарактеристикиНоменклатури_Записи.Where.Add(
                    new Where(ХарактеристикиНоменклатури_Const.Номенклатура, Comparison.EQ, НоменклатураВласник.Pointer.UnigueID.UGuid));
            }

            ТабличніСписки.ХарактеристикиНоменклатури_Записи.LoadRecords();

            if (ТабличніСписки.ХарактеристикиНоменклатури_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ХарактеристикиНоменклатури_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ХарактеристикиНоменклатури_Записи.Where.Clear();

            //Назва
            ТабличніСписки.ХарактеристикиНоменклатури_Записи.Where.Add(
                new Where(ХарактеристикиНоменклатури_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.ХарактеристикиНоменклатури_Записи.LoadRecords();

            if (ТабличніСписки.ХарактеристикиНоменклатури_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ХарактеристикиНоменклатури_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{ХарактеристикиНоменклатури_Const.FULLNAME} *", () =>
                {
                    ХарактеристикиНоменклатури_Елемент page = new ХарактеристикиНоменклатури_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else if (unigueID != null)
            {
                ХарактеристикиНоменклатури_Objest ХарактеристикиНоменклатури_Objest = new ХарактеристикиНоменклатури_Objest();
                if (await ХарактеристикиНоменклатури_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{ХарактеристикиНоменклатури_Objest.Назва}", () =>
                    {
                        ХарактеристикиНоменклатури_Елемент page = new ХарактеристикиНоменклатури_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ХарактеристикиНоменклатури_Objest = ХарактеристикиНоменклатури_Objest,
                        };

                        page.SetValue();

                        return page;
                    }, true);
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
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
                ХарактеристикиНоменклатури_Objest ХарактеристикиНоменклатури_Objest_Новий = ХарактеристикиНоменклатури_Objest.Copy(true);
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