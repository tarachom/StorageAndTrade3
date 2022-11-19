
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ПартіяТоварівКомпозит_PointerControl : PointerControl
    {
        public ПартіяТоварівКомпозит_PointerControl()
        {
            pointer = new ПартіяТоварівКомпозит_Pointer();
            WidthPresentation = 300;
            Caption = "Партія:";
        }

        ПартіяТоварівКомпозит_Pointer pointer;
        public ПартіяТоварівКомпозит_Pointer Pointer
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
            ПартіяТоварівКомпозит page = new ПартіяТоварівКомпозит(true);

            page.DirectoryPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (ПартіяТоварівКомпозит_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Партія", () => { return page; });

            page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ПартіяТоварівКомпозит_Pointer();
        }
    }
}