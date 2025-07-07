
/*     
        СтруктураПідприємства_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class СтруктураПідприємства_PointerControl : PointerControl
    {
        event EventHandler<СтруктураПідприємства_Pointer> PointerChanged;

        public СтруктураПідприємства_PointerControl()
        {
            pointer = new СтруктураПідприємства_Pointer();
            WidthPresentation = 300;
            Caption = $"{СтруктураПідприємства_Const.FULLNAME}:";
            PointerChanged += async (object? _, СтруктураПідприємства_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        СтруктураПідприємства_Pointer pointer;
        public СтруктураПідприємства_Pointer Pointer
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
            СтруктураПідприємства_ШвидкийВибір page = new СтруктураПідприємства_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new СтруктураПідприємства_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new СтруктураПідприємства_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
