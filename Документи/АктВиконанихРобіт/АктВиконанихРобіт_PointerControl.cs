
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class АктВиконанихРобіт_PointerControl : PointerControl
    {
        public АктВиконанихРобіт_PointerControl()
        {
            pointer = new АктВиконанихРобіт_Pointer();
            WidthPresentation = 300;
            Caption = "Акт виконаних робіт:";
        }

        АктВиконанихРобіт_Pointer pointer;
        public АктВиконанихРобіт_Pointer Pointer
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
            АктВиконанихРобіт page = new АктВиконанихРобіт(true);

            page.DocumentPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (АктВиконанихРобіт_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Акт виконаних робіт", () => { return page; });

            page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new АктВиконанихРобіт_Pointer();
        }
    }
}