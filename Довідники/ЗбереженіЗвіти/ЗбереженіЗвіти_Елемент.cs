
/*
        ЗбереженіЗвіти_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk3;

using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class ЗбереженіЗвіти_Елемент : ДовідникЕлемент
    {
        public ЗбереженіЗвіти_Objest Елемент { get; init; } = new ЗбереженіЗвіти_Objest();

        #region Fields
        Entry Назва = new Entry() { WidthRequest = 500 };
        TextView Опис = new TextView() { WrapMode = WrapMode.Word };
        Label Інформація = new Label() { UseMarkup = true, UseUnderline = false, Wrap = true };

        #endregion

        #region TabularParts

        // Таблична частина "ЗвітСторінка"
        ЗбереженіЗвіти_ТабличнаЧастина_ЗвітСторінка ЗвітСторінка = new ЗбереженіЗвіти_ТабличнаЧастина_ЗвітСторінка() { WidthRequest = 1500, HeightRequest = 700 };

        #endregion

        public ЗбереженіЗвіти_Елемент() : base()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
        {
            // Назва
            CreateField(vBox, "Назва:", Назва);

            // Опис
            CreateFieldView(vBox, "Опис:", Опис, 500, 60);

            // Інформація
            CreateField(vBox, null, Інформація);

            // Таблична частина "ЗвітСторінка" 
            CreateTablePart(vBox, null, ЗвітСторінка);
        }

        protected override void CreatePack2(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {
            Назва.Text = Елемент.Назва;
            Опис.Buffer.Text = Елемент.Опис;
            Інформація.LabelMarkup = Елемент.Інформація;
            Інформація.Visible = !string.IsNullOrEmpty(Елемент.Інформація);
            
            // Таблична частина "ЗвітСторінка"
            ЗвітСторінка.ЕлементВласник = Елемент;
            await ЗвітСторінка.LoadRecords();
        }

        protected override void GetValue()
        {
            Елемент.Назва = Назва.Text;
            Елемент.Опис = Опис.Buffer.Text;
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSaved = false;
            try
            {
                if (await Елемент.Save())
                {
                    await ЗвітСторінка.SaveRecords(); // Таблична частина "ЗвітСторінка"
                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Елемент.GetBasis(), Caption, ex);
            }
            return isSaved;
        }
    }
}
