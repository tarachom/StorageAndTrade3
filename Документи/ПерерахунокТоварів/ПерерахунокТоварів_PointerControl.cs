

/*     
        ПерерахунокТоварів_PointerControl.cs
        PointerControl
*/

using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ПерерахунокТоварів_PointerControl : PointerControl
    {
        event EventHandler<ПерерахунокТоварів_Pointer> PointerChanged;

        public ПерерахунокТоварів_PointerControl()
        {
            pointer = new ПерерахунокТоварів_Pointer();
            WidthPresentation = 300;
            Caption = $"{ПерерахунокТоварів_Const.FULLNAME}:";
            PointerChanged += async (object? _, ПерерахунокТоварів_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ПерерахунокТоварів_Pointer pointer;
        public ПерерахунокТоварів_Pointer Pointer
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
            ПерерахунокТоварів page = new ПерерахунокТоварів
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new ПерерахунокТоварів_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ПерерахунокТоварів_Const.FULLNAME}", () => page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ПерерахунокТоварів_Pointer();
        }
    }
}
