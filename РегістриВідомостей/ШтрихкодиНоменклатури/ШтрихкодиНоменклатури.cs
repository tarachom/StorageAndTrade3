
/*
        ШтрихкодиНоменклатури.cs
*/

using Gtk;
using InterfaceGtk3;
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
                    new Where(ШтрихкодиНоменклатури_Const.Номенклатура, Comparison.EQ, НоменклатураВласник.Pointer.UniqueID.UGuid));
            }

            if (!ХарактеристикиНоменклатуриВласник.Pointer.IsEmpty())
            {
                ТабличніСписки.ШтрихкодиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ШтрихкодиНоменклатури_Const.ХарактеристикаНоменклатури, Comparison.EQ, ХарактеристикиНоменклатуриВласник.Pointer.UniqueID.UGuid));
            }

            await ТабличніСписки.ШтрихкодиНоменклатури_Записи.LoadRecords(TreeViewGrid, SelectPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ШтрихкодиНоменклатури_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!НоменклатураВласник.Pointer.IsEmpty())
            {
                ТабличніСписки.ШтрихкодиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ШтрихкодиНоменклатури_Const.Номенклатура, Comparison.EQ, НоменклатураВласник.Pointer.UniqueID.UGuid));
            }

            if (!ХарактеристикиНоменклатуриВласник.Pointer.IsEmpty())
            {
                ТабличніСписки.ШтрихкодиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(ШтрихкодиНоменклатури_Const.ХарактеристикаНоменклатури, Comparison.EQ, ХарактеристикиНоменклатуриВласник.Pointer.UniqueID.UGuid));
            }

            //Штрихкод
            ТабличніСписки.ШтрихкодиНоменклатури_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(ШтрихкодиНоменклатури_Const.Штрихкод, Comparison.LIKE, searchText));

            await ТабличніСписки.ШтрихкодиНоменклатури_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            ШтрихкодиНоменклатури_Елемент page = new ШтрихкодиНоменклатури_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                НоменклатураДляНового = НоменклатураВласник.Pointer,
                ХарактеристикаДляНового = ХарактеристикиНоменклатуриВласник.Pointer
            };

            if (IsNew)
                page.Елемент.New();
            else if (uniqueID == null || !await page.Елемент.Read(uniqueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
            page.SetValue();
        }

        protected override async ValueTask Delete(UniqueID uniqueID)
        {
            ШтрихкодиНоменклатури_Objest ШтрихкодиНоменклатури_Objest = new ШтрихкодиНоменклатури_Objest();
            if (await ШтрихкодиНоменклатури_Objest.Read(uniqueID))
                await ШтрихкодиНоменклатури_Objest.Delete();
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            ШтрихкодиНоменклатури_Objest ШтрихкодиНоменклатури_Objest = new ШтрихкодиНоменклатури_Objest();
            if (await ШтрихкодиНоменклатури_Objest.Read(uniqueID))
            {
                ШтрихкодиНоменклатури_Objest ШтрихкодиНоменклатури_Objest_Новий = ШтрихкодиНоменклатури_Objest.Copy();
                await ШтрихкодиНоменклатури_Objest_Новий.Save();

                return ШтрихкодиНоменклатури_Objest_Новий.UniqueID;
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