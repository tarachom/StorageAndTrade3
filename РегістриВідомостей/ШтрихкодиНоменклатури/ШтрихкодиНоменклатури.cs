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

using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = StorageAndTrade_1_0.РегістриВідомостей.ТабличніСписки;
using StorageAndTrade_1_0.РегістриВідомостей;

namespace StorageAndTrade
{
    class ШтрихкодиНоменклатури : РегістриВідомостейЖурнал
    {
        public Номенклатура_PointerControl НоменклатураВласник = new Номенклатура_PointerControl();
        public ХарактеристикиНоменклатури_PointerControl ХарактеристикиНоменклатуриВласник = new ХарактеристикиНоменклатури_PointerControl();

        public ШтрихкодиНоменклатури() : base()
        {
            ТабличніСписки.ШтрихкодиНоменклатури_Записи.AddColumns(TreeViewGrid);

            //Номенклатура Власник
            HBoxTop.PackStart(НоменклатураВласник, false, false, 2);
            НоменклатураВласник.Caption = "Номенклатура:";
            НоменклатураВласник.AfterSelectFunc = () =>
            {
                SelectPointerItem?.Clear();
                LoadRecords();
            };

            //Характеристика Власник
            HBoxTop.PackStart(ХарактеристикиНоменклатуриВласник, false, false, 2);
            ХарактеристикиНоменклатуриВласник.Caption = "Характеристика:";
            ХарактеристикиНоменклатуриВласник.BeforeClickOpenFunc = () =>
            {
                ХарактеристикиНоменклатуриВласник.НоменклатураВласник = НоменклатураВласник.Pointer;
            };
            ХарактеристикиНоменклатуриВласник.AfterSelectFunc = () =>
            {
                SelectPointerItem?.Clear();
                LoadRecords();
            };
        }

        protected override async void LoadRecords()
        {
            ТабличніСписки.ШтрихкодиНоменклатури_Записи.SelectPointerItem = SelectPointerItem;

            ТабличніСписки.ШтрихкодиНоменклатури_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            if (!НоменклатураВласник.Pointer.IsEmpty())
            {
                ТабличніСписки.ШтрихкодиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ШтрихкодиНоменклатури_Const.Номенклатура, Comparison.EQ, НоменклатураВласник.Pointer.UnigueID.UGuid));
            }

            if (!ХарактеристикиНоменклатуриВласник.Pointer.IsEmpty())
            {
                ТабличніСписки.ШтрихкодиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ШтрихкодиНоменклатури_Const.ХарактеристикаНоменклатури, Comparison.EQ, ХарактеристикиНоменклатуриВласник.Pointer.UnigueID.UGuid));
            }

            await ТабличніСписки.ШтрихкодиНоменклатури_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ШтрихкодиНоменклатури_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ШтрихкодиНоменклатури_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ШтрихкодиНоменклатури_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ШтрихкодиНоменклатури_Записи.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.ШтрихкодиНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!НоменклатураВласник.Pointer.IsEmpty())
            {
                ТабличніСписки.ШтрихкодиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ШтрихкодиНоменклатури_Const.Номенклатура, Comparison.EQ, НоменклатураВласник.Pointer.UnigueID.UGuid));
            }

            if (!ХарактеристикиНоменклатуриВласник.Pointer.IsEmpty())
            {
                ТабличніСписки.ШтрихкодиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ШтрихкодиНоменклатури_Const.ХарактеристикаНоменклатури, Comparison.EQ, ХарактеристикиНоменклатуриВласник.Pointer.UnigueID.UGuid));
            }

            //Штрихкод
            ТабличніСписки.ШтрихкодиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid, 
                new Where(ШтрихкодиНоменклатури_Const.Штрихкод, Comparison.LIKE, searchText));

            await ТабличніСписки.ШтрихкодиНоменклатури_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{ШтрихкодиНоменклатури_Const.FULLNAME} *", () =>
                {
                    ШтрихкодиНоменклатури_Елемент page = new ШтрихкодиНоменклатури_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true,
                        НоменклатураДляНового = НоменклатураВласник.Pointer,
                        ХарактеристикаДляНового = ХарактеристикиНоменклатуриВласник.Pointer
                    };

                    page.SetValue();

                    return page;
                });
            }
            else if (unigueID != null)
            {
                ШтрихкодиНоменклатури_Objest ШтрихкодиНоменклатури_Objest = new ШтрихкодиНоменклатури_Objest();
                if (await ШтрихкодиНоменклатури_Objest.Read(unigueID))
                {
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{ШтрихкодиНоменклатури_Objest.Штрихкод}", () =>
                    {
                        ШтрихкодиНоменклатури_Елемент page = new ШтрихкодиНоменклатури_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ШтрихкодиНоменклатури_Objest = ШтрихкодиНоменклатури_Objest,
                        };

                        page.SetValue();

                        return page;
                    });
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override async ValueTask Delete(UnigueID unigueID)
        {
            ШтрихкодиНоменклатури_Objest ШтрихкодиНоменклатури_Objest = new ШтрихкодиНоменклатури_Objest();
            if (await ШтрихкодиНоменклатури_Objest.Read(unigueID))
                await ШтрихкодиНоменклатури_Objest.Delete();
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ШтрихкодиНоменклатури_Objest ШтрихкодиНоменклатури_Objest = new ШтрихкодиНоменклатури_Objest();
            if (await ШтрихкодиНоменклатури_Objest.Read(unigueID))
            {
                ШтрихкодиНоменклатури_Objest ШтрихкодиНоменклатури_Objest_Новий = ШтрихкодиНоменклатури_Objest.Copy();
                await ШтрихкодиНоменклатури_Objest_Новий.Save();

                return ШтрихкодиНоменклатури_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "РегістриВідомостей.ШтрихкодиНоменклатури";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            LoadRecords();
        }
    }
}