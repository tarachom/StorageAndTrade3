
/*

        ВстановленняЦінНоменклатури_PointerControl.cs
        PointerContro

*/

using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ВстановленняЦінНоменклатури_PointerControl : PointerControl
    {
        event EventHandler<ВстановленняЦінНоменклатури_Pointer> PointerChanged;

        public ВстановленняЦінНоменклатури_PointerControl()
        {
            pointer = new ВстановленняЦінНоменклатури_Pointer();
            WidthPresentation = 300;
            Caption = $"{ВстановленняЦінНоменклатури_Const.FULLNAME}:";
            PointerChanged += async (object? _, ВстановленняЦінНоменклатури_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ВстановленняЦінНоменклатури_Pointer pointer;
        public ВстановленняЦінНоменклатури_Pointer Pointer
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
            ВстановленняЦінНоменклатури page = new ВстановленняЦінНоменклатури
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new ВстановленняЦінНоменклатури_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ВстановленняЦінНоменклатури_Const.FULLNAME}", () => page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ВстановленняЦінНоменклатури_Pointer();
        }
    }
}