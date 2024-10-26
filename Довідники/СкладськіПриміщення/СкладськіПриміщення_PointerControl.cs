/*

        СкладськіПриміщення_PointerControl.cs
        PointerControl

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    public class СкладськіПриміщення_PointerControl : PointerControl
    {
        event EventHandler<СкладськіПриміщення_Pointer> PointerChanged;

        public СкладськіПриміщення_PointerControl()
        {
            pointer = new СкладськіПриміщення_Pointer();
            WidthPresentation = 300;
            Caption = $"{СкладськіПриміщення_Const.FULLNAME}:";
            PointerChanged += async (object? _, СкладськіПриміщення_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        СкладськіПриміщення_Pointer pointer;
        public СкладськіПриміщення_Pointer Pointer
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

        public Склади_Pointer СкладВласник = new Склади_Pointer();

        protected override async void OpenSelect(object? sender, EventArgs args)
        {
            Popover popover = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };

            BeforeClickOpenFunc?.Invoke();

            СкладськіПриміщення_ШвидкийВибір page = new СкладськіПриміщення_ШвидкийВибір
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new СкладськіПриміщення_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };

            page.СкладВласник.Pointer = СкладВласник;

            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new СкладськіПриміщення_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}