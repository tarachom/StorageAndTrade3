
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Каси_PointerControl : PointerControl
    {
        public Каси_PointerControl()
        {
            pointer = new Каси_Pointer();
            WidthPresentation = 300;
            Caption = "Каса:";
        }

        Каси_Pointer pointer;
        public Каси_Pointer Pointer
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
            Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Каси", () =>
            {
                Каси page = new Каси(true);

                page.DirectoryPointerItem = Pointer;
                page.CallBack_OnSelectPointer = (Каси_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;
                };

                page.LoadRecords();

                return page;
            });
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Каси_Pointer();
        }
    }
}