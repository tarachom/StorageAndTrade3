
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Валюти_PointerControl : PointerControl
    {
        public Валюти_PointerControl()
        {
            pointer = new Валюти_Pointer();
            WidthPresentation = 300;
            Caption = "Валюта:";
        }

        Валюти_Pointer pointer;
        public Валюти_Pointer Pointer
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
            Валюти page = new Валюти(true);

            page.DirectoryPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (Валюти_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Валюти", () => { return page; });

            page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Валюти_Pointer();
        }
    }
}