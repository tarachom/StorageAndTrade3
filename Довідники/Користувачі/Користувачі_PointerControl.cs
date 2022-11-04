
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Користувачі_PointerControl : PointerControl
    {
        public Користувачі_PointerControl()
        {
            pointer = new Користувачі_Pointer();
            WidthPresentation = 300;
            Caption = "Користувач:";
        }

        Користувачі_Pointer pointer;
        public Користувачі_Pointer Pointer
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
            Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Користувачі", () =>
            {
                Користувачі page = new Користувачі();

                page.DirectoryPointerItem = Pointer;
                page.CallBack_OnSelectPointer = (Користувачі_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;
                };

                page.LoadRecords();

                return page;
            });
        }
    }
}