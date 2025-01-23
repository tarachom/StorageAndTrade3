
/*
        ВидиЗапасів_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk;

using GeneratedCode.Довідники;
using GeneratedCode.Документи;
using GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    class ВидиЗапасів_Елемент : ДовідникЕлемент
    {
        public ВидиЗапасів_Objest Елемент { get; set; } = new ВидиЗапасів_Objest();

        #region Fields
        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        Організації_PointerControl Організація = new Організації_PointerControl() { Caption = "Організація", WidthPresentation = 500 };
        ComboBoxText ТипЗапасів = new ComboBoxText();
        Валюти_PointerControl Валюта = new Валюти_PointerControl() { Caption = "Валюта", WidthPresentation = 500 };
        Контрагенти_PointerControl Контрагент = new Контрагенти_PointerControl() { Caption = "Контрагент", WidthPresentation = 500 };
        ДоговориКонтрагентів_PointerControl Договір = new ДоговориКонтрагентів_PointerControl() { Caption = "Договір", WidthPresentation = 500 };

        #endregion

        #region TabularParts

        #endregion

        public ВидиЗапасів_Елемент() : base()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

            foreach (var field in ПсевдонімиПерелічення.ТипЗапасів_List())
                ТипЗапасів.Append(field.Value.ToString(), field.Name);
        }

        protected override void CreatePack1(Box vBox)
        {

            // Код
            CreateField(vBox, "Код:", Код);

            // Назва
            CreateField(vBox, "Назва:", Назва);

            // Організація
            CreateField(vBox, null, Організація);

            // ТипЗапасів
            CreateField(vBox, "ТипЗапасів:", ТипЗапасів);

            // Валюта
            CreateField(vBox, null, Валюта);

            // Контрагент
            CreateField(vBox, null, Контрагент);
            Контрагент.AfterSelectFunc = async () => await Контрагент.ПривязкаДоДоговору(Договір);

            // Договір
            CreateField(vBox, null, Договір);
        }

        protected override void CreatePack2(Box vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            Код.Text = Елемент.Код;
            Назва.Text = Елемент.Назва;
            Організація.Pointer = Елемент.Організація;
            ТипЗапасів.ActiveId = Елемент.ТипЗапасів.ToString(); if (ТипЗапасів.Active == -1) ТипЗапасів.Active = 0;
            Валюта.Pointer = Елемент.Валюта;
            Контрагент.Pointer = Елемент.Контрагент;
            Договір.Pointer = Елемент.Договір;

        }

        protected override void GetValue()
        {
            Елемент.Код = Код.Text;
            Елемент.Назва = Назва.Text;
            Елемент.Організація = Організація.Pointer;
            if (ТипЗапасів.Active != -1) Елемент.ТипЗапасів = Enum.Parse<ТипЗапасів>(ТипЗапасів.ActiveId);
            Елемент.Валюта = Валюта.Pointer;
            Елемент.Контрагент = Контрагент.Pointer;
            Елемент.Договір = Договір.Pointer;

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
