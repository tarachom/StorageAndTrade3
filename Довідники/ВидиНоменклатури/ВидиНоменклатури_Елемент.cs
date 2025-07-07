
/*
        ВидиНоменклатури_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk3;

using GeneratedCode.Довідники;
using GeneratedCode.Документи;
using GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class ВидиНоменклатури_Елемент : ДовідникЕлемент
    {
        public ВидиНоменклатури_Objest Елемент { get; set; } = new ВидиНоменклатури_Objest();

        #region Fields
        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        TextView Опис = new TextView() { WidthRequest = 500, WrapMode = WrapMode.Word };
        ПакуванняОдиниціВиміру_PointerControl ОдиницяВиміру = new ПакуванняОдиниціВиміру_PointerControl() { Caption = "Пакування:", WidthPresentation = 300 };
        ComboBoxText ТипНоменклатури = new ComboBoxText();

        #endregion

        #region TabularParts

        #endregion

        public ВидиНоменклатури_Елемент() : base()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;


            foreach (var field in ПсевдонімиПерелічення.ТипиНоменклатури_List())
                ТипНоменклатури.Append(field.Value.ToString(), field.Name);

        }

        protected override void CreatePack1(Box vBox)
        {

            // Код
            CreateField(vBox, "Код:", Код);

            // Назва
            CreateField(vBox, "Назва:", Назва);

            // Опис
            CreateFieldView(vBox, "Опис:", Опис, 500, 200);

            // ОдиницяВиміру
            CreateField(vBox, null, ОдиницяВиміру);

            // ТипНоменклатури
            CreateField(vBox, "Тип:", ТипНоменклатури);

        }

        protected override void CreatePack2(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            Опис.Buffer.Text = Елемент.Опис;
            ОдиницяВиміру.Pointer = Елемент.ОдиницяВиміру;
            ТипНоменклатури.ActiveId = Елемент.ТипНоменклатури.ToString(); if (ТипНоменклатури.Active == -1) ТипНоменклатури.Active = 0;

        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.Опис = Опис.Buffer.Text;
            Елемент.ОдиницяВиміру = ОдиницяВиміру.Pointer;
            if (ТипНоменклатури.Active != -1) Елемент.ТипНоменклатури = Enum.Parse<ТипиНоменклатури>(ТипНоменклатури.ActiveId);

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
