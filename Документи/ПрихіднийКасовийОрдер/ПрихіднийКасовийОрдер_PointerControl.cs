
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ПрихіднийКасовийОрдер_PointerControl : PointerControl
    {
        public ПрихіднийКасовийОрдер_PointerControl()
        {
            pointer = new ПрихіднийКасовийОрдер_Pointer();
            WidthPresentation = 300;
            Caption = "Прихідний касовий ордер:";
        }

        ПрихіднийКасовийОрдер_Pointer pointer;
        public ПрихіднийКасовийОрдер_Pointer Pointer
        {
            get
            {
                return pointer;
            }
            set
            {
                pointer = value;

                if (pointer != null)
                    Presentation = pointer.GetPresentation();
                else
                    Presentation = "";
            }
        }

        //Відбір по періоду в журналі
        public bool UseWherePeriod { get; set; } = false;

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            ПрихіднийКасовийОрдер page = new ПрихіднийКасовийОрдер(true);

            page.DocumentPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (ПрихіднийКасовийОрдер_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Прихідний касовий ордер", () => { return page; });

            if (UseWherePeriod)
                page.SetValue();
            else
                page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ПрихіднийКасовийОрдер_Pointer();
        }
    }
}