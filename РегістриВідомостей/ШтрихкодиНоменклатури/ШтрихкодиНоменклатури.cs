
/*
        ШтрихкодиНоменклатури.cs
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using ТабличніСписки = GeneratedCode.РегістриВідомостей.ТабличніСписки;
using GeneratedCode.РегістриВідомостей;

namespace StorageAndTrade.РегістриВідомостей
{
    class ШтрихкодиНоменклатури : РегістриВідомостейЖурнал
    {
        public Номенклатура_PointerControl НоменклатураВласник = new Номенклатура_PointerControl();
        public ХарактеристикиНоменклатури_PointerControl ХарактеристикиНоменклатуриВласник = new ХарактеристикиНоменклатури_PointerControl();

        public ШтрихкодиНоменклатури()
        {
            ТабличніСписки.ШтрихкодиНоменклатури_Записи.AddColumns(TreeViewGrid);
            ТабличніСписки.ШтрихкодиНоменклатури_Записи.Сторінки(TreeViewGrid, new Сторінки.Налаштування() { PageSize = 300, Тип = Сторінки.ТипЖурналу.РегістриВідомостей });

            //Номенклатура Власник
            HBoxTop.PackStart(НоменклатураВласник, false, false, 2);
            НоменклатураВласник.Caption = "Номенклатура:";
            НоменклатураВласник.AfterSelectFunc = async () =>
            {
                SelectPointerItem?.Clear();
                await BeforeLoadRecords();
            };

            //Характеристика Власник
            HBoxTop.PackStart(ХарактеристикиНоменклатуриВласник, false, false, 2);
            ХарактеристикиНоменклатуриВласник.Caption = "Характеристика:";
            ХарактеристикиНоменклатуриВласник.BeforeClickOpenFunc = () => ХарактеристикиНоменклатуриВласник.Власник = НоменклатураВласник.Pointer;
            ХарактеристикиНоменклатуриВласник.AfterSelectFunc = async () =>
            {
                SelectPointerItem?.Clear();
                await BeforeLoadRecords();
            };
        }

        public override async ValueTask LoadRecords()
        {
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

            await ТабличніСписки.ШтрихкодиНоменклатури_Записи.LoadRecords(TreeViewGrid, SelectPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
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

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            ШтрихкодиНоменклатури_Елемент page = new ШтрихкодиНоменклатури_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew,
                НоменклатураДляНового = НоменклатураВласник.Pointer,
                ХарактеристикаДляНового = ХарактеристикиНоменклатуриВласник.Pointer
            };

            if (IsNew)
                page.ШтрихкодиНоменклатури_Objest.New();
            else if (unigueID == null || !await page.ШтрихкодиНоменклатури_Objest.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
            page.SetValue();
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

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }
    }
}