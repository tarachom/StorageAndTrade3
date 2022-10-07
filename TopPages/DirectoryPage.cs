using Gtk;
using System;
using System.IO;

class DirectoryPage : VBox
{
    Label label;
    Entry entry;

    Button buttonOk;

    public DirectoryPage() : base()
    {
        new VBox(false, 0);
        BorderWidth = 10;
        Halign = Align.Start;

        AddTst();
        AddTst();
        AddTst();
        AddTst();
        AddTst();

        ShowAll();
    }

    void AddTst()
    {
        HBox hbox = new HBox(false, 0);
        hbox.Halign = Align.Start;

        label = new Label("Test");
        hbox.PackStart(label, false, false, 5);

        entry = new Entry();
        hbox.PackStart(entry, false, false, 5);

        buttonOk = new Button("OK");
        buttonOk.Clicked += OnButtonOkClicked;
        hbox.PackStart(buttonOk, false, false, 5);

        LinkButton lb = new LinkButton("test");
        hbox.PackStart(lb, false, false, 5);

        PackStart(hbox, false, false, 5);
    }

    void OnButtonOkClicked(object? sender, EventArgs args)
    {
        entry.Text = "OK dir";
    }
}