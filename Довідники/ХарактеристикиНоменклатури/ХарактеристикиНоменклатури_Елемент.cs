
/*
        ХарактеристикиНоменклатури_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk3;

using GeneratedCode.Довідники;
using GeneratedCode.Документи;
using GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class ХарактеристикиНоменклатури_Елемент : ДовідникЕлемент
    {
        public ХарактеристикиНоменклатури_Objest Елемент { get; init; } = new ХарактеристикиНоменклатури_Objest();

        public Номенклатура_Pointer ВласникДляНового = new Номенклатура_Pointer();

        #region Fields
        
        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Номенклатура_PointerControl Номенклатура = new Номенклатура_PointerControl() { Caption = "Номенклатура", WidthPresentation = 420 };
        TextView НазваПовна = new TextView() { WidthRequest = 500, WrapMode = WrapMode.Word };

        #endregion

        #region TabularParts

        #endregion

        public ХарактеристикиНоменклатури_Елемент() : base()
        {
            Element = Елемент;
        }

        protected override void CreatePack1(Box vBox)
        {
            // Код
            CreateField(vBox, "Код:", Код);

            // Назва
            CreateField(vBox, "Назва:", Назва);

            // Номенклатура
            CreateField(vBox, null, Номенклатура);

            // НазваПовна
            CreateFieldView(vBox, "Повна назва:", НазваПовна, 500, 200);
        }

        protected override void CreatePack2(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                Елемент.Номенклатура = ВласникДляНового;

            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            Номенклатура.Pointer = Елемент.Номенклатура;
            НазваПовна.Buffer.Text = Елемент.НазваПовна;
        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.Номенклатура = Номенклатура.Pointer;
            Елемент.НазваПовна = НазваПовна.Buffer.Text;
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
