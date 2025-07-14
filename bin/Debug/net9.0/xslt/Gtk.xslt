<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" indent="yes" />

  <xsl:template name="License">
/*
Copyright (C) 2019-2025 TARAKHOMYN YURIY IVANOVYCH
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
using InterfaceGtk3;
using AccountingSoftware;
using <xsl:value-of select="Configuration/NameSpaceGeneratedCode"/>.Перелічення;
using <xsl:value-of select="Configuration/NameSpace"/>;

namespace <xsl:value-of select="Configuration/NameSpaceGeneratedCode"/>.Довідники.ТабличніСписки
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
      <xsl:variable name="IconTree">
          <xsl:choose>
              <xsl:when test="$DirectoryType = 'Hierarchical' and IconTree = 'Folder'">ДляДерева</xsl:when>
              <xsl:otherwise>ДляТабличногоСписку</xsl:otherwise>
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

        <xsl:for-each select="Fields/AdditionalField[Visible = 'True']">
        string <xsl:value-of select="Name"/> = "";</xsl:for-each>

        object[] ToArray()
        {
            return
            [
                DeletionLabel ? InterfaceGtk3.Іконки.<xsl:value-of select="$IconTree"/>.Delete : InterfaceGtk3.Іконки.<xsl:value-of select="$IconTree"/>.Normal,
                ID,
                <xsl:for-each select="Fields/Field">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/>*/ <xsl:value-of select="Name"/>,
                </xsl:for-each>
                <xsl:for-each select="Fields/AdditionalField[Visible = 'True']">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/>*/ <xsl:value-of select="Name"/>,
                </xsl:for-each>
            ];
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
                <xsl:for-each select="Fields/AdditionalField[Visible = 'True']">
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
            <xsl:for-each select="Fields/AdditionalField[Visible = 'True']">
              <xsl:text>treeView.AppendColumn(new TreeViewColumn("</xsl:text><xsl:value-of select="normalize-space(Caption)"/>
              <xsl:text>", new CellRendererText() { Xpad = 4 }, "text", </xsl:text><xsl:value-of select="$CountField + position()"/>
              <xsl:text>) { MinWidth = 20, Resizable = true, SortColumnId = </xsl:text><xsl:value-of select="$CountField + position()"/>
              <xsl:if test="Size != '0'"><xsl:value-of select="concat(', FixedWidth = ', Size)"/></xsl:if> } ); /*<xsl:value-of select="Name"/>*/
            </xsl:for-each>

            //Пустишка
            treeView.AppendColumn(new TreeViewColumn());
        }

        public static void CreateFilter(TreeView treeView, ListFilterControl filterControl)
        {
          <xsl:if test="count(Fields/Field[FilterField = 'True']) != 0">
            List&lt;ListFilterControl.FilterListItem&gt; filterList = [];
            <xsl:for-each select="Fields/Field[FilterField = 'True']">
            { /* <xsl:value-of select="Name"/>, <xsl:value-of select="Type"/> */
                Switch sw = new();
                <xsl:choose>
                    <xsl:when test="Type = 'string'">
                        Entry <xsl:value-of select="Name"/> = new() { WidthRequest = 300 };
                        object get() =&gt; <xsl:value-of select="Name"/>.Text;
                    </xsl:when>
                    <xsl:when test="Type = 'boolean'">
                        CheckButton <xsl:value-of select="Name"/> = new();
                        <xsl:value-of select="Name"/>.Clicked += (sender, args) =&gt; sw.Active = <xsl:value-of select="Name"/>.Active;
                        object get() =&gt; <xsl:value-of select="Name"/>.Active;
                    </xsl:when>
                    <xsl:when test="Type = 'integer'">
                        IntegerControl <xsl:value-of select="Name"/> = new();
                        object get() =&gt; <xsl:value-of select="Name"/>.Value;
                    </xsl:when>
                    <xsl:when test="Type = 'numeric'">
                        NumericControl <xsl:value-of select="Name"/> = new();
                        object get() =&gt; <xsl:value-of select="Name"/>.Value;
                    </xsl:when>
                    <xsl:when test="Type = 'date' or Type = 'datetime'">
                        DateTimeControl <xsl:value-of select="Name"/> = new() { <xsl:if test="Type = 'date'">OnlyDate = true</xsl:if> };
                        object get() =&gt; <xsl:value-of select="Name"/>.Value;
                    </xsl:when>
                    <xsl:when test="Type = 'time'">
                        TimeControl <xsl:value-of select="Name"/> = new();
                        object get() =&gt; <xsl:value-of select="Name"/>.Value;
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:value-of select="substring-after(Pointer, '.')"/>_PointerControl <xsl:value-of select="Name"/> = new() { Caption = "", AfterSelectFunc = () =&gt; sw.Active = true };
                        object get() =&gt; <xsl:value-of select="Name"/>.Pointer.UnigueID.UGuid;
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                        ComboBoxText <xsl:value-of select="Name"/> = new();
                        foreach (var item in ПсевдонімиПерелічення.<xsl:value-of select="substring-after(Pointer, '.')"/>_List())
                            <xsl:value-of select="Name"/>.Append(item.Value.ToString(), item.Name);
                        <xsl:value-of select="Name"/>.Active = 0;
                        object get() =&gt; (int)Enum.Parse&lt;<xsl:value-of select="substring-after(Pointer, '.')"/>&gt;(<xsl:value-of select="Name"/>.ActiveId);
                    </xsl:when>
                    <xsl:otherwise>
                        Label <xsl:value-of select="Name"/> = new("<xsl:value-of select="Type"/>");
                        object get() =&gt; new();
                    </xsl:otherwise>
                </xsl:choose>
                filterList.Add(new(<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="Name"/>, get, sw));
                filterControl.Append("<xsl:value-of select="normalize-space(Caption)"/>:", <xsl:value-of select="Name"/>, sw);
            }
            </xsl:for-each>
            filterControl.GetWhere = () =&gt;
            {
                List&lt;Where&gt; listWhere = [];
                ДодатиВідбір(treeView, listWhere, true);
                foreach (var filter in filterList)
                    if (filter.IsOn.Active)
                        listWhere.Add(new Where(filter.Field, Comparison.EQ, filter.GetValueFunc.Invoke()));

                return listWhere.Count != 0;
            };
          </xsl:if>
        }

        public static async ValueTask UpdateRecords(TreeView treeView, List&lt;ObjectChanged&gt; recordsChanged)
        {
            <xsl:value-of select="$StoreType"/> Store = (<xsl:value-of select="$StoreType"/>)treeView.Model;
            Dictionary&lt;Guid, (TreeIter Iter, TypeObjectChanged Type)&gt; records = [];

            //Update
            List&lt;ObjectChanged&gt; recordsChangedUpdate = [.. recordsChanged.Where(x =&gt; x.Type == TypeObjectChanged.Update)];
            void findIter(TreeIter iter)
            {
                do
                {
                    Guid uid = Guid.Parse((string)Store.GetValue(iter, 1));
                    if (recordsChangedUpdate.Any(x =&gt; x.Uid == uid)) records.Add(uid, (iter, TypeObjectChanged.Update));
                    <xsl:if test="$DirectoryType = 'Hierarchical'">
                    if (Store.IterChildren(out TreeIter iterChildren, iter)) findIter(iterChildren);
                    </xsl:if>
                }
                while (Store.IterNext(ref iter));
            }
            if (Store.GetIterFirst(out TreeIter iter)) findIter(iter);

            if (records.Count &gt; 0)
            {
                Довідники.<xsl:value-of select="$DirectoryName"/>_Select<xsl:text> </xsl:text><xsl:value-of select="$DirectoryName"/>_Select = new Довідники.<xsl:value-of select="$DirectoryName"/>_Select();
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

                <xsl:for-each select="Fields/Field[Type = 'date']">
                    <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.SqlFunc.Add(new SqlFunc(Довідники.<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="Name"/>, "TO_CHAR", ["'dd.mm.yyyy'"]));
                </xsl:for-each>

                <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.Where.Add(new Where("uid", Comparison.IN, "'" + string.Join("', '", records.Select(x =&gt; x.Key)) + "'", true));

                <xsl:for-each select="Fields/Field[Type = 'pointer']">
                    <xsl:value-of select="substring-before(Pointer, '.')"/>.<xsl:value-of select="substring-after(Pointer, '.')"/>_Pointer.GetJoin(<xsl:value-of select="$DirectoryName"/>_Select.QuerySelect, Довідники.<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="Name"/>,
                    <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.Table, "join_tab_<xsl:value-of select="position()"/>", "<xsl:value-of select="Name"/>");
                </xsl:for-each>

                <xsl:for-each select="Fields/AdditionalField[Visible = 'True']">
                    /* Additional Field */
                    <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.FieldAndAlias.Add(
                    new ValueName&lt;string&gt;(@$"(<xsl:value-of select="normalize-space(Value)"/>)", "<xsl:value-of select="Name"/>"));
                </xsl:for-each>

                /* SELECT */
                await <xsl:value-of select="$DirectoryName"/>_Select.Select();

                while (<xsl:value-of select="$DirectoryName"/>_Select.MoveNext())
                {
                    Довідники.<xsl:value-of select="$DirectoryName"/>_Pointer? current = <xsl:value-of select="$DirectoryName"/>_Select.Current;
                    if (current != null)
                    {
                        Dictionary&lt;string, object&gt; Fields = current.Fields;
                        <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/> Record = new <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/>
                        {
                            ID = current.UnigueID.ToString(),
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
                            <xsl:for-each select="Fields/AdditionalField[Visible = 'True']">
                                <xsl:value-of select="Name"/> = Fields["<xsl:value-of select="Name"/>"].ToString() ?? "",
                            </xsl:for-each>
                        };
                        (TreeIter Iter, TypeObjectChanged Type) = records[current.UnigueID.UGuid];
                        Store.SetValues(Iter, Record.ToArray());
                    }
                }
            }
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? openFolder = null, 
          UnigueID? selectPointerItem = null, UnigueID? directoryPointerItem = null)
        {
            TreePath? /*FirstPath = null,*/ SelectPath = null, CurrentPath = null;
            UnigueID? unigueIDSelect = selectPointerItem ?? directoryPointerItem;
            <xsl:value-of select="$StoreType"/> Store = (<xsl:value-of select="$StoreType"/>)treeView.Model;
            
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

            <xsl:for-each select="Fields/Field[Type = 'date']">
                <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.SqlFunc.Add(new SqlFunc(Довідники.<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="Name"/>, "TO_CHAR", ["'dd.mm.yyyy'"]));
            </xsl:for-each>

            <xsl:if test="$DirectoryType = 'Hierarchical'">
            if (openFolder != null) 
                  ДодатиВідбір(treeView, new Where("uid", Comparison.NOT, openFolder.UGuid));
            </xsl:if>

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

            <xsl:for-each select="Fields/AdditionalField[Visible = 'True']">
                /* Additional Field */
                <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect.FieldAndAlias.Add(
                  new ValueName&lt;string&gt;(@$"(<xsl:value-of select="normalize-space(Value)"/>)", "<xsl:value-of select="Name"/>"));
                /*
                <xsl:value-of select="Value"/>
                */
            </xsl:for-each>

            <xsl:if test="$DirectoryType != 'Hierarchical'">
            /* Pages */
            var pages = treeView.Data["Pages"];
            Сторінки.Налаштування? settingsPages = pages != null ? (Сторінки.Налаштування)pages : null;
            if (settingsPages != null)
                await ЗаповнитиСторінки(<xsl:value-of select="$DirectoryName"/>_Select.SplitSelectToPages, settingsPages, <xsl:value-of select="$DirectoryName"/>_Select.QuerySelect, unigueIDSelect);
            </xsl:if>

            /* SELECT */
            await <xsl:value-of select="$DirectoryName"/>_Select.Select();
            Store.Clear();

            <xsl:if test="$DirectoryType = 'Hierarchical'">
            Dictionary&lt;string, TreeIter&gt; nodeDictionary = new();
            TreeIter rootIter = Store.AppendValues(new <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/>(){ ID = Guid.Empty.ToString() }.ToArray());
            TreePath rootPath = Store.GetPath(rootIter);
            </xsl:if>

            string? uidSelect = unigueIDSelect?.ToString();
            while (<xsl:value-of select="$DirectoryName"/>_Select.MoveNext())
            {
                Довідники.<xsl:value-of select="$DirectoryName"/>_Pointer? current = <xsl:value-of select="$DirectoryName"/>_Select.Current;
                <xsl:if test="$DirectoryType = 'Hierarchical'">
                string Parent = <xsl:value-of select="$DirectoryName"/>_Select.Parent?.ToString() ?? Guid.Empty.ToString();
                int Level = <xsl:value-of select="$DirectoryName"/>_Select.Level;
                </xsl:if>
                if (current != null)
                {
                    Dictionary&lt;string, object&gt; Fields = current.Fields;
                    <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/> Record = new <xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularListName"/>
                    {
                        ID = current.UnigueID.ToString(),
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
                        <xsl:for-each select="Fields/AdditionalField[Visible = 'True']">
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
                    /*FirstPath ??= CurrentPath;*/
                    if (uidSelect != null &amp;&amp; Record.ID == uidSelect) SelectPath = CurrentPath;
                }
            }
            <xsl:if test="$DirectoryType = 'Hierarchical'">
            treeView.ExpandToPath(rootPath);
            treeView.SetCursor(rootPath, treeView.Columns[0], false);
            if (SelectPath != null) treeView.ExpandToPath(SelectPath);
            </xsl:if>
            if (SelectPath != null) treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            <xsl:if test="$DirectoryType = 'Hierarchical'">
            treeView.ActivateRow(SelectPath != null ? SelectPath: rootPath, treeView.Columns[0]);
            </xsl:if>
        }
    }
	    </xsl:for-each>
    #endregion
    </xsl:for-each>
}

namespace <xsl:value-of select="Configuration/NameSpaceGeneratedCode"/>.Документи.ТабличніСписки
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

        object[] ToArray()
        {
            return
            [ 
                DeletionLabel ? InterfaceGtk3.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk3.Іконки.ДляТабличногоСписку.Normal,
                ID, 
                /*Проведений документ*/ Spend, 
                <xsl:for-each select="Fields/Field">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/>*/ <xsl:value-of select="Name"/>,
                </xsl:for-each>
            ];
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

        public static void CreateFilter(TreeView treeView, ListFilterControl filterControl)
        {
          <xsl:if test="count(Fields/Field[FilterField = 'True']) != 0">
            List&lt;ListFilterControl.FilterListItem&gt; filterList = [];
            <xsl:for-each select="Fields/Field[FilterField = 'True']">
            { /* <xsl:value-of select="Name"/>, <xsl:value-of select="Type"/> */
                Switch sw = new();
                <xsl:choose>
                    <xsl:when test="Type = 'string'">
                        Entry <xsl:value-of select="Name"/> = new() { WidthRequest = 300 };
                        object get() =&gt; <xsl:value-of select="Name"/>.Text;
                    </xsl:when>
                    <xsl:when test="Type = 'boolean'">
                        CheckButton <xsl:value-of select="Name"/> = new();
                        <xsl:value-of select="Name"/>.Clicked += (sender, args) =&gt; sw.Active = <xsl:value-of select="Name"/>.Active;
                        object get() =&gt; <xsl:value-of select="Name"/>.Active;
                    </xsl:when>
                    <xsl:when test="Type = 'integer'">
                        IntegerControl <xsl:value-of select="Name"/> = new();
                        object get() =&gt; <xsl:value-of select="Name"/>.Value;
                    </xsl:when>
                    <xsl:when test="Type = 'numeric'">
                        NumericControl <xsl:value-of select="Name"/> = new();
                        object get() =&gt; <xsl:value-of select="Name"/>.Value;
                    </xsl:when>
                    <xsl:when test="Type = 'date' or Type = 'datetime'">
                        DateTimeControl <xsl:value-of select="Name"/> = new() { <xsl:if test="Type = 'date'">OnlyDate = true</xsl:if> };
                        object get() =&gt; <xsl:value-of select="Name"/>.Value;
                    </xsl:when>
                    <xsl:when test="Type = 'time'">
                        TimeControl <xsl:value-of select="Name"/> = new();
                        object get() =&gt; <xsl:value-of select="Name"/>.Value;
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:value-of select="substring-after(Pointer, '.')"/>_PointerControl <xsl:value-of select="Name"/> = new() { Caption = "", AfterSelectFunc = () =&gt; sw.Active = true };
                        object get() =&gt; <xsl:value-of select="Name"/>.Pointer.UnigueID.UGuid;
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                        ComboBoxText <xsl:value-of select="Name"/> = new();
                        foreach (var item in ПсевдонімиПерелічення.<xsl:value-of select="substring-after(Pointer, '.')"/>_List())
                            <xsl:value-of select="Name"/>.Append(item.Value.ToString(), item.Name);
                        <xsl:value-of select="Name"/>.Active = 0;
                        object get() =&gt; (int)Enum.Parse&lt;<xsl:value-of select="substring-after(Pointer, '.')"/>&gt;(<xsl:value-of select="Name"/>.ActiveId);
                    </xsl:when>
                    <xsl:otherwise>
                        Label <xsl:value-of select="Name"/> = new("<xsl:value-of select="Type"/>");
                        object get() =&gt; new();
                    </xsl:otherwise>
                </xsl:choose>
                filterList.Add(new(<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Name"/>, get, sw));
                filterControl.Append("<xsl:value-of select="normalize-space(Caption)"/>:", <xsl:value-of select="Name"/>, sw);
            }
            </xsl:for-each>
            filterControl.GetWhere = () =&gt;
            {
                List&lt;Where&gt; listWhere = [];
                ДодатиВідбір(treeView, listWhere, true);
                foreach (var filter in filterList)
                    if (filter.IsOn.Active)
                        listWhere.Add(new Where(filter.Field, Comparison.EQ, filter.GetValueFunc.Invoke()));

                bool existFilter = listWhere.Count != 0;
                if (existFilter &amp;&amp; filterControl.Період != null &amp;&amp; filterControl.UsePeriod.Active)
                {
                    Where? where = ПеріодДляЖурналу.ВідбірПоПеріоду(Документи.<xsl:value-of select="$DocumentName"/>_Const.ДатаДок, filterControl.Період.Period, filterControl.Період.DateStart, filterControl.Період.DateStop);
                    if (where != null) listWhere.Add(where);
                }

                return existFilter;
            };
          </xsl:if>
        }

        public static async ValueTask UpdateRecords(TreeView treeView, List&lt;ObjectChanged&gt; recordsChanged)
        {
            ListStore Store = (ListStore)treeView.Model;
            Dictionary&lt;Guid, (TreeIter Iter, TypeObjectChanged Type)&gt; records = [];

            //Update
            List&lt;ObjectChanged&gt; recordsChangedUpdate = [.. recordsChanged.Where(x =&gt; x.Type == TypeObjectChanged.Update)];
            if (Store.GetIterFirst(out TreeIter iter)) 
                do
                {
                    Guid uid = Guid.Parse((string)Store.GetValue(iter, 1));
                    if (recordsChangedUpdate.Any(x =&gt; x.Uid == uid)) records.Add(uid, (iter, TypeObjectChanged.Update));
                }
                while (Store.IterNext(ref iter));

            if (records.Count &gt; 0)
            {
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

                <xsl:for-each select="Fields/Field[Type = 'date']">
                    <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.SqlFunc.Add(new SqlFunc(Документи.<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Name"/>, "TO_CHAR", ["'dd.mm.yyyy'"]));
                </xsl:for-each>

                <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.Where.Add(new Where("uid", Comparison.IN, "'" + string.Join("', '", records.Select(x =&gt; x.Key)) + "'", true));

                <xsl:for-each select="Fields/Field[Type = 'pointer']">
                    <xsl:value-of select="substring-before(Pointer, '.')"/>.<xsl:value-of select="substring-after(Pointer, '.')"/>_Pointer.GetJoin(<xsl:value-of select="$DocumentName"/>_Select.QuerySelect, Документи.<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Name"/>,
                    <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.Table, "join_tab_<xsl:value-of select="position()"/>", "<xsl:value-of select="Name"/>");
                </xsl:for-each>

                /* SELECT */
                await <xsl:value-of select="$DocumentName"/>_Select.Select();
                
                while (<xsl:value-of select="$DocumentName"/>_Select.MoveNext())
                {
                    Документи.<xsl:value-of select="$DocumentName"/>_Pointer? current = <xsl:value-of select="$DocumentName"/>_Select.Current;
                    if (current != null)
                    {
                        Dictionary&lt;string, object&gt; Fields = current.Fields;
                        <xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularListName"/> Record = new <xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularListName"/>
                        {
                            ID = current.UnigueID.ToString(),
                            Spend = (bool)Fields["spend"], /*Проведений документ*/
                            DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
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
                        (TreeIter Iter, TypeObjectChanged Type) = records[current.UnigueID.UGuid];
                        Store.SetValues(Iter, Record.ToArray());
                    }
                }
            }
        }

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? selectPointerItem = null, UnigueID? directoryPointerItem = null)
        {
            TreePath? /*FirstPath = null,*/ SelectPath = null, CurrentPath = null;
            UnigueID? unigueIDSelect = selectPointerItem ?? directoryPointerItem;
            ListStore Store = (ListStore)treeView.Model;

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

            <xsl:for-each select="Fields/Field[Type = 'date']">
                <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.SqlFunc.Add(new SqlFunc(Документи.<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Name"/>, "TO_CHAR", ["'dd.mm.yyyy'"]));
            </xsl:for-each>

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) <xsl:value-of select="$DocumentName"/>_Select.QuerySelect.Where = (List&lt;Where&gt;)where;

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

            /* Pages */
            var pages = treeView.Data["Pages"];
            Сторінки.Налаштування? settingsPages = pages != null ? (Сторінки.Налаштування)pages : null;
            if (settingsPages != null)
                await ЗаповнитиСторінки(<xsl:value-of select="$DocumentName"/>_Select.SplitSelectToPages, settingsPages, <xsl:value-of select="$DocumentName"/>_Select.QuerySelect, unigueIDSelect);

            /* SELECT */
            await <xsl:value-of select="$DocumentName"/>_Select.Select();
            Store.Clear();

            string? uidSelect = unigueIDSelect?.ToString();
            while (<xsl:value-of select="$DocumentName"/>_Select.MoveNext())
            {
                Документи.<xsl:value-of select="$DocumentName"/>_Pointer? current = <xsl:value-of select="$DocumentName"/>_Select.Current;

                if (current != null)
                {
                    Dictionary&lt;string, object&gt; Fields = current.Fields;
                    <xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularListName"/> Record = new <xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularListName"/>
                    {
                        ID = current.UnigueID.ToString(),
                        Spend = (bool)Fields["spend"], /*Проведений документ*/
                        DeletionLabel = (bool)Fields["deletion_label"], /*Помітка на видалення*/
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
                    /*FirstPath ??= CurrentPath;*/
                    if (uidSelect != null &amp;&amp; Record.ID == uidSelect) SelectPath = CurrentPath;
                }
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null &amp;&amp; settingsPages != null &amp;&amp; settingsPages.CurrentPage == settingsPages.Record.Pages) //Для останньої сторінки
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
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
    
    public class Журнали_<xsl:value-of select="$JournalName"/> : ТабличнийСписок
    {
        bool DeletionLabel = false;
        bool Spend = false;
        string ID = "";
        string Type = ""; //Тип документу
        <xsl:for-each select="Fields/Field">
        string <xsl:value-of select="Name"/> = "";</xsl:for-each>

        // Масив для запису стрічки в Store
        object[] ToArray()
        {
            return 
            [
                DeletionLabel ? InterfaceGtk3.Іконки.ДляТабличногоСписку.Delete : InterfaceGtk3.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Type, 
                /*Проведений документ*/ Spend,
                <xsl:for-each select="Fields/Field">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/><xsl:text>*/ </xsl:text>
                  <xsl:value-of select="Name"/>,
                </xsl:for-each> 
            ];
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

        // Завантаження даних
        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? selectPointerItem = null) 
        {
            TreePath? SelectPath = null, CurrentPath = null;
            ListStore Store = (ListStore)treeView.Model;

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
                  
                  query.FieldAndAlias.Add(new ValueName&lt;string&gt;("'<xsl:value-of select="$DocumentName"/>'", "type"));
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
                              query.FieldAndAlias.Add(new ValueName&lt;string&gt;(Документи.<xsl:value-of select="$DocumentName"/>_Const.TABLE + "." + Документи.<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="DocField"/><xsl:value-of select="$SqlType"/>, "<xsl:value-of select="Name"/>"));
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>
                          query.FieldAndAlias.Add(new ValueName&lt;string&gt;("''", "<xsl:value-of select="Name"/>")); /* Empty Field - <xsl:value-of select="Name"/>*/
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

            /* Pages */
            var pages = treeView.Data["Pages"];
            Сторінки.Налаштування? settingsPages = pages != null ? (Сторінки.Налаштування)pages : null;
            if (settingsPages != null)
               unionAllQuery = await ЗаповнитиСторінки(Config.Kernel.DataBase.SplitSelectToPagesForJournal, settingsPages, unionAllQuery, paramQuery);

            var recordResult = await Config.Kernel.DataBase.SelectRequest(unionAllQuery, paramQuery);
            Store.Clear();

            string? uidSelect = selectPointerItem?.ToString();
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
                if (uidSelect != null &amp;&amp; record.ID == uidSelect) SelectPath = CurrentPath;
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null &amp;&amp; settingsPages != null &amp;&amp; settingsPages.CurrentPage == settingsPages.Record.Pages) //Для останньої сторінки
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
          </xsl:if>
        }
    }
    #endregion
    </xsl:for-each>
}

namespace <xsl:value-of select="Configuration/NameSpaceGeneratedCode"/>.РегістриВідомостей.ТабличніСписки
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

        object[] ToArray()
        {
            return
            [
                InterfaceGtk3.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Період,
                <xsl:for-each select="Fields/Field">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/><xsl:text>*/ </xsl:text><xsl:value-of select="Name"/>,
                </xsl:for-each> 
            ];
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

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? selectPointerItem = null)
        {
            TreePath? SelectPath = null, CurrentPath = null;
            ListStore Store = (ListStore)treeView.Model;

            РегістриВідомостей.<xsl:value-of select="$RegisterName"/>_RecordsSet <xsl:value-of select="$RegisterName"/>_RecordsSet = new РегістриВідомостей.<xsl:value-of select="$RegisterName"/>_RecordsSet();
            <xsl:value-of select="$RegisterName"/>_RecordsSet.FillJoin(["period"]);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) <xsl:value-of select="$RegisterName"/>_RecordsSet.QuerySelect.Where = (List&lt;Where&gt;)where;

            /* Pages */
            var pages = treeView.Data["Pages"];
            Сторінки.Налаштування? settingsPages = pages != null ? (Сторінки.Налаштування)pages : null;
            if (settingsPages != null)
                await ЗаповнитиСторінки(<xsl:value-of select="$RegisterName"/>_RecordsSet.SplitSelectToPages, settingsPages, <xsl:value-of select="$RegisterName"/>_RecordsSet.QuerySelect, selectPointerItem);

            await <xsl:value-of select="$RegisterName"/>_RecordsSet.Read();
            Store.Clear();
            
            string? uidSelect = selectPointerItem?.ToString();
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
                        <xsl:when test="Type = 'date'">
                            <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>.ToString("dd.MM.yyyy") ?? "",
                        </xsl:when>
                        <xsl:when test="Type = 'time'">
                            <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>.ToString(@"hh\:mm\:ss") ?? "",
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>.ToString() ?? "",
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:for-each>
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);
                if (uidSelect != null &amp;&amp; row.ID == uidSelect) SelectPath = CurrentPath;
            }
            if (SelectPath != null)
                treeView.SetCursor(SelectPath, treeView.Columns[0], false);
            else if (CurrentPath != null &amp;&amp; settingsPages != null &amp;&amp; settingsPages.CurrentPage == settingsPages.Record.Pages) //Для останньої сторінки
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);
        }
    }
	    </xsl:for-each>
    #endregion
    </xsl:for-each>
}

namespace <xsl:value-of select="Configuration/NameSpaceGeneratedCode"/>.РегістриНакопичення.ТабличніСписки
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
        string Period = "";
        string OwnerName = "";
        <xsl:for-each select="Fields/Field">
        string <xsl:value-of select="Name"/> = "";</xsl:for-each>

        object[] ToArray()
        {
            return
            [
                InterfaceGtk3.Іконки.ДляТабличногоСписку.Normal, 
                ID, 
                Income ? "+" : "-", 
                Period, 
                OwnerName,
                <xsl:for-each select="Fields/Field">
                  <xsl:text>/*</xsl:text><xsl:value-of select="Name"/><xsl:text>*/ </xsl:text><xsl:value-of select="Name"/>,
                </xsl:for-each> 
            ];
        }

        public static void AddColumns(TreeView treeView, string[]? hiddenColumn = null)
        {
            treeView.Model = new ListStore(
            [
                /*Image*/ typeof(Gdk.Pixbuf), 
                /*ID*/ typeof(string), 
                /*Income*/ typeof(string), 
                /*Period*/ typeof(string),
                /*OwnerName*/ typeof(string),
                <xsl:for-each select="Fields/Field">
                    <xsl:text>/*</xsl:text><xsl:value-of select="Name"/>*/ typeof(string),
                </xsl:for-each>
            ]);

            bool IsHiddenColumn(string column){ return hiddenColumn != null ? !hiddenColumn.Contains(column) : true; }

            treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0)); /* { Ypad = 0 } */
            treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
            treeView.AppendColumn(new TreeViewColumn("Рух", new CellRendererText() { Xalign = 0.5f }, "text", 2) { Visible = IsHiddenColumn("income") });
            treeView.AppendColumn(new TreeViewColumn("Період", new CellRendererText(), "text", 3) { Visible = IsHiddenColumn("period") });
            treeView.AppendColumn(new TreeViewColumn("Регістратор", new CellRendererText(), "text", 4) { Visible = IsHiddenColumn("owner") });
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

        public static async ValueTask LoadRecords(TreeView treeView, UnigueID? selectPointerItem = null, bool docname_required = true, bool position_last = true)
        {
            TreePath? SelectPath = null, CurrentPath = null;
            ListStore Store = (ListStore)treeView.Model;

            РегістриНакопичення.<xsl:value-of select="$RegisterName"/>_RecordsSet <xsl:value-of select="$RegisterName"/>_RecordsSet = new РегістриНакопичення.<xsl:value-of select="$RegisterName"/>_RecordsSet();
            <xsl:value-of select="$RegisterName"/>_RecordsSet.FillJoin(["period"], docname_required);

            /* Where */
            var where = treeView.Data["Where"];
            if (where != null) <xsl:value-of select="$RegisterName"/>_RecordsSet.QuerySelect.Where = (List&lt;Where&gt;)where;

            <xsl:for-each select="Fields/Field[SortField = 'True']">
              <xsl:variable name="SortDirection">
                  <xsl:choose>
                      <xsl:when test="SortDirection = 'True'">SelectOrder.DESC</xsl:when>
                      <xsl:otherwise>SelectOrder.ASC</xsl:otherwise>
                  </xsl:choose>
              </xsl:variable>
              <xsl:value-of select="$RegisterName"/>_RecordsSet.QuerySelect.Order.Add(
              <xsl:choose>
                  <xsl:when test="Type = 'pointer'">"<xsl:value-of select="Name"/>"</xsl:when>
                  <xsl:otherwise>РегістриНакопичення.<xsl:value-of select="$RegisterName"/>_Const.<xsl:value-of select="Name"/></xsl:otherwise>
              </xsl:choose>
              <xsl:text>, </xsl:text><xsl:value-of select="$SortDirection"/>);
            </xsl:for-each>

            /* Pages */
            var pages = treeView.Data["Pages"];
            Сторінки.Налаштування? settingsPages = pages != null ? (Сторінки.Налаштування)pages : null;
            if (settingsPages != null)
                await ЗаповнитиСторінки(<xsl:value-of select="$RegisterName"/>_RecordsSet.SplitSelectToPages, settingsPages, <xsl:value-of select="$RegisterName"/>_RecordsSet.QuerySelect, selectPointerItem);
                
            /* Read */
            await <xsl:value-of select="$RegisterName"/>_RecordsSet.Read();
            Store.Clear();

            string? uidSelect = selectPointerItem?.ToString();
            foreach (<xsl:value-of select="$RegisterName"/>_RecordsSet.Record record in <xsl:value-of select="$RegisterName"/>_RecordsSet.Records)
            {
                <xsl:value-of select="$RegisterName"/>_<xsl:value-of select="$TabularListName"/> row = new <xsl:value-of select="$RegisterName"/>_<xsl:value-of select="$TabularListName"/>
                {
                    ID = record.UID.ToString(),
                    Period = record.Period.ToString(),
                    Income = record.Income,
                    OwnerName = record.OwnerName,
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
                        <xsl:when test="Type = 'date'">
                            <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>.ToString("dd.MM.yyyy") ?? "",
                        </xsl:when>
                        <xsl:when test="Type = 'time'">
                            <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>.ToString(@"hh\:mm\:ss") ?? "",
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>.ToString() ?? "",
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:for-each>
                };

                TreeIter CurrentIter = Store.AppendValues(row.ToArray());
                CurrentPath = Store.GetPath(CurrentIter);
                if (uidSelect != null &amp;&amp; row.ID == uidSelect) SelectPath = CurrentPath;
            }
            if (position_last)
            {
                if (SelectPath != null)
                    treeView.SetCursor(SelectPath, treeView.Columns[0], false);
                /*else if (CurrentPath != null &amp;&amp; settingsPages != null &amp;&amp; settingsPages.CurrentPage == settingsPages.Record.Pages) //Для останньої сторінки
                treeView.SetCursor(CurrentPath, treeView.Columns[0], false);*/
            }
        }
    }
	    </xsl:for-each>
    #endregion
    </xsl:for-each>
}

  </xsl:template>
</xsl:stylesheet>