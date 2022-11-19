
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Організації_PointerControl : PointerControl
    {
        public Організації_PointerControl()
        {
            pointer = new Організації_Pointer();
            WidthPresentation = 300;
            Caption = "Організація:";
        }

        Організації_Pointer pointer;
        public Організації_Pointer Pointer
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
            Організації page = new Організації(true);

            page.DirectoryPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (Організації_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Організації", () => { return page; });

            page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Організації_Pointer();
        }
    }
}