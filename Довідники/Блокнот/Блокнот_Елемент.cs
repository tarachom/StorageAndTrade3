
/*
        Блокнот_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk3;

using GeneratedCode.Довідники;
using GeneratedCode.Документи;
using GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class Блокнот_Елемент : ДовідникЕлемент
    {
        public Блокнот_Objest Елемент { get; init; } = new Блокнот_Objest();

        #region Fields
        Entry Код = new Entry() { WidthRequest = 500 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        DateTimeControl ДатаЗапису = new DateTimeControl();
        TextView Опис = new TextView() { WidthRequest = 500, WrapMode = WrapMode.Word };
        Entry Лінк = new Entry() { WidthRequest = 500 };

        #endregion

        #region TabularParts

        #endregion

        public Блокнот_Елемент() : base()
        {
            Element = Елемент;
        }

        protected override void CreatePack1(Box vBox)
        {

            // Код
            CreateField(vBox, "Код:", Код);

            // Назва
            CreateField(vBox, "Назва:", Назва);

            // ДатаЗапису
            CreateField(vBox, "ДатаЗапису:", ДатаЗапису);

            // Опис
            CreateFieldView(vBox, "Опис:", Опис, 500, 200);

            // Лінк
            CreateField(vBox, "Лінк:", Лінк);

        }

        protected override void CreatePack2(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            ДатаЗапису.Value = Елемент.ДатаЗапису;
            Опис.Buffer.Text = Елемент.Опис;
            Лінк.Text = Елемент.Лінк;

        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.ДатаЗапису = ДатаЗапису.Value;
            Елемент.Опис = Опис.Buffer.Text;
            Елемент.Лінк = Лінк.Text;

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
