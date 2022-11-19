
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Виробники_PointerControl : PointerControl
    {
        public Виробники_PointerControl()
        {
            pointer = new Виробники_Pointer();
            WidthPresentation = 300;
            Caption = "Виробник:";
        }

        Виробники_Pointer pointer;
        public Виробники_Pointer Pointer
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
            Виробники page = new Виробники(true);

            page.DirectoryPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (Виробники_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Виробники", () => { return page; });

            page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Виробники_Pointer();
        }
    }
}