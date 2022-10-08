using Gtk;
using System;
using System.IO;

using AccountingSoftware;

namespace StorageAndTrade
{
    class JournalPage : VBox
    {
        #region Notebook

        private Notebook JournalNotebook;
        private Dictionary<int, NotebookPage> NotebookPagesDictionary;
        private Dictionary<int, NameValue<System.Action<NotebookPage>>> PageAndActionDictionary;

        #endregion

        public JournalPage() : base()
        {
            JournalNotebook = new Notebook();
            JournalNotebook.TabPos = PositionType.Top;

            NotebookPagesDictionary = new Dictionary<int, NotebookPage>();
            PageAndActionDictionary = new Dictionary<int, NameValue<Action<NotebookPage>>>();

            int counter = 0;

            PageAndActionDictionary.Add(counter, new NameValue<Action<NotebookPage>>("Головна", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Повний", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Продажі", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Покупки", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Склад", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Фінанси", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            CreateTopNotebookPages();

            PackStart(JournalNotebook, true, true, 0);

            ShowAll();

            JournalNotebook.SwitchPage += OnTopNotebookSelectPage;
            OnTopNotebookSelectPage(null, new SwitchPageArgs());
        }

        void CreateTopNotebookPages()
        {
            foreach (NameValue<Action<NotebookPage>> page in PageAndActionDictionary.Values)
            {
                ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In };
                scroll.SetPolicy(PolicyType.Never, PolicyType.Automatic);

                int numPage = JournalNotebook.AppendPage(scroll, new Label { Text = page.Name, Expand = false, Halign = Align.End });

                NotebookPagesDictionary.Add(numPage,
                    new NotebookPage
                    {
                        NumPage = numPage,
                        NamePage = page.Name,
                        ScrolledWindow = scroll
                    });
            }
        }

        void OnTopNotebookSelectPage(object? sender, SwitchPageArgs args)
        {
            NotebookPage notebookPage = NotebookPagesDictionary[JournalNotebook.CurrentPage];

            if (!notebookPage.IsConstruct)
                PageAndActionDictionary[JournalNotebook.CurrentPage]?.Value?.Invoke(notebookPage);
        }
    }
}
