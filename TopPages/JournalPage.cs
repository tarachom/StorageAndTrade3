using Gtk;
using System;
using System.IO;

class JournalPage : VBox
{
    private Notebook JournalNotebook;

    public JournalPage() : base()
    {
        JournalNotebook = new Notebook();
        JournalNotebook.TabPos = PositionType.Left;

        CreateTopNotebookPages(new string[] { "Повний", "Продажі", "Закупки", "Склад", "Каса" });

        PackStart(JournalNotebook, true, true, 0);



        ShowAll();
    }

    void CreateTopNotebookPages(string[] names)
    {
        foreach (string name in names)
        {
            ScrolledWindow scroll = new ScrolledWindow();
            //scroll.Expand = true;
            scroll.ShadowType = ShadowType.In;
            scroll.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            int numPage = JournalNotebook.AppendPage(scroll, new Label { Text = name, Expand = false });

            VBox vbox = new VBox(false, 5);
            scroll.Add(vbox);

            AddTst(vbox);
            AddTst(vbox);
            AddTst(vbox);
            AddTst(vbox);
            AddTst(vbox);
            AddTst(vbox);

        }
    }

    Label label;
    Entry entry;
    Button buttonOk;

    void AddTst(VBox vbox)
    {
        HBox hbox = new HBox(false, 0);
        hbox.Halign = Align.Start;

        label = new Label("Test");
        hbox.PackStart(label, false, false, 5);

        entry = new Entry();
        hbox.PackStart(entry, false, false, 5);

        buttonOk = new Button("OK");
        //buttonOk.Clicked += OnButtonOkClicked;
        hbox.PackStart(buttonOk, false, false, 5);

        LinkButton lb = new LinkButton("test");
        hbox.PackStart(lb, false, false, 5);

        vbox.PackStart(hbox, false, false, 5);
    }
}