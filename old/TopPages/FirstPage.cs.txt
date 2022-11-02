using Gtk;
using System;
using System.IO;

namespace StorageAndTrade
{
    class FirstPage : VBox
    {
        ListStore store;

        enum Column
        {
            Icon,
            Fixed,
            Name,
            Place,
            Year
        }

        public class Actress
        {
            public string Icon;

            public string Name;
            public bool Fixed;
            public string Place;
            public int Year;

            public Actress(string filename, bool fix, string name, string place, int year)
            {
                Icon = filename;
                Fixed = fix;
                Name = name;
                Place = place;
                Year = year;
            }
        }

        Actress[] actresses =
        {
        new Actress( "doc_text_image.png", true, "Jessica Alba", "Pomona", 1981),
        new Actress("doc_text_image.png",true,"Sigourney Weaver", "New York", 1949),
        new Actress("doc_text_image.png",true,"Angelina Jolie", "Los Angeles", 1975),
        new Actress("doc_text_image.png",true,"Natalie Portman", "Jerusalem", 1981),
        new Actress("doc_text_image.png",true,"Rachel Weissz", "London", 1971),
        new Actress("doc_text_image.png",true,"Scarlett Johansson", "New York", 1984),
        new Actress("doc_text_image.png",true,"Scarlett Johansson", "New York", 1984)
    };

        public FirstPage() : base()
        {
            new VBox(false, 0);
            BorderWidth = 10;

            store = CreateModel();

            TreeView treeView = new TreeView(store);
            PackStart(treeView, true, true, 5);

            AddColumns(treeView);

            ShowAll();
        }

        void AddColumns(TreeView treeView)
        {
            CellRendererPixbuf renderer = new CellRendererPixbuf();
            TreeViewColumn column = new TreeViewColumn("", renderer, "pixbuf", Column.Icon);
            //column.SortColumnId = (int)Column.Icon;AppendColumn ("Icon", new Gtk.CellRendererPixbuf (), "pixbuf", 0);
            treeView.AppendColumn(column);

            CellRendererToggle rendererToggle = new CellRendererToggle();
            //rendererToggle.Toggled += RendererToggle_Toggled;
            column = new TreeViewColumn("Проведений", rendererToggle, "active", Column.Fixed);
            //column.SortColumnId = (int)Column.Fixed;
            treeView.AppendColumn(column);

            CellRendererText rendererText = new CellRendererText();
            column = new TreeViewColumn("Назва", rendererText, "text", Column.Name);
            column.FixedWidth = 200;
            //column.SortColumnId = (int)Column.Name;
            treeView.AppendColumn(column);

            rendererText = new CellRendererText();
            column = new TreeViewColumn("Place", rendererText, "text", Column.Place);
            column.FixedWidth = 300;
            //column.SortColumnId = (int)Column.Place;
            treeView.AppendColumn(column);

            rendererText = new CellRendererText();
            column = new TreeViewColumn("Рік", rendererText, "text", Column.Year);
            //column.SortColumnId = (int)Column.Year;
            treeView.AppendColumn(column);


        }

        ListStore CreateModel()
        {
            ListStore store = new ListStore(typeof(Gdk.Pixbuf), typeof(bool), typeof(string), typeof(string), typeof(int));

            foreach (Actress act in actresses)
            {
                TreeIter iter = store.AppendValues(new Gdk.Pixbuf(act.Icon), act.Fixed, act.Name, act.Place, act.Year);
                //store.SetValue(iter, (int)Column.Fixed, true);
            }

            return store;
        }




    }
}