using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Організації_PointerControl : PointerControl
    {
        public Організації_PointerControl()
        {
            pointer = new Організації_Pointer();
            WidthPresentation = 300;
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
            Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Організації", () =>
            {
                Організації page = new Організації();

                page.DirectoryPointerItem = Pointer;
                page.CallBack_OnSelectPointer = (Організації_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;
                };

                page.LoadRecords();

                return page;
            });
        }
    }
}