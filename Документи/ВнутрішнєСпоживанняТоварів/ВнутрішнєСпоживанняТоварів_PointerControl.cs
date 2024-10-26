
/*

        ВнутрішнєСпоживанняТоварів_PointerControl.cs
        PointerControl

*/

using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ВнутрішнєСпоживанняТоварів_PointerControl : PointerControl
    {
        event EventHandler<ВнутрішнєСпоживанняТоварів_Pointer> PointerChanged;

        public ВнутрішнєСпоживанняТоварів_PointerControl()
        {
            pointer = new ВнутрішнєСпоживанняТоварів_Pointer();
            WidthPresentation = 300;
            Caption = $"{ВнутрішнєСпоживанняТоварів_Const.FULLNAME}:";
            PointerChanged += async (object? _, ВнутрішнєСпоживанняТоварів_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ВнутрішнєСпоживанняТоварів_Pointer pointer;
        public ВнутрішнєСпоживанняТоварів_Pointer Pointer
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
            ВнутрішнєСпоживанняТоварів page = new ВнутрішнєСпоживанняТоварів
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new ВнутрішнєСпоживанняТоварів_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ВнутрішнєСпоживанняТоварів_Const.FULLNAME}", () => page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ВнутрішнєСпоживанняТоварів_Pointer();
        }
    }
}