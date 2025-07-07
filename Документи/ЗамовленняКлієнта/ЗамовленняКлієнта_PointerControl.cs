
/*

        ЗамовленняКлієнта_PointerControl.cs
        PointerControl

*/

using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode.Документи;

namespace StorageAndTrade
{
    class ЗамовленняКлієнта_PointerControl : PointerControl
    {
        event EventHandler<ЗамовленняКлієнта_Pointer> PointerChanged;

        public ЗамовленняКлієнта_PointerControl()
        {
            pointer = new ЗамовленняКлієнта_Pointer();
            WidthPresentation = 300;
            Caption = $"{ЗамовленняКлієнта_Const.FULLNAME}:";
            PointerChanged += async (object? _, ЗамовленняКлієнта_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ЗамовленняКлієнта_Pointer pointer;
        public ЗамовленняКлієнта_Pointer Pointer
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
            ЗамовленняКлієнта page = new ЗамовленняКлієнта
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new ЗамовленняКлієнта_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ЗамовленняКлієнта_Const.FULLNAME}", () =>  page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ЗамовленняКлієнта_Pointer();
        }
    }
}