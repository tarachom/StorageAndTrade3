/*

        Каси_PointerControl.cs
        PointerControl

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Каси_MultiplePointerControl : MultiplePointerControl
    {
        event EventHandler PointersChanged;

        public Каси_MultiplePointerControl()
        {
            WidthPresentation = 300;
            Caption = $"{Каси_Const.FULLNAME}:";
            PointersChanged += (object? sender, EventArgs args) =>
            {
                Presentation = pointers.Count + " елементів";
            };

            PointersChanged.Invoke(null, new());
        }

        List<Каси_Pointer> pointers = [];

        public List<Каси_Pointer> Pointers
        {
            get
            {
                return pointers;
            }
        }

        void Add(Каси_Pointer Каса)
        {
            pointers.Add(Каса);
            PointersChanged?.Invoke(null, new());
        }

        protected override async void OpenSelect(object? sender, EventArgs args)
        {
            Popover popover = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };

            Каси_ШвидкийВибір page = new Каси_ШвидкийВибір
            {
                PopoverParent = popover,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Add(new Каси_Pointer(selectPointer));
                },
                CallBack_OnMultipleSelectPointer = (UnigueID[] selectPointers) =>
                {
                    foreach (var selectPointer in selectPointers)
                        Add(new Каси_Pointer(selectPointer));
                }
            };

            if (pointers.Count != 0)
                page.DirectoryPointerItem = pointers[^1].UnigueID;

            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        public async ValueTask Додати(ListBox listBox, Каси_Pointer Каса)
        {
            Box hBox = new Box(Orientation.Horizontal, 0);

            ListBoxRow listBoxRow = [hBox];
            listBox.Add(listBoxRow);

            //Presentation
            hBox.PackStart(new Label(await Каса.GetPresentation()) { Halign = Align.Start }, true, true, 0);

            //Remove
            LinkButton linkRemove = new LinkButton("") { Halign = Align.Start, Image = new Image(InterfaceGtk.Іконки.ДляКнопок.Clean), AlwaysShowImage = true };
            linkRemove.Clicked += (object? sender, EventArgs args) =>
            {
                pointers.Remove(Каса);
                listBox.Remove(listBoxRow);

                PointersChanged.Invoke(null, new());
            };

            hBox.PackEnd(linkRemove, false, false, 0);
        }

        protected override async void OnMultiple(object? sender, EventArgs args)
        {
            Popover popover = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 4 };

            ListBox listBox = new() { SelectionMode = SelectionMode.None };

            foreach (Каси_Pointer Каса in Pointers)
                await Додати(listBox, Каса);

            ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In, HeightRequest = 300, WidthRequest = 500 };
            scroll.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scroll.Add(listBox);

            popover.Add(scroll);
            popover.ShowAll();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            pointers = [];
            PointersChanged?.Invoke(null, new());
        }
    }
}