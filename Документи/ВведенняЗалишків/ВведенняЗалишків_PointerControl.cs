
/*

        ВведенняЗалишків_PointerControl.cs
        PointerControl

*/

using InterfaceGtk;
using AccountingSoftware;

using GeneratedCode.Документи;

namespace StorageAndTrade
{
    class ВведенняЗалишків_PointerControl : PointerControl
    {
        event EventHandler<ВведенняЗалишків_Pointer> PointerChanged;

        public ВведенняЗалишків_PointerControl()
        {
            pointer = new ВведенняЗалишків_Pointer();
            WidthPresentation = 300;
            Caption = $"{ВведенняЗалишків_Const.FULLNAME}:";
            PointerChanged += async (object? _, ВведенняЗалишків_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ВведенняЗалишків_Pointer pointer;
        public ВведенняЗалишків_Pointer Pointer
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
            ВведенняЗалишків page = new ВведенняЗалишків
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new ВведенняЗалишків_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ВведенняЗалишків_Const.FULLNAME}", () => page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ВведенняЗалишків_Pointer();
        }
    }
}