<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" indent="yes" />

  <xsl:template name="License">
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
  </xsl:template>

  <xsl:template match="/">
    <xsl:call-template name="License" />
/*
 *
 * Конфігурації "<xsl:value-of select="Configuration/Name"/>"
 * Автор <xsl:value-of select="Configuration/Author"/>
 * Дата конфігурації: <xsl:value-of select="Configuration/DateTimeSave"/>
 *
 *
 * Цей код згенерований в Конфігураторі 3. Шаблон Gtk.xslt
 *
 */

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using <xsl:value-of select="Configuration/NameSpaceGenerationCode"/>.Перелічення;
using <xsl:value-of select="Configuration/NameSpace"/>;

namespace <xsl:value-of select="Configuration/NameSpaceGenerationCode"/>.Довідники.ТабличніСписки
{
    <xsl:for-each select="Configuration/Directories/Directory">
      <xsl:variable name="DirectoryName" select="Name"/>
      <!-- Для ієрархії -->
      <xsl:variable name="DirectoryType" select="Type"/>
      <xsl:variable name="StoreType">
          <xsl:choose>
              <xsl:when test="$DirectoryType = 'Hierarchical'">TreeStore</xsl:when>
              <xsl:otherwise>ListStore</xsl:otherwise>
          </xsl:choose>
      </xsl:variable>
      <xsl:variable name="SelectType">
          <xsl:choose>
              <xsl:when test="$DirectoryType = 'Hierarchical'">SelectHierarchical</xsl:when>
              <xsl:otherwise>Select</xsl:otherwise>
          </xsl:choose>
      </xsl:variable>
    #region DIRECTORY "<xsl:value-of select="$DirectoryName"/>"
      <xsl:for-each select="TabularLists/TabularList">
        <xsl:variable name="TabularListName" select="Name"/>
    public class <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/> : ТабличнийСписок
    {
        bool DeletionLabel = false;
        string ID = "";
        <xsl:for-each select="Fields/Field">
        string <xsl:value-of select="Name"/> = "<xsl:if test="position() = 1 and $DirectoryType = 'Hierarchical'">Дерево</xsl:if>";
        </xsl:for-each>

        <xsl:for-each select="Fields/AdditionalField">
        string <xsl:value-of select="Name"/> = "";</xsl:for-each>

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID,
                <xsl:for-each select="Fields/Field">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/>*/ <xsl:value-of select="Name"/>,
                </xsl:for-each>
                <xsl:for-each select="Fields/AdditionalField">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/>*/ <xsl:value-of select="Name"/>,
                </xsl:for-each>
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new <xsl:value-of select="$StoreType"/>(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string),
                <xsl:for-each select="Fields/Field">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/>*/ typeof(string),  
                </xsl:for-each>
                <xsl:for-each select="Fields/AdditionalField">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/>*/ typeof(string), 
                </xsl:for-each>
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 4 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });

            /* Поля */
            <xsl:for-each select="Fields/Field">
              <xsl:text>treeView.AppendColumn(new TreeViewColumn("</xsl:text><xsl:value-of select="normalize-space(Caption)"/>
              <xsl:text>", new CellRendererText() { Xpad = 4 }, "text", </xsl:text><xsl:value-of select="position() + 1"/>
              <xsl:text>) { MinWidth = 20, Resizable = true, SortColumnId = </xsl:text><xsl:value-of select="position() + 1"/>
              <xsl:if test="Size != '0'"><xsl:value-of select="concat(', FixedWidth = ', Size)"/></xsl:if> } ); /*<xsl:value-of select="Name"/>*/
            </xsl:for-each>

            /* Додаткові поля */
            <xsl:variable name="CountField" select="count(Fields/Field) + 1"/>
            <xsl:for-each select="Fields/AdditionalField">
              <xsl:text>treeView.AppendColumn(new TreeViewColumn("</xsl:text><xsl:value-of select="normalize-space(Caption)"/>
              <xsl:text>", new CellRendererText() { Xpad = 4 }, "text", </xsl:text><xsl:value-of select="$CountField + position()"/>
              <xsl:text>) { MinWidth = 20, Resizable = true, SortColumnId = </xsl:text><xsl:value-of select="$CountField + position()"/>
              <xsl:if test="Size != '0'"><xsl:value-of select="concat(', FixedWidth = ', Size)"/></xsl:if> } ); /*<xsl:value-of select="Name"/>*/
            </xsl:for-each>

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static UnigueID? DirectoryPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;
        <xsl:if test="$DirectoryType = 'Hierarchical'">
        public static TreePath? RootPath;
        </xsl:if>
        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            <xsl:choose>
                <xsl:when test="count(Fields/Field[FilterField = 'True']) != 0">
                  List&lt;Tuple&lt;string, Widget, Switch&gt;&gt; widgets = [];
                  <xsl:for-each select="Fields/Field[FilterField = 'True']">
                  { /* <xsl:value-of select="Name"/>, <xsl:value-of select="Type"/> */
                      Switch sw = new();
                      <xsl:choose>
                          <xsl:when test="Type = 'string'">Entry <xsl:value-of select="Name"/> = new() { WidthRequest = 400 };</xsl:when>
                          <xsl:when test="Type = 'boolean'">CheckButton <xsl:value-of select="Name"/> = new();</xsl:when>
                          <xsl:when test="Type = 'integer'">IntegerControl <xsl:value-of select="Name"/> = new();</xsl:when>
                          <xsl:when test="Type = 'numeric'">NumericControl <xsl:value-of select="Name"/> = new();</xsl:when>
                          <xsl:when test="Type = 'date'">DateTimeControl <xsl:value-of select="Name"/> = new() { OnlyDate = true };</xsl:when>
                          <xsl:when test="Type = 'datetime'">DateTimeControl <xsl:value-of select="Name"/> = new();</xsl:when>
                          <xsl:when test="Type = 'time'">TimeControl <xsl:value-of select="Name"/> = new();</xsl:when>
                          <xsl:when test="Type = 'pointer'"><xsl:value-of select="substring-after(Pointer, '.')"/>_PointerControl <xsl:value-of select="Name"/> = new() { Caption = "", AfterSelectFunc = () =&gt; sw.Active = true };</xsl:when>
                          <xsl:when test="Type = 'enum'">ComboBoxText <xsl:value-of select="Name"/> = new();
                          foreach (var item in ПсевдонімиПерелічення.<xsl:value-of select="substring-after(Pointer, '.')"/>_List()) <xsl:value-of select="Name"/>.Append(item.Value.ToString(), item.Name);
                          <xsl:value-of select="Name"/>.Active = 0;
                          </xsl:when>
                          <xsl:otherwise>Label <xsl:value-of select="Name"/>  = new("<xsl:value-of select="Type"/>");</xsl:otherwise>
                      </xsl:choose>
                      widgets.Add(new("<xsl:value-of select="Name"/>", <xsl:value-of select="Name"/>, sw));
                      ДодатиЕлементВФільтр(listBox, "<xsl:value-of select="normalize-space(Caption)"/>:", <xsl:value-of select="Name"/>, sw);
                  }
                  </xsl:for-each>
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List&lt;Where&gt; listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { <xsl:for-each select="Fields/Field[FilterField = 'True']">
                                      <xsl:text>"</xsl:text><xsl:value-of select="Name"/>" =&gt; <xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="Name"/>,
                                  </xsl:for-each> _ =&gt; null };
                                  object? value = widget.Item1 switch { <xsl:for-each select="Fields/Field[FilterField = 'True']">
                                      <xsl:text>"</xsl:text><xsl:value-of select="Name"/><xsl:text>" =&gt; </xsl:text>
                                      <xsl:choose>
                                          <xsl:when test="Type = 'string'">((Entry)widget.Item2).Text</xsl:when>
                                          <xsl:when test="Type = 'boolean'">((CheckButton)widget.Item2).Active</xsl:when>
                                          <xsl:when test="Type = 'integer'">((IntegerControl)widget.Item2).Value</xsl:when>
                                          <xsl:when test="Type = 'numeric'">((NumericControl)widget.Item2).Value</xsl:when>
                                          <xsl:when test="Type = 'date' or Type = 'datetime'">((DateTimeControl)widget.Item2).Value</xsl:when>
                                          <xsl:when test="Type = 'time'">((TimeControl)widget.Item2).Value</xsl:when>
                                          <xsl:when test="Type = 'pointer'">((<xsl:value-of select="substring-after(Pointer, '.')"/>_PointerControl)widget.Item2).Pointer.UnigueID.UGuid</xsl:when>
                                          <xsl:when test="Type = 'enum'">(int)Enum.Parse&lt;<xsl:value-of select="substring-after(Pointer, '.')"/>&gt;(((ComboBoxText)widget.Item2).ActiveId)</xsl:when>
                                      </xsl:choose>,
                                  </xsl:for-each> _ =&gt; null };
                                  if (field != null &amp;&amp; value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                </xsl:when>
                <xsl:otherwise>
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                </xsl:otherwise>
            </xsl:choose>
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Довідники.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$SelectType"/><xsl:text> </xsl:text><xsl:value-of select="$DirectoryName"/>_Select = new Довідники.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$SelectType"/>();
            <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                <xsl:for-each select="Fields/Field[Type != 'pointer']">
                    <xsl:text>/*</xsl:text><xsl:value-of select="Name"/><xsl:text>*/ </xsl:text>
                    <xsl:text>Довідники.</xsl:text>
                    <xsl:value-of select="$DirectoryName"/>
                    <xsl:text>_Const.</xsl:text>
                    <xsl:value-of select="Name"/>,
                </xsl:for-each>
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.Where = (List&lt;Where&gt;)where;

            <xsl:for-each select="Fields/Field[SortField = 'True']">
              <xsl:variable name="SortDirection">
                  <xsl:choose>
                      <xsl:when test="SortDirection = 'True'">SelectOrder.DESC</xsl:when>
                      <xsl:otherwise>SelectOrder.ASC</xsl:otherwise>
                  </xsl:choose>
              </xsl:variable>
              <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.Order.Add(
              <xsl:choose>
                  <xsl:when test="Type = 'pointer'">"<xsl:value-of select="Name"/>"</xsl:when>
                  <xsl:otherwise> Довідники.<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="Name"/></xsl:otherwise>
              </xsl:choose>
              <xsl:text>, </xsl:text><xsl:value-of select="$SortDirection"/>);
            </xsl:for-each>

            <xsl:for-each select="Fields/Field[Type = 'pointer']">
                <xsl:value-of select="substring-before(Pointer, '.')"/>.<xsl:value-of select="substring-after(Pointer, '.')"/>_Pointer.GetJoin(<xsl:value-of select="$DirectoryName"/>_Select.QuerySelect, Довідники.<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="Name"/>,
                <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.Table, "join_tab_<xsl:value-of select="position()"/>", "<xsl:value-of select="Name"/>");
            </xsl:for-each>

            <xsl:for-each select="Fields/AdditionalField">
                /* Additional Field */
                <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.FieldAndAlias.Add(
                  new NameValue&lt;string&gt;(@$"(<xsl:value-of select="normalize-space(Value)"/>)", "<xsl:value-of select="Name"/>"));
                /*
                <xsl:value-of select="Value"/>
                */
            </xsl:for-each>

            /* SELECT */
            await <xsl:value-of select="$DirectoryName"/>_Select.Select();

            <xsl:value-of select="$StoreType"/> Store = (<xsl:value-of select="$StoreType"/>)treeView.Model;
            Store.Clear();

            <xsl:if test="$DirectoryType = 'Hierarchical'">
            Dictionary&lt;string, TreeIter&gt; nodeDictionary = new();
            TreeIter rootIter = Store.AppendValues(new <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/>(){ ID = Guid.Empty.ToString() }.ToArray());
            RootPath = Store.GetPath(rootIter);
            </xsl:if>

            while (<xsl:value-of select="$DirectoryName"/>_Select.MoveNext())
            {
                Довідники.<xsl:value-of select="$DirectoryName"/>_Pointer? cur = <xsl:value-of select="$DirectoryName"/>_Select.Current;
                <xsl:if test="$DirectoryType = 'Hierarchical'">
                string Parent = <xsl:value-of select="$DirectoryName"/>_Select.Parent?.ToString() ?? Guid.Empty.ToString();
                int Level = <xsl:value-of select="$DirectoryName"/>_Select.Level;
                </xsl:if>
                if (cur != null)
                {
                    Dictionary&lt;string, object&gt; Fields = cur.Fields!;
                    <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/> Record = new <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/>
                    {
                        ID = cur.UnigueID.ToString(),
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        <xsl:for-each select="Fields/Field">
                          <xsl:value-of select="Name"/><xsl:text> = </xsl:text>
                          <xsl:choose>
                            <xsl:when test="Type = 'pointer'">
                              <xsl:text>Fields["</xsl:text><xsl:value-of select="Name"/>"].ToString() ?? "",
                            </xsl:when>
                            <xsl:when test="Type = 'enum'">
                              <xsl:text>Перелічення.ПсевдонімиПерелічення.</xsl:text><xsl:value-of select="substring-after(Pointer, '.')"/>_Alias((
                              <xsl:text>(</xsl:text><xsl:value-of select="Pointer"/>)(Fields[<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="Name"/>] != DBNull.Value ? Fields[<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="Name"/>] : 0)) ),
                            </xsl:when>
                            <xsl:when test="Type = 'boolean'">
                              <xsl:text>(Fields[</xsl:text><xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="Name"/>] != DBNull.Value ? (bool)Fields[<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="Name"/>] : false) ? "Так" : "",
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text>Fields[</xsl:text><xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="Name"/>].ToString() ?? "",
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:for-each>
                        <xsl:for-each select="Fields/AdditionalField">
                            <xsl:value-of select="Name"/> = Fields["<xsl:value-of select="Name"/>"].ToString() ?? "",
                        </xsl:for-each>
                    };
                    <xsl:choose>
                      <xsl:when test="$DirectoryType = 'Hierarchical'">
                        TreeIter CurrentIter = Level == 1 ? Store.AppendValues(rootIter, Record.ToArray()) : Store.AppendValues(nodeDictionary[Parent], Record.ToArray());
                        nodeDictionary.Add(Record.ID, CurrentIter);
                      </xsl:when>
                      <xsl:otherwise>
                        TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                      </xsl:otherwise>
                    </xsl:choose>

                    CurrentPath = Store.GetPath(CurrentIter);
                    FirstPath ??= CurrentPath;

                    if (DirectoryPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DirectoryPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    </xsl:for-each>

      <!-- ДЕРЕВО -->
      <!--
      <xsl:for-each select="TabularLists/TabularList[IsTree = '11']">
        <xsl:variable name="TabularListName" select="Name"/>
    /* ДЕРЕВО */
    public class <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/>
    {
        bool DeletionLabel = false;
        string ID = "";
        string Назва = "";

        Array ToArray()
        {
            return new object[] 
            {
                DeletionLabel ? InterfaceGtk.Іконки.ДляДерева.Delete : InterfaceGtk.Іконки.ДляДерева.Normal,
                ID,
                Назва
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new TreeStore
            (
                typeof(Gdk.Pixbuf) /* Image */, 
                typeof(string)     /* ID */, 
                typeof(string)     /* Назва */
            );

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText(), "text", 2));
        }

        /* Шлях до корінної вітки */
        public static TreePath? RootPath;

        /* Шлях який спозиціонований у функції LoadTree - параметр selectPointer */
        public static TreePath? SelectPath;

        /*
        openFolder - відкрита папка, яку потрібно ВИКЛЮЧИТИ з вибірки. 
                     Також будуть виключені всі папки які входять в дану папку
        selectPointer - елемент на який потрібно спозиціонуватися
        owner - Власник (якщо таке поле є в табличному списку)
        */
        public static async ValueTask LoadTree(TreeView treeView, UnigueID? openFolder, UnigueID? selectPointer <xsl:if test="count(Fields/Field[Name = 'Власник']) = 1">, UnigueID? owner</xsl:if>)
        {
            RootPath = SelectPath = null;

            <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/> rootRecord = new <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/>
            {
                ID = Guid.Empty.ToString(),
                Назва = " Дерево "
            };

            #region SQL

            string query = $@"
                WITH RECURSIVE r AS (
                    SELECT 
                        uid, 
                        {<xsl:value-of select="$DirectoryName"/>_Const.Назва}, 
                        {<xsl:value-of select="$DirectoryName"/>_Const.Родич}, 
                        1 AS level,
                        deletion_label
                    FROM {<xsl:value-of select="$DirectoryName"/>_Const.TABLE}
                    WHERE {<xsl:value-of select="$DirectoryName"/>_Const.Родич} = '{Guid.Empty}'";
                -->
                        <!-- Якщо є поле Власник у табличному списку --><!--
                        <xsl:if test="count(Fields/Field[Name = 'Власник']) = 1">
                            if (owner != null &amp;&amp; !owner.IsEmpty()) query += $@"
                        AND {<xsl:value-of select="$DirectoryName"/>_Const.Власник} = '{owner}'";
                        </xsl:if>

                            if (openFolder != null) query += $@"
                        AND uid != '{openFolder}'";

                            query += $@"
                    UNION ALL
                    SELECT 
                        {<xsl:value-of select="$DirectoryName"/>_Const.TABLE}.uid, 
                        {<xsl:value-of select="$DirectoryName"/>_Const.TABLE}.{<xsl:value-of select="$DirectoryName"/>_Const.Назва}, 
                        {<xsl:value-of select="$DirectoryName"/>_Const.TABLE}.{<xsl:value-of select="$DirectoryName"/>_Const.Родич}, 
                        r.level + 1 AS level,
                        {<xsl:value-of select="$DirectoryName"/>_Const.TABLE}.deletion_label 
                    FROM {<xsl:value-of select="$DirectoryName"/>_Const.TABLE}
                        JOIN r ON {<xsl:value-of select="$DirectoryName"/>_Const.TABLE}.{<xsl:value-of select="$DirectoryName"/>_Const.Родич} = r.uid";

                        --><!-- Якщо є поле Власник у табличному списку --><!--
                        <xsl:if test="count(Fields/Field[Name = 'Власник']) = 1">
                            if (owner != null &amp;&amp; !owner.IsEmpty()) query += $@"
                        AND {<xsl:value-of select="$DirectoryName"/>_Const.Власник} = '{owner}'";
                        </xsl:if>

                            if (openFolder != null) query += $@"
                    WHERE {<xsl:value-of select="$DirectoryName"/>_Const.TABLE}.uid != '{openFolder}'";

                            query += $@"
                )
                SELECT 
                    uid, 
                    {<xsl:value-of select="$DirectoryName"/>_Const.Назва} AS Назва, 
                    {<xsl:value-of select="$DirectoryName"/>_Const.Родич} AS Родич, 
                    level,
                    deletion_label
                FROM r
                ORDER BY level, {<xsl:value-of select="$DirectoryName"/>_Const.Назва} ASC
                ";

            #endregion

            var recordResult = await Config.Kernel.DataBase.SelectRequest(query);

            Dictionary&lt;string, TreeIter&gt; nodeDictionary = new Dictionary&lt;string, TreeIter&gt;();

            TreeStore Store = (TreeStore)treeView.Model;
            Store.Clear();

            TreeIter rootIter = Store.AppendValues(rootRecord.ToArray());
            RootPath = Store.GetPath(rootIter);

            if (recordResult.Result)
                foreach (var row in recordResult.ListRow)
                {
                    string uid = row["uid"].ToString() ?? Guid.Empty.ToString();
                    string fieldName = row["Назва"].ToString() ?? "";
                    string fieldParent = row["Родич"].ToString() ?? Guid.Empty.ToString();
                    int level = (int)row["level"];
                    bool deletionLabel = (bool)row["deletion_label"];

                    <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/> record = new <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/>
                    {
                        DeletionLabel = deletionLabel,
                        ID = uid,
                        Назва = fieldName
                    };
                    
                    TreeIter Iter;

                    if (level == 1)
                        Iter = Store.AppendValues(rootIter, record.ToArray());
                    else
                    {
                        TreeIter parentIter = nodeDictionary[fieldParent];
                        Iter = Store.AppendValues(parentIter, record.ToArray());
                    }

                    nodeDictionary.Add(uid, Iter);

                    if (selectPointer != null)
                        if (uid == selectPointer.ToString())
                            SelectPath = Store.GetPath(Iter);
                }
        }
    }
      </xsl:for-each>-->
    #endregion
    </xsl:for-each>
}

namespace <xsl:value-of select="Configuration/NameSpaceGenerationCode"/>.Документи.ТабличніСписки
{
    <xsl:for-each select="Configuration/Documents/Document">
      <xsl:variable name="DocumentName" select="Name"/>
    #region DOCUMENT "<xsl:value-of select="$DocumentName"/>"
    
      <xsl:for-each select="TabularLists/TabularList">
        <xsl:variable name="TabularListName" select="Name"/>
    public class <xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularListName"/> : ТабличнийСписок
    {
        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        <xsl:for-each select="Fields/Field">
        string <xsl:value-of select="Name"/> = "";</xsl:for-each>

        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                <xsl:for-each select="Fields/Field">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/>*/ <xsl:value-of select="Name"/>,
                </xsl:for-each>
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Spend Проведений документ*/ typeof(bool),
                <xsl:for-each select="Fields/Field">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/>*/ typeof(string),  
                </xsl:for-each>
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 2)); /*Проведений документ*/
            /* */
            <xsl:for-each select="Fields/Field">
              <xsl:text>treeView.AppendColumn(new TreeViewColumn("</xsl:text><xsl:value-of select="normalize-space(Caption)"/>
              <xsl:text>", new CellRendererText() { Xpad = 4 }, "text", </xsl:text><xsl:value-of select="position() + 2"/>
              <xsl:text>) { MinWidth = 20, Resizable = true</xsl:text>
              <xsl:if test="Size != '0'"><xsl:value-of select="concat(', FixedWidth = ', Size)"/></xsl:if> } ); /*<xsl:value-of select="Name"/>*/
            </xsl:for-each>
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.<xsl:value-of select="$DocumentName"/>_Const.ДатаДок, типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? DocumentPointerItem { get; set; }
        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? FirstPath;
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static ListBox CreateFilter(TreeView treeView)
        {
            ListBox listBox = new() { SelectionMode = SelectionMode.None };
            <xsl:choose>
                <xsl:when test="count(Fields/Field[FilterField = 'True']) != 0">
                  List&lt;Tuple&lt;string, Widget, Switch&gt;&gt; widgets = [];
                  <xsl:for-each select="Fields/Field[FilterField = 'True']">
                  { /* <xsl:value-of select="Name"/>, <xsl:value-of select="Type"/> */
                      Switch sw = new();
                      <xsl:choose>
                          <xsl:when test="Type = 'string'">Entry <xsl:value-of select="Name"/> = new() { WidthRequest = 400 };</xsl:when>
                          <xsl:when test="Type = 'boolean'">CheckButton <xsl:value-of select="Name"/> = new();</xsl:when>
                          <xsl:when test="Type = 'integer'">IntegerControl <xsl:value-of select="Name"/> = new();</xsl:when>
                          <xsl:when test="Type = 'numeric'">NumericControl <xsl:value-of select="Name"/> = new();</xsl:when>
                          <xsl:when test="Type = 'date'">DateTimeControl <xsl:value-of select="Name"/> = new() { OnlyDate = true };</xsl:when>
                          <xsl:when test="Type = 'datetime'">DateTimeControl <xsl:value-of select="Name"/> = new();</xsl:when>
                          <xsl:when test="Type = 'time'">TimeControl <xsl:value-of select="Name"/> = new();</xsl:when>
                          <xsl:when test="Type = 'pointer'"><xsl:value-of select="substring-after(Pointer, '.')"/>_PointerControl <xsl:value-of select="Name"/> = new() { Caption = "", AfterSelectFunc = () =&gt; sw.Active = true };</xsl:when>
                          <xsl:when test="Type = 'enum'">ComboBoxText <xsl:value-of select="Name"/> = new();
                          foreach (var item in ПсевдонімиПерелічення.<xsl:value-of select="substring-after(Pointer, '.')"/>_List()) <xsl:value-of select="Name"/>.Append(item.Value.ToString(), item.Name);
                          <xsl:value-of select="Name"/>.Active = 0;
                          </xsl:when>
                          <xsl:otherwise>Label <xsl:value-of select="Name"/>  = new("<xsl:value-of select="Type"/>");</xsl:otherwise>
                      </xsl:choose>
                      widgets.Add(new("<xsl:value-of select="Name"/>", <xsl:value-of select="Name"/>, sw));
                      ДодатиЕлементВФільтр(listBox, "<xsl:value-of select="normalize-space(Caption)"/>:", <xsl:value-of select="Name"/>, sw);
                  }
                  </xsl:for-each>
                  {
                      Button bOn = new Button("Фільтрувати");
                      bOn.Clicked += async (object? sender, EventArgs args) =>
                      {
                          List&lt;Where&gt; listWhere = [];
                          foreach (var widget in widgets)
                              if (widget.Item3.Active)
                              {
                                  string? field = widget.Item1 switch { <xsl:for-each select="Fields/Field[FilterField = 'True']">
                                      <xsl:text>"</xsl:text><xsl:value-of select="Name"/>" =&gt; <xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Name"/>,
                                  </xsl:for-each> _ =&gt; null };
                                  object? value = widget.Item1 switch { <xsl:for-each select="Fields/Field[FilterField = 'True']">
                                      <xsl:text>"</xsl:text><xsl:value-of select="Name"/><xsl:text>" =&gt; </xsl:text>
                                      <xsl:choose>
                                          <xsl:when test="Type = 'string'">((Entry)widget.Item2).Text</xsl:when>
                                          <xsl:when test="Type = 'boolean'">((CheckButton)widget.Item2).Active</xsl:when>
                                          <xsl:when test="Type = 'integer'">((IntegerControl)widget.Item2).Value</xsl:when>
                                          <xsl:when test="Type = 'numeric'">((NumericControl)widget.Item2).Value</xsl:when>
                                          <xsl:when test="Type = 'date' or Type = 'datetime'">((DateTimeControl)widget.Item2).Value</xsl:when>
                                          <xsl:when test="Type = 'time'">((TimeControl)widget.Item2).Value</xsl:when>
                                          <xsl:when test="Type = 'pointer'">((<xsl:value-of select="substring-after(Pointer, '.')"/>_PointerControl)widget.Item2).Pointer.UnigueID.UGuid</xsl:when>
                                          <xsl:when test="Type = 'enum'">(int)Enum.Parse&lt;<xsl:value-of select="substring-after(Pointer, '.')"/>&gt;(((ComboBoxText)widget.Item2).ActiveId)</xsl:when>
                                      </xsl:choose>,
                                  </xsl:for-each> _ =&gt; null };
                                  if (field != null &amp;&amp; value != null) listWhere.Add(new Where(field, Comparison.EQ, value));
                              }
                          if (listWhere.Count != 0)
                          {
                              ДодатиВідбір(treeView, listWhere, true);
                              await LoadRecords(treeView);
                          }
                      };

                      Box vBox = new Box(Orientation.Vertical, 0) { Valign = Align.Center };
                      Box hBox = new Box(Orientation.Horizontal, 0) { Halign =  Align.Center };
                      vBox.PackStart(hBox, false, false, 5);
                      hBox.PackStart(bOn, false, false, 5);
                      
                      listBox.Add(new ListBoxRow() { vBox });
                  }
                </xsl:when>
                <xsl:otherwise>
                  listBox.Add(new ListBoxRow() { new Label("Фільтри відсутні") });
                </xsl:otherwise>
            </xsl:choose>
            return listBox;
        }

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            FirstPath = SelectPath = CurrentPath = null;

            Документи.<xsl:value-of select="$DocumentName"/>_Select <xsl:value-of select="$DocumentName"/>_Select = new Документи.<xsl:value-of select="$DocumentName"/>_Select();
            <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.Field.AddRange(
            [
                /*Помітка на видалення*/ "deletion_label",
                /*Проведений документ*/ "spend",
                <xsl:for-each select="Fields/Field[Type != 'pointer']">
                    <xsl:text>/*</xsl:text><xsl:value-of select="Name"/><xsl:text>*/ </xsl:text>
                    <xsl:text>Документи.</xsl:text>
                    <xsl:value-of select="$DocumentName"/>
                    <xsl:text>_Const.</xsl:text>
                    <xsl:value-of select="Name"/>,
                </xsl:for-each>
            ]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.Where = (List&lt;Where&gt;)where;

            <!--<xsl:for-each select="Fields/Field[SortField = 'True' and Type != 'pointer']">
              /* ORDER */
              <xsl:variable name="SortDirection">
                  <xsl:choose>
                      <xsl:when test="SortDirection = 'True'">SelectOrder.DESC</xsl:when>
                      <xsl:otherwise>SelectOrder.ASC</xsl:otherwise>
                  </xsl:choose>
              </xsl:variable>
              <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.Order.Add(Документи.<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Name"/>, <xsl:value-of select="$SortDirection"/>);
            </xsl:for-each>

            <xsl:for-each select="Fields/Field[Type = 'pointer']">
                /* Join Table */
                <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.Joins.Add(
                    new Join(<xsl:value-of select="Join/table"/>, Документи.<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Join/field"/>, <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.Table, "<xsl:value-of select="Join/alias"/>"));
                <xsl:for-each select="FieldAndAlias">
                  /* Field */
                  <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.FieldAndAlias.Add(
                    new NameValue&lt;string&gt;("<xsl:value-of select="table"/>." + <xsl:value-of select="field"/>, "<xsl:value-of select="table"/>_field_<xsl:value-of select="position()"/>"));
                  <xsl:if test="../SortField = 'True'">
                    /* ORDER */
                    <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.Order.Add("<xsl:value-of select="table"/>_field_<xsl:value-of select="position()"/>", SelectOrder.ASC);
                  </xsl:if>
                </xsl:for-each>
            </xsl:for-each>-->

            <xsl:for-each select="Fields/Field[SortField = 'True']">
              <xsl:variable name="SortDirection">
                  <xsl:choose>
                      <xsl:when test="SortDirection = 'True'">SelectOrder.DESC</xsl:when>
                      <xsl:otherwise>SelectOrder.ASC</xsl:otherwise>
                  </xsl:choose>
              </xsl:variable>
              <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.Order.Add(
              <xsl:choose>
                  <xsl:when test="Type = 'pointer'">"<xsl:value-of select="Name"/>"</xsl:when>
                  <xsl:otherwise> Документи.<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Name"/></xsl:otherwise>
              </xsl:choose>
              <xsl:text>, </xsl:text><xsl:value-of select="$SortDirection"/>);
            </xsl:for-each>

            <xsl:for-each select="Fields/Field[Type = 'pointer']">
                <xsl:value-of select="substring-before(Pointer, '.')"/>.<xsl:value-of select="substring-after(Pointer, '.')"/>_Pointer.GetJoin(<xsl:value-of select="$DocumentName"/>_Select.QuerySelect, Документи.<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Name"/>,
                <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.Table, "join_tab_<xsl:value-of select="position()"/>", "<xsl:value-of select="Name"/>");
            </xsl:for-each>

            /* SELECT */
            await <xsl:value-of select="$DocumentName"/>_Select.Select();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            while (<xsl:value-of select="$DocumentName"/>_Select.MoveNext())
            {
                Документи.<xsl:value-of select="$DocumentName"/>_Pointer? cur = <xsl:value-of select="$DocumentName"/>_Select.Current;

                if (cur != null)
                {
                    Dictionary&lt;string, object&gt; Fields = cur.Fields!;
                    <xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularListName"/> Record = new <xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularListName"/>
                    {
                        ID = cur.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
                        <!--<xsl:variable name="CountPointer" select="count(Fields/Field[Type = 'pointer'])"/>
                        <xsl:variable name="CountNotPointer" select="count(Fields/Field[Type != 'pointer'])"/>
                        <xsl:for-each select="Fields/Field[Type = 'pointer']">
                          <xsl:value-of select="Name"/>
                          <xsl:text> = </xsl:text>
                          <xsl:variable name="CountAlias" select="count(FieldAndAlias)"/>
                          <xsl:for-each select="FieldAndAlias">
                            <xsl:if test="position() &gt; 1"> + " " + </xsl:if>
                            <xsl:text>Fields[</xsl:text>"<xsl:value-of select="table"/>_field_<xsl:value-of select="position()"/><xsl:text>"].ToString()</xsl:text>
                            <xsl:if test="$CountAlias = 1"> ?? ""</xsl:if>
                          </xsl:for-each>
                          <xsl:if test="$CountNotPointer != 0 or position() != $CountPointer">,</xsl:if> /**/
                        </xsl:for-each>
                        <xsl:for-each select="Fields/Field[Type != 'pointer']">
                          <xsl:value-of select="Name"/>
                          <xsl:text> = </xsl:text>
                          <xsl:choose>
                            <xsl:when test="Type = 'enum'">
                              <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                              <xsl:text>Перелічення.ПсевдонімиПерелічення.</xsl:text>
                              <xsl:value-of select="$namePointer"/>
                              <xsl:text>_Alias( </xsl:text>
                              <xsl:text>((</xsl:text>
                              <xsl:value-of select="Pointer"/>
                              <xsl:text>)</xsl:text>
                              <xsl:text>(Fields[</xsl:text>
                              <xsl:value-of select="$DocumentName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>] != DBNull.Value ? Fields[</xsl:text>
                              <xsl:value-of select="$DocumentName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>] : 0)) )</xsl:text>
                            </xsl:when>
                            <xsl:when test="Type = 'boolean'">
                              <xsl:text>(Fields[</xsl:text>
                              <xsl:value-of select="$DocumentName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>] != DBNull.Value ? (bool)Fields[</xsl:text>
                              <xsl:value-of select="$DocumentName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>] : false) ? "Так" : ""</xsl:text>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text>Fields[</xsl:text>
                              <xsl:value-of select="$DocumentName"/>
                              <xsl:text>_Const.</xsl:text>
                              <xsl:value-of select="Name"/>
                              <xsl:text>].ToString() ?? ""</xsl:text>
                            </xsl:otherwise>
                          </xsl:choose>
                          <xsl:if test="position() != $CountNotPointer">,</xsl:if> /**/
                        </xsl:for-each>-->
                        <xsl:for-each select="Fields/Field">
                          <xsl:value-of select="Name"/><xsl:text> = </xsl:text>
                          <xsl:choose>
                            <xsl:when test="Type = 'pointer'">
                              <xsl:text>Fields["</xsl:text><xsl:value-of select="Name"/>"].ToString() ?? "",
                            </xsl:when>
                            <xsl:when test="Type = 'enum'">
                              <xsl:text>Перелічення.ПсевдонімиПерелічення.</xsl:text><xsl:value-of select="substring-after(Pointer, '.')"/>_Alias((
                               <xsl:text>(</xsl:text><xsl:value-of select="Pointer"/>)(Fields[<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Name"/>] != DBNull.Value ? Fields[<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Name"/>] : 0)) ),
                            </xsl:when>
                            <xsl:when test="Type = 'boolean'">
                               <xsl:text>(Fields[</xsl:text><xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Name"/>] != DBNull.Value ? (bool)Fields[<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Name"/>] : false) ? "Так" : "",
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:text>Fields[</xsl:text><xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Name"/>].ToString() ?? "",
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:for-each>
                    };

                    TreeIter CurrentIter = Store.AppendValues(Record.ToArray());
                    CurrentPath = Store.GetPath(CurrentIter);

                    FirstPath ??= CurrentPath;

                    if (DocumentPointerItem != null || SelectPointerItem != null)
                    {
                        string UidSelect = SelectPointerItem != null ? SelectPointerItem.ToString() : DocumentPointerItem!.ToString();
                        if (Record.ID == UidSelect)
                            SelectPath = CurrentPath;
                    }
                }
            }
        }
    }
	    </xsl:for-each>
    #endregion
    </xsl:for-each>

    //
    // Журнали
    //

    <xsl:for-each select="Configuration/Journals/Journal">
      <xsl:variable name="JournalName" select="Name"/>
      <xsl:variable name="AllowDocument" select="AllowDocument"/>
    #region JOURNAL "<xsl:value-of select="$JournalName"/>"
    
    public class Журнали_<xsl:value-of select="$JournalName"/>
    {
        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        string Type = ""; //Тип документу
        <xsl:for-each select="Fields/Field">
        string <xsl:value-of select="Name"/> = "";</xsl:for-each>

        // Масив для запису стрічки в Store
        Array ToArray()
        {
            return new object[] 
            { 
                DeletionLabel ? InterfaceGtk.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Type, 
                /*Проведений документ*/ Spend,
                <xsl:for-each select="Fields/Field">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/><xsl:text>*/ </xsl:text>
                  <xsl:value-of select="Name"/>,
                </xsl:for-each> 
            };
        }

        // Добавлення колонок в список
        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                typeof(Gdk.Pixbuf), /* Image */
                typeof(string), /* ID */
                typeof(string), /* Type */
                typeof(bool), /* Spend Проведений документ */
                <xsl:for-each select="Fields/Field">
                    <xsl:text>typeof(string), </xsl:text>/*<xsl:value-of select="Name"/>*/
                </xsl:for-each>
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /*Image*/ /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false }); /*UID*/
            treeView.AppendColumn(new TreeViewColumn("Type", new CellRendererText(), "text", 2) { Visible = false }); /*Type*/
            treeView.AppendColumn(new TreeViewColumn("", new CellRendererToggle(), "active", 3)); /*Проведений документ*/
            /* */
            <xsl:for-each select="Fields/Field">
              <xsl:text>treeView.AppendColumn(new TreeViewColumn("</xsl:text>
              <xsl:value-of select="normalize-space(Name)"/>
              <xsl:text>", new CellRendererText() { Xpad = 4 }, "text", </xsl:text>
              <xsl:value-of select="position() + 3"/> <!-- УВАГА! Коефіціент зміщення нумерації колонок -->
              <xsl:text>) { MinWidth = 20, Resizable = true } )</xsl:text>; /*<xsl:value-of select="Name"/>*/
            </xsl:for-each>
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            Dictionary&lt;string, List&lt;Where&gt;&gt; WhereDict = [];
            if (!treeView.Data.ContainsKey("Where"))
                treeView.Data.Add("Where", WhereDict);
            else
                treeView.Data["Where"] = WhereDict;
            <xsl:for-each select="$AllowDocument/Item">
              <xsl:variable name="AllowName" select="Name"/>
              <xsl:variable name="DocField" select="../../TabularLists/TabularList[Name = $AllowName]/Fields/Field[WherePeriod = '1']/DocField" />
              <xsl:if test="normalize-space($DocField) != ''">
            {
                Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.<xsl:value-of select="$AllowName"/>_Const.<xsl:value-of select="$DocField"/>, типПеріоду, start, stop);
                if (where != null) WhereDict.Add("<xsl:value-of select="$AllowName"/>", [where]);
            }
              </xsl:if>
            </xsl:for-each>
        }

        public static void ОчиститиВідбір(TreeView treeView)
        {
            if (treeView.Data.ContainsKey("Where"))
                treeView.Data["Where"] = null;
        }

        // Список документів які входять в журнал
        public static Dictionary&lt;string, string&gt; AllowDocument()
        {
            return new Dictionary&lt;string, string&gt;()
            {
                <xsl:for-each select="$AllowDocument/Item">
                    <xsl:text>{"</xsl:text><xsl:value-of select="Name"/>", "<xsl:value-of select="normalize-space(FullName)"/>"},
                </xsl:for-each>
            };
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        // Завантаження даних
        public static async ValueTask LoadRecords(TreeView treeView) 
        {
            SelectPath = CurrentPath = null;

            List&lt;string&gt; allQuery = [];
            Dictionary&lt;string, object&gt; paramQuery = [];

          <xsl:if test="count(TabularLists/TabularList) != 0">
            <xsl:for-each select="TabularLists/TabularList">
              <xsl:variable name="DocumentName" select="Name"/>
              <xsl:variable name="Table" select="Table"/>
              
              <xsl:if test="count($AllowDocument/Item[Name = $DocumentName]) = 1">
              //Документ: <xsl:value-of select="$DocumentName"/>
              {
                  Query query = new Query(Документи.<xsl:value-of select="$DocumentName"/>_Const.TABLE);

                  // Встановлення відбору
                  var dataWhere = treeView.Data["Where"];
                  if (dataWhere != null)
                  {
                      var dictWhere = (Dictionary&lt;string, List&lt;Where&gt;&gt;)dataWhere;
                      if (dictWhere.TryGetValue("<xsl:value-of select="$DocumentName"/>", out List&lt;Where&gt;? listWhere))
                      {
                          query.Where = listWhere;
                          foreach(Where where in listWhere)
                              paramQuery.Add(where.Alias, where.Value);
                      }
                  }
                  
                  query.FieldAndAlias.Add(new NameValue&lt;string&gt;("'<xsl:value-of select="$DocumentName"/>'", "type"));
                  query.Field.Add("deletion_label");
                  query.Field.Add("spend");
                  <xsl:for-each select="Fields/Field">
                      <!-- Приведення даних в запиті до певного типу -->
                      <xsl:variable name="SqlType">
                          <xsl:if test="normalize-space(SqlType) != ''">
                              <xsl:text> + "::</xsl:text>
                              <xsl:value-of select="normalize-space(SqlType)"/>
                              <xsl:text>"</xsl:text>
                          </xsl:if>
                      </xsl:variable>
                      <xsl:choose>
                        <xsl:when test="normalize-space(DocField) != ''">
                          <xsl:choose>
                            <xsl:when test="Type = 'pointer'">
                              <xsl:value-of select="substring-before(Pointer, '.')"/>.<xsl:value-of select="substring-after(Pointer, '.')"/>_Pointer.GetJoin(query, Документи.<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="DocField"/>, query.Table, "join_tab_<xsl:value-of select="position()"/>", "<xsl:value-of select="Name"/>");
                            </xsl:when>
                            <xsl:otherwise>
                              query.FieldAndAlias.Add(new NameValue&lt;string&gt;(Документи.<xsl:value-of select="$DocumentName"/>_Const.TABLE + "." + Документи.<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="DocField"/><xsl:value-of select="$SqlType"/>, "<xsl:value-of select="Name"/>"));
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          query.FieldAndAlias.Add(new NameValue&lt;string&gt;("''", "<xsl:value-of select="Name"/>")); /* Empty Field - <xsl:value-of select="Name"/>*/
                        </xsl:otherwise>
                      </xsl:choose>
                  </xsl:for-each>
                  allQuery.Add(query.Construct());
              }
              </xsl:if>
            </xsl:for-each>

            string unionAllQuery = string.Join("\nUNION\n", allQuery);

            <xsl:if test="count(Fields/Field[SortField = '1']) != 0">
                <xsl:text>unionAllQuery += "\nORDER BY </xsl:text>
                <xsl:for-each select="Fields/Field[SortField = '1']">
                    <xsl:if test="position() != 1">
                        <xsl:text>, </xsl:text>
                    </xsl:if>
                    <xsl:value-of select="Name"/>
                </xsl:for-each>
                <xsl:text>";</xsl:text>
            </xsl:if>

            var recordResult = await Config.Kernel.DataBase.SelectRequest(unionAllQuery, paramQuery);

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (Dictionary&lt;string, object&gt; row in recordResult.ListRow)
            {
                Журнали_<xsl:value-of select="$JournalName"/> record = new Журнали_<xsl:value-of select="$JournalName"/>
                {
                    ID = row["uid"].ToString() ?? "",
                    Type = row["type"].ToString() ?? "",
                    DeletionLabel = (bool)row["deletion_label"],
                    Spend = (bool)row["spend"],
                    <xsl:for-each select="Fields/Field">
                        <xsl:value-of select="Name"/> = row["<xsl:value-of select="Name"/>"] != DBNull.Value ? (row["<xsl:value-of select="Name"/>"].ToString() ?? "") : "",
                    </xsl:for-each>
                };

                TreeIter CurrentIter = Store.AppendValues(record.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                {
                    if (record.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
                }
            }
          </xsl:if>
        }
    }
    #endregion
    </xsl:for-each>
}

namespace <xsl:value-of select="Configuration/NameSpaceGenerationCode"/>.РегістриВідомостей.ТабличніСписки
{
    <xsl:for-each select="Configuration/RegistersInformation/RegisterInformation">
      <xsl:variable name="RegisterName" select="Name"/>
    #region REGISTER "<xsl:value-of select="$RegisterName"/>"
    
      <xsl:for-each select="TabularLists/TabularList">
        <xsl:variable name="TabularListName" select="Name"/>
    public class <xsl:value-of select="$RegisterName"/>_<xsl:value-of select="$TabularListName"/> : ТабличнийСписок
    {
        string ID = "";
        string Період = "";
        <xsl:for-each select="Fields/Field">
        string <xsl:value-of select="Name"/> = "";</xsl:for-each>

        Array ToArray()
        {
            return new object[] 
            { 
                InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Період,
                <xsl:for-each select="Fields/Field">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/><xsl:text>*/ </xsl:text><xsl:value-of select="Name"/>,
                </xsl:for-each> 
            };
        }

        public static void AddColumns(TreeView treeView)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Період*/ typeof(string),
                <xsl:for-each select="Fields/Field">
                    <xsl:text>/*</xsl:text><xsl:value-of select="Name"/>*/ typeof(string),
                </xsl:for-each>
            ]);

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 2));
            /* */
            <xsl:for-each select="Fields/Field">
              <xsl:text>treeView.AppendColumn(new TreeViewColumn("</xsl:text><xsl:value-of select="normalize-space(Caption)"/>
              <xsl:text>", new CellRendererText() { Xpad = 4 }, "text", </xsl:text><xsl:value-of select="position() + 2"/>
              <xsl:text>) { MinWidth = 20, Resizable = true, SortColumnId = </xsl:text><xsl:value-of select="position() + 2"/>
              <xsl:if test="Size != '0'"><xsl:value-of select="concat(', FixedWidth = ', Size)"/></xsl:if> } ); /*<xsl:value-of select="Name"/>*/
            </xsl:for-each>
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду("period", типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView)
        {
            SelectPath = CurrentPath = null;

            РегістриВідомостей.<xsl:value-of select="$RegisterName"/>_RecordsSet <xsl:value-of select="$RegisterName"/>_RecordsSet = new РегістриВідомостей.<xsl:value-of select="$RegisterName"/>_RecordsSet();
            <xsl:value-of select="$RegisterName"/>_RecordsSet.FillJoin(["period"]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) <xsl:value-of select="$RegisterName"/>_RecordsSet.QuerySelect.Where = (List&lt;Where&gt;)where;

            await <xsl:value-of select="$RegisterName"/>_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (<xsl:value-of select="$RegisterName"/>_RecordsSet.Record record in <xsl:value-of select="$RegisterName"/>_RecordsSet.Records)
            {
                <xsl:value-of select="$RegisterName"/>_<xsl:value-of select="$TabularListName"/> row = new <xsl:value-of select="$RegisterName"/>_<xsl:value-of select="$TabularListName"/>
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    <xsl:for-each select="Fields/Field">
                      <xsl:value-of select="Name"/><xsl:text> = </xsl:text>
                      <xsl:choose>
                        <xsl:when test="Type = 'pointer'">
                          <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>.Назва,
                        </xsl:when>
                        <xsl:when test="Type = 'enum'">
                          <xsl:text>Перелічення.ПсевдонімиПерелічення.</xsl:text><xsl:value-of select="substring-after(Pointer, '.')"/>_Alias((
                            <xsl:text>(</xsl:text><xsl:value-of select="Pointer"/>)(record.<xsl:value-of select="Name"/> != DBNull.Value ? record.<xsl:value-of select="Name"/> : 0)) ),
                        </xsl:when>
                        <xsl:when test="Type = 'boolean'">
                            <xsl:text>(record.</xsl:text><xsl:value-of select="Name"/> != DBNull.Value ? (bool)record.<xsl:value-of select="Name"/> : false) ? "Так" : "",
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>.ToString() ?? "",
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:for-each>
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                    if (row.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
            }
        }
    }
	    </xsl:for-each>
    #endregion
    </xsl:for-each>
}

namespace <xsl:value-of select="Configuration/NameSpaceGenerationCode"/>.РегістриНакопичення.ТабличніСписки
{
    <xsl:for-each select="Configuration/RegistersAccumulation/RegisterAccumulation">
      <xsl:variable name="RegisterName" select="Name"/>
    #region REGISTER "<xsl:value-of select="$RegisterName"/>"
    
      <xsl:for-each select="TabularLists/TabularList">
        <xsl:variable name="TabularListName" select="Name"/>
    public class <xsl:value-of select="$RegisterName"/>_<xsl:value-of select="$TabularListName"/> : ТабличнийСписок
    {
        string ID = "";
        bool Income = false;
        string Період = "";
        string Документ = "";
        <xsl:for-each select="Fields/Field">
        string <xsl:value-of select="Name"/> = "";</xsl:for-each>

        Array ToArray()
        {
            return new object[] 
            { 
                InterfaceGtk.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Income ? "+" : "-", 
                Період, 
                Документ,
                <xsl:for-each select="Fields/Field">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/><xsl:text>*/ </xsl:text><xsl:value-of select="Name"/>,
                </xsl:for-each> 
            };
        }

        public static void AddColumns(TreeView treeView, string[]? hiddenColumn = null)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Income*/ typeof(string), 
                /*Період*/ typeof(string),
                /*Документ*/ typeof(string),
                <xsl:for-each select="Fields/Field">
                    <xsl:text>/*</xsl:text><xsl:value-of select="Name"/>*/ typeof(string),
                </xsl:for-each>
            ]);

            bool IsHiddenColumn(string column){ return hiddenColumn != null ? !hiddenColumn.Contains(column) : true; }

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Рух", new CellRendererText() { Xalign = 0.5f }, "text", 2) { Visible = IsHiddenColumn("income") });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 3) { Visible = IsHiddenColumn("period") });
            treeView.AppendColumn(new TreeViewColumn("Документ", new CellRendererText(), "text", 4) { Visible = IsHiddenColumn("owner") });
            /* */
            <xsl:for-each select="Fields/Field">
              <xsl:text>treeView.AppendColumn(new TreeViewColumn("</xsl:text><xsl:value-of select="normalize-space(Caption)"/>
              <xsl:text>", new CellRendererText() { Xpad = 4 }, "text", </xsl:text><xsl:value-of select="position() + 4"/>
              <xsl:text>) { MinWidth = 20, Resizable = true, SortColumnId = </xsl:text><xsl:value-of select="position() + 4"/>
              <xsl:if test="Size != '0'"><xsl:value-of select="concat(', FixedWidth = ', Size)"/></xsl:if> } ); /*<xsl:value-of select="Name"/>*/
            </xsl:for-each>
            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void ДодатиВідбірПоПеріоду(TreeView treeView, ПеріодДляЖурналу.ТипПеріоду типПеріоду, DateTime? start = null, DateTime? stop = null)
        {
            ОчиститиВідбір(treeView);
            Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду("period", типПеріоду, start, stop);
            if (where != null) ДодатиВідбір(treeView, where);               
        }

        public static void ДодатиВідбірПоДокументу(TreeView treeView, Guid owner)
        {
            ДодатиВідбір(treeView, new Where("owner", Comparison.EQ, owner), true);
        }

        public static UnigueID? SelectPointerItem { get; set; }
        public static TreePath? SelectPath;
        public static TreePath? CurrentPath;

        public static async ValueTask LoadRecords(TreeView treeView, bool docname_required = true)
        {
            SelectPath = CurrentPath = null;

            РегістриНакопичення.<xsl:value-of select="$RegisterName"/>_RecordsSet <xsl:value-of select="$RegisterName"/>_RecordsSet = new РегістриНакопичення.<xsl:value-of select="$RegisterName"/>_RecordsSet();
             <xsl:value-of select="$RegisterName"/>_RecordsSet.FillJoin(["period"], docname_required);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) <xsl:value-of select="$RegisterName"/>_RecordsSet.QuerySelect.Where = (List&lt;Where&gt;)where;

            /* Read */
            await <xsl:value-of select="$RegisterName"/>_RecordsSet.Read();

            ListStore Store = (ListStore)treeView.Model;
            Store.Clear();

            foreach (<xsl:value-of select="$RegisterName"/>_RecordsSet.Record record in <xsl:value-of select="$RegisterName"/>_RecordsSet.Records)
            {
                <xsl:value-of select="$RegisterName"/>_<xsl:value-of select="$TabularListName"/> row = new <xsl:value-of select="$RegisterName"/>_<xsl:value-of select="$TabularListName"/>
                {
                    ID = record.UID.ToString(),
                    Період = record.Period.ToString(),
                    Income = record.Income,
                    Документ = record.OwnerName,
                    <xsl:for-each select="Fields/Field">
                      <xsl:value-of select="Name"/><xsl:text> = </xsl:text>
                      <xsl:choose>
                        <xsl:when test="Type = 'pointer'">
                          <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>.Назва,
                        </xsl:when>
                        <xsl:when test="Type = 'enum'">
                          <xsl:text>Перелічення.ПсевдонімиПерелічення.</xsl:text>
                          <xsl:value-of select="substring-after(Pointer, '.')"/>_Alias(record.<xsl:value-of select="Name"/>),
                        </xsl:when>
                        <xsl:when test="Type = 'boolean'">
                            <xsl:text>record.</xsl:text><xsl:value-of select="Name"/> ? "Так" : "",
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>.ToString() ?? "",
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:for-each>
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);

                if (SelectPointerItem != null)
                    if (row.ID == SelectPointerItem.ToString())
                        SelectPath = CurrentPath;
            }
        }
    }
	    </xsl:for-each>
    #endregion
    </xsl:for-each>
}

  </xsl:template>
</xsl:stylesheet>