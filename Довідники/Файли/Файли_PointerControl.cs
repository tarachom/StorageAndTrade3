
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Файли_PointerControl : PointerControl
    {
        public Файли_PointerControl()
        {
            pointer = new Файли_Pointer();
            WidthPresentation = 300;
            Caption = "Файл:";
        }

        Файли_Pointer pointer;
        public Файли_Pointer Pointer
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
            Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Файли", () =>
            {
                Файли page = new Файли(true);

                page.DirectoryPointerItem = Pointer;
                page.CallBack_OnSelectPointer = (Файли_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;
                };

                page.LoadRecords();

                return page;
            });
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Файли_Pointer();
        }
    }
}