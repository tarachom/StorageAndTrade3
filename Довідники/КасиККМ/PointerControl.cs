
/*     
        КасиККМ_PointerControl.cs
        PointerControl
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    public class КасиККМ_PointerControl : PointerControl
    {
        event EventHandler<КасиККМ_Pointer> PointerChanged;

        public КасиККМ_PointerControl()
        {
            pointer = new КасиККМ_Pointer();
            WidthPresentation = 300;
            Caption = $"{КасиККМ_Const.FULLNAME}:";
            PointerChanged += async (_, pointer) => Presentation = pointer != null ? await pointer.GetPresentation() : "";
        }

        КасиККМ_Pointer pointer;
        public КасиККМ_Pointer Pointer
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
            КасиККМ_ШвидкийВибір page = new КасиККМ_ШвидкийВибір() 
            { 
                PopoverParent = popover, 
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = selectPointer =>
                {
                    Pointer = new КасиККМ_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new КасиККМ_Pointer();
            AfterSelectFunc?.Invoke();
            AfterClearFunc?.Invoke();
        }
    }
}
    