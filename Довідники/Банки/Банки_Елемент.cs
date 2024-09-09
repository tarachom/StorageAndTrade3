

/*     файл:     Банки_Елемент.cs     */

using Gtk;
using InterfaceGtk;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Банки_Елемент : ДовідникЕлемент
    {
        public Банки_Objest Елемент { get; set; } = new Банки_Objest();

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

        public Банки_Елемент()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
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

        protected override void CreatePack2(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            ТипНаселеногоПункту.Text = Елемент.ТипНаселеногоПункту;
            КодМФО.Text = Елемент.КодМФО;
            НазваГоловноїУстановиАнг.Text = Елемент.НазваГоловноїУстановиАнг;
            КодЄДРПОУ.Text = Елемент.КодЄДРПОУ;
            ПовнаНазва.Text = Елемент.ПовнаНазва;
            УнікальнийКодБанку.Text = Елемент.УнікальнийКодБанку;
            КодОбластіОпераційноїДіяльності.Text = Елемент.КодОбластіОпераційноїДіяльності;
            НазваОбластіОпераційноїДіяльності.Text = Елемент.НазваОбластіОпераційноїДіяльності;
            КодОбластіЗгідноСтатуту.Text = Елемент.КодОбластіЗгідноСтатуту;
            НазваОбластіЗгідноСтатуту.Text = Елемент.НазваОбластіЗгідноСтатуту;
            ПоштовийІндекс.Text = Елемент.ПоштовийІндекс;
            НазваНаселеногоПункту.Text = Елемент.НазваНаселеногоПункту;
            Адреса.Text = Елемент.Адреса;
            КодТелефонногоЗвязку.Text = Елемент.КодТелефонногоЗвязку;
            НомерТелефону.Text = Елемент.НомерТелефону;
            ЧисловийКодСтануУстанови.Text = Елемент.ЧисловийКодСтануУстанови;
            НазваСтануУстанови.Text = Елемент.НазваСтануУстанови;
            ДатаЗміниСтану.Text = Елемент.ДатаЗміниСтану;
            ДатаВідкриттяУстанови.Text = Елемент.ДатаВідкриттяУстанови;
            ДатаЗакриттяУстанови.Text = Елемент.ДатаЗакриттяУстанови;
            КодНБУ.Text = Елемент.КодНБУ;
            НомерЛіцензії.Text = Елемент.НомерЛіцензії;
            ДатаЛіцензії.Text = Елемент.ДатаЛіцензії;
            КодСтатусу.Text = Елемент.КодСтатусу;
            Статус.Text = Елемент.Статус;
            ДатаЗапису.Text = Елемент.ДатаЗапису;

        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.ТипНаселеногоПункту = ТипНаселеногоПункту.Text;
            Елемент.КодМФО = КодМФО.Text;
            Елемент.НазваГоловноїУстановиАнг = НазваГоловноїУстановиАнг.Text;
            Елемент.КодЄДРПОУ = КодЄДРПОУ.Text;
            Елемент.ПовнаНазва = ПовнаНазва.Text;
            Елемент.УнікальнийКодБанку = УнікальнийКодБанку.Text;
            Елемент.КодОбластіОпераційноїДіяльності = КодОбластіОпераційноїДіяльності.Text;
            Елемент.НазваОбластіОпераційноїДіяльності = НазваОбластіОпераційноїДіяльності.Text;
            Елемент.КодОбластіЗгідноСтатуту = КодОбластіЗгідноСтатуту.Text;
            Елемент.НазваОбластіЗгідноСтатуту = НазваОбластіЗгідноСтатуту.Text;
            Елемент.ПоштовийІндекс = ПоштовийІндекс.Text;
            Елемент.НазваНаселеногоПункту = НазваНаселеногоПункту.Text;
            Елемент.Адреса = Адреса.Text;
            Елемент.КодТелефонногоЗвязку = КодТелефонногоЗвязку.Text;
            Елемент.НомерТелефону = НомерТелефону.Text;
            Елемент.ЧисловийКодСтануУстанови = ЧисловийКодСтануУстанови.Text;
            Елемент.НазваСтануУстанови = НазваСтануУстанови.Text;
            Елемент.ДатаЗміниСтану = ДатаЗміниСтану.Text;
            Елемент.ДатаВідкриттяУстанови = ДатаВідкриттяУстанови.Text;
            Елемент.ДатаЗакриттяУстанови = ДатаЗакриттяУстанови.Text;
            Елемент.КодНБУ = КодНБУ.Text;
            Елемент.НомерЛіцензії = НомерЛіцензії.Text;
            Елемент.ДатаЛіцензії = ДатаЛіцензії.Text;
            Елемент.КодСтатусу = КодСтатусу.Text;
            Елемент.Статус = Статус.Text;
            Елемент.ДатаЗапису = ДатаЗапису.Text;

        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            try
            {
                return await Елемент.Save();
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Елемент.GetBasis(), Caption, ex);
                return false;
            }
        }
    }
}
