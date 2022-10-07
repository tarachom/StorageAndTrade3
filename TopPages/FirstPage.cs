using Gtk;
using System;
using System.IO;

class FirstPage : VBox
{
    Label label;
    Entry entry;

    Button buttonOk;

    public FirstPage() : base()
    {
        new VBox(false, 0);
        BorderWidth = 10;
        Halign = Align.Start;

        HBox hbox = new HBox(false, 0);
        hbox.Halign = Align.Start;

        label = new Label("Test");
        hbox.PackStart(label, false, false, 0);

        entry = new Entry();
        hbox.PackStart(entry, false, false, 0);

        buttonOk = new Button("OK");
        buttonOk.Clicked += OnButtonOkClicked;
        hbox.PackStart(buttonOk, false, false, 0);

        PackStart(hbox, false, false, 0);
        ShowAll();
    }

    void OnButtonOkClicked(object? sender, EventArgs args)
    {
        entry.Text = "OK";
    }
}