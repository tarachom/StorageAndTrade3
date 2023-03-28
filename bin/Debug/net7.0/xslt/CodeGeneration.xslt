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
  
  <!-- Для задання типу поля -->
  <xsl:template name="FieldType">
    <xsl:choose>
      <xsl:when test="Type = 'string'">
        <xsl:text>string</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'string[]'">
        <xsl:text>string[]</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'integer'">
        <xsl:text>int</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'integer[]'">
        <xsl:text>int[]</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'numeric'">
        <xsl:text>decimal</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'numeric[]'">
        <xsl:text>decimal[]</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'boolean'">
        <xsl:text>bool</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'time'">
        <xsl:text>TimeSpan</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'date' or Type = 'datetime'">
        <xsl:text>DateTime</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'pointer'">
        <xsl:value-of select="Pointer"/>
        <xsl:text>_Pointer</xsl:text>
      </xsl:when>
      <!--
      <xsl:when test="Type = 'empty_pointer'">
        <xsl:text>EmptyPointer</xsl:text>
      </xsl:when>
      -->
	  <xsl:when test="Type = 'any_pointer'">
        <xsl:text>Guid</xsl:text>
      </xsl:when>
	  <xsl:when test="Type = 'composite_pointer'">
        <xsl:text>UuidAndText</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'enum'">
        <xsl:value-of select="Pointer"/>
      </xsl:when>
	  <xsl:when test="Type = 'bytea'">
	    <xsl:text>byte[]</xsl:text>
      </xsl:when>
    </xsl:choose>    
  </xsl:template>
  
  <!-- Для конструкторів. Значення поля по замовчуванню відповідно до типу -->
  <xsl:template name="DefaultFieldValue">
    <xsl:choose>
      <xsl:when test="Type = 'string'">
        <xsl:text>""</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'string[]'">
        <xsl:text>new string[] { }</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'integer'">
        <xsl:text>0</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'integer[]'">
        <xsl:text>new int[] { }</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'numeric'">
        <xsl:text>0</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'numeric[]'">
        <xsl:text>new decimal[] { }</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'boolean'">
        <xsl:text>false</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'time'">
        <xsl:text>DateTime.MinValue.TimeOfDay</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'date' or Type = 'datetime'">
        <xsl:text>DateTime.MinValue</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'pointer'">
        <xsl:text>new </xsl:text>
        <xsl:value-of select="Pointer"/>
        <xsl:text>_Pointer()</xsl:text>
      </xsl:when>
      <!--
      <xsl:when test="Type = 'empty_pointer'">
        <xsl:text>new EmptyPointer()</xsl:text>
      </xsl:when>
      -->
	  <xsl:when test="Type = 'any_pointer'">
        <xsl:text>new Guid()</xsl:text>
      </xsl:when>
	  <xsl:when test="Type = 'composite_pointer'">
        <xsl:text>new UuidAndText()</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'enum'">
        <xsl:text>0</xsl:text>
      </xsl:when>
	  <xsl:when test="Type = 'bytea'">
	    <xsl:text>new byte[] { }</xsl:text>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <!-- Для параметрів функцій. Значення параметра по замовчуванню відповідно до типу -->
  <xsl:template name="DefaultParamValue">
    <xsl:choose>
      <xsl:when test="Type = 'string'">
        <xsl:text>""</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'string[]'">
        <xsl:text>null</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'integer'">
        <xsl:text>0</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'integer[]'">
        <xsl:text>null</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'numeric'">
        <xsl:text>0</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'numeric[]'">
        <xsl:text>null</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'boolean'">
        <xsl:text>false</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'time'">
        <xsl:text>null</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'date' or Type = 'datetime'">
        <xsl:text>null</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'pointer'">
        <xsl:text>null</xsl:text>
      </xsl:when>
      <!--
      <xsl:when test="Type = 'empty_pointer'">
        <xsl:text>null</xsl:text>
      </xsl:when>
      -->
	  <xsl:when test="Type = 'any_pointer'">
        <xsl:text>Guid.Empty</xsl:text>
      </xsl:when>
	  <xsl:when test="Type = 'composite_pointer'">
        <xsl:text>null</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'enum'">
        <xsl:text>0</xsl:text>
      </xsl:when>
	  <xsl:when test="Type = 'bytea'">
        <xsl:text>null</xsl:text>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  
  <!-- Для присвоєння значення полям -->
  <xsl:template name="ReadFieldValue">
     <xsl:param name="BaseFieldContainer" />
     
     <xsl:choose>
        <xsl:when test="Type = 'string'">
          <xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]?.ToString() ?? ""</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'string[]'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(string[])</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : new string[] { }</xsl:text>
        </xsl:when>
       <xsl:when test="Type = 'integer'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(int)</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : 0</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'integer[]'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(int[])</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : new int[] { }</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'numeric'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(decimal)</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : 0</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'numeric[]'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(decimal[])</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : new decimal[] { }</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'boolean'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>bool.Parse(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]?.ToString() ?? "False")</xsl:text>
          <xsl:text> : false</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'time'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>TimeSpan.Parse(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]?.ToString() ?? DateTime.MinValue.TimeOfDay.ToString())</xsl:text>
          <xsl:text> : DateTime.MinValue.TimeOfDay</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'date' or Type = 'datetime'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>DateTime.Parse(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]?.ToString() ?? DateTime.MinValue.ToString())</xsl:text>
          <xsl:text> : DateTime.MinValue</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'pointer'">
          <xsl:text>new </xsl:text><xsl:value-of select="Pointer"/>
          <xsl:text>_Pointer(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"])</xsl:text>
        </xsl:when>
        <!--
        <xsl:when test="Type = 'empty_pointer'">
          <xsl:text>new EmptyPointer()</xsl:text>
        </xsl:when>
        -->
		<xsl:when test="Type = 'any_pointer'">
		  <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>Guid.Parse(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]?.ToString() ?? Guid.Empty.ToString())</xsl:text>
          <xsl:text> : Guid.Empty</xsl:text>
        </xsl:when>
		<xsl:when test="Type = 'composite_pointer'">
		  <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(UuidAndText)</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : new UuidAndText()</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'enum'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(</xsl:text><xsl:value-of select="Pointer"/><xsl:text>)</xsl:text>
          <xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : 0</xsl:text>
        </xsl:when>
		<xsl:when test="Type = 'bytea'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(byte[])</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : new byte[] { }</xsl:text>
        </xsl:when>
     </xsl:choose>
  </xsl:template>

  <!-- Для перетворення поля в ХМЛ стрічку -->
  <xsl:template name="SerializeFieldValue">
    <xsl:text>"&lt;</xsl:text><xsl:value-of select="Name"/><xsl:text>&gt;" + </xsl:text>
    <xsl:choose>
      <xsl:when test="Type = 'enum'">
        <xsl:text>((int)</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'boolean'">
        <xsl:text>(</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'string'">
        <xsl:text>"&lt;![CDATA[" + </xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'string[]'">
        <xsl:text>ArrayToXml&lt;string&gt;.Convert(</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'integer[]'">
        <xsl:text>ArrayToXml&lt;int&gt;.Convert(</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'numeric[]'">
        <xsl:text>ArrayToXml&lt;decimal&gt;.Convert(</xsl:text>
      </xsl:when>
    </xsl:choose>
    <xsl:value-of select="Name"/>
    <xsl:choose>
      <xsl:when test="Type = 'enum'">
        <xsl:text>).ToString()</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'boolean'">
        <xsl:text> == true ? "1" : "0")</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'integer' or Type = 'numeric' or 
                Type = 'date' or Type = 'datetime' or Type = 'time' or
                Type = 'pointer' or Type = 'any_pointer' or Type = 'composite_pointer'">
        <xsl:text>.ToString()</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'string'">
        <xsl:text> + "]]&gt;"</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'string[]' or Type = 'integer[]' or Type = 'numeric[]'">
        <xsl:text>).ToString()</xsl:text>
      </xsl:when>
    </xsl:choose> 
    <xsl:text> + "&lt;/</xsl:text><xsl:value-of select="Name"/><xsl:text>&gt;" </xsl:text>
  </xsl:template>
  
  <!-- Документування коду -->
  <xsl:template name="CommentSummary">
    <xsl:variable name="normalize_space_Desc" select="normalize-space(Desc)" />
    <xsl:if test="$normalize_space_Desc != ''">
    <xsl:text>///&lt;summary</xsl:text>&gt;
    <xsl:text>///</xsl:text>
    <xsl:value-of select="$normalize_space_Desc"/>.
    <xsl:text>///&lt;/summary&gt;</xsl:text>
    </xsl:if>
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
 * Цей код згенерований в Конфігураторі 3. Шаблон CodeGeneration.xslt
 *
 */

using AccountingSoftware;
using System.Xml;

namespace <xsl:value-of select="Configuration/NameSpace"/>
{
    public static class Config
    {
        public static Kernel? Kernel { get; set; }
		
        public static void ReadAllConstants()
        {
            <xsl:for-each select="Configuration/ConstantsBlocks/ConstantsBlock">
                   <xsl:text>Константи.</xsl:text><xsl:value-of select="Name"/>.ReadAll();
            </xsl:for-each>
        }
    }

    public class Functions
    {
        /*
          Функція для типу який задається користувачем.
          Повертає презентацію для uuidAndText.
          В @pointer - повертає групу (Документи або Довідники)
            @type - повертає назву типу
        */
        public static string GetBasisObjectPresentation(UuidAndText uuidAndText, out string pointer, out string type)
        {
            pointer = type = "";

            if (uuidAndText.IsEmpty() || String.IsNullOrEmpty(uuidAndText.Text) || uuidAndText.Text.IndexOf(".") == -1)
                return "";

            string[] pointer_and_type = uuidAndText.Text.Split(".", StringSplitOptions.None);

            if (pointer_and_type.Length == 2)
            {
                pointer = pointer_and_type[0];
                type = pointer_and_type[1];

                if (pointer == "Документи")
                {
                    <xsl:variable name="DocCount" select="count(Configuration/Documents/Document)"/>
                    <xsl:if test="$DocCount != 0">
                    switch (type)
                    {
                        <xsl:for-each select="Configuration/Documents/Document">
                            <xsl:variable name="DocumentName" select="Name"/>
                        case "<xsl:value-of select="$DocumentName"/>": return new Документи.<xsl:value-of select="$DocumentName"/>_Pointer(uuidAndText.Uuid).GetPresentation();
                        </xsl:for-each>
                    }
                    </xsl:if>
                    <xsl:if test="$DocCount = 0">
                    return "";
                    </xsl:if>
                }
                else if (pointer == "Довідники")
                {
                    <xsl:variable name="DirCount" select="count(Configuration/Directories/Directory)"/>
                    <xsl:if test="$DirCount != 0">
                    switch (type)
                    {
                        <xsl:for-each select="Configuration/Directories/Directory">
                            <xsl:variable name="DirectoryName" select="Name"/>
                        case "<xsl:value-of select="$DirectoryName"/>": return new Довідники.<xsl:value-of select="$DirectoryName"/>_Pointer(uuidAndText.Uuid).GetPresentation();
                        </xsl:for-each>
                    }
                    </xsl:if>
                    <xsl:if test="$DirCount = 0">
                    return "";
                    </xsl:if>
                }
            }

            return "";
        }
    }
}

namespace <xsl:value-of select="Configuration/NameSpace"/>.Константи
{
    <xsl:for-each select="Configuration/ConstantsBlocks/ConstantsBlock">
	  #region CONSTANTS BLOCK "<xsl:value-of select="Name"/>"
    public static class <xsl:value-of select="Name"/>
    {
        public static void ReadAll()
        {
            <xsl:variable name="Constants" select="Constants/Constant" />
		        <xsl:if test="count($Constants) &gt; 0">
            Dictionary&lt;string, object&gt; fieldValue = new Dictionary&lt;string, object&gt;();
            bool IsSelect = Config.Kernel!.DataBase.SelectAllConstants("tab_constants",
                 <xsl:text>new string[] { </xsl:text>
                 <xsl:for-each select="$Constants">
                   <xsl:if test="position() != 1">
                     <xsl:text>, </xsl:text>
                   </xsl:if>
                   <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"</xsl:text>
                 </xsl:for-each> }, fieldValue);
            
            if (IsSelect)
            {
                <xsl:for-each select="$Constants">
                  <xsl:text>m_</xsl:text>
                  <xsl:value-of select="Name"/>
                  <xsl:text>_Const = </xsl:text>
                  <xsl:call-template name="ReadFieldValue">
                    <xsl:with-param name="BaseFieldContainer">fieldValue</xsl:with-param>
                  </xsl:call-template>;
                </xsl:for-each>
            }
			      </xsl:if>
        }
        
        <xsl:for-each select="Constants/Constant">
        static <xsl:call-template name="FieldType" />
        <xsl:text> m_</xsl:text>
        <xsl:value-of select="Name"/>
        <xsl:text>_Const = </xsl:text>
        <xsl:call-template name="DefaultFieldValue" />;
        <xsl:text>public static </xsl:text>
        <xsl:call-template name="FieldType" />
        <xsl:text> </xsl:text>
        <xsl:value-of select="Name"/>_Const
        {
            get 
            {
                return m_<xsl:value-of select="Name"/><xsl:text>_Const</xsl:text>
                <xsl:if test="Type = 'pointer'">
                <xsl:variable name="groupPointer" select="substring-before(Pointer, '.')" />
                <xsl:choose>
                  <xsl:when test="$groupPointer = 'Довідники'">
                    <xsl:text>.GetNewDirectoryPointer()</xsl:text>
                  </xsl:when>
                  <xsl:when test="$groupPointer = 'Документи'">
                    <xsl:text>.GetNewDocumentPointer()</xsl:text>
                  </xsl:when>
                </xsl:choose>
                </xsl:if>;
            }
            set
            {
                m_<xsl:value-of select="Name"/>_Const = value;
                Config.Kernel!.DataBase.SaveConstants("tab_constants", "<xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                <xsl:choose>
                  <xsl:when test="Type = 'enum'">
                    <xsl:text>(int)</xsl:text>
                  </xsl:when>
                </xsl:choose>
                <xsl:text>m_</xsl:text>
                <xsl:value-of select="Name"/>
                <xsl:text>_Const</xsl:text>
                <xsl:choose>
                  <xsl:when test="Type = 'pointer'">
                    <xsl:text>.UnigueID.UGuid</xsl:text>
                  </xsl:when>
                </xsl:choose>);
            }
        }
        </xsl:for-each>

        <xsl:for-each select="Constants/Constant">
          <xsl:variable name="ConstantsName" select="Name"/>
          <xsl:for-each select="TabularParts/TablePart">
            <!-- TableParts -->
            <xsl:variable name="TablePartName" select="Name"/>
            <xsl:variable name="TablePartFullName" select="concat($ConstantsName, '_', $TablePartName)"/>
        
        public class <xsl:value-of select="$TablePartFullName"/>_TablePart : ConstantsTablePart
        {
            public <xsl:value-of select="$TablePartFullName"/>_TablePart() : base(Config.Kernel!, "<xsl:value-of select="Table"/>",
                 <xsl:text>new string[] { </xsl:text>
                 <xsl:for-each select="Fields/Field">
                   <xsl:if test="position() != 1">
                     <xsl:text>, </xsl:text>
                   </xsl:if>
                   <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"</xsl:text>
                 </xsl:for-each> }) 
            {
                Records = new List&lt;Record&gt;();
            }
            
            public const string TABLE = "<xsl:value-of select="Table"/>";
            <xsl:for-each select="Fields/Field">
            public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>
            public List&lt;Record&gt; Records { get; set; }
        
            public void Read()
            {
                Records.Clear();
                base.BaseRead();

                foreach (Dictionary&lt;string, object&gt; fieldValue in base.FieldValueList) 
                {
                    Record record = new Record();
                    record.UID = (Guid)fieldValue["uid"];
                    
                    <xsl:for-each select="Fields/Field">
                      <xsl:text>record.</xsl:text>
                      <xsl:value-of select="Name"/>
                      <xsl:text> = </xsl:text>
                      <xsl:call-template name="ReadFieldValue">
                        <xsl:with-param name="BaseFieldContainer">fieldValue</xsl:with-param>
                      </xsl:call-template>;
                    </xsl:for-each>
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public void Save(bool clear_all_before_save /*= true*/) 
            {
                base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary&lt;string, object&gt; fieldValue = new Dictionary&lt;string, object&gt;();

                    <xsl:for-each select="Fields/Field">
                        <xsl:text>fieldValue.Add("</xsl:text>
                        <xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                        <xsl:if test="Type = 'enum'">
                            <xsl:text>(int)</xsl:text>
                          </xsl:if>
                        <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>
                        <xsl:choose>
                        <xsl:when test="Type = 'pointer'">
                            <xsl:text>.UnigueID.UGuid</xsl:text>
                        </xsl:when>
                        </xsl:choose>
                        <xsl:text>)</xsl:text>;
                    </xsl:for-each>
                    base.BaseSave(record.UID, fieldValue);
                }
                
                base.BaseCommitTransaction();
            }
        
            public void Delete()
            {
                base.BaseDelete();
            }
            
            public class Record : ConstantsTablePartRecord
            {
                <xsl:for-each select="Fields/Field">
                  <xsl:text>public </xsl:text>
                  <xsl:call-template name="FieldType" />
                  <xsl:text> </xsl:text>
                  <xsl:value-of select="Name"/>
                  <xsl:text> { get; set; } = </xsl:text>
                  <xsl:call-template name="DefaultFieldValue" />;
                </xsl:for-each>
            }
        }
          </xsl:for-each>
        </xsl:for-each>     
    }
    #endregion
    </xsl:for-each>
}

namespace <xsl:value-of select="Configuration/NameSpace"/>.Довідники
{
    <xsl:for-each select="Configuration/Directories/Directory">
      <xsl:variable name="DirectoryName" select="Name"/>
    #region DIRECTORY "<xsl:value-of select="$DirectoryName"/>"
    public static class <xsl:value-of select="$DirectoryName"/>_Const
    {
        public const string TABLE = "<xsl:value-of select="Table"/>";
        <xsl:for-each select="Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>
    }

    public class <xsl:value-of select="$DirectoryName"/>_Objest : DirectoryObject
    {
        public <xsl:value-of select="$DirectoryName"/>_Objest() : base(Config.Kernel!, "<xsl:value-of select="Table"/>",
             <xsl:text>new string[] { </xsl:text>
             <xsl:for-each select="Fields/Field">
               <xsl:if test="position() != 1">
                 <xsl:text>, </xsl:text>
               </xsl:if>
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"</xsl:text>
             </xsl:for-each> }) 
        {
            <xsl:for-each select="Fields/Field">
              <xsl:value-of select="Name"/>
              <xsl:text> = </xsl:text>
              <xsl:call-template name="DefaultFieldValue" />;
            </xsl:for-each>
            <xsl:if test="count(TabularParts/TablePart) &gt; 0">
            //Табличні частини
            </xsl:if>
            <xsl:for-each select="TabularParts/TablePart">
                <xsl:variable name="TablePartName" select="concat(Name, '_TablePart')"/>
                <xsl:value-of select="$TablePartName"/><xsl:text> = new </xsl:text>
                <xsl:value-of select="concat($DirectoryName, '_', $TablePartName)"/><xsl:text>(this)</xsl:text>;
            </xsl:for-each>
        }
        
        public void New()
        {
            BaseNew();
            <xsl:if test="normalize-space(TriggerFunctions/New) != ''">
                <xsl:value-of select="TriggerFunctions/New"/><xsl:text>(this)</xsl:text>;
            </xsl:if>
        }

        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                <xsl:for-each select="Fields/Field">
                  <xsl:value-of select="Name"/>
                  <xsl:text> = </xsl:text>
                  <xsl:call-template name="ReadFieldValue">
                    <xsl:with-param name="BaseFieldContainer">base.FieldValue</xsl:with-param>
                  </xsl:call-template>;
                </xsl:for-each>
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            <xsl:if test="normalize-space(TriggerFunctions/BeforeSave) != ''">
                <xsl:value-of select="TriggerFunctions/BeforeSave"/><xsl:text>(this)</xsl:text>;
            </xsl:if>
            <xsl:for-each select="Fields/Field">
              <xsl:text>base.FieldValue["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] = </xsl:text>
              <xsl:if test="Type = 'enum'">
                  <xsl:text>(int)</xsl:text>      
              </xsl:if>
              <xsl:value-of select="Name"/>
              <xsl:choose>
                <xsl:when test="Type = 'pointer'">
                  <xsl:text>.UnigueID.UGuid</xsl:text>
                </xsl:when>
              </xsl:choose>;
            </xsl:for-each>
            BaseSave();
            <xsl:if test="normalize-space(TriggerFunctions/AfterSave) != ''">
                <xsl:value-of select="TriggerFunctions/AfterSave"/><xsl:text>(this);</xsl:text>      
            </xsl:if>
            BaseWriteFullTextSearch(GetBasis(), new string[] { <xsl:for-each select="Fields/Field[IsFullTextSearch = '1' and Type = 'string']">
              <xsl:if test="position() != 1">
                <xsl:text>, </xsl:text>
              </xsl:if>
              <xsl:value-of select="Name"/>
            </xsl:for-each> });
        }

        public <xsl:value-of select="$DirectoryName"/>_Objest Copy()
        {
            <xsl:value-of select="$DirectoryName"/>_Objest copy = new <xsl:value-of select="$DirectoryName"/>_Objest();
            <xsl:for-each select="Fields/Field">
              <xsl:text>copy.</xsl:text><xsl:value-of select="Name"/><xsl:text> = </xsl:text><xsl:value-of select="Name"/>;
            </xsl:for-each>
            copy.New();
            return copy;
        }

        public void Delete()
        {
            <xsl:if test="normalize-space(TriggerFunctions/BeforeDelete) != ''">
                <xsl:value-of select="TriggerFunctions/BeforeDelete"/><xsl:text>(this);</xsl:text>      
            </xsl:if>
            base.BaseDelete(<xsl:text>new string[] { </xsl:text>
            <xsl:for-each select="TabularParts/TablePart">
               <xsl:if test="position() != 1">
                 <xsl:text>, </xsl:text>
               </xsl:if>
               <xsl:text>"</xsl:text><xsl:value-of select="Table"/><xsl:text>"</xsl:text>
            </xsl:for-each> });
        }
        
        public <xsl:value-of select="$DirectoryName"/>_Pointer GetDirectoryPointer()
        {
            return new <xsl:value-of select="$DirectoryName"/>_Pointer(UnigueID.UGuid);
        }

        public UuidAndText GetBasis()
        {
            return new UuidAndText(UnigueID.UGuid, "Довідники.<xsl:value-of select="$DirectoryName"/>");
        }
        
        <xsl:for-each select="Fields/Field">
          <xsl:text>public </xsl:text>
          <xsl:call-template name="FieldType" />
          <xsl:text> </xsl:text>
          <xsl:value-of select="Name"/>
          <xsl:text> { get; set; </xsl:text>}
        </xsl:for-each>
        <xsl:if test="count(TabularParts/TablePart) &gt; 0">
        //Табличні частини
        </xsl:if>
        <xsl:for-each select="TabularParts/TablePart">
            <xsl:variable name="TablePartName" select="concat(Name, '_TablePart')"/>
            <xsl:text>public </xsl:text><xsl:value-of select="concat($DirectoryName, '_', $TablePartName)"/><xsl:text> </xsl:text>
            <xsl:value-of select="$TablePartName"/><xsl:text> { get; set; </xsl:text>}
        </xsl:for-each>
    }

    public class <xsl:value-of select="$DirectoryName"/>_Pointer : DirectoryPointer
    {
        public <xsl:value-of select="$DirectoryName"/>_Pointer(object? uid = null) : base(Config.Kernel!, "<xsl:value-of select="Table"/>")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public <xsl:value-of select="$DirectoryName"/>_Pointer(UnigueID uid, Dictionary&lt;string, object&gt;? fields = null) : base(Config.Kernel!, "<xsl:value-of select="Table"/>")
        {
            base.Init(uid, fields);
        }
        
        public <xsl:value-of select="$DirectoryName"/>_Objest? GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>ObjestItem = new <xsl:value-of select="$DirectoryName"/>_Objest();
            return <xsl:value-of select="$DirectoryName"/>ObjestItem.Read(base.UnigueID) ? <xsl:value-of select="$DirectoryName"/>ObjestItem : null;
        }

        public <xsl:value-of select="$DirectoryName"/>_Pointer GetNewDirectoryPointer()
        {
            return new <xsl:value-of select="$DirectoryName"/>_Pointer(base.UnigueID);
        }

        public string Назва { get; set; } = "";

        public string GetPresentation()
        {
            return Назва = base.BasePresentation(
              <xsl:text>new string[] { </xsl:text>
              <xsl:for-each select="Fields/Field[IsPresentation=1]">
                <xsl:if test="position() != 1">
                  <xsl:text>, </xsl:text>
                </xsl:if>
                <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"</xsl:text>
              </xsl:for-each> }
            );
        }
		
        public <xsl:value-of select="$DirectoryName"/>_Pointer GetEmptyPointer()
        {
            return new <xsl:value-of select="$DirectoryName"/>_Pointer();
        }

        public UuidAndText GetBasis()
        {
            return new UuidAndText(UnigueID.UGuid, "Довідники.<xsl:value-of select="$DirectoryName"/>");
        }
    }
    
    public class <xsl:value-of select="$DirectoryName"/>_Select : DirectorySelect
    {
        public <xsl:value-of select="$DirectoryName"/>_Select() : base(Config.Kernel!, "<xsl:value-of select="Table"/>") { }        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new <xsl:value-of select="$DirectoryName"/>_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public <xsl:value-of select="$DirectoryName"/>_Pointer? Current { get; private set; }
        
        public <xsl:value-of select="$DirectoryName"/>_Pointer FindByField(string name, object value)
        {
            <xsl:value-of select="$DirectoryName"/>_Pointer itemPointer = new <xsl:value-of select="$DirectoryName"/>_Pointer();
            DirectoryPointer directoryPointer = base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public List&lt;<xsl:value-of select="$DirectoryName"/>_Pointer&gt; FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List&lt;<xsl:value-of select="$DirectoryName"/>_Pointer&gt; directoryPointerList = new List&lt;<xsl:value-of select="$DirectoryName"/>_Pointer&gt;();
            foreach (DirectoryPointer directoryPointer in base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new <xsl:value-of select="$DirectoryName"/>_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      <xsl:for-each select="TabularParts/TablePart"> <!-- TableParts -->
        <xsl:variable name="TablePartName" select="Name"/>
        <xsl:variable name="TablePartFullName" select="concat($DirectoryName, '_', $TablePartName)"/>
    
    public class <xsl:value-of select="$TablePartFullName"/>_TablePart : DirectoryTablePart
    {
        public <xsl:value-of select="$TablePartFullName"/>_TablePart(<xsl:value-of select="$DirectoryName"/>_Objest owner) : base(Config.Kernel!, "<xsl:value-of select="Table"/>",
             <xsl:text>new string[] { </xsl:text>
             <xsl:for-each select="Fields/Field">
               <xsl:if test="position() != 1">
                 <xsl:text>, </xsl:text>
               </xsl:if>
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"</xsl:text>
             </xsl:for-each> }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List&lt;Record&gt;();
        }
        <xsl:for-each select="Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>

        public <xsl:value-of select="$DirectoryName"/>_Objest Owner { get; private set; }
        
        public List&lt;Record&gt; Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary&lt;string, object&gt; fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                <xsl:for-each select="Fields/Field">
                  <xsl:text>record.</xsl:text>
                  <xsl:value-of select="Name"/>
                  <xsl:text> = </xsl:text>
                  <xsl:call-template name="ReadFieldValue">
                    <xsl:with-param name="BaseFieldContainer">fieldValue</xsl:with-param>
                  </xsl:call-template>;
                </xsl:for-each>
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);
            
            foreach (Record record in Records)
            {
                Dictionary&lt;string, object&gt; fieldValue = new Dictionary&lt;string, object&gt;();

                <xsl:for-each select="Fields/Field">
                    <xsl:text>fieldValue.Add("</xsl:text>
                    <xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                    <xsl:if test="Type = 'enum'">
                      <xsl:text>(int)</xsl:text>
                    </xsl:if>
                    <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>
                    <xsl:choose>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:text>.UnigueID.UGuid</xsl:text>
                    </xsl:when>
                    </xsl:choose>
                    <xsl:text>)</xsl:text>;
                </xsl:for-each>
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        
        public void Delete()
        {
            base.BaseDelete(Owner.UnigueID);
        }
        
        public class Record : DirectoryTablePartRecord
        {
            <xsl:for-each select="Fields/Field">
              <xsl:text>public </xsl:text>
              <xsl:call-template name="FieldType" />
              <xsl:text> </xsl:text>
              <xsl:value-of select="Name"/>
              <xsl:text> { get; set; } = </xsl:text>
              <xsl:call-template name="DefaultFieldValue" />;
            </xsl:for-each>
        }
    }
      </xsl:for-each> <!-- TableParts -->
   
    #endregion
    </xsl:for-each>
}

namespace <xsl:value-of select="Configuration/NameSpace"/>.Перелічення
{
    <xsl:for-each select="Configuration/Enums/Enum">
    #region ENUM "<xsl:value-of select="Name"/>"
    public enum <xsl:value-of select="Name"/>
    {
         <xsl:variable name="CountEnumField" select="count(Fields/Field)" />
         <xsl:for-each select="Fields/Field">
             <xsl:value-of select="Name"/>
             <xsl:text> = </xsl:text>
             <xsl:value-of select="Value"/>
             <xsl:if test="position() &lt; $CountEnumField">
         <xsl:text>,
         </xsl:text>
             </xsl:if>
         </xsl:for-each>
    }
    #endregion
    </xsl:for-each>

    public static class ПсевдонімиПерелічення
    {
    <xsl:for-each select="Configuration/Enums/Enum">
        <xsl:variable name="EnumName" select="Name" />
        <xsl:variable name="CountEnumField" select="count(Fields/Field)" />
        #region ENUM "<xsl:value-of select="$EnumName"/>"
        public static string <xsl:value-of select="$EnumName"/>_Alias(<xsl:value-of select="$EnumName"/> value)
        {
            switch (value)
            {
                <xsl:for-each select="Fields/Field">
                  <xsl:variable name="ReturnValue">
                      <xsl:choose>
                          <xsl:when test="normalize-space(Desc) != ''">
                              <xsl:value-of select="normalize-space(Desc)"/>
                          </xsl:when>
                          <xsl:otherwise>
                              <xsl:value-of select="normalize-space(Name)"/>
                          </xsl:otherwise>
                      </xsl:choose>
                  </xsl:variable>
                case <xsl:value-of select="$EnumName"/>.<xsl:value-of select="Name"/>: return "<xsl:value-of select="$ReturnValue"/>";
                </xsl:for-each>
                default: return "";
            }
        }

        public static NameValue&lt;<xsl:value-of select="$EnumName"/>&gt;[] <xsl:value-of select="$EnumName"/>_Array()
        {
            NameValue&lt;<xsl:value-of select="$EnumName"/>&gt;[] value = new NameValue&lt;<xsl:value-of select="$EnumName"/>&gt;[<xsl:value-of select="$CountEnumField"/>];
            <xsl:for-each select="Fields/Field">
              <xsl:variable name="ReturnValue">
                  <xsl:choose>
                      <xsl:when test="normalize-space(Desc) != ''">
                          <xsl:value-of select="normalize-space(Desc)"/>
                      </xsl:when>
                      <xsl:otherwise>
                          <xsl:value-of select="normalize-space(Name)"/>
                      </xsl:otherwise>
                  </xsl:choose>
              </xsl:variable>
            value[<xsl:value-of select="position() - 1"/>] = new NameValue&lt;<xsl:value-of select="$EnumName"/>&gt;("<xsl:value-of select="$ReturnValue"/>", <xsl:value-of select="$EnumName"/>.<xsl:value-of select="Name"/>);
            </xsl:for-each>
            return value;
        }
        #endregion
    </xsl:for-each>
    }
}

namespace <xsl:value-of select="Configuration/NameSpace"/>.Документи
{
    <xsl:for-each select="Configuration/Documents/Document">
      <xsl:variable name="DocumentName" select="Name"/>
    #region DOCUMENT "<xsl:value-of select="$DocumentName"/>"
    public static class <xsl:value-of select="$DocumentName"/>_Const
    {
        public const string TABLE = "<xsl:value-of select="Table"/>";
        <xsl:for-each select="Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>
    }

    public static class <xsl:value-of select="$DocumentName"/>_Export
    {
        public static void ToXmlFile(<xsl:value-of select="$DocumentName"/>_Pointer <xsl:value-of select="$DocumentName"/>, string pathToSave)
        {
            <xsl:value-of select="$DocumentName"/>_Objest <xsl:value-of select="$DocumentName"/>_Objest = <xsl:value-of select="$DocumentName"/>.GetDocumentObject(true);

            XmlWriter xmlWriter = XmlWriter.Create(pathToSave, new XmlWriterSettings() { Indent = true, Encoding = System.Text.Encoding.UTF8 });
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("root");
            xmlWriter.WriteAttributeString("uid", <xsl:value-of select="$DocumentName"/>_Objest.UnigueID.ToString());
            <xsl:for-each select="Fields/Field">
            xmlWriter.WriteStartElement("<xsl:value-of select="Name"/>");
            xmlWriter.WriteAttributeString("type", "<xsl:value-of select="Type"/>");
            <xsl:choose>
              <xsl:when test="Type = 'pointer'">
                <xsl:variable name="groupPointer" select="substring-before(Pointer, '.')" />
                <xsl:choose>
                  <xsl:when test="$groupPointer = 'Довідники' or $groupPointer = 'Документи'">
                    xmlWriter.WriteAttributeString("pointer", "<xsl:value-of select="Pointer"/>");
                    xmlWriter.WriteAttributeString("uid", <xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/>.UnigueID.ToString());
                    xmlWriter.WriteString(<xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/>.GetPresentation());
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
              <xsl:when test="Type = 'enum'">
                xmlWriter.WriteAttributeString("pointer", "<xsl:value-of select="Pointer"/>");
                xmlWriter.WriteAttributeString("uid", ((int)<xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/>).ToString());
                xmlWriter.WriteString(<xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/>.ToString());
              </xsl:when>
              <xsl:when test="Type = 'composite_pointer'">
                xmlWriter.WriteRaw(((UuidAndText)<xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/>).ToXml());
              </xsl:when>
              <xsl:otherwise>
                xmlWriter.WriteValue(<xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/>);
              </xsl:otherwise>
            </xsl:choose>
            xmlWriter.WriteEndElement(); //<xsl:value-of select="Name"/>
            </xsl:for-each>

            <xsl:if test="count(TabularParts/TablePart) &gt; 0">

                /* 
                Табличні частини
                */

                xmlWriter.WriteStartElement("TabularParts");
                <xsl:for-each select="TabularParts/TablePart">
                    <xsl:variable name="TablePartName" select="Name"/>
                    <xsl:variable name="TablePartFullName" select="concat($DocumentName, '_', $TablePartName)"/>
                    xmlWriter.WriteStartElement("TablePart");
                    xmlWriter.WriteAttributeString("name", "<xsl:value-of select="Name"/>");

                    foreach(<xsl:value-of select="$TablePartFullName"/>_TablePart.Record record in <xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="$TablePartName"/>_TablePart.Records)
                    {
                        xmlWriter.WriteStartElement("row");
                        xmlWriter.WriteAttributeString("uid", record.UID.ToString());
                        <xsl:for-each select="Fields/Field">
                        xmlWriter.WriteStartElement("<xsl:value-of select="Name"/>");
                        xmlWriter.WriteAttributeString("type", "<xsl:value-of select="Type"/>");
                        <xsl:choose>
                          <xsl:when test="Type = 'pointer'">
                            <xsl:variable name="groupPointer" select="substring-before(Pointer, '.')" />
                            <xsl:choose>
                              <xsl:when test="$groupPointer = 'Довідники' or $groupPointer = 'Документи'">
                                xmlWriter.WriteAttributeString("pointer", "<xsl:value-of select="Pointer"/>");
                                xmlWriter.WriteAttributeString("uid", record.<xsl:value-of select="Name"/>.UnigueID.ToString());
                                xmlWriter.WriteString(record.<xsl:value-of select="Name"/>.GetPresentation());
                              </xsl:when>
                            </xsl:choose>
                          </xsl:when>
                          <xsl:when test="Type = 'enum'">
                            xmlWriter.WriteAttributeString("pointer", "<xsl:value-of select="Pointer"/>");
                            xmlWriter.WriteAttributeString("uid", ((int)record.<xsl:value-of select="Name"/>).ToString());
                            xmlWriter.WriteString(record.<xsl:value-of select="Name"/>.ToString());
                          </xsl:when>
                          <xsl:when test="Type = 'composite_pointer'">
                            xmlWriter.WriteRaw(((UuidAndText)record.<xsl:value-of select="Name"/>).ToXml());
                          </xsl:when>
                          <xsl:otherwise>
                            xmlWriter.WriteValue(record.<xsl:value-of select="Name"/>);
                          </xsl:otherwise>
                        </xsl:choose>
                        xmlWriter.WriteEndElement(); //<xsl:value-of select="Name"/>
                        </xsl:for-each>
                        xmlWriter.WriteEndElement(); //row
                    }

                    xmlWriter.WriteEndElement(); //TablePart
                </xsl:for-each>
                xmlWriter.WriteEndElement(); //TabularParts
            </xsl:if>

            xmlWriter.WriteEndElement(); //root
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }
    }

    public class <xsl:value-of select="$DocumentName"/>_Objest : DocumentObject
    {
        public <xsl:value-of select="$DocumentName"/>_Objest() : base(Config.Kernel!, "<xsl:value-of select="Table"/>", "<xsl:value-of select="$DocumentName"/>",
             <xsl:text>new string[] { </xsl:text>
             <xsl:for-each select="Fields/Field">
               <xsl:if test="position() != 1">
                 <xsl:text>, </xsl:text>
               </xsl:if>
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"</xsl:text>
             </xsl:for-each> }) 
        {
            <xsl:for-each select="Fields/Field">
              <xsl:value-of select="Name"/>
              <xsl:text> = </xsl:text>
              <xsl:call-template name="DefaultFieldValue" />;
            </xsl:for-each>
            <xsl:if test="count(TabularParts/TablePart) &gt; 0">
            //Табличні частини
            </xsl:if>
            <xsl:for-each select="TabularParts/TablePart">
                <xsl:variable name="TablePartName" select="concat(Name, '_TablePart')"/>
                <xsl:value-of select="$TablePartName"/><xsl:text> = new </xsl:text>
                <xsl:value-of select="concat($DocumentName, '_', $TablePartName)"/><xsl:text>(this)</xsl:text>;
            </xsl:for-each>
        }
        
        public void New()
        {
            BaseNew();
            <xsl:if test="normalize-space(TriggerFunctions/New) != ''">
                <xsl:value-of select="TriggerFunctions/New"/><xsl:text>(this)</xsl:text>;
            </xsl:if>
        }

        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                <xsl:for-each select="Fields/Field">
                  <xsl:value-of select="Name"/>
                  <xsl:text> = </xsl:text>
                  <xsl:call-template name="ReadFieldValue">
                    <xsl:with-param name="BaseFieldContainer">base.FieldValue</xsl:with-param>
                  </xsl:call-template>;
                </xsl:for-each>
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            <xsl:if test="normalize-space(TriggerFunctions/BeforeSave) != ''">
                <xsl:value-of select="TriggerFunctions/BeforeSave"/><xsl:text>(this)</xsl:text>;
            </xsl:if>
            <xsl:for-each select="Fields/Field">
              <xsl:text>base.FieldValue["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] = </xsl:text>
              <xsl:if test="Type = 'enum'">
                  <xsl:text>(int)</xsl:text>      
              </xsl:if>
              <xsl:value-of select="Name"/>
              <xsl:choose>
                <xsl:when test="Type = 'pointer'">
                  <xsl:text>.UnigueID.UGuid</xsl:text>
                </xsl:when>
              </xsl:choose>;
            </xsl:for-each>
            BaseSave();
            <xsl:if test="normalize-space(TriggerFunctions/AfterSave) != ''">
                <xsl:value-of select="TriggerFunctions/AfterSave"/><xsl:text>(this);</xsl:text>      
            </xsl:if>
            BaseWriteFullTextSearch(GetBasis(), new string[] { <xsl:for-each select="Fields/Field[IsFullTextSearch = '1' and Type = 'string']">
              <xsl:if test="position() != 1">
                <xsl:text>, </xsl:text>
              </xsl:if>
              <xsl:value-of select="Name"/>
            </xsl:for-each> });
        }

        public bool SpendTheDocument(DateTime spendDate)
        {
            <xsl:choose>
                <xsl:when test="normalize-space(SpendFunctions/Spend) != ''">
                <xsl:text>bool rezult = </xsl:text><xsl:value-of select="SpendFunctions/Spend"/><xsl:text>(this)</xsl:text>;
                <xsl:text>BaseSpend(rezult, spendDate)</xsl:text>;
                <xsl:text>return rezult;</xsl:text>
                </xsl:when>
                <xsl:otherwise>
                <xsl:text>BaseSpend(false, DateTime.MinValue)</xsl:text>;
                <xsl:text>return false;</xsl:text>
                </xsl:otherwise>
            </xsl:choose>
        }

        public void ClearSpendTheDocument()
        {
            <xsl:if test="normalize-space(SpendFunctions/ClearSpend) != ''">
                <xsl:value-of select="SpendFunctions/ClearSpend"/>
              <xsl:text>(this)</xsl:text>;
            </xsl:if>
            <xsl:text>BaseSpend(false, DateTime.MinValue);</xsl:text>
        }

        public <xsl:value-of select="$DocumentName"/>_Objest Copy()
        {
            <xsl:value-of select="$DocumentName"/>_Objest copy = new <xsl:value-of select="$DocumentName"/>_Objest();
            <xsl:for-each select="Fields/Field">
              <xsl:text>copy.</xsl:text><xsl:value-of select="Name"/><xsl:text> = </xsl:text><xsl:value-of select="Name"/>;
            </xsl:for-each>
            return copy;
        }

        public void Delete()
        {
            <xsl:if test="normalize-space(TriggerFunctions/BeforeDelete) != ''">
                <xsl:value-of select="TriggerFunctions/BeforeDelete"/><xsl:text>(this);</xsl:text>      
            </xsl:if>
            base.BaseDelete(<xsl:text>new string[] { </xsl:text>
            <xsl:for-each select="TabularParts/TablePart">
              <xsl:if test="position() != 1">
                <xsl:text>, </xsl:text>
              </xsl:if>
              <xsl:text>"</xsl:text><xsl:value-of select="Table"/><xsl:text>"</xsl:text>
            </xsl:for-each> });
        }
        
        public <xsl:value-of select="$DocumentName"/>_Pointer GetDocumentPointer()
        {
            return new <xsl:value-of select="$DocumentName"/>_Pointer(UnigueID.UGuid);
        }

        public UuidAndText GetBasis()
        {
            return new UuidAndText(UnigueID.UGuid, "Документи.<xsl:value-of select="$DocumentName"/>");
        }
        
        <xsl:for-each select="Fields/Field">
          <xsl:text>public </xsl:text>
          <xsl:call-template name="FieldType" />
          <xsl:text> </xsl:text>
          <xsl:value-of select="Name"/>
          <xsl:text> { get; set; </xsl:text>}
        </xsl:for-each>
        <xsl:if test="count(TabularParts/TablePart) &gt; 0">
        //Табличні частини
        </xsl:if>
        <xsl:for-each select="TabularParts/TablePart">
            <xsl:variable name="TablePartName" select="concat(Name, '_TablePart')"/>
            <xsl:text>public </xsl:text><xsl:value-of select="concat($DocumentName, '_', $TablePartName)"/><xsl:text> </xsl:text>
            <xsl:value-of select="$TablePartName"/><xsl:text> { get; set; </xsl:text>}
        </xsl:for-each>
    }
    
    public class <xsl:value-of select="$DocumentName"/>_Pointer : DocumentPointer
    {
        public <xsl:value-of select="$DocumentName"/>_Pointer(object? uid = null) : base(Config.Kernel!, "<xsl:value-of select="Table"/>", "<xsl:value-of select="$DocumentName"/>")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public <xsl:value-of select="$DocumentName"/>_Pointer(UnigueID uid, Dictionary&lt;string, object&gt;? fields = null) : base(Config.Kernel!, "<xsl:value-of select="Table"/>", "<xsl:value-of select="$DocumentName"/>")
        {
            base.Init(uid, fields);
        }

        public string Назва { get; set; } = "";

        public string GetPresentation()
        {
            return Назва = base.BasePresentation(
              <xsl:text>new string[] { </xsl:text>
              <xsl:for-each select="Fields/Field[IsPresentation=1]">
                <xsl:if test="position() != 1">
                  <xsl:text>, </xsl:text>
                </xsl:if>
                <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"</xsl:text>
              </xsl:for-each> }
            );
        }

        public <xsl:value-of select="$DocumentName"/>_Pointer GetNewDocumentPointer()
        {
            return new <xsl:value-of select="$DocumentName"/>_Pointer(base.UnigueID);
        }

        public <xsl:value-of select="$DocumentName"/>_Pointer GetEmptyPointer()
        {
            return new <xsl:value-of select="$DocumentName"/>_Pointer();
        }

        public UuidAndText GetBasis()
        {
            return new UuidAndText(UnigueID.UGuid, "Документи.<xsl:value-of select="$DocumentName"/>");
        }

        public <xsl:value-of select="$DocumentName"/>_Objest GetDocumentObject(bool readAllTablePart = false)
        {
            <xsl:value-of select="$DocumentName"/>_Objest <xsl:value-of select="$DocumentName"/>ObjestItem = new <xsl:value-of select="$DocumentName"/>_Objest();
            <xsl:value-of select="$DocumentName"/>ObjestItem.Read(base.UnigueID);
            <xsl:if test="count(TabularParts/TablePart) != 0">
            if (readAllTablePart)
            {   
                <xsl:for-each select="TabularParts/TablePart">
                <xsl:value-of select="$DocumentName"/>ObjestItem.<xsl:value-of select="concat(Name, '_TablePart')"/>.Read();</xsl:for-each>
            }
            </xsl:if>
            return <xsl:value-of select="$DocumentName"/>ObjestItem;
        }
    }

    public class <xsl:value-of select="$DocumentName"/>_Select : DocumentSelect
    {		
        public <xsl:value-of select="$DocumentName"/>_Select() : base(Config.Kernel!, "<xsl:value-of select="Table"/>") { }
        
        public bool Select() { return base.BaseSelect(); }
        
        public bool SelectSingle() { if (base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new <xsl:value-of select="$DocumentName"/>_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public <xsl:value-of select="$DocumentName"/>_Pointer? Current { get; private set; }
    }

      <xsl:for-each select="TabularParts/TablePart"> <!-- TableParts -->
        <xsl:variable name="TablePartName" select="Name"/>
        <xsl:variable name="TablePartFullName" select="concat($DocumentName, '_', $TablePartName)"/>
    
    public class <xsl:value-of select="$TablePartFullName"/>_TablePart : DocumentTablePart
    {
        public <xsl:value-of select="$TablePartFullName"/>_TablePart(<xsl:value-of select="$DocumentName"/>_Objest owner) : base(Config.Kernel!, "<xsl:value-of select="Table"/>",
             <xsl:text>new string[] { </xsl:text>
             <xsl:for-each select="Fields/Field">
               <xsl:if test="position() != 1">
                 <xsl:text>, </xsl:text>
               </xsl:if>
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"</xsl:text>
             </xsl:for-each> }) 
        {
            if (owner == null) throw new Exception("owner null");
            
            Owner = owner;
            Records = new List&lt;Record&gt;();
        }
        <xsl:for-each select="Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>

        public <xsl:value-of select="$DocumentName"/>_Objest Owner { get; private set; }
        
        public List&lt;Record&gt; Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead(Owner.UnigueID);

            foreach (Dictionary&lt;string, object&gt; fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                <xsl:for-each select="Fields/Field">
                  <xsl:text>record.</xsl:text>
                  <xsl:value-of select="Name"/>
                  <xsl:text> = </xsl:text>
                  <xsl:call-template name="ReadFieldValue">
                    <xsl:with-param name="BaseFieldContainer">fieldValue</xsl:with-param>
                  </xsl:call-template>;
                </xsl:for-each>
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary&lt;string, object&gt; fieldValue = new Dictionary&lt;string, object&gt;();

                <xsl:for-each select="Fields/Field">
                    <xsl:text>fieldValue.Add("</xsl:text>
                    <xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                    <xsl:if test="Type = 'enum'">
                      <xsl:text>(int)</xsl:text>
                    </xsl:if>
                    <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>
                    <xsl:choose>
                        <xsl:when test="Type = 'pointer'">
                            <xsl:text>.UnigueID.UGuid</xsl:text>
                        </xsl:when>				
                    </xsl:choose>
                    <xsl:text>)</xsl:text>;
                </xsl:for-each>
                base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            base.BaseCommitTransaction();
        }
        <!-- /* видалити */
        public string GeneratingTextForFullTextSearch() 
        {
            string fullText = "";
            <xsl:if test="count(Fields/Field[IsFullTextSearch = '1']) != 0">
            foreach (Record record in Records)
            {
                fullText += string.Join(" ", new string[] { 
                    <xsl:for-each select="Fields/Field[IsFullTextSearch = '1']">
                      <xsl:if test="position() != 1">, 
                      </xsl:if>
                      <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>
                      <xsl:choose>
                          <xsl:when test="Type = 'pointer'">
                              <xsl:text>.GetPresentation()</xsl:text>
                          </xsl:when>
                          <xsl:otherwise>
                              <xsl:text>.ToString()</xsl:text>
                          </xsl:otherwise>
                      </xsl:choose>
                    </xsl:for-each> }) + "\n";
            }
            </xsl:if>
            return fullText;
        }
        -->
        public void Delete()
        {
            base.BaseDelete(Owner.UnigueID);
        }

        public List&lt;Record&gt; Copy()
        {
            List&lt;Record&gt; copyRecords = new List&lt;Record&gt;();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
        }

        public class Record : DocumentTablePartRecord
        {
            <xsl:for-each select="Fields/Field">
              <xsl:text>public </xsl:text>
              <xsl:call-template name="FieldType" />
              <xsl:text> </xsl:text>
              <xsl:value-of select="Name"/>
              <xsl:text> { get; set; } = </xsl:text>
              <xsl:call-template name="DefaultFieldValue" />;
            </xsl:for-each>
        }
    }
      </xsl:for-each> <!-- TableParts -->
    
    #endregion
    </xsl:for-each>
}

namespace <xsl:value-of select="Configuration/NameSpace"/>.Журнали
{
    #region Journal
    public class Journal_Select: JournalSelect
    {
        public Journal_Select() : base(Config.Kernel!,
             <xsl:text>new string[] { </xsl:text>
             <xsl:for-each select="Configuration/Documents/Document">
               <xsl:if test="position() != 1">
                 <xsl:text>, </xsl:text>
               </xsl:if>
               <xsl:text>"</xsl:text><xsl:value-of select="Table"/><xsl:text>"</xsl:text>
             </xsl:for-each>},
			       <xsl:text>new string[] { </xsl:text>
             <xsl:for-each select="Configuration/Documents/Document">
               <xsl:if test="position() != 1">
                 <xsl:text>, </xsl:text>
               </xsl:if>
               <xsl:text>"</xsl:text><xsl:value-of select="Name"/><xsl:text>"</xsl:text>
             </xsl:for-each>}) { }

        public DocumentObject? GetDocumentObject(bool readAllTablePart = true)
        {
            if (Current == null)
                return null;

            switch (Current.TypeDocument)
            {
                <xsl:for-each select="Configuration/Documents/Document">
                    <xsl:text>case </xsl:text>"<xsl:value-of select="Name"/>": return new Документи.<xsl:value-of select="Name"/>_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
                </xsl:for-each>
            }
			
			      return null;
        }
    }
    #endregion
<!--
    public class Journal_Document : JournalObject
    {
        public Journal_Document(string documentType, UnigueID uid) : base(Config.Kernel!)
        {
            switch (documentType)
            {
			    <xsl:for-each select="Configuration/Documents/Document">
					<xsl:variable name="DocumentName" select="Name"/>
					<xsl:text>case </xsl:text>"<xsl:value-of select="$DocumentName"/>": { base.Table = "<xsl:value-of select="Table"/>"; base.TypeDocument = "<xsl:value-of select="$DocumentName"/>"; break; }
				</xsl:for-each>
            }
            base.BaseRead(uid);
        }
    }
-->
}

namespace <xsl:value-of select="Configuration/NameSpace"/>.РегістриВідомостей
{
    <xsl:for-each select="Configuration/RegistersInformation/RegisterInformation">
       <xsl:variable name="RegisterName" select="Name"/>
    #region REGISTER "<xsl:value-of select="$RegisterName"/>"
    public static class <xsl:value-of select="$RegisterName"/>_Const
    {
        public const string TABLE = "<xsl:value-of select="Table"/>";
        <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>
    }

    public class <xsl:value-of select="$RegisterName"/>_RecordsSet : RegisterInformationRecordsSet
    {
        public <xsl:value-of select="$RegisterName"/>_RecordsSet() : base(Config.Kernel!, "<xsl:value-of select="Table"/>",
             <xsl:text>new string[] { </xsl:text>
             <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
               <xsl:if test="position() != 1">
                 <xsl:text>, </xsl:text>
               </xsl:if>
               <!--<xsl:value-of select="name(../..)"/>-->
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"</xsl:text>
             </xsl:for-each> }) 
        {
            Records = new List&lt;Record&gt;();
        }
		
        public List&lt;Record&gt; Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            base.BaseRead();
            foreach (Dictionary&lt;string, object&gt; fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                
                record.UID = (Guid)fieldValue["uid"];
                record.Period = DateTime.Parse(fieldValue["period"]?.ToString() ?? DateTime.MinValue.ToString());
                record.Owner = (Guid)fieldValue["owner"];
                <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                  <xsl:text>record.</xsl:text>
                  <xsl:value-of select="Name"/>
                  <xsl:text> = </xsl:text>
                  <xsl:call-template name="ReadFieldValue">
                    <xsl:with-param name="BaseFieldContainer">fieldValue</xsl:with-param>
                  </xsl:call-template>;
                </xsl:for-each>
                Records.Add(record);
            }
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner)
        {
            base.BaseBeginTransaction();
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary&lt;string, object&gt; fieldValue = new Dictionary&lt;string, object&gt;();
                <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                    <xsl:text>fieldValue.Add("</xsl:text>
                    <xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                    <xsl:if test="Type = 'enum'">
                        <xsl:text>(int)</xsl:text>      
                    </xsl:if>
					          <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>
                    <xsl:if test="Type = 'pointer'">
                    <xsl:text>.UnigueID.UGuid</xsl:text>
                    </xsl:if>
                    <xsl:text>)</xsl:text>;
                </xsl:for-each>
                base.BaseSave(record.UID, period, owner, fieldValue);
            }
            base.BaseCommitTransaction();
        }
        
        public void Delete(Guid owner)
        {
            base.BaseDelete(owner);
        }

        public class Record : RegisterInformationRecord
        {
            <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
              <xsl:text>public </xsl:text>
              <xsl:call-template name="FieldType" />
              <xsl:text> </xsl:text>
              <xsl:value-of select="Name"/>
              <xsl:text> { get; set; } = </xsl:text>
              <xsl:call-template name="DefaultFieldValue" />;
            </xsl:for-each>
        }
    }

    public class <xsl:value-of select="$RegisterName"/>_Objest : RegisterInformationObject
    {
		    public <xsl:value-of select="$RegisterName"/>_Objest() : base(Config.Kernel!, "<xsl:value-of select="Table"/>",
             <xsl:text>new string[] { </xsl:text>
             <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
               <xsl:if test="position() != 1">
                 <xsl:text>, </xsl:text>
               </xsl:if>
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"</xsl:text>
             </xsl:for-each> }) 
        {
            <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
              <xsl:value-of select="Name"/>
              <xsl:text> = </xsl:text>
              <xsl:call-template name="DefaultFieldValue" />;
            </xsl:for-each>
        }
        
        public bool Read(UnigueID uid)
        {
            if (BaseRead(uid))
            {
                <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                  <xsl:value-of select="Name"/>
                  <xsl:text> = </xsl:text>
                  <xsl:call-template name="ReadFieldValue">
                    <xsl:with-param name="BaseFieldContainer">base.FieldValue</xsl:with-param>
                  </xsl:call-template>;
                </xsl:for-each>
                BaseClear();
                return true;
            }
            else
                return false;
        }
        
        public void Save()
        {
            <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
              <xsl:text>base.FieldValue["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] = </xsl:text>
              <xsl:if test="Type = 'enum'">
                  <xsl:text>(int)</xsl:text>      
              </xsl:if>
              <xsl:value-of select="Name"/>
              <xsl:choose>
                <xsl:when test="Type = 'pointer'">
                  <xsl:text>.UnigueID.UGuid</xsl:text>
                </xsl:when>
              </xsl:choose>;
            </xsl:for-each>
            BaseSave();
        }

        public <xsl:value-of select="$RegisterName"/>_Objest Copy()
        {
            <xsl:value-of select="$RegisterName"/>_Objest copy = new <xsl:value-of select="$RegisterName"/>_Objest();
            <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
              <xsl:text>copy.</xsl:text><xsl:value-of select="Name"/><xsl:text> = </xsl:text><xsl:value-of select="Name"/>;
            </xsl:for-each>
            copy.New();
            return copy;
        }

        public void Delete()
        {
			      base.BaseDelete();
        }

        <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
          <xsl:text>public </xsl:text>
          <xsl:call-template name="FieldType" />
          <xsl:text> </xsl:text>
          <xsl:value-of select="Name"/>
          <xsl:text> { get; set; </xsl:text>}
        </xsl:for-each>
    }
	
    #endregion
  </xsl:for-each>
}

namespace <xsl:value-of select="Configuration/NameSpace"/>.РегістриНакопичення
{
    public static class VirtualTablesСalculation
    {
        /* Функція повного очищення віртуальних таблиць */
        public static void ClearAll()
        {
            /*  */
        }

        /* Функція для обчислення віртуальних таблиць  */
        public static void Execute(DateTime period, string regAccumName)
        {
            <xsl:variable name="QueryAllCountCalculation" select="count(Configuration/RegistersAccumulation/RegisterAccumulation/QueryBlockList/QueryBlock[FinalCalculation = '0']/Query)"/>
            <xsl:if test="$QueryAllCountCalculation != 0">
            Dictionary&lt;string, object&gt; paramQuery = new Dictionary&lt;string, object&gt;();
            paramQuery.Add("ПеріодДеньВідбір", period);

            switch(regAccumName)
            {
            <xsl:for-each select="Configuration/RegistersAccumulation/RegisterAccumulation">
                <xsl:variable name="QueryCount" select="count(QueryBlockList/QueryBlock[FinalCalculation = '0']/Query)"/>
                <xsl:if test="$QueryCount != 0">
                case "<xsl:value-of select="Name"/>":
                {
                    byte transactionID = Config.Kernel!.DataBase.BeginTransaction();
                    <xsl:for-each select="QueryBlockList/QueryBlock[FinalCalculation = '0']">
                    /* QueryBlock: <xsl:value-of select="Name"/> */
                        <xsl:for-each select="Query">
                            <xsl:sort select="@position" data-type="number" order="ascending" />
                    Config.Kernel!.DataBase.ExecuteSQL($@"<xsl:value-of select="normalize-space(.)"/>", paramQuery, transactionID);
                        </xsl:for-each>
                    </xsl:for-each>
                    Config.Kernel!.DataBase.CommitTransaction(transactionID);
                    break;
                }
                </xsl:if>
            </xsl:for-each>
                    default:
                        break;
            }
            </xsl:if>
        }

        /* Функція для обчислення підсумкових віртуальних таблиць */
        public static void ExecuteFinalCalculation(List&lt;string&gt; regAccumNameList)
        {
            <xsl:variable name="QueryAllCountFinalCalculation" select="count(Configuration/RegistersAccumulation/RegisterAccumulation/QueryBlockList/QueryBlock[FinalCalculation = '1']/Query)"/>
            <xsl:if test="$QueryAllCountFinalCalculation != 0">
            foreach (string regAccumName in regAccumNameList)
                switch(regAccumName)
                {
                <xsl:for-each select="Configuration/RegistersAccumulation/RegisterAccumulation">
                    <xsl:variable name="QueryCount" select="count(QueryBlockList/QueryBlock[FinalCalculation = '1']/Query)"/>
                    <xsl:if test="$QueryCount != 0">
                    case "<xsl:value-of select="Name"/>":
                    {
                        byte transactionID = Config.Kernel!.DataBase.BeginTransaction();
                        <xsl:for-each select="QueryBlockList/QueryBlock[FinalCalculation = '1']">
                        /* QueryBlock: <xsl:value-of select="Name"/> */
                            <xsl:for-each select="Query">
                                <xsl:sort select="@position" data-type="number" order="ascending" />
                        Config.Kernel!.DataBase.ExecuteSQL($@"<xsl:value-of select="normalize-space(.)"/>", null, transactionID);
                            </xsl:for-each>
                        </xsl:for-each>
                        Config.Kernel!.DataBase.CommitTransaction(transactionID);
                        break;
                    }
                    </xsl:if>
                </xsl:for-each>
                        default:
                            break;
                }
            </xsl:if>
        }
    }

    <xsl:for-each select="Configuration/RegistersAccumulation/RegisterAccumulation">
	    <xsl:variable name="Documents" select="../../Documents"/>
      <xsl:variable name="RegisterName" select="Name"/>
    #region REGISTER "<xsl:value-of select="$RegisterName"/>"
    public static class <xsl:value-of select="$RegisterName"/>_Const
    {
        public const string TABLE = "<xsl:value-of select="Table"/>";
		public static readonly string[] AllowDocumentSpendTable = new string[] { <xsl:for-each select="AllowDocumentSpend/Name">
		    <xsl:if test="position() != 1">
                <xsl:text>, </xsl:text>
            </xsl:if>
			<xsl:variable name="AllowDocumentSpendName" select="."/>
            <xsl:text>"</xsl:text><xsl:value-of select="$Documents/Document[Name = $AllowDocumentSpendName]/Table"/><xsl:text>"</xsl:text>
		</xsl:for-each> };
		public static readonly string[] AllowDocumentSpendType = new string[] { <xsl:for-each select="AllowDocumentSpend/Name">
		    <xsl:if test="position() != 1">
                <xsl:text>, </xsl:text>
            </xsl:if>
            <xsl:text>"</xsl:text><xsl:value-of select="."/><xsl:text>"</xsl:text>
		</xsl:for-each> };
        <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>
    }
	
    public class <xsl:value-of select="$RegisterName"/>_RecordsSet : RegisterAccumulationRecordsSet
    {
        public <xsl:value-of select="$RegisterName"/>_RecordsSet() : base(Config.Kernel!, "<xsl:value-of select="Table"/>", "<xsl:value-of select="$RegisterName"/>",
             <xsl:text>new string[] { </xsl:text>
             <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
               <xsl:if test="position() != 1">
                 <xsl:text>, </xsl:text>
               </xsl:if>
               <!--<xsl:value-of select="name(../..)"/>-->
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"</xsl:text>
             </xsl:for-each> }) 
        {
            Records = new List&lt;Record&gt;();
        }
		
        public List&lt;Record&gt; Records { get; set; }
        
        public void Read()
        {
            Records.Clear();
            
            base.BaseRead();
            
            foreach (Dictionary&lt;string, object&gt; fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                record.Period = DateTime.Parse(fieldValue["period"]?.ToString() ?? DateTime.MinValue.ToString());
                record.Income = (bool)fieldValue["income"];
                record.Owner = (Guid)fieldValue["owner"];
                <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                  <xsl:text>record.</xsl:text>
                  <xsl:value-of select="Name"/>
                  <xsl:text> = </xsl:text>
                  <xsl:call-template name="ReadFieldValue">
                    <xsl:with-param name="BaseFieldContainer">fieldValue</xsl:with-param>
                  </xsl:call-template>;
                </xsl:for-each>
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public void Save(DateTime period, Guid owner) 
        {
            base.BaseBeginTransaction();
            base.BaseSelectPeriodForOwner(owner, period);
            base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary&lt;string, object&gt; fieldValue = new Dictionary&lt;string, object&gt;();
                <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                    <xsl:text>fieldValue.Add("</xsl:text>
                    <xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                    <xsl:if test="Type = 'enum'">
                        <xsl:text>(int)</xsl:text>      
                    </xsl:if>
					          <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>
                    <xsl:if test="Type = 'pointer'">
                    <xsl:text>.UnigueID.UGuid</xsl:text>
                    </xsl:if>
                    <xsl:text>)</xsl:text>;
                </xsl:for-each>
                base.BaseSave(record.UID, period, record.Income, owner, fieldValue);
            }
            base.BaseTrigerAdd(period, owner);
            base.BaseCommitTransaction();
        }

        public void Delete(Guid owner)
        {
            base.BaseSelectPeriodForOwner(owner);
            base.BaseDelete(owner);
        }
        
        public class Record : RegisterAccumulationRecord
        {
            <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
              <xsl:text>public </xsl:text>
              <xsl:call-template name="FieldType" />
               <xsl:text> </xsl:text>
              <xsl:value-of select="Name"/>
              <xsl:text> { get; set; } = </xsl:text>
              <xsl:call-template name="DefaultFieldValue" />;
            </xsl:for-each>
        }
    }
    
    <xsl:for-each select="TabularParts/TablePart">
        <!-- TableParts -->
        <xsl:variable name="TablePartName" select="Name"/>
        <xsl:variable name="TablePartFullName" select="concat($RegisterName, '_', $TablePartName)"/>
    
    public class <xsl:value-of select="$TablePartFullName"/>_TablePart : RegisterAccumulationTablePart
    {
        public <xsl:value-of select="$TablePartFullName"/>_TablePart() : base(Config.Kernel!, "<xsl:value-of select="Table"/>",
              <xsl:text>new string[] { </xsl:text>
              <xsl:for-each select="Fields/Field">
                <xsl:if test="position() != 1">
                  <xsl:text>, </xsl:text>
                </xsl:if>
                <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"</xsl:text>
              </xsl:for-each> }) 
        {
            Records = new List&lt;Record&gt;();
        }
        
        public const string TABLE = "<xsl:value-of select="Table"/>";
        <xsl:for-each select="Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>
        public List&lt;Record&gt; Records { get; set; }
    
        public void Read()
        {
            Records.Clear();
            base.BaseRead();

            foreach (Dictionary&lt;string, object&gt; fieldValue in base.FieldValueList) 
            {
                Record record = new Record();
                record.UID = (Guid)fieldValue["uid"];
                
                <xsl:for-each select="Fields/Field">
                  <xsl:text>record.</xsl:text>
                  <xsl:value-of select="Name"/>
                  <xsl:text> = </xsl:text>
                  <xsl:call-template name="ReadFieldValue">
                    <xsl:with-param name="BaseFieldContainer">fieldValue</xsl:with-param>
                  </xsl:call-template>;
                </xsl:for-each>
                Records.Add(record);
            }
        
            base.BaseClear();
        }
    
        public void Save(bool clear_all_before_save /*= true*/) 
        {
            base.BaseBeginTransaction();
            
            if (clear_all_before_save)
                base.BaseDelete();

            foreach (Record record in Records)
            {
                Dictionary&lt;string, object&gt; fieldValue = new Dictionary&lt;string, object&gt;();

                <xsl:for-each select="Fields/Field">
                    <xsl:text>fieldValue.Add("</xsl:text>
                    <xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                    <xsl:if test="Type = 'enum'">
                        <xsl:text>(int)</xsl:text>
                      </xsl:if>
                    <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>
                    <xsl:choose>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:text>.UnigueID.UGuid</xsl:text>
                    </xsl:when>
                    </xsl:choose>
                    <xsl:text>)</xsl:text>;
                </xsl:for-each>
                base.BaseSave(record.UID, fieldValue);
            }
            
            base.BaseCommitTransaction();
        }
    
        public void Delete()
        {
            base.BaseDelete();
        }
        
        public class Record : RegisterAccumulationTablePartRecord
        {
            <xsl:for-each select="Fields/Field">
              <xsl:text>public </xsl:text>
              <xsl:call-template name="FieldType" />
              <xsl:text> </xsl:text>
              <xsl:value-of select="Name"/>
              <xsl:text> { get; set; } = </xsl:text>
              <xsl:call-template name="DefaultFieldValue" />;
            </xsl:for-each>
        }            
    }
    </xsl:for-each> <!-- TableParts -->
    #endregion
  </xsl:for-each>
}
  </xsl:template>
</xsl:stylesheet>