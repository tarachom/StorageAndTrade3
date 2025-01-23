

/*     
        Номенклатура_MultiplePointerControl.cs
        MultiplePointerControl
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    public class Номенклатура_MultiplePointerControl : MultiplePointerControl
    {
        event EventHandler<Номенклатура_Pointer> PointerChanged;

        public Номенклатура_MultiplePointerControl()
        {
            pointer = new Номенклатура_Pointer();
            WidthPresentation = 300;
            Caption = $"{Номенклатура_Const.FULLNAME}:";
            PointerChanged += async (object? _, Номенклатура_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
                if (pointers.Count > 1) Presentation += $" ... {pointers.Count}";
            };
        }

        Номенклатура_Pointer pointer;
        List<Номенклатура_Pointer> pointers = [];
        public Номенклатура_Pointer Pointer
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

        public Номенклатура_Pointer[] GetPointers()
        {
            Номенклатура_Pointer[] copy = new Номенклатура_Pointer[pointers.Count];
            pointers.CopyTo(copy);

            return copy;
        }

        void Add(Номенклатура_Pointer item)
        {
            if (!pointers.Exists((Номенклатура_Pointer x) => x.UnigueID.ToString() == item.UnigueID.ToString()))
                pointers.Add(item);

            Pointer = item;
            //AfterSelectFunc?.Invoke();
        }



        protected override async void OpenSelect(object? sender, EventArgs args)
        {
            Popover popover = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };
            BeforeClickOpenFunc?.Invoke();
            Номенклатура_ШвидкийВибір page = new Номенклатура_ШвидкийВибір
            {
                PopoverParent = popover,
                DirectoryPointerItem = pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Add(new Номенклатура_Pointer(selectPointer));
                },
                CallBack_OnMultipleSelectPointer = (UnigueID[] selectPointers) =>
                {
                    foreach (var selectPointer in selectPointers)
                        Add(new Номенклатура_Pointer(selectPointer));
                }
            };

            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override async ValueTask FillList(ListBox listBox)
        {
            foreach (Номенклатура_Pointer item in pointers)
            {
                Box hBox = new Box(Orientation.Horizontal, 0);
                ListBoxRow listBoxRow = [hBox];

                LinkButton linkName = new LinkButton("", await item.GetPresentation()) { Halign = Align.Start, Image = new Image(InterfaceGtk.Іконки.ДляКнопок.Doc), AlwaysShowImage = true };
                linkName.Clicked += (object? sender, EventArgs args) =>
                {
                    if (Pointer.UnigueID.ToString() != item.UnigueID.ToString())
                        Pointer = item;
                };

                hBox.PackStart(linkName, true, true, 0);

                //Remove
                LinkButton linkRemove = new LinkButton("") { Halign = Align.Start, Image = new Image(InterfaceGtk.Іконки.ДляКнопок.Clean), AlwaysShowImage = true };
                linkRemove.Clicked += (object? sender, EventArgs args) =>
                {
                    pointers.Remove(item);
                    listBox.Remove(listBoxRow);

                    if (Pointer.UnigueID.ToString() == item.UnigueID.ToString())
                        Pointer = pointers.Count > 0 ? pointers[0] : new Номенклатура_Pointer();
                    else
                        PointerChanged?.Invoke(null, pointer);
                };

                hBox.PackEnd(linkRemove, false, false, 0);

                listBox.Add(listBoxRow);
                listBox.ShowAll();
            }
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            pointers = [];
            Pointer = new Номенклатура_Pointer();
            AfterSelectFunc?.Invoke();
            AfterClearFunc?.Invoke();
        }
    }
}
