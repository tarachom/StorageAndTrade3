

/*     файл:     Банки_Елемент.cs     */

using Gtk;

using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class Банки_Елемент : ДовідникЕлемент
    {
        public Банки_Objest Банки_Objest { get; set; } = new Банки_Objest();

        #region Fields

        Entry Код = new Entry() { WidthRequest = 500 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Entry ТипНаселеногоПункту = new Entry() { WidthRequest = 500 };
        Entry КодМФО = new Entry() { WidthRequest = 500 };
        Entry НазваГоловноїУстановиАнг = new Entry() { WidthRequest = 500 };
        Entry КодЄДРПОУ = new Entry() { WidthRequest = 500 };
        Entry ПовнаНазва = new Entry() { WidthRequest = 500 };
        Entry УнікальнийКодБанку = new Entry() { WidthRequest = 500 };
        Entry КодОбластіОпераційноїДіяльності = new Entry() { WidthRequest = 500 };
        Entry НазваОбластіОпераційноїДіяльності = new Entry() { WidthRequest = 500 };
        Entry КодОбластіЗгідноСтатуту = new Entry() { WidthRequest = 500 };
        Entry НазваОбластіЗгідноСтатуту = new Entry() { WidthRequest = 500 };
        Entry ПоштовийІндекс = new Entry() { WidthRequest = 500 };
        Entry НазваНаселеногоПункту = new Entry() { WidthRequest = 500 };
        Entry Адреса = new Entry() { WidthRequest = 500 };
        Entry КодТелефонногоЗвязку = new Entry() { WidthRequest = 500 };
        Entry НомерТелефону = new Entry() { WidthRequest = 500 };
        Entry ЧисловийКодСтануУстанови = new Entry() { WidthRequest = 500 };
        Entry НазваСтануУстанови = new Entry() { WidthRequest = 500 };
        Entry ДатаЗміниСтану = new Entry() { WidthRequest = 500 };
        Entry ДатаВідкриттяУстанови = new Entry() { WidthRequest = 500 };
        Entry ДатаЗакриттяУстанови = new Entry() { WidthRequest = 500 };
        Entry КодНБУ = new Entry() { WidthRequest = 500 };
        Entry НомерЛіцензії = new Entry() { WidthRequest = 500 };
        Entry ДатаЛіцензії = new Entry() { WidthRequest = 500 };
        Entry КодСтатусу = new Entry() { WidthRequest = 500 };
        Entry Статус = new Entry() { WidthRequest = 500 };
        Entry ДатаЗапису = new Entry() { WidthRequest = 500 };

        #endregion

        #region TabularParts

        #endregion

        public Банки_Елемент() : base()
        {

        }

        protected override void CreatePack1(VBox vBox)
        {

            CreateField(vBox, "Код:", Код);

            CreateField(vBox, "Назва:", Назва);

            CreateField(vBox, "ТипНаселеногоПункту:", ТипНаселеногоПункту);

            CreateField(vBox, "КодМФО:", КодМФО);

            CreateField(vBox, "НазваГоловноїУстановиАнг:", НазваГоловноїУстановиАнг);

            CreateField(vBox, "КодЄДРПОУ:", КодЄДРПОУ);

            CreateField(vBox, "ПовнаНазва:", ПовнаНазва);

            CreateField(vBox, "УнікальнийКодБанку:", УнікальнийКодБанку);

            CreateField(vBox, "КодОбластіОпераційноїДіяльності:", КодОбластіОпераційноїДіяльності);

            CreateField(vBox, "НазваОбластіОпераційноїДіяльності:", НазваОбластіОпераційноїДіяльності);

            CreateField(vBox, "КодОбластіЗгідноСтатуту:", КодОбластіЗгідноСтатуту);

            CreateField(vBox, "НазваОбластіЗгідноСтатуту:", НазваОбластіЗгідноСтатуту);

            CreateField(vBox, "ПоштовийІндекс:", ПоштовийІндекс);

            CreateField(vBox, "НазваНаселеногоПункту:", НазваНаселеногоПункту);

            CreateField(vBox, "Адреса:", Адреса);

            CreateField(vBox, "КодТелефонногоЗвязку:", КодТелефонногоЗвязку);

            CreateField(vBox, "НомерТелефону:", НомерТелефону);

            CreateField(vBox, "ЧисловийКодСтануУстанови:", ЧисловийКодСтануУстанови);

            CreateField(vBox, "НазваСтануУстанови:", НазваСтануУстанови);

            CreateField(vBox, "ДатаЗміниСтану:", ДатаЗміниСтану);

            CreateField(vBox, "ДатаВідкриттяУстанови:", ДатаВідкриттяУстанови);

            CreateField(vBox, "ДатаЗакриттяУстанови:", ДатаЗакриттяУстанови);

            CreateField(vBox, "КодНБУ:", КодНБУ);

            CreateField(vBox, "НомерЛіцензії:", НомерЛіцензії);

            CreateField(vBox, "ДатаЛіцензії:", ДатаЛіцензії);

            CreateField(vBox, "КодСтатусу:", КодСтатусу);

            CreateField(vBox, "Статус:", Статус);

            CreateField(vBox, "ДатаЗапису:", ДатаЗапису);

        }

        protected override void CreatePack2(VBox vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                Банки_Objest.New();

            Код.Text = Банки_Objest.Код;
            Назва.Text = Банки_Objest.Назва;
            ТипНаселеногоПункту.Text = Банки_Objest.ТипНаселеногоПункту;
            КодМФО.Text = Банки_Objest.КодМФО;
            НазваГоловноїУстановиАнг.Text = Банки_Objest.НазваГоловноїУстановиАнг;
            КодЄДРПОУ.Text = Банки_Objest.КодЄДРПОУ;
            ПовнаНазва.Text = Банки_Objest.ПовнаНазва;
            УнікальнийКодБанку.Text = Банки_Objest.УнікальнийКодБанку;
            КодОбластіОпераційноїДіяльності.Text = Банки_Objest.КодОбластіОпераційноїДіяльності;
            НазваОбластіОпераційноїДіяльності.Text = Банки_Objest.НазваОбластіОпераційноїДіяльності;
            КодОбластіЗгідноСтатуту.Text = Банки_Objest.КодОбластіЗгідноСтатуту;
            НазваОбластіЗгідноСтатуту.Text = Банки_Objest.НазваОбластіЗгідноСтатуту;
            ПоштовийІндекс.Text = Банки_Objest.ПоштовийІндекс;
            НазваНаселеногоПункту.Text = Банки_Objest.НазваНаселеногоПункту;
            Адреса.Text = Банки_Objest.Адреса;
            КодТелефонногоЗвязку.Text = Банки_Objest.КодТелефонногоЗвязку;
            НомерТелефону.Text = Банки_Objest.НомерТелефону;
            ЧисловийКодСтануУстанови.Text = Банки_Objest.ЧисловийКодСтануУстанови;
            НазваСтануУстанови.Text = Банки_Objest.НазваСтануУстанови;
            ДатаЗміниСтану.Text = Банки_Objest.ДатаЗміниСтану;
            ДатаВідкриттяУстанови.Text = Банки_Objest.ДатаВідкриттяУстанови;
            ДатаЗакриттяУстанови.Text = Банки_Objest.ДатаЗакриттяУстанови;
            КодНБУ.Text = Банки_Objest.КодНБУ;
            НомерЛіцензії.Text = Банки_Objest.НомерЛіцензії;
            ДатаЛіцензії.Text = Банки_Objest.ДатаЛіцензії;
            КодСтатусу.Text = Банки_Objest.КодСтатусу;
            Статус.Text = Банки_Objest.Статус;
            ДатаЗапису.Text = Банки_Objest.ДатаЗапису;

        }

        protected override void GetValue()
        {
            Банки_Objest.Код = Код.Text;
            Банки_Objest.Назва = Назва.Text;
            Банки_Objest.ТипНаселеногоПункту = ТипНаселеногоПункту.Text;
            Банки_Objest.КодМФО = КодМФО.Text;
            Банки_Objest.НазваГоловноїУстановиАнг = НазваГоловноїУстановиАнг.Text;
            Банки_Objest.КодЄДРПОУ = КодЄДРПОУ.Text;
            Банки_Objest.ПовнаНазва = ПовнаНазва.Text;
            Банки_Objest.УнікальнийКодБанку = УнікальнийКодБанку.Text;
            Банки_Objest.КодОбластіОпераційноїДіяльності = КодОбластіОпераційноїДіяльності.Text;
            Банки_Objest.НазваОбластіОпераційноїДіяльності = НазваОбластіОпераційноїДіяльності.Text;
            Банки_Objest.КодОбластіЗгідноСтатуту = КодОбластіЗгідноСтатуту.Text;
            Банки_Objest.НазваОбластіЗгідноСтатуту = НазваОбластіЗгідноСтатуту.Text;
            Банки_Objest.ПоштовийІндекс = ПоштовийІндекс.Text;
            Банки_Objest.НазваНаселеногоПункту = НазваНаселеногоПункту.Text;
            Банки_Objest.Адреса = Адреса.Text;
            Банки_Objest.КодТелефонногоЗвязку = КодТелефонногоЗвязку.Text;
            Банки_Objest.НомерТелефону = НомерТелефону.Text;
            Банки_Objest.ЧисловийКодСтануУстанови = ЧисловийКодСтануУстанови.Text;
            Банки_Objest.НазваСтануУстанови = НазваСтануУстанови.Text;
            Банки_Objest.ДатаЗміниСтану = ДатаЗміниСтану.Text;
            Банки_Objest.ДатаВідкриттяУстанови = ДатаВідкриттяУстанови.Text;
            Банки_Objest.ДатаЗакриттяУстанови = ДатаЗакриттяУстанови.Text;
            Банки_Objest.КодНБУ = КодНБУ.Text;
            Банки_Objest.НомерЛіцензії = НомерЛіцензії.Text;
            Банки_Objest.ДатаЛіцензії = ДатаЛіцензії.Text;
            Банки_Objest.КодСтатусу = КодСтатусу.Text;
            Банки_Objest.Статус = Статус.Text;
            Банки_Objest.ДатаЗапису = ДатаЗапису.Text;

        }

        #endregion

        protected override void Save()
        {
            try
            {
                Банки_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            UnigueID = Банки_Objest.UnigueID;
            Caption = Назва.Text;
        }
    }
}
