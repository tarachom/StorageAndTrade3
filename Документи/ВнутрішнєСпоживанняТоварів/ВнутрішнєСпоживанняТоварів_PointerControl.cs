
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

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Вибір - Документи: Внутрішнє споживання товарів", () =>
            {
                ВнутрішнєСпоживанняТоварів page = new ВнутрішнєСпоживанняТоварів(true);

                page.DocumentPointerItem = Pointer;
                page.CallBack_OnSelectPointer = (ВнутрішнєСпоживанняТоварів_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;
                };

                page.LoadRecords();

                return page;
            });
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ВнутрішнєСпоживанняТоварів_Pointer();
        }
    }
}