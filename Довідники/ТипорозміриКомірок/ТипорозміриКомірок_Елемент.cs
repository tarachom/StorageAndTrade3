
/*
        ТипорозміриКомірок_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk3;

using GeneratedCode.Довідники;
using GeneratedCode.Документи;
using GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class ТипорозміриКомірок_Елемент : ДовідникЕлемент
    {
        public ТипорозміриКомірок_Objest Елемент { get; set; } = new ТипорозміриКомірок_Objest();

        #region Fields
        Entry Назва = new Entry() { WidthRequest = 250 };
        Entry Висота = new Entry() { WidthRequest = 100 };
        Entry Ширина = new Entry() { WidthRequest = 100 };
        Entry Глибина = new Entry() { WidthRequest = 100 };
        Entry Обєм = new Entry() { WidthRequest = 100 };
        Entry Вантажопідйомність = new Entry() { WidthRequest = 100 };

        #endregion

        #region TabularParts

        #endregion

        public ТипорозміриКомірок_Елемент() : base()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;
        }

        protected override void CreatePack1(Box vBox)
        {
            // Назва
            CreateField(vBox, "Назва:", Назва);

            // Висота
            CreateField(vBox, "Висота:", Висота);

            // Ширина
            CreateField(vBox, "Ширина:", Ширина);

            // Глибина
            CreateField(vBox, "Глибина:", Глибина);

            // Обєм
            CreateField(vBox, "Обєм:", Обєм);

            // Вантажопідйомність
            CreateField(vBox, "Вантажопідйомність:", Вантажопідйомність);
        }

        protected override void CreatePack2(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Назва.Text = Елемент.Назва;
            Висота.Text = Елемент.Висота;
            Ширина.Text = Елемент.Ширина;
            Глибина.Text = Елемент.Глибина;
            Обєм.Text = Елемент.Обєм;
            Вантажопідйомність.Text = Елемент.Вантажопідйомність;
        }

        protected override void GetValue()
        {
            Елемент.Назва = Назва.Text;
            Елемент.Висота = Висота.Text;
            Елемент.Ширина = Ширина.Text;
            Елемент.Глибина = Глибина.Text;
            Елемент.Обєм = Обєм.Text;
            Елемент.Вантажопідйомність = Вантажопідйомність.Text;
        }

        #endregion

        protected override async ValueTask<bool> Save()
        {
            bool isSaved = false;
            try
            {
                if (await Елемент.Save())
                {
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
