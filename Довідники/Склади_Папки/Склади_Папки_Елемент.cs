
/*
        Склади_Папки_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk3;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class Склади_Папки_Елемент : ДовідникЕлемент
    {
        public Склади_Папки_Objest Елемент { get; set; } = new Склади_Папки_Objest();

        public Склади_Папки_Pointer РодичДляНового { get; set; } = new Склади_Папки_Pointer();

        #region Fields
        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Склади_Папки_PointerControl Родич = new Склади_Папки_PointerControl() { Caption = "Папка:", WidthPresentation = 500 };

        #endregion

        #region TabularParts

        #endregion

        public Склади_Папки_Елемент() : base()
        {
            Element = Елемент;
        }

        protected override void CreatePack1(Box vBox)
        {
            // Код
            CreateField(vBox, "Код:", Код);

            // Назва
            CreateField(vBox, "Назва:", Назва);

            // Родич
            CreateField(vBox, null, Родич);
        }

        protected override void CreatePack2(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                Елемент.Родич = РодичДляНового;
            else
                Родич.OpenFolder = Елемент.UnigueID;

            Назва.Text = Елемент.Назва;
            Код.Text = Елемент.Код;
            Родич.Pointer = Елемент.Родич;
        }

        protected override void GetValue()
        {
            Елемент.Назва = Назва.Text;
            Елемент.Код = Код.Text;
            Елемент.Родич = Родич.Pointer;
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
