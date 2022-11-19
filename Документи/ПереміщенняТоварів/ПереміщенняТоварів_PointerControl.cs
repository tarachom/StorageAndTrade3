
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ПереміщенняТоварів_PointerControl : PointerControl
    {
        public ПереміщенняТоварів_PointerControl()
        {
            pointer = new ПереміщенняТоварів_Pointer();
            WidthPresentation = 300;
            Caption = "Переміщення товарів:";
        }

        ПереміщенняТоварів_Pointer pointer;
        public ПереміщенняТоварів_Pointer Pointer
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

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            ПереміщенняТоварів page = new ПереміщенняТоварів(true);

            page.DocumentPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (ПереміщенняТоварів_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Переміщення товарів", () => { return page; });

            page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ПереміщенняТоварів_Pointer();
        }
    }
}