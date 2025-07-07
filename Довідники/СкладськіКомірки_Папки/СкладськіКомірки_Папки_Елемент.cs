
/*
        СкладськіКомірки_Папки_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk3;

using GeneratedCode.Довідники;
using GeneratedCode.Документи;
using GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class СкладськіКомірки_Папки_Елемент : ДовідникЕлемент
    {
        public СкладськіКомірки_Папки_Objest Елемент { get; init; } = new СкладськіКомірки_Папки_Objest();

        public СкладськіПриміщення_Pointer ВласникДляНового = new СкладськіПриміщення_Pointer();

        public СкладськіКомірки_Папки_Pointer РодичДляНового { get; set; } = new СкладськіКомірки_Папки_Pointer();

        #region Fields
        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        СкладськіКомірки_Папки_PointerControl Родич = new СкладськіКомірки_Папки_PointerControl() { Caption = "Папка", WidthPresentation = 500 };
        СкладськіПриміщення_PointerControl Власник = new СкладськіПриміщення_PointerControl() { Caption = "Приміщення", WidthPresentation = 500 };

        #endregion

        #region TabularParts

        #endregion

        public СкладськіКомірки_Папки_Елемент() : base()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;


        }

        protected override void CreatePack1(Box vBox)
        {

            // Код
            CreateField(vBox, "Код:", Код);

            // Назва
            CreateField(vBox, "Назва:", Назва);

            // Родич
            CreateField(vBox, null, Родич);

            // Власник
            CreateField(vBox, null, Власник);

        }

        protected override void CreatePack2(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {

            if (IsNew)
                Елемент.Власник = ВласникДляНового;

            if (IsNew)
                Елемент.Родич = РодичДляНового;
            else
                Родич.OpenFolder = Елемент.UnigueID;
            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            Родич.Pointer = Елемент.Родич;
            Власник.Pointer = Елемент.Власник;

        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.Родич = Родич.Pointer;
            Елемент.Власник = Власник.Pointer;

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
