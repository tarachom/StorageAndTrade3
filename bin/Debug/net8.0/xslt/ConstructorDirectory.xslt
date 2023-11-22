<?xml version="1.0" encoding="utf-8"?>
<!--
/*
Copyright (C) 2019-2023 TARAKHOMYN YURIY IVANOVYCH
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
Автор:    Тарахомин Юрій Іванович
Адреса:   Україна, м. Львів
Сайт:     accounting.org.ua
*/
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="text" indent="yes" />

    <!-- Файл -->
    <xsl:param name="File" />

    <xsl:template match="root">

        <xsl:choose>
            <xsl:when test="$File = 'Element'">
                <xsl:call-template name="DirectoryElement" />
            </xsl:when>
            <xsl:when test="$File = 'List'">
                <xsl:call-template name="DirectoryList" />
            </xsl:when>
            <xsl:when test="$File = 'ListSmallSelect'">
                <xsl:call-template name="DirectoryListSmallSelect" />
            </xsl:when>
            <xsl:when test="$File = 'ListAndTree'">
                <xsl:call-template name="DirectoryListAndTree" />
            </xsl:when>
            <xsl:when test="$File = 'PointerControl'">
                <xsl:call-template name="DirectoryPointerControl" />
            </xsl:when>

            <xsl:when test="$File = 'ElementTree'">
                <xsl:call-template name="DirectoryElement">
                    <xsl:with-param name="IsTree">1</xsl:with-param>
                </xsl:call-template>
            </xsl:when>
            <xsl:when test="$File = 'Tree'">
                <xsl:call-template name="DirectoryTree" />
            </xsl:when>
                <xsl:when test="$File = 'TreeSmallSelect'">
                <xsl:call-template name="DirectoryTreeSmallSelect" />
            </xsl:when>
            <xsl:when test="$File = 'PointerControlTree'">
                <xsl:call-template name="DirectoryPointerControlTree" />
            </xsl:when>
        </xsl:choose>

    </xsl:template>

<!--- 
//
// ============================ Елемент ============================
//
-->

    <!-- Елемент -->
    <xsl:template name="DirectoryElement">
        <xsl:param name="IsTree" />
        <xsl:variable name="DirectoryName" select="Directory/Name"/>
        <xsl:variable name="Fields" select="Directory/Fields/Field"/>
        <xsl:variable name="TabularParts" select="Directory/TabularParts/TablePart"/>

/*
        <xsl:value-of select="$DirectoryName"/>_Елемент.cs
        Елемент
*/

using Gtk;

using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class <xsl:value-of select="$DirectoryName"/>_Елемент : ДовідникЕлемент
    {
        public <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>_Objest { get; set; } = new <xsl:value-of select="$DirectoryName"/>_Objest();
        <xsl:if test="$IsTree = '1'">
            public <xsl:value-of select="$DirectoryName"/>_Pointer РодичДляНового { get; set; } = new <xsl:value-of select="$DirectoryName"/>_Pointer();
        </xsl:if>

        #region Fields
        <xsl:for-each select="$Fields">
            <xsl:choose>
                <xsl:when test="Type = 'string'">
                     <xsl:choose>
                        <xsl:when test="Multiline = '1'">
                    TextView <xsl:value-of select="Name"/> = new TextView();
                        </xsl:when>
                        <xsl:otherwise>
                    Entry <xsl:value-of select="Name"/> = new Entry() { WidthRequest = 500 };
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:when>
                <xsl:when test="Type = 'integer'">
                    IntegerControl <xsl:value-of select="Name"/> = new IntegerControl();
                </xsl:when>
                <xsl:when test="Type = 'numeric'">
                    NumericControl <xsl:value-of select="Name"/> = new NumericControl();
                </xsl:when>
                <xsl:when test="Type = 'boolean'">
                    CheckButton <xsl:value-of select="Name"/> = new CheckButton("<xsl:value-of select="Name"/>");
                </xsl:when>
                <xsl:when test="Type = 'date' or Type = 'datetime'">
                    DateTimeControl <xsl:value-of select="Name"/> = new DateTimeControl()<xsl:if test="Type = 'date'">{ OnlyDate = true }</xsl:if>;
                </xsl:when>
                <xsl:when test="Type = 'time'">
                    TimeControl <xsl:value-of select="Name"/> = new TimeControl();
                </xsl:when>
                <xsl:when test="Type = 'composite_pointer'">
                    CompositePointerControl <xsl:value-of select="Name"/> = new CompositePointerControl();
                </xsl:when>
                <xsl:when test="Type = 'pointer'">
                    <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                    <xsl:value-of select="$namePointer"/>_PointerControl <xsl:value-of select="Name"/> = new <xsl:value-of select="$namePointer"/>_PointerControl() { Caption = "<xsl:value-of select="Name"/>", WidthPresentation = 300 };
                </xsl:when>
                <xsl:when test="Type = 'enum'">
                    <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                    ComboBoxText <xsl:value-of select="Name"/> = new ComboBoxText();
                </xsl:when>
                <xsl:when test="Type = 'any_pointer'">
                    //Guid <xsl:value-of select="Name"/> = new Guid();
                </xsl:when>
                <xsl:when test="Type = 'bytea'">
                    //byte[] <xsl:value-of select="Name"/> = new byte[]{ };
                </xsl:when>
                <xsl:when test="Type = 'string[]'">
                    //string[] <xsl:value-of select="Name"/> = new string[]{ };
                </xsl:when>
                <xsl:when test="Type = 'integer'">
                    //int[] <xsl:value-of select="Name"/> = new int[]{ };
                </xsl:when>
                <xsl:when test="Type = 'numeric'">
                    //decimal[] <xsl:value-of select="Name"/> = new decimal[]{ };
                </xsl:when>
            </xsl:choose>
        </xsl:for-each>
        #endregion

        #region TabularParts
        <xsl:for-each select="$TabularParts">
            <xsl:value-of select="$DirectoryName"/>_ТабличнаЧастина_<xsl:value-of select="Name"/><xsl:text> </xsl:text>
            <xsl:value-of select="Name"/> = new <xsl:value-of select="$DirectoryName"/>_ТабличнаЧастина_<xsl:value-of select="Name"/>();
        </xsl:for-each>
        #endregion

        public <xsl:value-of select="$DirectoryName"/>_Елемент() : base() 
        { 
            <xsl:for-each select="$Fields">
                <xsl:if test="Type = 'enum'">
                    <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                    foreach (var field in ПсевдонімиПерелічення.<xsl:value-of select="$namePointer"/>_List())
                        <xsl:value-of select="Name"/>.Append(field.Value.ToString(), field.Name);
                </xsl:if>
            </xsl:for-each>
        }

        protected override void CreatePack1(VBox vBox)
        {
            <xsl:for-each select="$Fields">
                //<xsl:value-of select="Name"/>
                <xsl:choose>
                    <xsl:when test="Type = 'string' or Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                        <xsl:choose>
                            <xsl:when test="Type = 'string' and Multiline = '1'">
                CreateFieldView(vBox, "<xsl:value-of select="Name"/>:", <xsl:value-of select="Name"/>, 500, 200);
                            </xsl:when>
                            <xsl:otherwise>
                CreateField(vBox, "<xsl:value-of select="Name"/>:", <xsl:value-of select="Name"/>);
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:when test="Type = 'composite_pointer' or Type = 'boolean'">
                CreateField(vBox, null, <xsl:value-of select="Name"/>);
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">

                CreateField(vBox, null, <xsl:value-of select="Name"/>);
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                CreateField(vBox, "<xsl:value-of select="Name"/>:", <xsl:value-of select="Name"/>);
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>
        }

        protected override void CreatePack2(VBox vBox)
        {
            <xsl:for-each select="$TabularParts">
                CreateTablePart(vBox, "<xsl:value-of select="Name"/>:", <xsl:value-of select="Name"/>);
            </xsl:for-each>
        }

        #region Присвоєння / зчитування значень

        public override async void SetValue()
        {            
            <xsl:choose>
                <xsl:when test="$IsTree = '1'">
                    if (IsNew)
                    {
                        await <xsl:value-of select="$DirectoryName"/>_Objest.New();
                        <xsl:value-of select="$DirectoryName"/>_Objest.Родич = РодичДляНового;
                    }
                    else
                        Родич.OpenFolder = <xsl:value-of select="$DirectoryName"/>_Objest.UnigueID;
                </xsl:when>
                <xsl:otherwise>
                    if (IsNew)
                        <xsl:value-of select="$DirectoryName"/>_Objest.New();
                </xsl:otherwise>
            </xsl:choose>

            <xsl:for-each select="$Fields">
                <xsl:choose>
                    <xsl:when test="Type = 'string'">
                        <xsl:choose>
                            <xsl:when test="Multiline = '1'">
                                <xsl:value-of select="Name"/>.Buffer.Text = <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/>;
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="Name"/>.Text = <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/>;
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:when test="Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                        <xsl:value-of select="Name"/>.Value = <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'boolean'">
                        <xsl:value-of select="Name"/>.Active = <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'composite_pointer'">
                        <xsl:value-of select="Name"/>.Pointer = <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:value-of select="Name"/>.Pointer = <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                        <xsl:value-of select="Name"/>.ActiveId = <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/>.ToString();
                        if (<xsl:value-of select="Name"/>.Active == -1) <xsl:value-of select="Name"/>.Active = 0;
                    </xsl:when>
                    <xsl:when test="Type = 'boolean' or Type = 'any_pointer' or Type = 'bytea' or Type = 'string[]' or Type = 'integer' or Type = 'numeric'">
                        //<xsl:value-of select="Name"/> = <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/>;
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>

            <xsl:for-each select="$TabularParts">
                /* Таблична частина: <xsl:value-of select="Name"/> */
                <xsl:value-of select="Name"/>.<xsl:value-of select="$DirectoryName"/>_Objest = <xsl:value-of select="$DirectoryName"/>_Objest;
                <xsl:value-of select="Name"/>.LoadRecords();
            </xsl:for-each>
        }

        protected override void GetValue()
        {
            <xsl:for-each select="$Fields">
                <xsl:choose>
                    <xsl:when test="Type = 'string'">
                        <xsl:choose>
                            <xsl:when test="Multiline = '1'">
                                <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Buffer.Text;
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Text;
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                     <xsl:when test="Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                        <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Value;
                    </xsl:when>
                    <xsl:when test="Type = 'boolean'">
                        <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Active;
                    </xsl:when>
                    <xsl:when test="Type = 'composite_pointer'">
                        <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Pointer;
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Pointer;
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        <xsl:text>if (</xsl:text><xsl:value-of select="Name"/>.Active != -1) 
                            <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/> = Enum.Parse&lt;<xsl:value-of select="$namePointer"/>&gt;(<xsl:value-of select="Name"/>.ActiveId);
                    </xsl:when>
                    <xsl:when test="Type = 'boolean' or Type = 'any_pointer' or Type = 'bytea' or Type = 'string[]' or Type = 'integer' or Type = 'numeric'">
                        //<xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>;
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>
        }

        #endregion

        protected override void Save()
        {
            try
            {
                <xsl:value-of select="$DirectoryName"/>_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            <xsl:for-each select="$TabularParts">
                <xsl:value-of select="Name"/>.SaveRecords();
            </xsl:for-each>

            UnigueID = <xsl:value-of select="$DirectoryName"/>_Objest.UnigueID;
            Caption = Назва.Text;
        }
    }
}
    </xsl:template>

<!--- 
//
// ============================ Список ============================
//
-->

    <!-- Список -->
    <xsl:template name="DirectoryList">
        <xsl:variable name="DirectoryName" select="Directory/Name"/>
        <xsl:variable name="TabularParts" select="Directory/TabularParts/TablePart"/>

/*     
        <xsl:value-of select="$DirectoryName"/>.cs
        Список
*/

using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class <xsl:value-of select="$DirectoryName"/> : ДовідникЖурнал
    {
        public <xsl:value-of select="$DirectoryName"/>() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.Store;
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.Where.Clear();

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.LoadRecords();

            if (ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length &lt; 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.Where.Clear();

            //Назва
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.Where.Add(
                new Where(<xsl:value-of select="$DirectoryName"/>_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.LoadRecords();

            if (ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{<xsl:value-of select="$DirectoryName"/>_Const.FULLNAME} *", () =>
                {
                    <xsl:value-of select="$DirectoryName"/>_Елемент page = new <xsl:value-of select="$DirectoryName"/>_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else if (unigueID != null)
            {
                <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>_Objest = new <xsl:value-of select="$DirectoryName"/>_Objest();
                if (<xsl:value-of select="$DirectoryName"/>_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{<xsl:value-of select="$DirectoryName"/>_Objest.Назва}", () =>
                    {
                        <xsl:value-of select="$DirectoryName"/>_Елемент page = new <xsl:value-of select="$DirectoryName"/>_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            <xsl:value-of select="$DirectoryName"/>_Objest = <xsl:value-of select="$DirectoryName"/>_Objest,
                        };

                        page.SetValue();

                        return page;
                    }, true);
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override void SetDeletionLabel(UnigueID unigueID)
        {
            <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>_Objest = new <xsl:value-of select="$DirectoryName"/>_Objest();
            if (<xsl:value-of select="$DirectoryName"/>_Objest.Read(unigueID))
                <xsl:value-of select="$DirectoryName"/>_Objest.SetDeletionLabel(!<xsl:value-of select="$DirectoryName"/>_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>_Objest = new <xsl:value-of select="$DirectoryName"/>_Objest();
            if (<xsl:value-of select="$DirectoryName"/>_Objest.Read(unigueID))
            {
                <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>_Objest_Новий = <xsl:value-of select="$DirectoryName"/>_Objest.Copy(true);
                <xsl:value-of select="$DirectoryName"/>_Objest_Новий.Save();
                <xsl:for-each select="$TabularParts">
                    /* Таблична частина: <xsl:value-of select="Name"/> */
                    <xsl:value-of select="$DirectoryName"/>_Objest_Новий.<xsl:value-of select="Name"/>_TablePart.Save(false);
                </xsl:for-each>
                return <xsl:value-of select="$DirectoryName"/>_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        #endregion
    }
}
    </xsl:template>

<!--- 
//
// ============================ ШвидкийВибір ============================
//
-->

    <!-- ШвидкийВибір -->
    <xsl:template name="DirectoryListSmallSelect">
        <xsl:variable name="DirectoryName" select="Directory/Name"/>

/*     
        <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;

using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_ЗаписиШвидкийВибір.Store;
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Сторінка
            {
                LinkButton linkPage = new LinkButton($" {<xsl:value-of select="$DirectoryName"/>_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkPage.Clicked += (object? sender, EventArgs args) =&gt;
                {
                    <xsl:value-of select="$DirectoryName"/> page = new <xsl:value-of select="$DirectoryName"/>()
                    {
                        DirectoryPointerItem = DirectoryPointerItem,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    Program.GeneralForm?.CreateNotebookPage($"Вибір - {<xsl:value-of select="$DirectoryName"/>_Const.FULLNAME}", () =&gt; { return page; }, true);

                    page.LoadRecords();
                };

                HBoxTop.PackStart(linkPage, false, false, 10);
            }

            //Новий
            {
                LinkButton linkNew = new LinkButton("Новий");
                linkNew.Clicked += (object? sender, EventArgs args) =&gt;
                {
                    <xsl:value-of select="$DirectoryName"/>_Елемент page = new <xsl:value-of select="$DirectoryName"/>_Елемент
                    {
                        IsNew = true,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    Program.GeneralForm?.CreateNotebookPage($"{<xsl:value-of select="$DirectoryName"/>_Const.FULLNAME} *", () =&gt; { return page; }, true);

                    page.SetValue();
                };

                HBoxTop.PackStart(linkNew, false, false, 0);
            }
        }

        public override void LoadRecords()
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_ЗаписиШвидкийВибір.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_ЗаписиШвидкийВибір.Where.Clear();

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_ЗаписиШвидкийВибір.LoadRecords();

            if (ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_ЗаписиШвидкийВибір.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_ЗаписиШвидкийВибір.SelectPath, TreeViewGrid.Columns[0], false);
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length &lt; 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_ЗаписиШвидкийВибір.Where.Clear();

            //Код
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_ЗаписиШвидкийВибір.Where.Add(
                new Where(<xsl:value-of select="$DirectoryName"/>_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            //Назва
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_ЗаписиШвидкийВибір.Where.Add(
                new Where(Comparison.OR, <xsl:value-of select="$DirectoryName"/>_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_ЗаписиШвидкийВибір.LoadRecords();
        }
    }
}
    </xsl:template>

<!--- 
//
// ============================ Список та Дерево ============================
//
-->

    <!-- Список та Дерево-->
    <xsl:template name="DirectoryListAndTree">
        <xsl:variable name="DirectoryName" select="Directory/Name"/>
        <xsl:variable name="TabularParts" select="Directory/TabularParts/TablePart"/>

/*     
        <xsl:value-of select="$DirectoryName"/>.cs 
        Список та Дерево 
*/

using Gtk;

using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class <xsl:value-of select="$DirectoryName"/> : ДовідникЖурнал
    {
        CheckButton checkButtonIsHierarchy = new CheckButton("Ієрархія папок") { Active = true };
        <xsl:value-of select="$DirectoryName"/>_Папки_Дерево ДеревоПапок;

        public <xsl:value-of select="$DirectoryName"/>() : base()
        {
            //Враховувати ієрархію папок
            checkButtonIsHierarchy.Clicked += OnCheckButtonIsHierarchyClicked;
            HBoxTop.PackStart(checkButtonIsHierarchy, false, false, 10);

            //Дерево папок
            ДеревоПапок = new <xsl:value-of select="$DirectoryName"/>_Папки_Дерево() { WidthRequest = 500 };
            ДеревоПапок.CallBack_RowActivated = LoadRecords_TreeCallBack;
            HPanedTable.Pack2(ДеревоПапок, false, true);

            TreeViewGrid.Model = ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.Store;
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            if (DirectoryPointerItem != null || SelectPointerItem != null)
            {
                UnigueID? unigueID = SelectPointerItem != null ? SelectPointerItem : DirectoryPointerItem;

                <xsl:value-of select="$DirectoryName"/>_Objest? контрагенти_Objest = new <xsl:value-of select="$DirectoryName"/>_Pointer(unigueID ?? new UnigueID()).GetDirectoryObject();
                if (контрагенти_Objest != null)
                    ДеревоПапок.DirectoryPointerItem = контрагенти_Objest.Папка.UnigueID;
            }

            ДеревоПапок.LoadTree();
        }

        void LoadRecords_TreeCallBack()
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.Where.Clear();

            if (checkButtonIsHierarchy.Active)
                ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.Where.Add(new Where(<xsl:value-of select="$DirectoryName"/>_Const.Папка, Comparison.EQ,
                    ДеревоПапок.DirectoryPointerItem?.UGuid ?? new UnigueID().UGuid));

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.LoadRecords();

            if (ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length &lt; 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.Where.Clear();

            //Код
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.Where.Add(
                new Where(<xsl:value-of select="$DirectoryName"/>_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            //Назва
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.Where.Add(
                new Where(Comparison.OR, <xsl:value-of select="$DirectoryName"/>_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.LoadRecords();

            if (ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{<xsl:value-of select="$DirectoryName"/>_Const.FULLNAME} *", () =>
                {
                    <xsl:value-of select="$DirectoryName"/>_Елемент page = new <xsl:value-of select="$DirectoryName"/>_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else if (unigueID != null)
            {
                <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>_Objest = new <xsl:value-of select="$DirectoryName"/>_Objest();
                if (<xsl:value-of select="$DirectoryName"/>_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{<xsl:value-of select="$DirectoryName"/>_Objest.Назва}", () =>
                    {
                        <xsl:value-of select="$DirectoryName"/>_Елемент page = new <xsl:value-of select="$DirectoryName"/>_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            <xsl:value-of select="$DirectoryName"/>_Objest = <xsl:value-of select="$DirectoryName"/>_Objest,
                        };

                        page.SetValue();

                        return page;
                    }, true);
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override void SetDeletionLabel(UnigueID unigueID)
        {
            <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>_Objest = new <xsl:value-of select="$DirectoryName"/>_Objest();
            if (<xsl:value-of select="$DirectoryName"/>_Objest.Read(unigueID))
                <xsl:value-of select="$DirectoryName"/>_Objest.SetDeletionLabel(!<xsl:value-of select="$DirectoryName"/>_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>_Objest = new <xsl:value-of select="$DirectoryName"/>_Objest();
            if (<xsl:value-of select="$DirectoryName"/>_Objest.Read(unigueID))
            {
                <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>_Objest_Новий = <xsl:value-of select="$DirectoryName"/>_Objest.Copy(true);
                <xsl:value-of select="$DirectoryName"/>_Objest_Новий.Save();
                <xsl:for-each select="$TabularParts">
                    /* Таблична частина: <xsl:value-of select="Name"/> */
                    <xsl:value-of select="$DirectoryName"/>_Objest_Новий.<xsl:value-of select="Name"/>_TablePart.Save(false);
                </xsl:for-each>
                return <xsl:value-of select="$DirectoryName"/>_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        #endregion

        void OnCheckButtonIsHierarchyClicked(object? sender, EventArgs args)
        {
            LoadRecords();
        }
    }
}
    </xsl:template>

<!--- 
//
// ============================ Дерево ============================
//
-->

    <!-- Дерево -->
    <xsl:template name="DirectoryTree">
        <xsl:variable name="DirectoryName" select="Directory/Name"/>

/*
        <xsl:value-of select="$DirectoryName"/>_Дерево.cs
        Дерево
*/

using Gtk;

using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class <xsl:value-of select="$DirectoryName"/>_Дерево : ДовідникДерево
    {
        public <xsl:value-of select="$DirectoryName"/>_Дерево() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.Store;
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.AddColumns(TreeViewGrid);
        }

        public override void LoadTree()
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.LoadTree(OpenFolder, DirectoryPointerItem);

            TreeViewGrid.ExpandToPath(ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.RootPath);
            TreeViewGrid.SetCursor(ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.RootPath, TreeViewGrid.Columns[0], false);

            if (ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.SelectPath != null)
            {
                TreeViewGrid.ExpandToPath(ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.SelectPath);
                TreeViewGrid.SetCursor(ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            }

            RowActivated();
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{<xsl:value-of select="$DirectoryName"/>_Const.FULLNAME} *", () =>
                {
                    <xsl:value-of select="$DirectoryName"/>_Елемент page = new <xsl:value-of select="$DirectoryName"/>_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadTree,
                        IsNew = true,
                        РодичДляНового = new <xsl:value-of select="$DirectoryName"/>_Pointer(DirectoryPointerItem ?? new UnigueID())
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else if (unigueID != null)
            {
                <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>_Objest = new <xsl:value-of select="$DirectoryName"/>_Objest();
                if (<xsl:value-of select="$DirectoryName"/>_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{<xsl:value-of select="$DirectoryName"/>_Objest.Назва}", () =>
                    {
                        <xsl:value-of select="$DirectoryName"/>_Елемент page = new <xsl:value-of select="$DirectoryName"/>_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadTree,
                            IsNew = false,
                            <xsl:value-of select="$DirectoryName"/>_Objest = <xsl:value-of select="$DirectoryName"/>_Objest
                        };

                        page.SetValue();

                        return page;
                    }, true);
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }


        #region ToolBar

        protected override void SetDeletionLabel(UnigueID unigueID)
        {
            <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>_Objest = new <xsl:value-of select="$DirectoryName"/>_Objest();
            if (<xsl:value-of select="$DirectoryName"/>_Objest.Read(unigueID))
                <xsl:value-of select="$DirectoryName"/>_Objest.SetDeletionLabel(!<xsl:value-of select="$DirectoryName"/>_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>_Objest = new <xsl:value-of select="$DirectoryName"/>_Objest();
            if (<xsl:value-of select="$DirectoryName"/>_Objest.Read(unigueID))
            {
                <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>_Objest_Новий = <xsl:value-of select="$DirectoryName"/>_Objest.Copy(true);
                <xsl:value-of select="$DirectoryName"/>_Objest_Новий.Save();

                return <xsl:value-of select="$DirectoryName"/>_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        #endregion
    }
}
    </xsl:template>

<!--- 
//
// ============================ Дерево Швидкий вибір ============================
//
-->

    <!-- Дерево Швидкий вибір -->
    <xsl:template name="DirectoryTreeSmallSelect">
        <xsl:variable name="DirectoryName" select="Directory/Name"/>

/*
        <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір.cs
        Дерево Швидкий вибір
*/

using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    class <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public UnigueID? OpenFolder { get; set; }

        public <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір() : base(false)
        {
            TreeViewGrid.Model = ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_ЗаписиШвидкийВибір.Store;
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_ЗаписиШвидкийВибір.AddColumns(TreeViewGrid);

            //Сторінка
            {
                LinkButton linkPage = new LinkButton($" {<xsl:value-of select="$DirectoryName"/>_Const.FULLNAME}") { Halign = Align.Start, Image = new Image(AppContext.BaseDirectory + "images/doc.png"), AlwaysShowImage = true };
                linkPage.Clicked += (object? sender, EventArgs args) =&gt;
                {
                    <xsl:value-of select="$DirectoryName"/>_Дерево page = new <xsl:value-of select="$DirectoryName"/>_Дерево()
                    {
                        DirectoryPointerItem = DirectoryPointerItem,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                        OpenFolder = OpenFolder
                    };

                    Program.GeneralForm?.CreateNotebookPage($"Вибір - {<xsl:value-of select="$DirectoryName"/>_Const.FULLNAME}", () =&gt; { return page; }, true);

                    page.LoadTree();
                };

                HBoxTop.PackStart(linkPage, false, false, 10);
            }

            //Новий
            {
                LinkButton linkNew = new LinkButton("Новий");
                linkNew.Clicked += (object? sender, EventArgs args) =&gt;
                {
                    <xsl:value-of select="$DirectoryName"/>_Елемент page = new <xsl:value-of select="$DirectoryName"/>_Елемент
                    {
                        IsNew = true,
                        CallBack_OnSelectPointer = CallBack_OnSelectPointer
                    };

                    Program.GeneralForm?.CreateNotebookPage($"{<xsl:value-of select="$DirectoryName"/>_Const.FULLNAME} *", () =&gt; { return page; }, true);

                    page.SetValue();
                };

                HBoxTop.PackStart(linkNew, false, false, 0);
            }
        }

        public void LoadTree()
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_ЗаписиШвидкийВибір.LoadTree(OpenFolder, DirectoryPointerItem);

            TreeViewGrid.ExpandToPath(ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_ЗаписиШвидкийВибір.RootPath);
            TreeViewGrid.SetCursor(ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_ЗаписиШвидкийВибір.RootPath, TreeViewGrid.Columns[0], false);

            if (ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_ЗаписиШвидкийВибір.SelectPath != null)
            {
                TreeViewGrid.ExpandToPath(ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_ЗаписиШвидкийВибір.SelectPath);
                TreeViewGrid.SetCursor(ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_ЗаписиШвидкийВибір.SelectPath, TreeViewGrid.Columns[0], false);
            }
        }
    }
}
    </xsl:template>

<!--- 
//
// ============================ PointerControl ============================
//
-->

    <!-- PointerControl -->
    <xsl:template name="DirectoryPointerControl">
        <xsl:variable name="DirectoryName" select="Directory/Name"/>

/*     
        <xsl:value-of select="$DirectoryName"/>_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;

using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class <xsl:value-of select="$DirectoryName"/>_PointerControl : PointerControl
    {
        public <xsl:value-of select="$DirectoryName"/>_PointerControl()
        {
            pointer = new <xsl:value-of select="$DirectoryName"/>_Pointer();
            WidthPresentation = 300;
            Caption = $"{<xsl:value-of select="$DirectoryName"/>_Const.FULLNAME}:";
        }

        <xsl:value-of select="$DirectoryName"/>_Pointer pointer;
        public <xsl:value-of select="$DirectoryName"/>_Pointer Pointer
        {
            get
            {
                return pointer;
            }
            set
            {
                pointer = value;
                Presentation = pointer != null ? Task.Run(async () =&gt; { return await pointer.GetPresentation(); }).Result : "";
            }
        }

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            Popover PopoverSmallSelect = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };

            if (BeforeClickOpenFunc != null)
                BeforeClickOpenFunc.Invoke();

            <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір page = new <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір() 
            { 
                PopoverParent = PopoverSmallSelect, 
                DirectoryPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =&gt;
                {
                    Pointer = new <xsl:value-of select="$DirectoryName"/>_Pointer(selectPointer);

                    if (AfterSelectFunc != null)
                        AfterSelectFunc.Invoke();
                }
            };

            PopoverSmallSelect.Add(page);
            PopoverSmallSelect.ShowAll();

            page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new <xsl:value-of select="$DirectoryName"/>_Pointer();

            if (AfterSelectFunc != null)
                AfterSelectFunc.Invoke();
        }
    }
}
    </xsl:template>

<!--- 
//
// ============================ PointerControlTree ============================
//
-->

    <!-- PointerControlTree -->
    <xsl:template name="DirectoryPointerControlTree">
        <xsl:variable name="DirectoryName" select="Directory/Name"/>

/*     
        <xsl:value-of select="$DirectoryName"/>_PointerControl.cs
        PointerControl (Дерево)
*/

using Gtk;

using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class <xsl:value-of select="$DirectoryName"/>_PointerControl : PointerControl
    {
        public <xsl:value-of select="$DirectoryName"/>_PointerControl()
        {
            pointer = new <xsl:value-of select="$DirectoryName"/>_Pointer();
            WidthPresentation = 300;
            Caption = $"{<xsl:value-of select="$DirectoryName"/>_Const.FULLNAME}:";
        }

        public UnigueID? OpenFolder { get; set; }

        <xsl:value-of select="$DirectoryName"/>_Pointer pointer;
        public <xsl:value-of select="$DirectoryName"/>_Pointer Pointer
        {
            get
            {
                return pointer;
            }
            set
            {
                pointer = value;
                Presentation = (pointer != null ? pointer.GetPresentation() : "");
            }
        }

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            Popover PopoverSmallSelect = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };

            if (BeforeClickOpenFunc != null)
                BeforeClickOpenFunc.Invoke();

            <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір page = new <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір() 
            { PopoverParent = PopoverSmallSelect, OpenFolder = OpenFolder, DirectoryPointerItem = Pointer.UnigueID };
            page.CallBack_OnSelectPointer = (UnigueID selectPointer) =&gt;
            {
                Pointer = new <xsl:value-of select="$DirectoryName"/>_Pointer(selectPointer);

                if (AfterSelectFunc != null)
                    AfterSelectFunc.Invoke();
            };

            PopoverSmallSelect.Add(page);
            PopoverSmallSelect.ShowAll();

            page.LoadTree();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new <xsl:value-of select="$DirectoryName"/>_Pointer();

            if (AfterSelectFunc != null)
                AfterSelectFunc.Invoke();
        }
    }
}
    </xsl:template>

</xsl:stylesheet>