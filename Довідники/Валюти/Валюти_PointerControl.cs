
/*     
        Валюти_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class Валюти_PointerControl : PointerControl
    {
        event EventHandler<Валюти_Pointer> PointerChanged;

        public Валюти_PointerControl()
        {
            pointer = new Валюти_Pointer();
            WidthPresentation = 300;
            Caption = $"{Валюти_Const.FULLNAME}:";
            PointerChanged += async (object? _, Валюти_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        Валюти_Pointer pointer;
        public Валюти_Pointer Pointer
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
            Popover popover = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };
            BeforeClickOpenFunc?.Invoke();
            Валюти_ШвидкийВибір page = new Валюти_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new Валюти_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Валюти_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
