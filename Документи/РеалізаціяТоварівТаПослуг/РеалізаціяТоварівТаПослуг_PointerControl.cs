
/*

        РеалізаціяТоварівТаПослуг_PointerControl.cs
        PointerControl

*/

using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode.Документи;

namespace StorageAndTrade
{
    class РеалізаціяТоварівТаПослуг_PointerControl : PointerControl
    {
        event EventHandler<РеалізаціяТоварівТаПослуг_Pointer> PointerChanged;

        public РеалізаціяТоварівТаПослуг_PointerControl()
        {
            pointer = new РеалізаціяТоварівТаПослуг_Pointer();
            WidthPresentation = 300;
            Caption = $"{РеалізаціяТоварівТаПослуг_Const.FULLNAME}:";
            PointerChanged += async (object? _, РеалізаціяТоварівТаПослуг_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        РеалізаціяТоварівТаПослуг_Pointer pointer;
        public РеалізаціяТоварівТаПослуг_Pointer Pointer
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
            РеалізаціяТоварівТаПослуг page = new РеалізаціяТоварівТаПослуг
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new РеалізаціяТоварівТаПослуг_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {РеалізаціяТоварівТаПослуг_Const.FULLNAME}", () => page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new РеалізаціяТоварівТаПослуг_Pointer();
        }
    }
}