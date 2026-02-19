<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="text" indent="yes" />

    <!-- Файл -->
    <xsl:param name="File" />

    <!-- Простори імен -->
    <xsl:param name="NameSpaceGeneratedCode" />
    <xsl:param name="NameSpace" />

    <xsl:template match="root">

        <xsl:choose>

            <xsl:when test="$File = 'Triggers'">
                <xsl:call-template name="TablePartTriggers" />
            </xsl:when>
            <xsl:when test="$File = 'TablePart'">
                <xsl:call-template name="TablePart" />
            </xsl:when>
            <xsl:when test="$File = 'List'">
                <xsl:call-template name="TablePartList" />
            </xsl:when>
            <xsl:when test="$File = 'Report'">
                <xsl:call-template name="TablePartReport" />
            </xsl:when>
            
        </xsl:choose>

    </xsl:template>

    <!-- Для задання типу поля -->
    <xsl:template name="FieldType">
        <xsl:choose>
            <xsl:when test="Type = 'string'">string</xsl:when>
            <xsl:when test="Type = 'string[]'">string[]</xsl:when>
            <xsl:when test="Type = 'integer'">int</xsl:when>
            <xsl:when test="Type = 'integer[]'">int[]</xsl:when>
            <xsl:when test="Type = 'numeric'">decimal</xsl:when>
            <xsl:when test="Type = 'numeric[]'">decimal[]</xsl:when>
            <xsl:when test="Type = 'boolean'">bool</xsl:when>
            <xsl:when test="Type = 'time'">TimeSpan</xsl:when>
            <xsl:when test="Type = 'date' or Type = 'datetime'">DateTime</xsl:when>
            <xsl:when test="Type = 'pointer'"><xsl:value-of select="substring-after(Pointer, '.')"/>_Pointer</xsl:when>
            <xsl:when test="Type = 'any_pointer'">Guid</xsl:when>
            <xsl:when test="Type = 'composite_pointer'">UuidAndText</xsl:when>
            <xsl:when test="Type = 'composite_text'">NameAndText</xsl:when>
            <xsl:when test="Type = 'enum'"><xsl:value-of select="substring-after(Pointer, '.')"/></xsl:when>
            <xsl:when test="Type = 'bytea'">byte[]</xsl:when>
            <xsl:when test="Type = 'uuid[]'">Guid[]</xsl:when>
        </xsl:choose>    
    </xsl:template>

    <!-- Для конструкторів. Значення поля по замовчуванню відповідно до типу -->
    <xsl:template name="DefaultFieldValue">
        <xsl:choose>
            <xsl:when test="Type = 'string'">""</xsl:when>
            <xsl:when test="Type = 'string[]' or Type = 'integer[]' or Type = 'numeric[]' or Type = 'bytea' or Type = 'uuid[]'">[]</xsl:when>
            <xsl:when test="Type = 'integer' or Type = 'numeric' or Type = 'enum'">0</xsl:when>
            <xsl:when test="Type = 'boolean'">false</xsl:when>
            <xsl:when test="Type = 'time'">DateTime.MinValue.TimeOfDay</xsl:when>
            <xsl:when test="Type = 'date' or Type = 'datetime'">DateTime.MinValue</xsl:when>
            <xsl:when test="Type = 'pointer'">new()<!--<xsl:value-of select="substring-after(Pointer, '.')"/>_Pointer()--></xsl:when>
            <xsl:when test="Type = 'any_pointer'">new()<!--Guid()--></xsl:when>
            <xsl:when test="Type = 'composite_pointer'">new()<!--UuidAndText()--></xsl:when>
            <xsl:when test="Type = 'composite_text'">new()<!--NameAndText()--></xsl:when>
        </xsl:choose>
    </xsl:template>

<!--- 
//
// ============================ Triggers ============================
//
-->

    <xsl:template name="TablePartTriggers">
        <xsl:variable name="TablePartName" select="TablePart/Name"/>

        <!-- Назви функцій -->
        <xsl:variable name="TriggerFunctions" select="TablePart/TriggerFunctions"/>

        <xsl:variable name="OwnerTypeName">
            <xsl:choose>
                <xsl:when test="TablePart/OwnerType = 'Directory'">Довідник</xsl:when>
                <xsl:when test="TablePart/OwnerType = 'Document'">Документ</xsl:when>
                <xsl:otherwise>[...]</xsl:otherwise>
            </xsl:choose>
        </xsl:variable>
        <xsl:variable name="OwnerTypeNameSpace">
            <xsl:choose>
                <xsl:when test="TablePart/OwnerType = 'Directory'">Довідники</xsl:when>
                <xsl:when test="TablePart/OwnerType = 'Document'">Документи</xsl:when>
                <xsl:otherwise>[...]</xsl:otherwise>
            </xsl:choose>
        </xsl:variable>
        <xsl:variable name="OwnerName" select="TablePart/OwnerName"/>

/*
        <xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_Triggers.cs
        Тригери табличної частини <xsl:value-of select="$TablePartName"/>
*/

using AccountingSoftware;

namespace <xsl:value-of select="$NameSpaceGeneratedCode"/>.<xsl:value-of select="$OwnerTypeNameSpace"/>;

static class <xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_Triggers
{
    public static async ValueTask <xsl:value-of select="$TriggerFunctions/BeforeSave"/>(<xsl:value-of select="$OwnerName"/>_Objest <xsl:value-of select="$OwnerTypeName"/>Обєкт, <xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart ТабличнаЧастина)
    {
        await ValueTask.FromResult(true);
    }

    public static async ValueTask <xsl:value-of select="$TriggerFunctions/AfterSave"/>(<xsl:value-of select="$OwnerName"/>_Objest <xsl:value-of select="$OwnerTypeName"/>Обєкт, <xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart ТабличнаЧастина)
    {
        await ValueTask.FromResult(true);
    }
}
    </xsl:template>

<!--- 
//
// ============================ Таблична Частина ============================
//
-->

    <!-- Таблична Частина -->
    <xsl:template name="TablePart">
        <xsl:variable name="TablePartName" select="TablePart/Name"/>
        <xsl:variable name="IncludeIconColumn" select="TablePart/IncludeIconColumn"/>
        <xsl:variable name="FieldsTL" select="TablePart/ElementFields/Field"/>

        <xsl:variable name="OwnerExist" select="TablePart/OwnerExist"/>
        <xsl:variable name="OwnerType" select="TablePart/OwnerType"/>
        <xsl:variable name="OwnerName" select="TablePart/OwnerName"/>
        <xsl:variable name="OwnerBlockName">
            <xsl:if test="TablePart/OwnerType = 'Constants' and normalize-space(TablePart/OwnerBlockName) != ''">
                <xsl:value-of select="concat(normalize-space(TablePart/OwnerBlockName), '.')"/>
            </xsl:if>
        </xsl:variable>
/*
        <xsl:value-of select="$OwnerName"/>_ТабличнаЧастина_<xsl:value-of select="$TablePartName"/>.cs
        Таблична Частина
*/

using Gtk;
using InterfaceGtk4;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Документи;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Перелічення;

namespace <xsl:value-of select="$NameSpace"/>;

class <xsl:value-of select="$OwnerName"/>_ТабличнаЧастина_<xsl:value-of select="$TablePartName"/> : <xsl:value-of select="$OwnerType"/>FormTablePart
{
    <xsl:choose>
        <xsl:when test="$OwnerType = 'Constants'">
    <xsl:value-of select="$OwnerBlockName"/><xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart Таблиця { get; set; } = new();
        </xsl:when>
        <xsl:otherwise>
    public <xsl:value-of select="$OwnerName"/>_Objest? ЕлементВласник { get; set; }
        </xsl:otherwise>
    </xsl:choose>
    
    #region Data

    class ItemRow : RowTablePart
    {
    <xsl:for-each select="$FieldsTL">
        //
        // <xsl:value-of select="Name"/>
        //
        <xsl:text>public </xsl:text>
        <xsl:call-template name="FieldType" />
        <xsl:text> </xsl:text>
        <xsl:value-of select="Name"/>
        {
            get =&gt; <xsl:value-of select="Name"/>_;
            set
            {
                if (!<xsl:value-of select="Name"/>_.Equals(value))
                {
                    <xsl:value-of select="Name"/>_ = value;
                    Сhanged_<xsl:value-of select="Name"/>?.Invoke();
                }
            }
        }
        <xsl:call-template name="FieldType" />
        <xsl:text> </xsl:text>
        <xsl:value-of select="Name"/>
        <xsl:text>_ = </xsl:text>
        <xsl:call-template name="DefaultFieldValue" />;
        public Action? Сhanged_<xsl:value-of select="Name"/>;

    </xsl:for-each>

        /*
        Функції
        */
        
        public override ItemRow Copy()
        {
            return new()
            {
                <xsl:for-each select="$FieldsTL">
                    <xsl:value-of select="Name"/>
                    <xsl:text> = </xsl:text>
                    <xsl:value-of select="Name"/>
                    <xsl:choose>
                        <xsl:when test="Type = 'pointer'">.Copy()</xsl:when>
                    </xsl:choose>,
                </xsl:for-each>
            };
        }
    }

    #endregion

    protected override Gio.ListStore Store { get; } = Gio.ListStore.New(ItemRow.GetGType());

    public <xsl:value-of select="$OwnerName"/>_ТабличнаЧастина_<xsl:value-of select="$TablePartName"/>() : base(Program.BasicForm?.NotebookFunc)
    {
        MultiSelection model = MultiSelection.New(Store);
        model.OnSelectionChanged += GridOnSelectionChanged;

        Grid.Model = model;
    }

    protected override void Columns()
    {
        <xsl:if test="$IncludeIconColumn = '1'">
        //Image
        {
            SignalListItemFactory factory = SignalListItemFactory.New();
            factory.OnBind += (_, args) =&gt;
            {
                ListItem listItem = (ListItem)args.Object;
                ItemRow? row = (ItemRow?)listItem.Item;
                listItem.SetChild(ImageTablePartCell.NewForPixbuf(InterfaceGtk4.Icon.ForTabularLists.Normal));
            };
            ColumnViewColumn column = ColumnViewColumn.New("", factory);
            Grid.AppendColumn(column);
        }
        </xsl:if>
        <xsl:for-each select="$FieldsTL">
        //<xsl:value-of select="Name"/>
        {
            SignalListItemFactory factory = SignalListItemFactory.New();
            factory.OnSetup += (_, args) =&gt;
            {
                ListItem listItem = (ListItem)args.Object;
                var cell = <xsl:choose>
                    <xsl:when test="Type = 'string' and ReadOnly = '0'">new TextTablePartCell()</xsl:when>
                    <xsl:when test="Type = 'integer' and ReadOnly = '0'">new IntegerTablePartCell()</xsl:when>
                    <xsl:when test="Type = 'numeric' and ReadOnly = '0'">new NumericTablePartCell()</xsl:when>
                    <xsl:when test="Type = 'boolean'">new CheckTablePartCell()</xsl:when>
                    <xsl:when test="Type = 'pointer'">new <xsl:value-of select="substring-after(Pointer, '.')"/>_PointerTablePartCell()</xsl:when>
                    <xsl:when test="Type = 'enum'">new ComboTextTablePartCell();
                foreach (var field in ПсевдонімиПерелічення.<xsl:value-of select="substring-after(Pointer, '.')"/>_List())
                    cell.Combo.Append(field.Value.ToString(), field.Name)</xsl:when>
                    <xsl:when test="Type = 'date' or Type = 'datetime'">new DateTimeTablePartCell()</xsl:when>
                    <xsl:when test="Type = 'time'">new TimeTablePartCell()</xsl:when>
                    <xsl:otherwise>LabelTablePartCell.New(null)</xsl:otherwise>
                </xsl:choose>;
                <xsl:choose>
                    <xsl:when test="(Type = 'integer' or Type = 'numeric') and ReadOnly = '1'">
                cell.Halign = Align.End;
                    </xsl:when>
                    <xsl:when test="Type = 'boolean'">
                cell.Halign = Align.Center;
                    </xsl:when>
                    <xsl:when test="Type = 'date'">
                cell.OnlyDate = true;
                    </xsl:when>
                </xsl:choose>
                listItem.Child = cell;
            };
            factory.OnBind += (_, args) =&gt;
            {
                ListItem listItem = (ListItem)args.Object;
                var cell = (<xsl:choose>
                    <xsl:when test="Type = 'string'">TextTablePartCell</xsl:when>
                    <xsl:when test="Type = 'integer' and ReadOnly = '0'">IntegerTablePartCell</xsl:when>
                    <xsl:when test="Type = 'numeric'">NumericTablePartCell</xsl:when>
                    <xsl:when test="Type = 'boolean'">CheckTablePartCell</xsl:when>
                    <xsl:when test="Type = 'pointer'"><xsl:value-of select="substring-after(Pointer, '.')"/>_PointerTablePartCell</xsl:when>
                    <xsl:when test="Type = 'enum'">ComboTextTablePartCell</xsl:when>
                    <xsl:when test="Type = 'date' or Type = 'datetime'">DateTimeTablePartCell</xsl:when>
                    <xsl:when test="Type = 'time'">TimeTablePartCell</xsl:when>
                    <xsl:otherwise>LabelTablePartCell</xsl:otherwise>
                </xsl:choose>?)listItem.Child;
                ItemRow? row = (ItemRow?)listItem.Item;
                if (cell != null &amp;&amp; row != null)
                {
                    <xsl:choose>
                        <xsl:when test="(Type = 'string' or Type = 'integer' or Type = 'numeric') and ReadOnly = '0'">
                    cell.OnСhanged = () =&gt; row.<xsl:value-of select="Name"/> = cell.Value;
                    (row.Сhanged_<xsl:value-of select="Name"/> = () =&gt; cell.Value = row.<xsl:value-of select="Name"/>).Invoke();
                        </xsl:when>
                        <xsl:when test="Type = 'boolean'">
                    cell.OnСhanged = () =&gt; row.<xsl:value-of select="Name"/> = cell.Check.Active;
                    (row.Сhanged_<xsl:value-of select="Name"/> = () =&gt; cell.Value = row.<xsl:value-of select="Name"/>).Invoke();
                        </xsl:when>
                        <xsl:when test="Type = 'pointer'">
                        <xsl:if test="normalize-space(DirectoryOwner) != ''">
                            <xsl:variable name="DirectoryOwner" select="substring-after(DirectoryOwner, '.')"/>
                            <xsl:if test="count($FieldsTL[Name = $DirectoryOwner]) = 1">
                    cell.BeforeClickOpenFunc = () =&gt; cell.Власник  = row.<xsl:value-of select="$DirectoryOwner"/>;
                            </xsl:if>
                        </xsl:if>
                    cell.OnSelect = () =&gt; row.<xsl:value-of select="Name"/> = cell.Pointer;
                    (row.Сhanged_<xsl:value-of select="Name"/> = () =&gt; cell.Pointer = row.<xsl:value-of select="Name"/>).Invoke();
                        </xsl:when>
                        <xsl:when test="Type = 'enum'">
                    cell.OnСhanged = () =&gt; row.<xsl:value-of select="Name"/> = ПсевдонімиПерелічення.<xsl:value-of select="substring-after(Pointer, '.')"/>_FindByName(cell.Combo.ActiveId);
                    (row.Сhanged_<xsl:value-of select="Name"/> = () =&gt; cell.Value = row.<xsl:value-of select="Name"/>.ToString()).Invoke();
                        </xsl:when>
                        <xsl:when test="Type = 'date' or Type = 'datetime' or Type = 'time'">
                    cell.OnСhanged = () =&gt; row.<xsl:value-of select="Name"/> = cell.Value;
                    (row.Сhanged_<xsl:value-of select="Name"/> = () =&gt; cell.Value = row.<xsl:value-of select="Name"/>).Invoke();
                        </xsl:when>
                        <xsl:otherwise>
                    (row.Сhanged_<xsl:value-of select="Name"/> = () =&gt; cell.SetText(row.<xsl:value-of select="Name"/>)).Invoke();
                        </xsl:otherwise>
                    </xsl:choose>
                }
            };
            ColumnViewColumn column = ColumnViewColumn.New("<xsl:value-of select="Caption"/>", factory);
            column.Resizable = true;
            <xsl:if test="Size != 0">
            column.FixedWidth = <xsl:value-of select="Size"/>;
            </xsl:if>
            Grid.AppendColumn(column);
        }
        </xsl:for-each>
        { /* Пуста колонка для заповнення вільного простору */
            ColumnViewColumn column = ColumnViewColumn.New(null, null);
            column.Resizable = true;
            column.Expand = true;
            Grid.AppendColumn(column);
        }
    }

    public override async ValueTask LoadRecords()
    {
        <xsl:choose>
            <xsl:when test="$OwnerType = 'Constants'">
        Таблиця.QuerySelect.Clear();
            <xsl:for-each select="$FieldsTL[AutomaticNumbering = '1']">
        Таблиця.QuerySelect.Order.Add(<xsl:value-of select="$OwnerBlockName"/><xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart.<xsl:value-of select="Name"/>, SelectOrder.ASC);
            </xsl:for-each>
        await Таблиця.Read();
            </xsl:when>
            <xsl:otherwise>
        if (ЕлементВласник != null) <!-- відкриття if -->
        {
            <xsl:variable name="OrderFields">
                <xsl:for-each select="$FieldsTL[AutomaticNumbering = '1']">
                    <xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart.<xsl:value-of select="Name"/>
                    <xsl:text>,</xsl:text>
                </xsl:for-each>
            </xsl:variable>
            ЕлементВласник.<xsl:value-of select="$TablePartName"/>_TablePart.FillJoin([<xsl:value-of select="$OrderFields"/>]);
            await ЕлементВласник.<xsl:value-of select="$TablePartName"/>_TablePart.Read();
            </xsl:otherwise>
        </xsl:choose>

        Store.RemoveAll();

        <xsl:variable name="InRecords">
            <xsl:choose>
                <xsl:when test="$OwnerType = 'Constants'">Таблиця</xsl:when>
                <xsl:otherwise>ЕлементВласник.<xsl:value-of select="$TablePartName"/>_TablePart</xsl:otherwise>
            </xsl:choose>
        </xsl:variable>
        foreach (var record in <xsl:value-of select="$InRecords"/>.Records)
        {
            Store.Append(new ItemRow()
            {
                UnigueID = new(record.UID),
                <xsl:for-each select="$FieldsTL">
                <xsl:value-of select="Name"/> = record.<xsl:value-of select="Name"/>,
                </xsl:for-each>
            });

            if (SelectPosition &gt; 0)
            {
                Grid.Model.SelectItem(SelectPosition, true);
                ScrollTo(SelectPosition);
            }
        }
        <xsl:if test="$OwnerType != 'Constants'">}</xsl:if><!-- закриття if -->
    }

    public override async ValueTask SaveRecords()
    {
        <xsl:if test="$OwnerType != 'Constants'"><!-- відкриття if -->
        if (ЕлементВласник != null)
        {
        </xsl:if>

        <xsl:variable name="Records">
            <xsl:choose>
                <xsl:when test="$OwnerType = 'Constants'">Таблиця</xsl:when>
                <xsl:otherwise>ЕлементВласник.<xsl:value-of select="$TablePartName"/>_TablePart</xsl:otherwise>
            </xsl:choose>
        </xsl:variable>
        <xsl:value-of select="$Records"/>.Records.Clear();
        for (uint i = 0; i &lt;= Store.GetNItems(); i++)
        {
            ItemRow? row = (ItemRow?)Store.GetObject(i);
            if (row != null)
            {
                <xsl:value-of select="$Records"/>.Records.Add(new()
                {
                    UID = row.UnigueID.UGuid,
                    <xsl:for-each select="$FieldsTL">
                        <xsl:value-of select="Name"/> = row.<xsl:value-of select="Name"/>,
                    </xsl:for-each>
                });
            }
        }
        await <xsl:value-of select="$Records"/>.Save(true);
        //Update
        {
            uint position = 0;
            foreach (var record in <xsl:value-of select="$InRecords"/>.Records)
            {
                bool sel = Grid.Model.IsSelected(position);
                Store.Splice(position, 1, [new ItemRow()
                {
                    UnigueID = new(record.UID),
                    <xsl:for-each select="$FieldsTL">
                    <xsl:value-of select="Name"/> = record.<xsl:value-of select="Name"/>,
                    </xsl:for-each>
                }], 1);
                if (sel) Grid.Model.SelectItem(position, false);
                position++;
            }
        }
        <xsl:if test="$OwnerType != 'Constants'">}</xsl:if><!-- закриття if -->
    }

    public override bool NewRecord()
    {
        Store.Append(new ItemRow());
        return true;
    }
}
    </xsl:template>

<!--- 
//
// ============================ Список ============================
//
-->

    <!-- Список -->
    <xsl:template name="TablePartList">
        <xsl:variable name="TablePartName" select="TablePart/Name"/>
        <!--<xsl:variable name="Fields" select="TablePart/Fields/Field"/>-->
/*
        _ТабличнаЧастина_<xsl:value-of select="$TablePartName"/>.cs
        Таблична Частина Список
*/



    </xsl:template>

<!--- 
//
// ============================ Звіт ============================
//
-->

    <!-- Список -->
    <xsl:template name="TablePartReport">
        <xsl:variable name="TablePartName" select="TablePart/Name"/>
        <xsl:variable name="FieldsTL" select="TablePart/ElementFields/Field"/>

        <xsl:variable name="OwnerName" select="TablePart/OwnerName"/>
        <xsl:variable name="OwnerBlockName">
            <xsl:if test="TablePart/OwnerType = 'Constants' and normalize-space(TablePart/OwnerBlockName) != ''">
                <xsl:value-of select="concat(normalize-space(TablePart/OwnerBlockName), '.')"/>
            </xsl:if>
        </xsl:variable>
/*
        <xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_Звіт.cs
        Звіт
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Константи;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Документи;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.РегістриНакопичення;

namespace <xsl:value-of select="$NameSpace"/>
{
    public static class <xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_Звіт
    {
        public static async ValueTask Сформувати()
        {
            <xsl:variable name="CountFieldsTL" select="count(TablePart/ElementFields/Field)"/>
            string query = $@"
SELECT
    <xsl:for-each select="$FieldsTL">
    <xsl:choose>
        <xsl:when test="Type = 'pointer'">
            <xsl:variable name="name" select="Name" />
            <xsl:variable name="nameGroup" select="substring-before(Pointer, '.')" />
            <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
            <xsl:variable name="index">
                <xsl:for-each select="$FieldsTL[Type = 'pointer' and $namePointer = substring-after(Pointer, '.')]">
                    <xsl:if test="$name = Name and position() &gt; 1"><xsl:value-of select="position()"/></xsl:if>
                </xsl:for-each>
            </xsl:variable>
            <xsl:value-of select="$TablePartName"/>.{<xsl:value-of select="$OwnerBlockName"/><xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart.<xsl:value-of select="Name"/>} AS <xsl:value-of select="Name"/>,
            <xsl:choose>
                <xsl:when test="PresetntationFields/@Count = 1">
                    <xsl:value-of select="$nameGroup"/>_<xsl:value-of select="$namePointer"/><xsl:value-of select="$index"/>.{<xsl:value-of select="$namePointer"/>_Const.<xsl:value-of select="PresetntationFields/Field"/><xsl:text>}</xsl:text>
                </xsl:when>
                <xsl:when test="PresetntationFields/@Count &gt; 1">
                    <xsl:text>concat_ws (', '</xsl:text>
                    <xsl:for-each select="PresetntationFields/Field">
                        <xsl:value-of select="concat(', ', $nameGroup, '_', $namePointer, $index, '.{', $namePointer, '_Const.', text(), '}')"/>
                    </xsl:for-each>
                    <xsl:text>)</xsl:text>
                </xsl:when>
                <xsl:otherwise>'#'</xsl:otherwise>
            </xsl:choose> AS <xsl:value-of select="concat(Name, '_Назва')"/>
        </xsl:when>
        <xsl:otherwise>
            <xsl:value-of select="$TablePartName"/>.{<xsl:value-of select="$OwnerBlockName"/><xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart.<xsl:value-of select="Name"/>} AS <xsl:value-of select="Name"/>
        </xsl:otherwise>
    </xsl:choose>
    <xsl:if test="position() != $CountFieldsTL">,
    </xsl:if>
    </xsl:for-each>
FROM
    {<xsl:value-of select="$OwnerBlockName"/><xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart.TABLE} AS <xsl:value-of select="$TablePartName"/>
    <xsl:for-each select="$FieldsTL[Type = 'pointer']">
        <xsl:variable name="name" select="Name" />
        <xsl:variable name="nameGroup" select="substring-before(Pointer, '.')" />
        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
        <xsl:variable name="index">
            <xsl:for-each select="$FieldsTL[Type = 'pointer' and $namePointer = substring-after(Pointer, '.')]">
                <xsl:if test="$name = Name and position() &gt; 1"><xsl:value-of select="position()"/></xsl:if>
            </xsl:for-each>
        </xsl:variable>
    LEFT JOIN {<xsl:value-of select="$namePointer"/>_Const.TABLE} AS <xsl:value-of select="$nameGroup"/>_<xsl:value-of select="$namePointer"/><xsl:value-of select="$index"/> ON <xsl:value-of select="$nameGroup"/>_<xsl:value-of select="$namePointer"/><xsl:value-of select="$index"/>.uid = 
        <xsl:value-of select="$TablePartName"/>.{<xsl:value-of select="$OwnerBlockName"/><xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_TablePart.<xsl:value-of select="Name"/>}
    </xsl:for-each>
";
            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "<xsl:value-of select="$OwnerName"/>_<xsl:value-of select="$TablePartName"/>_Звіт",
                Caption = "<xsl:value-of select="$TablePartName"/>",
                Query = query,
                GetInfo = () =&gt; ValueTask.FromResult("")
            };

            <xsl:for-each select="$FieldsTL">
                <xsl:choose>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        <xsl:text>Звіт.ColumnSettings.Add("</xsl:text><xsl:value-of select="Name"/>_Назва", new("<xsl:value-of select="Caption"/>", "<xsl:value-of select="Name"/>", <xsl:value-of select="$namePointer"/>_Const.POINTER));
                    </xsl:when>
                    <xsl:when test="Type = 'integer' or Type = 'numeric'">
                        <xsl:text>Звіт.ColumnSettings.Add("</xsl:text><xsl:value-of select="Name"/>", new("<xsl:value-of select="Caption"/>", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:text>Звіт.ColumnSettings.Add("</xsl:text><xsl:value-of select="Name"/>", new("<xsl:value-of select="Caption"/>"));
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:for-each>
            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Program.GeneralNotebook);
        }
    }
}
    </xsl:template>

</xsl:stylesheet>