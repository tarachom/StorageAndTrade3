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

using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;

using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class БанківськіРахункиОрганізацій : ДовідникЖурнал
    {
        public БанківськіРахункиОрганізацій() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.БанківськіРахункиОрганізацій_Записи.Store;
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.Where.Clear();

            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.LoadRecords();

            if (ТабличніСписки.БанківськіРахункиОрганізацій_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.БанківськіРахункиОрганізацій_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.Where.Clear();

            //Назва
            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.Where.Add(
                new Where(БанківськіРахункиОрганізацій_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.БанківськіРахункиОрганізацій_Записи.LoadRecords();

            if (ТабличніСписки.БанківськіРахункиОрганізацій_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.БанківськіРахункиОрганізацій_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{БанківськіРахункиОрганізацій_Const.FULLNAME} *", () =>
                {
                    БанківськіРахункиОрганізацій_Елемент page = new БанківськіРахункиОрганізацій_Елемент
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
                БанківськіРахункиОрганізацій_Objest БанківськіРахункиОрганізацій_Objest = new БанківськіРахункиОрганізацій_Objest();
                if (БанківськіРахункиОрганізацій_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{БанківськіРахункиОрганізацій_Objest.Назва}", () =>
                    {
                        БанківськіРахункиОрганізацій_Елемент page = new БанківськіРахункиОрганізацій_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            БанківськіРахункиОрганізацій_Objest = БанківськіРахункиОрганізацій_Objest,
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
            БанківськіРахункиОрганізацій_Objest БанківськіРахункиОрганізацій_Objest = new БанківськіРахункиОрганізацій_Objest();
            if (БанківськіРахункиОрганізацій_Objest.Read(unigueID))
                БанківськіРахункиОрганізацій_Objest.SetDeletionLabel(!БанківськіРахункиОрганізацій_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            БанківськіРахункиОрганізацій_Objest БанківськіРахункиОрганізацій_Objest = new БанківськіРахункиОрганізацій_Objest();
            if (БанківськіРахункиОрганізацій_Objest.Read(unigueID))
            {
                БанківськіРахункиОрганізацій_Objest БанківськіРахункиОрганізацій_Objest_Новий = БанківськіРахункиОрганізацій_Objest.Copy(true);
                БанківськіРахункиОрганізацій_Objest_Новий.Save();

                return БанківськіРахункиОрганізацій_Objest_Новий.UnigueID;
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