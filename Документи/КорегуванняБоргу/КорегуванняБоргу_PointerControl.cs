
/*

        КорегуванняБоргу_PointerControl.cs
        PointerControl

*/

using InterfaceGtk;
using AccountingSoftware;

using GeneratedCode.Документи;

namespace StorageAndTrade
{
    class КорегуванняБоргу_PointerControl : PointerControl
    {
        event EventHandler<КорегуванняБоргу_Pointer> PointerChanged;

        public КорегуванняБоргу_PointerControl()
        {
            pointer = new КорегуванняБоргу_Pointer();
            WidthPresentation = 300;
            Caption = $"{КорегуванняБоргу_Const.FULLNAME}:";
            PointerChanged += async (object? _, КорегуванняБоргу_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        КорегуванняБоргу_Pointer pointer;
        public КорегуванняБоргу_Pointer Pointer
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
            КорегуванняБоргу page = new КорегуванняБоргу
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new КорегуванняБоргу_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {КорегуванняБоргу_Const.FULLNAME}", () => page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new КорегуванняБоргу_Pointer();
        }
    }
}