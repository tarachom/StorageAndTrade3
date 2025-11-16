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
                <xsl:call-template name="DirectoryTriggers" />
            </xsl:when>
            <xsl:when test="$File = 'Function'">
                <xsl:call-template name="DirectoryFunction" />
            </xsl:when>
            <xsl:when test="$File = 'Element'">
                <xsl:call-template name="DirectoryElement" />
            </xsl:when>
            <xsl:when test="$File = 'List'">
                <xsl:call-template name="DirectoryList" />
            </xsl:when>
            <xsl:when test="$File = 'ListSmallSelect'">
                <xsl:call-template name="DirectoryListSmallSelect" />
            </xsl:when>
            <xsl:when test="$File = 'PointerControl'">
                <xsl:call-template name="DirectoryPointerControl" />
            </xsl:when>
            <xsl:when test="$File = 'MultiplePointerControl'">
                <xsl:call-template name="DirectoryMultiplePointerControl" />
            </xsl:when>
            <xsl:when test="$File = 'ListAndTree'">
                <xsl:call-template name="DirectoryListAndTree" />
            </xsl:when>
            <xsl:when test="$File = 'Report'">
                <xsl:call-template name="DirectoryReport" />
            </xsl:when>
        </xsl:choose>

    </xsl:template>

<!--- 
//
// ============================ Triggers ============================
//
-->

    <xsl:template name="DirectoryTriggers">
        <xsl:variable name="DirectoryName" select="Directory/Name"/>
        <xsl:variable name="Fields" select="Directory/Fields/Field"/>
        <xsl:variable name="DirectoryAutomaticNumeration" select="Directory/AutomaticNumeration"/>

        <!-- Назви функцій -->
        <xsl:variable name="TriggerFunctions" select="Directory/TriggerFunctions"/>

/*
        <xsl:value-of select="$DirectoryName"/>_Triggers.cs
        Тригери
*/

using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Константи;
using AccountingSoftware;

namespace <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники
{
    static class <xsl:value-of select="$DirectoryName"/>_Triggers
    {
        public static async ValueTask <xsl:value-of select="$TriggerFunctions/New"/>(<xsl:value-of select="$DirectoryName"/>_Objest ДовідникОбєкт)
        {
            <xsl:if test="$DirectoryAutomaticNumeration = '1'">
                <xsl:text>ДовідникОбєкт.Код = (++НумераціяДовідників.</xsl:text>
                <xsl:value-of select="$DirectoryName"/>
                <xsl:text>_Const).ToString("D6");</xsl:text>
            </xsl:if>
            await ValueTask.FromResult(true);
        }

        public static async ValueTask <xsl:value-of select="$TriggerFunctions/Copying"/>(<xsl:value-of select="$DirectoryName"/>_Objest ДовідникОбєкт, <xsl:value-of select="$DirectoryName"/>_Objest Основа)
        {
            <xsl:if test="$Fields[Name = 'Назва']">
            ДовідникОбєкт.Назва += " - Копія";
            </xsl:if>
            await ValueTask.FromResult(true);
        }

        public static async ValueTask <xsl:value-of select="$TriggerFunctions/BeforeSave"/>(<xsl:value-of select="$DirectoryName"/>_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask <xsl:value-of select="$TriggerFunctions/AfterSave"/>(<xsl:value-of select="$DirectoryName"/>_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask <xsl:value-of select="$TriggerFunctions/SetDeletionLabel"/>(<xsl:value-of select="$DirectoryName"/>_Objest ДовідникОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask <xsl:value-of select="$TriggerFunctions/BeforeDelete"/>(<xsl:value-of select="$DirectoryName"/>_Objest ДовідникОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
    </xsl:template>

<!--- 
//
// ============================ Function ============================
//
-->

    <xsl:template name="Function_FuncToField">
        <xsl:choose>
            <xsl:when test="Type = 'string'">FuncToField = "LOWER"</xsl:when>
            <xsl:otherwise>FuncToField = "TO_CHAR", FuncToField_Param1 = "''"</xsl:otherwise>
        </xsl:choose>
    </xsl:template>

    <!-- Елемент -->
    <xsl:template name="DirectoryFunction">
        <xsl:variable name="DirectoryName" select="Directory/Name"/>
        <xsl:variable name="Fields" select="Directory/Fields/Field"/>
        <xsl:variable name="TabularParts" select="Directory/TabularParts/TablePart"/>
        <xsl:variable name="TabularList" select="Directory/TabularList"/>

        <!-- Відфільтровані поля по типу даних -->
        <xsl:variable name="FieldsFilter" select="$Fields[Type = 'string' or Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time']"/>

        <!-- Додатова інформація про підпорядкування довідника -->
        <xsl:variable name="DirectoryOwner" select="Directory/DirectoryOwner"/>
        <xsl:variable name="PointerFieldOwner" select="Directory/PointerFieldOwner"/>
/*
        <xsl:value-of select="$DirectoryName"/>_Функції.cs
        Функції
*/

using InterfaceGtk3;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники;

namespace <xsl:value-of select="$NameSpace"/>
{
    static class <xsl:value-of select="$DirectoryName"/>_Функції
    {
        public static List&lt;Where&gt; Відбори(string searchText)
        {
            return
            [
                <xsl:choose>
                    <xsl:when test="$FieldsFilter[IsSearch = '1']">
                        <xsl:for-each select="$FieldsFilter[IsSearch = '1']">
                //<xsl:value-of select="Name"/>
                new Where(<xsl:if test="position() != 1">Comparison.OR, </xsl:if><xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="Name"/>, Comparison.LIKE, searchText) { <xsl:call-template name="Function_FuncToField" /> },
                        </xsl:for-each>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:if test="$FieldsFilter[Name = 'Код']">
                //Код
                new Where(<xsl:value-of select="$DirectoryName"/>_Const.Код, Comparison.LIKE, searchText) { <xsl:call-template name="Function_FuncToField" /> },
                        </xsl:if>
                        <xsl:if test="$FieldsFilter[Name = 'Назва']">
                //Назва
                new Where(<xsl:if test="$FieldsFilter[Name = 'Код']">Comparison.OR, </xsl:if><xsl:value-of select="$DirectoryName"/>_Const.Назва, Comparison.LIKE, searchText) { <xsl:call-template name="Function_FuncToField" /> },
                        </xsl:if>
                    </xsl:otherwise>
                </xsl:choose>
            ];
        }

        public static async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null, 
            Action&lt;UnigueID?&gt;? сallBack_LoadRecords = null, 
            Action&lt;UnigueID&gt;? сallBack_OnSelectPointer = null<xsl:if test="normalize-space($DirectoryOwner) != ''">,
            <xsl:variable name="namePointer" select="substring-after($DirectoryOwner, '.')" />
            <xsl:value-of select="$namePointer"/>_Pointer? Власник = null</xsl:if>)
        {
            <xsl:value-of select="$DirectoryName"/>_Елемент page = new <xsl:value-of select="$DirectoryName"/>_Елемент
            {
                IsNew = IsNew,
                CallBack_LoadRecords = сallBack_LoadRecords,
                CallBack_OnSelectPointer = сallBack_OnSelectPointer
            };

            if (IsNew)
            {
                await page.Елемент.New();
                <xsl:if test="normalize-space($DirectoryOwner) != ''">
                <xsl:variable name="namePointer" select="substring-after($DirectoryOwner, '.')" />
                if (Власник != null)
                    page.ВласникДляНового = Власник;
                </xsl:if>
            }
            else if (unigueID == null || !await page.Елемент.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () =&gt; page);
            await NotebookFunction.AddLockObjectFunc(Program.GeneralNotebook, page.Name, page.Елемент);
            await page.LockInfo(page.Елемент);
            page.SetValue();
        }

        public static async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            <xsl:value-of select="$DirectoryName"/>_Pointer Вказівник = new(unigueID);
            bool? label = await Вказівник.GetDeletionLabel();
            if (label.HasValue) await Вказівник.SetDeletionLabel(!label.Value);
        }

        public static async ValueTask&lt;UnigueID?&gt; Copy(UnigueID unigueID)
        {
            <xsl:value-of select="$DirectoryName"/>_Objest Обєкт = new <xsl:value-of select="$DirectoryName"/>_Objest();
            if (await Обєкт.Read(unigueID))
            {
                <xsl:value-of select="$DirectoryName"/>_Objest Новий = await Обєкт.Copy(true);
                await Новий.Save();
                <xsl:for-each select="$TabularParts">
                    await Новий.<xsl:value-of select="Name"/>_TablePart.Save(false); // Таблична частина "<xsl:value-of select="Name"/>"
                </xsl:for-each>
                return Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }
    }
}
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
        <!--<xsl:variable name="Fields" select="Directory/Fields/Field"/>-->
        <!--<xsl:variable name="TabularParts" select="Directory/TabularParts/TablePart"/>-->
        <xsl:variable name="FieldsTL" select="Directory/ElementFields/Field"/>
        <xsl:variable name="TabularPartsTL" select="Directory/ElementTableParts/TablePart"/>

        <!-- Додатова інформація для ієрархічного довідника -->
        <xsl:variable name="DirectoryType" select="Directory/Type"/>
        <xsl:variable name="ParentField" select="Directory/ParentField"/> <!-- Поле Родич (тільки для ієрархічного) -->

        <!-- Додатова інформація про підпорядкування довідника -->
        <xsl:variable name="DirectoryOwner" select="Directory/DirectoryOwner"/>
        <xsl:variable name="PointerFieldOwner" select="Directory/PointerFieldOwner"/>
/*
        <xsl:value-of select="$DirectoryName"/>_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk3;

using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Документи;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Перелічення;

namespace <xsl:value-of select="$NameSpace"/>
{
    class <xsl:value-of select="$DirectoryName"/>_Елемент : ДовідникЕлемент
    {
        public <xsl:value-of select="$DirectoryName"/>_Objest Елемент { get; init; } = new <xsl:value-of select="$DirectoryName"/>_Objest();
        <xsl:if test="normalize-space($DirectoryOwner) != ''">
        <xsl:variable name="namePointer" select="substring-after($DirectoryOwner, '.')" />
        public <xsl:value-of select="$namePointer"/>_Pointer ВласникДляНового = new <xsl:value-of select="$namePointer"/>_Pointer();
        </xsl:if>
        <xsl:if test="$DirectoryType = 'Hierarchical'">
        public <xsl:value-of select="$DirectoryName"/>_Pointer РодичДляНового { get; set; } = new <xsl:value-of select="$DirectoryName"/>_Pointer();
        </xsl:if>
        #region Fields
        <xsl:for-each select="$FieldsTL">
            <xsl:variable name="Size">
                <xsl:choose>
                    <xsl:when test="Size != '0'"><xsl:value-of select="Size"/></xsl:when>
                    <xsl:otherwise>500</xsl:otherwise>
                </xsl:choose>
            </xsl:variable>
            <xsl:choose>
                <xsl:when test="Type = 'string'">
                    <xsl:choose>
                        <xsl:when test="Multiline = '1'">
                    <xsl:text>TextView </xsl:text><xsl:value-of select="Name"/> = new TextView() { WrapMode = WrapMode.Word };
                        </xsl:when>
                        <xsl:otherwise>
                    <xsl:text>Entry </xsl:text><xsl:value-of select="Name"/> = new Entry() { WidthRequest = <xsl:value-of select="$Size"/> };
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:when>
                <xsl:when test="Type = 'integer'">
                    <xsl:text>IntegerControl </xsl:text><xsl:value-of select="Name"/> = new IntegerControl();
                </xsl:when>
                <xsl:when test="Type = 'numeric'">
                    <xsl:text>NumericControl </xsl:text><xsl:value-of select="Name"/> = new NumericControl();
                </xsl:when>
                <xsl:when test="Type = 'boolean'">
                    <xsl:text>CheckButton </xsl:text><xsl:value-of select="Name"/> = new CheckButton("<xsl:value-of select="Name"/>");
                </xsl:when>
                <xsl:when test="Type = 'date' or Type = 'datetime'">
                    <xsl:text>DateTimeControl </xsl:text><xsl:value-of select="Name"/> = new DateTimeControl()<xsl:if test="Type = 'date'">{ OnlyDate = true }</xsl:if>;
                </xsl:when>
                <xsl:when test="Type = 'time'">
                    <xsl:text>TimeControl </xsl:text><xsl:value-of select="Name"/> = new TimeControl();
                </xsl:when>
                <xsl:when test="Type = 'composite_pointer'">
                    <xsl:text>CompositePointerControl </xsl:text><xsl:value-of select="Name"/> = new CompositePointerControl() { BoundConfType = "Довідники.<xsl:value-of select="$DirectoryName"/>.<xsl:value-of select="Name"/>" };
                </xsl:when>
                <xsl:when test="Type = 'composite_text'">
                    <xsl:text>//NameAndText </xsl:text><xsl:value-of select="Name"/> = new NameAndText();
                </xsl:when>
                <xsl:when test="Type = 'pointer'">
                    <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                    <xsl:value-of select="$namePointer"/>_PointerControl <xsl:value-of select="Name"/> = new <xsl:value-of select="$namePointer"/>_PointerControl() { Caption = "<xsl:value-of select="Caption"/>", WidthPresentation = <xsl:value-of select="$Size"/> };
                </xsl:when>
                <xsl:when test="Type = 'enum'">
                    <xsl:text>ComboBoxText </xsl:text><xsl:value-of select="Name"/> = new ComboBoxText();
                </xsl:when>
                <xsl:when test="Type = 'any_pointer'">
                    <xsl:text>//Guid </xsl:text><xsl:value-of select="Name"/> = new Guid();
                </xsl:when>
                <xsl:when test="Type = 'bytea'">
                    <xsl:text>//byte[] </xsl:text><xsl:value-of select="Name"/> = [];
                </xsl:when>
                <xsl:when test="Type = 'string[]'">
                    <xsl:text>//string[] </xsl:text><xsl:value-of select="Name"/> = [];
                </xsl:when>
                <xsl:when test="Type = 'integer[]'">
                    <xsl:text>//int[] </xsl:text><xsl:value-of select="Name"/> = [];
                </xsl:when>
                <xsl:when test="Type = 'numeric[]'">
                    <xsl:text>//decimal[] </xsl:text><xsl:value-of select="Name"/> = [];
                </xsl:when>
                <xsl:when test="Type = 'uuid[]'">
                    <xsl:text>//Guid[] </xsl:text><xsl:value-of select="Name"/> = [];
                </xsl:when>
            </xsl:choose>
        </xsl:for-each>
        #endregion

        #region TabularParts
        <xsl:for-each select="$TabularPartsTL">
            <xsl:variable name="Size">
                <xsl:choose>
                    <xsl:when test="Size != '0'"><xsl:value-of select="Size"/></xsl:when>
                    <xsl:otherwise>500</xsl:otherwise>
                </xsl:choose>
            </xsl:variable>
            <xsl:variable name="Height">
                <xsl:choose>
                    <xsl:when test="Height != '0'"><xsl:value-of select="Height"/></xsl:when>
                    <xsl:otherwise>300</xsl:otherwise>
                </xsl:choose>
            </xsl:variable>
            // Таблична частина "<xsl:value-of select="Name"/>"
            <xsl:value-of select="$DirectoryName"/>_ТабличнаЧастина_<xsl:value-of select="Name"/><xsl:text> </xsl:text><xsl:value-of select="Name"/> = new <xsl:value-of select="$DirectoryName"/>_ТабличнаЧастина_<xsl:value-of select="Name"/>() { WidthRequest = <xsl:value-of select="$Size"/>, HeightRequest = <xsl:value-of select="$Height"/> };
        </xsl:for-each>
        #endregion

        public <xsl:value-of select="$DirectoryName"/>_Елемент() : base() 
        { 
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

            <xsl:for-each select="$FieldsTL[Type = 'enum']">
                foreach (var field in ПсевдонімиПерелічення.<xsl:value-of select="substring-after(Pointer, '.')"/>_List())
                    <xsl:value-of select="Name"/>.Append(field.Value.ToString(), field.Name);
            </xsl:for-each>
        }

        protected override void CreatePack1(Box vBox)
        {
            <xsl:for-each select="$FieldsTL">
                // <xsl:value-of select="Name"/>
                <xsl:choose>
                    <xsl:when test="Type = 'string' or Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                        <xsl:choose>
                            <xsl:when test="Type = 'string' and Multiline = '1'">
                            <xsl:variable name="Size">
                                <xsl:choose>
                                    <xsl:when test="Size != '0'"><xsl:value-of select="Size"/></xsl:when>
                                    <xsl:otherwise>500</xsl:otherwise>
                                </xsl:choose>
                            </xsl:variable>
                            <xsl:variable name="Height">
                                <xsl:choose>
                                    <xsl:when test="Height != '0'"><xsl:value-of select="Height"/></xsl:when>
                                    <xsl:otherwise>200</xsl:otherwise>
                                </xsl:choose>
                            </xsl:variable>
                CreateFieldView(vBox, "<xsl:value-of select="Caption"/>:", <xsl:value-of select="Name"/>, <xsl:value-of select="$Size"/>, <xsl:value-of select="$Height"/>);
                            </xsl:when>
                            <xsl:otherwise>
                CreateField(vBox, "<xsl:value-of select="Caption"/>:", <xsl:value-of select="Name"/>);
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
                CreateField(vBox, "<xsl:value-of select="Caption"/>:", <xsl:value-of select="Name"/>);
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>
        }

        protected override void CreatePack2(Box vBox)
        {
            <xsl:for-each select="$TabularPartsTL">
                // Таблична частина "<xsl:value-of select="Name"/>" 
                CreateTablePart(vBox, "<xsl:value-of select="Caption"/>:", <xsl:value-of select="Name"/>);
            </xsl:for-each>
        }

        #region Присвоєння / зчитування значень

        public override <xsl:if test="count($TabularPartsTL) != 0">async</xsl:if> void SetValue()
        {
            <xsl:if test="normalize-space($DirectoryOwner) != ''">
            if (IsNew) 
                Елемент.<xsl:value-of select="$PointerFieldOwner"/> = ВласникДляНового;
            </xsl:if>
            <xsl:choose>
                <xsl:when test="$DirectoryType = 'Hierarchical'">
                    if (IsNew)
                        Елемент.<xsl:value-of select="$ParentField"/> = РодичДляНового;
                    else
                        <xsl:value-of select="$ParentField"/>.OpenFolder = Елемент.UnigueID;
                </xsl:when>
            </xsl:choose>

            <xsl:for-each select="$FieldsTL">
                <xsl:choose>
                    <xsl:when test="Type = 'string'">
                        <xsl:choose>
                            <xsl:when test="Multiline = '1'">
                                <xsl:value-of select="Name"/>.Buffer.Text = Елемент.<xsl:value-of select="Name"/>;
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="Name"/>.Text = Елемент.<xsl:value-of select="Name"/>;
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:when test="Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                        <xsl:value-of select="Name"/>.Value = Елемент.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'boolean'">
                        <xsl:value-of select="Name"/>.Active = Елемент.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'composite_pointer'">
                        <xsl:value-of select="Name"/>.Pointer = Елемент.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:value-of select="Name"/>.Pointer = Елемент.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                        <xsl:value-of select="Name"/>.ActiveId = Елемент.<xsl:value-of select="Name"/><xsl:text>.ToString(); </xsl:text>
                        <xsl:text>//if (</xsl:text><xsl:value-of select="Name"/>.Active == -1) <xsl:value-of select="Name"/>.Active = 0;
                    </xsl:when>
                    <xsl:when test="Type = 'any_pointer' or Type = 'composite_text' or Type = 'bytea' or Type = 'string[]' or Type = 'integer[]' or Type = 'numeric[]' or Type = 'uuid[]'">
                        <xsl:text>//</xsl:text><xsl:value-of select="Name"/> = Елемент.<xsl:value-of select="Name"/>;
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>

            <xsl:for-each select="$TabularPartsTL">
                // Таблична частина "<xsl:value-of select="Name"/>"
                <xsl:value-of select="Name"/>.ЕлементВласник = Елемент;
                await <xsl:value-of select="Name"/>.LoadRecords();
            </xsl:for-each>
        }

        protected override void GetValue()
        {
            <xsl:for-each select="$FieldsTL">
                <xsl:choose>
                    <xsl:when test="Type = 'string'">
                        <xsl:choose>
                            <xsl:when test="Multiline = '1'">
                        <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Buffer.Text;
                            </xsl:when>
                            <xsl:otherwise>
                        <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Text;
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:when test="Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                        <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Value;
                    </xsl:when>
                    <xsl:when test="Type = 'boolean'">
                        <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Active;
                    </xsl:when>
                    <xsl:when test="Type = 'composite_pointer'">
                        <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Pointer;
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Pointer;
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = ПсевдонімиПерелічення.<xsl:value-of select="$namePointer"/>_FindByName(<xsl:value-of select="Name"/>.ActiveId);
                    </xsl:when>
                    <xsl:when test="Type = 'any_pointer' or Type = 'composite_text' or Type = 'bytea' or Type = 'string[]' or Type = 'integer[]' or Type = 'numeric[]' or Type = 'uuid[]'">
                        <xsl:text>//Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>;
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>
        }

        #endregion

        protected override async ValueTask&lt;bool&gt; Save()
        {
            bool isSaved = false;
            try
            {
                if (await Елемент.Save())
                {
                    <xsl:for-each select="$TabularPartsTL">
                        <xsl:text>await </xsl:text><xsl:value-of select="Name"/>.SaveRecords(); // Таблична частина "<xsl:value-of select="Name"/>"
                    </xsl:for-each>
                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Елемент.GetBasis(), Caption, ex);
            }
            return isSaved;
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
        <xsl:variable name="TabularList" select="Directory/TabularList"/>

        <!-- Додатова інформація для ієрархічного довідника -->
        <xsl:variable name="DirectoryType" select="Directory/Type"/>
        <xsl:variable name="ParentField" select="Directory/ParentField"/> <!-- Поле Родич (тільки для ієрархічного) -->

        <!-- Додатова інформація про підпорядкування довідника -->
        <xsl:variable name="DirectoryOwner" select="Directory/DirectoryOwner"/>
        <xsl:variable name="PointerFieldOwner" select="Directory/PointerFieldOwner"/>
/*     
        <xsl:value-of select="$DirectoryName"/>.cs
        Список
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using <xsl:value-of select="$NameSpaceGeneratedCode"/>;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники;
using ТабличніСписки = <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники.ТабличніСписки;

namespace <xsl:value-of select="$NameSpace"/>
{
    public class <xsl:value-of select="$DirectoryName"/> : ДовідникЖурнал
    {
        <xsl:if test="normalize-space($DirectoryOwner) != ''">
        <xsl:variable name="namePointer" select="substring-after($DirectoryOwner, '.')" />
        public <xsl:value-of select="$namePointer"/>_PointerControl Власник = new <xsl:value-of select="$namePointer"/>_PointerControl() { Caption = "<xsl:value-of select="$PointerFieldOwner"/>:" };
        </xsl:if>

        public <xsl:value-of select="$DirectoryName"/>() : base()
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.AddColumns(TreeViewGrid);
            <xsl:if test="normalize-space($DirectoryOwner) != ''">
            HBoxTop.PackStart(Власник, false, false, 2);
            Власник.AfterSelectFunc = async () =&gt; await BeforeLoadRecords();
            </xsl:if>
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ОчиститиВідбір(TreeViewGrid);
            <xsl:if test="normalize-space($DirectoryOwner) != ''">
            if (!Власник.Pointer.UnigueID.IsEmpty())
                ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid,
                    new Where(<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="$PointerFieldOwner"/>, Comparison.EQ, Власник.Pointer.UnigueID.UGuid));
            </xsl:if>
            await ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ОчиститиВідбір(TreeViewGrid);
            <xsl:if test="normalize-space($DirectoryOwner) != ''">
            if (!Власник.Pointer.UnigueID.IsEmpty())
                ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid,
                    new Where(<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="$PointerFieldOwner"/>, Comparison.EQ, Власник.Pointer.UnigueID.UGuid));
            </xsl:if>
            //Відбори
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid, <xsl:value-of select="$DirectoryName"/>_Функції.Відбори(searchText));

            await ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid, OpenFolder);
        }
        
        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid);
        }

        public async ValueTask UpdateRecords(List&lt;ObjectChanged&gt; recordsChanged)
        {
            await ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await <xsl:value-of select="$DirectoryName"/>_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null<xsl:if test="normalize-space($DirectoryOwner) != ''">, Власник.Pointer</xsl:if>);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await <xsl:value-of select="$DirectoryName"/>_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask&lt;UnigueID?&gt; Copy(UnigueID unigueID)
        {
            return await <xsl:value-of select="$DirectoryName"/>_Функції.Copy(unigueID);
        }

        protected override async ValueTask VersionsHistory(UnigueID unigueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new <xsl:value-of select="$DirectoryName"/>_Pointer(unigueID).GetBasis());
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, <xsl:value-of select="$DirectoryName"/>_Const.POINTER);
            <xsl:if test="normalize-space($DirectoryType) = 'Hierarchical'">if (!CompositeMode)</xsl:if> await BeforeLoadRecords();
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
        <xsl:variable name="TabularList" select="Directory/TabularList"/>

        <!-- Додатова інформація для ієрархічного довідника -->
        <xsl:variable name="DirectoryType" select="Directory/Type"/>
        <xsl:variable name="ParentField" select="Directory/ParentField"/> <!-- Поле Родич (тільки для ієрархічного) -->

        <!-- Додатова інформація про підпорядкування довідника -->
        <xsl:variable name="DirectoryOwner" select="Directory/DirectoryOwner"/>
        <xsl:variable name="PointerFieldOwner" select="Directory/PointerFieldOwner"/>
/*     
        <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники;
using ТабличніСписки = <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники.ТабличніСписки;

namespace <xsl:value-of select="$NameSpace"/>
{
    class <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір : ДовідникШвидкийВибір
    {
        <xsl:if test="normalize-space($DirectoryOwner) != ''">
        <xsl:variable name="namePointer" select="substring-after($DirectoryOwner, '.')" />
        public <xsl:value-of select="$namePointer"/>_PointerControl Власник = new <xsl:value-of select="$namePointer"/>_PointerControl() { Caption = "<xsl:value-of select="$PointerFieldOwner"/>:" };
        </xsl:if>
        
        public <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір() : base()
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.AddColumns(TreeViewGrid);
            <xsl:if test="normalize-space($DirectoryOwner) != ''">
            HBoxTop.PackStart(Власник, false, false, 2); //Власник
            Власник.AfterSelectFunc = async () =&gt; await BeforeLoadRecords();
            </xsl:if>
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ОчиститиВідбір(TreeViewGrid);
            <xsl:if test="normalize-space($DirectoryOwner) != ''">
            if (!Власник.Pointer.UnigueID.IsEmpty())
                ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid,
                    new Where(<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="$PointerFieldOwner"/>, Comparison.EQ, Власник.Pointer.UnigueID.UGuid));
            </xsl:if>
            await ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ОчиститиВідбір(TreeViewGrid);
            <xsl:if test="normalize-space($DirectoryOwner) != ''">
            if (!Власник.Pointer.UnigueID.IsEmpty())
                ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid,
                    new Where(<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="$PointerFieldOwner"/>, Comparison.EQ, Власник.Pointer.UnigueID.UGuid));
            </xsl:if>            
            //Відбори
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid, <xsl:value-of select="$DirectoryName"/>_Функції.Відбори(searchText));

            await ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            <xsl:value-of select="$DirectoryName"/> page = new <xsl:value-of select="$DirectoryName"/>()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                CallBack_OnMultipleSelectPointer = CallBack_OnMultipleSelectPointer,
                OpenFolder = OpenFolder
            };
            <xsl:if test="normalize-space($DirectoryOwner) != ''">
            page.Власник.Pointer = Власник.Pointer;
            </xsl:if>
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, <xsl:value-of select="$DirectoryName"/>_Const.FULLNAME, () =&gt; page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await  <xsl:value-of select="$DirectoryName"/>_Функції.OpenPageElement(IsNew, unigueID, null, CallBack_OnSelectPointer<xsl:if test="normalize-space($DirectoryOwner) != ''">, Власник.Pointer</xsl:if>);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await <xsl:value-of select="$DirectoryName"/>_Функції.SetDeletionLabel(unigueID);
        }
    }
}
    </xsl:template>

<!--- 
//
// ============================ Список з Деревом ============================
//
-->

    <!-- Список з Деревом-->
    <xsl:template name="DirectoryListAndTree">
        <xsl:variable name="DirectoryName" select="Directory/Name"/>
        <!--<xsl:variable name="TabularParts" select="Directory/TabularParts/TablePart"/>-->
        <xsl:variable name="TabularList" select="Directory/TabularList"/>

        <!-- Додатова інформація для ієрархічного довідника -->
        <xsl:variable name="DirectoryType" select="Directory/Type"/>
        <xsl:variable name="ParentField" select="Directory/ParentField"/> <!-- Поле Родич (тільки для ієрархічного) -->

        <xsl:variable name="PointerFolders" select="Directory/PointerFolders"/> <!-- Окремий довідник для ієрархії (тільки для ієрархії в окремому довіднику) -->
        <xsl:variable name="FieldFolder"> <!-- Поле Папка для ієрархії в окремому довіднику -->
            <xsl:choose>
                <xsl:when test="normalize-space(Directory/FieldFolder) != ''"><xsl:value-of select="Directory/FieldFolder"/></xsl:when>
                <xsl:otherwise>Папка</xsl:otherwise>
            </xsl:choose>
        </xsl:variable>

        <!-- Додатова інформація про підпорядкування довідника -->
        <xsl:variable name="DirectoryOwner" select="Directory/DirectoryOwner"/>
        <xsl:variable name="PointerFieldOwner" select="Directory/PointerFieldOwner"/>
/*     
        <xsl:value-of select="$DirectoryName"/>.cs 
        Список з Деревом
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники;
using ТабличніСписки = <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники.ТабличніСписки;

namespace <xsl:value-of select="$NameSpace"/>
{
    public class <xsl:value-of select="$DirectoryName"/> : ДовідникЖурнал
    {
        <xsl:value-of select="$PointerFolders"/> ДеревоПапок;
        <xsl:if test="normalize-space($DirectoryOwner) != ''">
        <xsl:variable name="namePointer" select="substring-after($DirectoryOwner, '.')" />
        public <xsl:value-of select="$namePointer"/>_PointerControl Власник = new <xsl:value-of select="$namePointer"/>_PointerControl() { Caption = "<xsl:value-of select="$PointerFieldOwner"/>:" };
        </xsl:if>

        public <xsl:value-of select="$DirectoryName"/>()
        {
            AddIsHierarchy();

            //Дерево папок
            ДеревоПапок = new <xsl:value-of select="$PointerFolders"/>()
            {
                WidthRequest = 500,
                CompositeMode = true,
                CallBack_RowActivated = async () =&gt;
                {
                    if (IsHierarchy.Active)
                        await BeforeLoadRecords_OnTree();
                }                
            };
            ДеревоПапок.SetValue();
            HPanedTable.Pack2(ДеревоПапок, false, true);

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.AddColumns(TreeViewGrid);
            <xsl:if test="normalize-space($DirectoryOwner) != ''">
            HBoxTop.PackStart(Власник, false, false, 2); //Власник
            Власник.AfterSelectFunc = () =&gt; ДеревоПапок.Власник.Pointer = Власник.Pointer;
            </xsl:if>
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            if (IsHierarchy.Active)
            {
                if (DirectoryPointerItem != null || SelectPointerItem != null)
                {
                    <xsl:value-of select="$DirectoryName"/>_Objest? Обєкт = await new <xsl:value-of select="$DirectoryName"/>_Pointer(SelectPointerItem ?? DirectoryPointerItem ?? new UnigueID()).GetDirectoryObject();
                    if (Обєкт != null) ДеревоПапок.SelectPointerItem = Обєкт.<xsl:value-of select="$FieldFolder"/>.UnigueID;
                }

                await ДеревоПапок.LoadRecords();
            }
            else
                await BeforeLoadRecords_OnTree();
        }

        async ValueTask LoadRecords_TreeCallBack()
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ОчиститиВідбір(TreeViewGrid);
            <xsl:if test="normalize-space($DirectoryOwner) != ''">
            if (!Власник.Pointer.UnigueID.IsEmpty())
                ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid,
                    new Where(<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="$PointerFieldOwner"/>, Comparison.EQ, Власник.Pointer.UnigueID.UGuid));
            </xsl:if>
            if (checkButtonIsHierarchy.Active)
                ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid,
                    new Where(<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="$FieldFolder"/>, Comparison.EQ, ДеревоПапок.SelectPointerItem?.UGuid ?? new UnigueID().UGuid));

            await ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid, OpenFolder, SelectPointerItem, DirectoryPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ОчиститиВідбір(TreeViewGrid);
            <xsl:if test="normalize-space($DirectoryOwner) != ''">
            if (!Власник.Pointer.UnigueID.IsEmpty())
                ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid,
                    new Where(<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="$PointerFieldOwner"/>, Comparison.EQ, Власник.Pointer.UnigueID.UGuid));
            </xsl:if>
            //Відбори
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid, <xsl:value-of select="$DirectoryName"/>_Функції.Відбори(searchText));

            await ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid, OpenFolder);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid, SelectPointerItem, DirectoryPointerItem);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.CreateFilter(TreeViewGrid, filterControl);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await  <xsl:value-of select="$DirectoryName"/>_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords, null<xsl:if test="normalize-space($DirectoryOwner) != ''">, Власник.Pointer</xsl:if>);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await  <xsl:value-of select="$DirectoryName"/>_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask&lt;UnigueID?&gt; Copy(UnigueID unigueID)
        {
            return await  <xsl:value-of select="$DirectoryName"/>_Функції.Copy(unigueID);
        }

        protected override async ValueTask BeforeSetValue()
        {
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, LoadRecords, <xsl:value-of select="$DirectoryName"/>_Const.POINTER);
            await BeforeLoadRecords();
        }

        #endregion
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

        <!-- Додатова інформація про підпорядкування довідника -->
        <xsl:variable name="DirectoryOwner" select="Directory/DirectoryOwner"/>
        <xsl:variable name="PointerFieldOwner" select="Directory/PointerFieldOwner"/>
/*     
        <xsl:value-of select="$DirectoryName"/>_PointerControl.cs
        PointerControl
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники;

namespace <xsl:value-of select="$NameSpace"/>
{
    public class <xsl:value-of select="$DirectoryName"/>_PointerControl : PointerControl
    {
        event EventHandler&lt;<xsl:value-of select="$DirectoryName"/>_Pointer&gt; PointerChanged;

        public <xsl:value-of select="$DirectoryName"/>_PointerControl()
        {
            pointer = new <xsl:value-of select="$DirectoryName"/>_Pointer();
            WidthPresentation = 300;
            Caption = $"{<xsl:value-of select="$DirectoryName"/>_Const.FULLNAME}:";
            PointerChanged += async (_, pointer) =&gt; Presentation = pointer != null ? await pointer.GetPresentation() : "";
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
                PointerChanged?.Invoke(null, pointer);
            }
        }

        <xsl:if test="normalize-space($DirectoryOwner) != ''">
        <xsl:variable name="namePointer" select="substring-after($DirectoryOwner, '.')" />
        public <xsl:value-of select="$namePointer"/>_Pointer Власник { get; set; } = new <xsl:value-of select="$namePointer"/>_Pointer();
        </xsl:if>

        protected override async void OpenSelect(object? sender, EventArgs args)
        {
            Popover popover = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };
            BeforeClickOpenFunc?.Invoke();
            <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір page = new <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір() 
            { 
                PopoverParent = popover, 
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = selectPointer =&gt;
                {
                    Pointer = new <xsl:value-of select="$DirectoryName"/>_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            <xsl:if test="normalize-space($DirectoryOwner) != ''">
            page.Власник.Pointer = Власник;
            </xsl:if>
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new <xsl:value-of select="$DirectoryName"/>_Pointer();
            AfterSelectFunc?.Invoke();
            AfterClearFunc?.Invoke();
        }
    }
}
    </xsl:template>

<!--- 
//
// ============================ MultiplePointerControl ============================
//
-->

    <!-- MultiplePointerControl -->
    <xsl:template name="DirectoryMultiplePointerControl">
        <xsl:variable name="DirectoryName" select="Directory/Name"/>

        <!-- Додатова інформація про підпорядкування довідника -->
        <xsl:variable name="DirectoryOwner" select="Directory/DirectoryOwner"/>
        <xsl:variable name="PointerFieldOwner" select="Directory/PointerFieldOwner"/>

/*     
        <xsl:value-of select="$DirectoryName"/>_MultiplePointerControl.cs
        MultiplePointerControl
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники;

namespace <xsl:value-of select="$NameSpace"/>
{
    public class <xsl:value-of select="$DirectoryName"/>_MultiplePointerControl : MultiplePointerControl
    {
        event EventHandler&lt;<xsl:value-of select="$DirectoryName"/>_Pointer&gt; PointerChanged;

        public <xsl:value-of select="$DirectoryName"/>_MultiplePointerControl()
        {
            pointer = new <xsl:value-of select="$DirectoryName"/>_Pointer();
            WidthPresentation = 300;
            Caption = $"{<xsl:value-of select="$DirectoryName"/>_Const.FULLNAME}:";
            PointerChanged += async (_, pointer) =&gt;
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
                if (pointers.Count &gt; 1) Presentation += $" ... {pointers.Count}";
            };
        }

        <xsl:value-of select="$DirectoryName"/>_Pointer pointer;
        List&lt;<xsl:value-of select="$DirectoryName"/>_Pointer&gt; pointers = [];
        public <xsl:value-of select="$DirectoryName"/>_Pointer Pointer
        {
            get
            {
                return pointer;
            }
            set
            {
                pointer = value;
                PointerChanged?.Invoke(null, pointer);
            }
        }

        public <xsl:value-of select="$DirectoryName"/>_Pointer[] GetPointers()
        {
            <xsl:value-of select="$DirectoryName"/>_Pointer[] copy = new <xsl:value-of select="$DirectoryName"/>_Pointer[pointers.Count];
            pointers.CopyTo(copy);

            return copy;
        }

        void Add(<xsl:value-of select="$DirectoryName"/>_Pointer item)
        {
            if (!pointers.Exists((<xsl:value-of select="$DirectoryName"/>_Pointer x) =&gt; x.UnigueID.ToString() == item.UnigueID.ToString()))
                pointers.Add(item);

            Pointer = item;
            //AfterSelectFunc?.Invoke();
        }

        <xsl:if test="normalize-space($DirectoryOwner) != ''">
        <xsl:variable name="namePointer" select="substring-after($DirectoryOwner, '.')" />
        public <xsl:value-of select="$namePointer"/>_Pointer Власник { get; set; } = new <xsl:value-of select="$namePointer"/>_Pointer();
        </xsl:if>

        protected override async void OpenSelect(object? sender, EventArgs args)
        {
            Popover popover = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };
            BeforeClickOpenFunc?.Invoke();
            <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір page = new <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір
            {
                PopoverParent = popover,
                DirectoryPointerItem = pointer.UnigueID,
                CallBack_OnSelectPointer = selectPointer =&gt;
                {
                    Add(new <xsl:value-of select="$DirectoryName"/>_Pointer(selectPointer));
                },
                CallBack_OnMultipleSelectPointer = selectPointers =&gt;
                {
                    foreach (var selectPointer in selectPointers)
                        Add(new <xsl:value-of select="$DirectoryName"/>_Pointer(selectPointer));
                }
            };
            <xsl:if test="normalize-space($DirectoryOwner) != ''">
            page.Власник.Pointer = Власник;
            </xsl:if>
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override async ValueTask FillList(ListBox listBox)
        {
            foreach (<xsl:value-of select="$DirectoryName"/>_Pointer item in pointers)
            {
                Box hBox = new Box(Orientation.Horizontal, 0);
                ListBoxRow listBoxRow = [hBox];

                string presentation = await item.GetPresentation();

                LinkButton linkName = new LinkButton("", SubstringName(presentation)) { Halign = Align.Start, Image = new Image(InterfaceGtk3.Іконки.ДляКнопок.Doc), AlwaysShowImage = true, TooltipText = presentation };
                linkName.Clicked += (sender, args) =&gt;
                {
                    if (Pointer.UnigueID.ToString() != item.UnigueID.ToString())
                        Pointer = item;
                };

                hBox.PackStart(linkName, true, true, 0);

                //Remove
                LinkButton linkRemove = new LinkButton("") { Halign = Align.Start, Image = new Image(InterfaceGtk3.Іконки.ДляКнопок.Clean), AlwaysShowImage = true };
                linkRemove.Clicked += (sender, args) =&gt;
                {
                    pointers.Remove(item);
                    listBox.Remove(listBoxRow);

                    if (Pointer.UnigueID.ToString() == item.UnigueID.ToString())
                        Pointer = pointers.Count &gt; 0 ? pointers[0] : new <xsl:value-of select="$DirectoryName"/>_Pointer();
                    else
                        PointerChanged?.Invoke(null, pointer);
                };

                hBox.PackEnd(linkRemove, false, false, 0);

                listBox.Add(listBoxRow);
                listBox.ShowAll();
            }
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            pointers = [];
            Pointer = new <xsl:value-of select="$DirectoryName"/>_Pointer();
            AfterSelectFunc?.Invoke();
            AfterClearFunc?.Invoke();
        }
    }
}
    </xsl:template>

<!--- 
//
// ============================ Звіт ============================
//
-->

    <!-- Список -->
    <xsl:template name="DirectoryReport">
        <xsl:variable name="DirectoryName" select="Directory/Name"/>
        <xsl:variable name="FieldsTL" select="Directory/ElementFields/Field"/>

/*
        <xsl:value-of select="$DirectoryName"/>_Звіт.cs
        Звіт
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Документи;

namespace <xsl:value-of select="$NameSpace"/>
{
    public static class <xsl:value-of select="$DirectoryName"/>_Звіт
    {
        public static async ValueTask Сформувати()
        {
            <xsl:variable name="CountFieldsTL" select="count($FieldsTL)"/>
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
            <xsl:value-of select="$DirectoryName"/>.{<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="Name"/>} AS <xsl:value-of select="Name"/>,
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
            <xsl:value-of select="$DirectoryName"/>.{<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="Name"/>} AS <xsl:value-of select="Name"/>
        </xsl:otherwise>
    </xsl:choose>
    <xsl:if test="position() != $CountFieldsTL">,
    </xsl:if>
    </xsl:for-each>
FROM
    {<xsl:value-of select="$DirectoryName"/>_Const.TABLE} AS <xsl:value-of select="$DirectoryName"/>
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
        <xsl:value-of select="$DirectoryName"/>.{<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="Name"/>}
    </xsl:for-each>
";
            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "<xsl:value-of select="$DirectoryName"/>_Звіт",
                Caption = "<xsl:value-of select="$DirectoryName"/>",
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