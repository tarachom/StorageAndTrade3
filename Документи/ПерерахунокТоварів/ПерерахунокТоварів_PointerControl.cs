

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
        public ПерерахунокТоварів_PointerControl()
        {
            pointer = new ПерерахунокТоварів_Pointer();
            WidthPresentation = 300;
            Caption = $"{ПерерахунокТоварів_Const.FULLNAME}:";
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
                Presentation = pointer != null ? Task.Run(async () => { return await pointer.GetPresentation(); }).Result : "";
            }
        }

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            ПерерахунокТоварів page = new ПерерахунокТоварів
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new ПерерахунокТоварів_Pointer(selectPointer);
                }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ПерерахунокТоварів_Const.FULLNAME}", () => { return page; });

            page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ПерерахунокТоварів_Pointer();
        }
    }
}
