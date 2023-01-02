
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ВнутрішнєСпоживанняТоварів_PointerControl : PointerControl
    {
        public ВнутрішнєСпоживанняТоварів_PointerControl()
        {
            pointer = new ВнутрішнєСпоживанняТоварів_Pointer();
            WidthPresentation = 300;
            Caption = "Внутрішнє споживання товарів:";
        }

        ВнутрішнєСпоживанняТоварів_Pointer pointer;
        public ВнутрішнєСпоживанняТоварів_Pointer Pointer
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
            ВнутрішнєСпоживанняТоварів page = new ВнутрішнєСпоживанняТоварів(true);

            page.DocumentPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (ВнутрішнєСпоживанняТоварів_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Внутрішнє споживання товарів", () => { return page; });

            if (UseWherePeriod)
                page.SetValue();
            else
                page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ВнутрішнєСпоживанняТоварів_Pointer();
        }
    }
}