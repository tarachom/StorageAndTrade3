
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ВидиЦін_PointerControl : PointerControl
    {
        public ВидиЦін_PointerControl()
        {
            pointer = new ВидиЦін_Pointer();
            WidthPresentation = 300;
            Caption = "Види цін:";
        }

        ВидиЦін_Pointer pointer;
        public ВидиЦін_Pointer Pointer
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
            ВидиЦін page = new ВидиЦін(true);

            page.DirectoryPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (ВидиЦін_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Види цін", () => { return page; });

            page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ВидиЦін_Pointer();
        }
    }
}