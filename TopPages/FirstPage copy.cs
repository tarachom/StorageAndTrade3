using Gtk;
using System;
using System.IO;

class FirstPageC : VBox
{
    Label label;
    Entry entry;

    Button buttonOk;

    public FirstPageC() : base()
    {
        new VBox(false, 0);
        BorderWidth = 10;
        Halign = Align.Start;

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

        PackStart(hbox, false, false, 0);



        ShowAll();
    }

    void OnButtonOkClicked(object? sender, EventArgs args)
    {
        entry.Text = "OK";
    }
}