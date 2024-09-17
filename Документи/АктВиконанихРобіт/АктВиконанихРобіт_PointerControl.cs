

/*     
        АктВиконанихРобіт_PointerControl.cs
        PointerControl
*/
using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class АктВиконанихРобіт_PointerControl : PointerControl
    {
        event EventHandler<АктВиконанихРобіт_Pointer> PointerChanged;

        public АктВиконанихРобіт_PointerControl()
        {
            pointer = new АктВиконанихРобіт_Pointer();
            WidthPresentation = 300;
            Caption = $"{АктВиконанихРобіт_Const.FULLNAME}:";
            PointerChanged += async (object? _, АктВиконанихРобіт_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
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
                PointerChanged?.Invoke(null, pointer);
            }
        }

        protected override async void OpenSelect(object? sender, EventArgs args)
        {
            BeforeClickOpenFunc?.Invoke();
            АктВиконанихРобіт page = new АктВиконанихРобіт
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new АктВиконанихРобіт_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {АктВиконанихРобіт_Const.FULLNAME}", () => page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new АктВиконанихРобіт_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
