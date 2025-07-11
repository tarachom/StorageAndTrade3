

/*     
        ВільніЗалишки.cs
        Список

        Табличний список - Записи
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using ТабличніСписки = GeneratedCode.РегістриНакопичення.ТабличніСписки;

namespace StorageAndTrade.РегістриНакопичення
{
    public class ВільніЗалишки : РегістриНакопиченняЖурнал
    {
        public ВільніЗалишки() : base()
        {
            ТабличніСписки.ВільніЗалишки_Записи.AddColumns(TreeViewGrid);
            
            HBoxTop.PackStart(new Label("Таблиці розрахунків:"), false, false, 0);
            CreateLink(HBoxTop, "Залишки", async () => await ВільніЗалишки_Залишки_Звіт.Сформувати(Період.DateStartControl.ПочатокДня(), Період.DateStopControl.КінецьДня()));
            CreateLink(HBoxTop, "Підсумки", async () => await ВільніЗалишки_Підсумки_Звіт.Сформувати());
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ВільніЗалишки_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ВільніЗалишки_Записи.LoadRecords(TreeViewGrid, SelectPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ВільніЗалишки_Записи.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.ВільніЗалишки_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText) { FuncToField = "to_char", FuncToField_Param1 = "'DD.MM.YYYY'" });

            await ТабличніСписки.ВільніЗалишки_Записи.LoadRecords(TreeViewGrid);
        }

        const string КлючНалаштуванняКористувача = "РегістриНакопичення.ВільніЗалишки";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
