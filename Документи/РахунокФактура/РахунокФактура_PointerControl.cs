
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class РахунокФактура_PointerControl : PointerControl
    {
        public РахунокФактура_PointerControl()
        {
            pointer = new РахунокФактура_Pointer();
            WidthPresentation = 300;
            Caption = "Рахунок фактура:";
        }

        РахунокФактура_Pointer pointer;
        public РахунокФактура_Pointer Pointer
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
            РахунокФактура page = new РахунокФактура(true);

            page.DocumentPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (РахунокФактура_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Рахунок фактура", () => { return page; });

            if (UseWherePeriod)
                page.SetValue();
            else
                page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new РахунокФактура_Pointer();
        }
    }
}