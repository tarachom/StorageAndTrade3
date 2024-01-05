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
        <xsl:text>[]</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'integer'">
        <xsl:text>0</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'integer[]'">
        <xsl:text>[]</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'numeric'">
        <xsl:text>0</xsl:text>
      </xsl:when>
      <xsl:when test="Type = 'numeric[]'">
        <xsl:text>[]</xsl:text>
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
        <xsl:text>[]</xsl:text>
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
  
  <!-- Для присвоєння значення полям. Для масиву полів-->
  <xsl:template name="ReadFieldValue">
     <xsl:param name="BaseFieldContainer" />
     
     <xsl:choose>
        <xsl:when test="Type = 'string'">
          <xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"].ToString() ?? ""</xsl:text>
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
          <xsl:text>(bool)</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
          <xsl:text> : false</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'time'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>TimeSpan.Parse(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]?.ToString() ?? DateTime.MinValue.TimeOfDay.ToString())</xsl:text>
          <xsl:text> : DateTime.MinValue.TimeOfDay</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'date' or Type = 'datetime'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>DateTime.Parse(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"].ToString() ?? DateTime.MinValue.ToString())</xsl:text>
          <xsl:text> : DateTime.MinValue</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'pointer'">
          <xsl:text>new </xsl:text><xsl:value-of select="Pointer"/>
          <xsl:text>_Pointer(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"])</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'any_pointer'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"] != DBNull.Value) ? </xsl:text>
          <xsl:text>(Guid)</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>["</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>"]</xsl:text>
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

  <!-- Для присвоєння значення полям 2. Для одного значення без використання масиву полів-->
  <xsl:template name="ReadFieldValue2">
     <xsl:param name="BaseFieldContainer" />
     
     <xsl:choose>
        <xsl:when test="Type = 'string'">
          <xsl:value-of select="$BaseFieldContainer"/><xsl:text>.ToString() ?? ""</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'string[]'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(string[])</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text></xsl:text>
          <xsl:text> : new string[] { }</xsl:text>
        </xsl:when>
       <xsl:when test="Type = 'integer'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(int)</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text></xsl:text>
          <xsl:text> : 0</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'integer[]'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(int[])</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text></xsl:text>
          <xsl:text> : new int[] { }</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'numeric'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(decimal)</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text></xsl:text>
          <xsl:text> : 0</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'numeric[]'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(decimal[])</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text></xsl:text>
          <xsl:text> : new decimal[] { }</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'boolean'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(bool)</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text></xsl:text>
          <xsl:text> : false</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'time'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>TimeSpan.Parse(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>?.ToString() ?? DateTime.MinValue.TimeOfDay.ToString())</xsl:text>
          <xsl:text> : DateTime.MinValue.TimeOfDay</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'date' or Type = 'datetime'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>DateTime.Parse(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>.ToString() ?? DateTime.MinValue.ToString())</xsl:text>
          <xsl:text> : DateTime.MinValue</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'pointer'">
          <xsl:text>new </xsl:text><xsl:value-of select="Pointer"/>
          <xsl:text>_Pointer(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text>)</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'any_pointer'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(Guid)</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text></xsl:text>
          <xsl:text> : Guid.Empty</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'composite_pointer'">
		    <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(UuidAndText)</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text></xsl:text>
          <xsl:text> : new UuidAndText()</xsl:text>
        </xsl:when>
        <xsl:when test="Type = 'enum'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(</xsl:text><xsl:value-of select="Pointer"/><xsl:text>)</xsl:text>
          <xsl:value-of select="$BaseFieldContainer"/><xsl:text></xsl:text>
          <xsl:text> : 0</xsl:text>
        </xsl:when>
		    <xsl:when test="Type = 'bytea'">
          <xsl:text>(</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text> != DBNull.Value) ? </xsl:text>
          <xsl:text>(byte[])</xsl:text><xsl:value-of select="$BaseFieldContainer"/><xsl:text></xsl:text>
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

namespace <xsl:value-of select="Configuration/NameSpaceGenerationCode"/>
{
    public static class Config
    {
        public static Kernel Kernel { get; set; } = new Kernel();
        public static async void StartBackgroundTask()
        {
            /*
            Схема роботи:

            1. В процесі запису в регістр залишків - додається запис у таблицю тригерів.
              Запис в таблицю тригерів містить дату запису в регістр, назву регістру.

            2. Раз на 5 сек викликається процедура SpetialTableRegAccumTrigerExecute і
              відбувається розрахунок віртуальних таблиць регістрів залишків.

              Розраховуються тільки змінені регістри на дату проведення документу і
              додатково на дату якщо змінена дата документу і документ уже був проведений.

              Додатково розраховуються підсумки в кінці всіх розрахунків.
            */

            if (Kernel.Session == Guid.Empty)
                throw new Exception("Порожня сесія користувача. Спочатку потрібно залогінитись, а тоді вже викликати функцію StartBackgroundTask()");

            while (true)
            {                
                //Зупинка розрахунків використовується при масовому перепроведенні документів щоб
                //провести всі документ, а тоді вже розраховувати регістри
                if (!Константи.Системні.ЗупинитиФоновіЗадачі_Const)
                {
                    //Виконання обчислень
                    await Kernel.DataBase.SpetialTableRegAccumTrigerExecute
                    (
                        Kernel.Session,
                        РегістриНакопичення.VirtualTablesСalculation.Execute, 
                        РегістриНакопичення.VirtualTablesСalculation.ExecuteFinalCalculation
                    );
                }

                //Затримка на 5 сек
                await Task.Delay(5000);
            }
        }
    }

    public class Functions
    {
        public record CompositePointerPresentation_Record
        {
            public string result = "";
            public string pointer = "";
            public string type = "";
        }
        /*
          Функція для типу який задається користувачем.
          Повертає презентацію для uuidAndText.
          В @pointer - повертає групу (Документи або Довідники)
            @type - повертає назву типу
        */
        public static async ValueTask&lt;CompositePointerPresentation_Record&gt; CompositePointerPresentation(UuidAndText uuidAndText)
        {
            CompositePointerPresentation_Record record = new();

            if (uuidAndText.IsEmpty() || string.IsNullOrEmpty(uuidAndText.Text) || uuidAndText.Text.IndexOf(".") == -1)
                return record;

            string[] pointer_and_type = uuidAndText.Text.Split(".", StringSplitOptions.None);

            if (pointer_and_type.Length == 2)
            {
                record.pointer = pointer_and_type[0];
                record.type = pointer_and_type[1];

                if (record.pointer == "Документи")
                {
                    <xsl:variable name="DocCount" select="count(Configuration/Documents/Document)"/>
                    <xsl:if test="$DocCount != 0">
                    switch (record.type)
                    {
                        <xsl:for-each select="Configuration/Documents/Document">
                            <xsl:variable name="DocumentName" select="Name"/>
                        case "<xsl:value-of select="$DocumentName"/>": record.result = await new Документи.<xsl:value-of select="$DocumentName"/>_Pointer(uuidAndText.Uuid).GetPresentation(); return record;
                        </xsl:for-each>
                    }
                    </xsl:if>
                    <xsl:if test="$DocCount = 0">
                    return record;
                    </xsl:if>
                }
                else if (record.pointer == "Довідники")
                {
                    <xsl:variable name="DirCount" select="count(Configuration/Directories/Directory)"/>
                    <xsl:if test="$DirCount != 0">
                    switch (record.type)
                    {
                        <xsl:for-each select="Configuration/Directories/Directory">
                            <xsl:variable name="DirectoryName" select="Name"/>
                        case "<xsl:value-of select="$DirectoryName"/>": record.result = await new Довідники.<xsl:value-of select="$DirectoryName"/>_Pointer(uuidAndText.Uuid).GetPresentation(); return record;
                        </xsl:for-each>
                    }
                    </xsl:if>
                    <xsl:if test="$DirCount = 0">
                    return record;
                    </xsl:if>
                }
            }

            return record;
        }
    }
}

namespace <xsl:value-of select="Configuration/NameSpaceGenerationCode"/>.Константи
{
    <xsl:for-each select="Configuration/ConstantsBlocks/ConstantsBlock">
	  #region CONSTANTS BLOCK "<xsl:value-of select="Name"/>"
    public static class <xsl:value-of select="Name"/>
    {       
        <xsl:for-each select="Constants/Constant">
        <xsl:text>public static </xsl:text>
        <xsl:call-template name="FieldType" />
        <xsl:text> </xsl:text>
        <xsl:value-of select="Name"/>_Const
        {
            get 
            {
                var recordResult = Task.Run( async () =&gt; { return await Config.Kernel.DataBase.SelectConstants(SpecialTables.Constants, "<xsl:value-of select="NameInTable"/>"); } ).Result;
                <xsl:text>var result = recordResult.Result ? (</xsl:text>
                <xsl:call-template name="ReadFieldValue2">
                  <xsl:with-param name="BaseFieldContainer">recordResult.Value</xsl:with-param>
                </xsl:call-template>
                <xsl:text>) : </xsl:text>
                <xsl:call-template name="DefaultFieldValue" />;
                <xsl:text>return result</xsl:text>
                <xsl:if test="Type = 'pointer'">
                <xsl:variable name="groupPointer" select="substring-before(Pointer, '.')" />
                <xsl:choose>
                  <xsl:when test="$groupPointer = 'Довідники' or $groupPointer = 'Документи'">
                    <xsl:text>.Copy()</xsl:text>
                  </xsl:when>
                </xsl:choose>
                </xsl:if>;
            }
            set
            {
                Config.Kernel.DataBase.SaveConstants(SpecialTables.Constants, "<xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                <xsl:choose>
                  <xsl:when test="Type = 'enum'">
                    <xsl:text>(int)</xsl:text>
                  </xsl:when>
                </xsl:choose>
                <xsl:text>value</xsl:text>
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
            public <xsl:value-of select="$TablePartFullName"/>_TablePart() : base(Config.Kernel, "<xsl:value-of select="Table"/>",
                 <xsl:text>[</xsl:text>
                 <xsl:for-each select="Fields/Field">
                   <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                 </xsl:for-each>]) { }
            
            public const string TABLE = "<xsl:value-of select="Table"/>";
            <xsl:for-each select="Fields/Field">
            public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>
            public List&lt;Record&gt; Records { get; set; } = [];
        
            public async ValueTask Read()
            {
                Records.Clear();
                await base.BaseRead();

                foreach (Dictionary&lt;string, object&gt; fieldValue in base.FieldValueList) 
                {
                    Record record = new Record()
                    {
                        UID = (Guid)fieldValue["uid"],
                        <xsl:for-each select="Fields/Field">
                          <xsl:value-of select="Name"/>
                          <xsl:text> = </xsl:text>
                          <xsl:call-template name="ReadFieldValue">
                            <xsl:with-param name="BaseFieldContainer">fieldValue</xsl:with-param>
                          </xsl:call-template>,
                        </xsl:for-each>
                    };
                    Records.Add(record);
                }
            
                base.BaseClear();
            }
        
            public async ValueTask Save(bool clear_all_before_save /*= true*/) 
            {
                await base.BaseBeginTransaction();
                
                if (clear_all_before_save)
                    await base.BaseDelete();

                foreach (Record record in Records)
                {
                    Dictionary&lt;string, object&gt; fieldValue = new Dictionary&lt;string, object&gt;()
                    {
                        <xsl:for-each select="Fields/Field">
                            <xsl:text>{"</xsl:text>
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
                            <xsl:text>}</xsl:text>,
                        </xsl:for-each>
                    };
                    record.UID = await base.BaseSave(record.UID, fieldValue);
                }
                
                await base.BaseCommitTransaction();
            }
        
            public async ValueTask Delete()
            {
                await base.BaseDelete();
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

namespace <xsl:value-of select="Configuration/NameSpaceGenerationCode"/>.Довідники
{
    <xsl:for-each select="Configuration/Directories/Directory">
      <xsl:variable name="DirectoryName" select="Name"/>
    #region DIRECTORY "<xsl:value-of select="$DirectoryName"/>"
    public static class <xsl:value-of select="$DirectoryName"/>_Const
    {
        public const string TABLE = "<xsl:value-of select="Table"/>";
        public const string POINTER = "Довідники.<xsl:value-of select="$DirectoryName"/>"; /* Повна назва вказівника */
        public const string FULLNAME = "<xsl:value-of select="normalize-space(FullName)"/>"; /* Повна назва об'єкта */
        public const string DELETION_LABEL = "deletion_label"; /* Помітка на видалення true|false */
        <xsl:for-each select="Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>
    }

    public class <xsl:value-of select="$DirectoryName"/>_Objest : DirectoryObject
    {
        public <xsl:value-of select="$DirectoryName"/>_Objest() : base(Config.Kernel, "<xsl:value-of select="Table"/>",
             <xsl:text>[</xsl:text>
             <xsl:for-each select="Fields/Field">
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
             </xsl:for-each>]) 
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
        
        public async ValueTask New()
        {
            BaseNew();
            <xsl:choose>
              <xsl:when test="normalize-space(TriggerFunctions/New) != ''">
                await <xsl:value-of select="TriggerFunctions/New"/><xsl:text>(this)</xsl:text>;
              </xsl:when>
              <xsl:otherwise>
                await ValueTask.FromResult(true);
              </xsl:otherwise>
            </xsl:choose>
        }

        public async ValueTask&lt;bool&gt; Read(UnigueID uid)
        {
            if (await BaseRead(uid))
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

        /* синхронна функція для Read(UnigueID uid) */
        public bool ReadSync(UnigueID uid) { return Task.Run&lt;bool&gt;(async () =&gt; { return await Read(uid); }).Result; }
        
        public async ValueTask&lt;bool&gt; Save()
        {
            <xsl:if test="normalize-space(TriggerFunctions/BeforeSave) != ''">
                await <xsl:value-of select="TriggerFunctions/BeforeSave"/><xsl:text>(this)</xsl:text>;
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
            bool result = await BaseSave();
            if (result)
            {
                <xsl:if test="normalize-space(TriggerFunctions/AfterSave) != ''">
                    await <xsl:value-of select="TriggerFunctions/AfterSave"/><xsl:text>(this);</xsl:text>      
                </xsl:if>
                await BaseWriteFullTextSearch(GetBasis(), [<xsl:for-each select="Fields/Field[IsFullTextSearch = '1' and Type = 'string']"><xsl:value-of select="Name"/>, </xsl:for-each>]);
            }
            return result;
        }

        public async ValueTask&lt;<xsl:value-of select="$DirectoryName"/>_Objest&gt; Copy(bool copyTableParts = false)
        {
            <xsl:value-of select="$DirectoryName"/>_Objest copy = new <xsl:value-of select="$DirectoryName"/>_Objest()
            {
                <xsl:for-each select="Fields/Field">
                    <xsl:value-of select="Name"/><xsl:text> = </xsl:text><xsl:value-of select="Name"/>,
                </xsl:for-each>
            };
            <xsl:if test="count(TabularParts/TablePart) != 0">
            if (copyTableParts)
            {
            <xsl:for-each select="TabularParts/TablePart">
                <xsl:variable name="TablePartName" select="concat(Name, '_TablePart')"/>
                //<xsl:value-of select="Name"/> - Таблична частина
                await <xsl:value-of select="$TablePartName"/>.Read();
                copy.<xsl:value-of select="$TablePartName"/>.Records = <xsl:value-of select="$TablePartName"/>.Copy();
            </xsl:for-each>
            }
            </xsl:if>

            await copy.New();
            <xsl:if test="normalize-space(TriggerFunctions/Copying) != ''">
                await <xsl:value-of select="TriggerFunctions/Copying"/><xsl:text>(copy, this);</xsl:text>      
            </xsl:if>

            <xsl:choose>
                <xsl:when test="count(TabularParts/TablePart) = 0 and normalize-space(TriggerFunctions/Copying) = ''">
            return await ValueTask.FromResult&lt;<xsl:value-of select="$DirectoryName"/>_Objest&gt;(copy);
                </xsl:when>
                <xsl:otherwise>
            return copy;
                </xsl:otherwise>
            </xsl:choose>
        }

        public async ValueTask SetDeletionLabel(bool label = true)
        {
            <xsl:if test="normalize-space(TriggerFunctions/SetDeletionLabel) != ''">
                await <xsl:value-of select="TriggerFunctions/SetDeletionLabel"/><xsl:text>(this, label);</xsl:text>      
            </xsl:if>
            await base.BaseDeletionLabel(label);
        }

        public async ValueTask Delete()
        {
            <xsl:if test="normalize-space(TriggerFunctions/BeforeDelete) != ''">
                await <xsl:value-of select="TriggerFunctions/BeforeDelete"/><xsl:text>(this);</xsl:text>      
            </xsl:if>
            await base.BaseDelete(<xsl:text>new string[] { </xsl:text>
            <xsl:for-each select="TabularParts/TablePart">
               <xsl:if test="position() != 1">
                 <xsl:text>, </xsl:text>
               </xsl:if>
               <xsl:text>"</xsl:text><xsl:value-of select="Table"/><xsl:text>"</xsl:text>
            </xsl:for-each> });
        }

        /* синхронна функція для Delete() */
        public bool DeleteSync() { return Task.Run&lt;bool&gt;(async () =&gt; { await Delete(); return true; }).Result; } 
        
        public <xsl:value-of select="$DirectoryName"/>_Pointer GetDirectoryPointer()
        {
            return new <xsl:value-of select="$DirectoryName"/>_Pointer(UnigueID.UGuid);
        }

        public UuidAndText GetBasis()
        {
            return new UuidAndText(UnigueID.UGuid, <xsl:value-of select="$DirectoryName"/>_Const.POINTER);
        }

        public async ValueTask&lt;string&gt; GetPresentation()
        {
            return await base.BasePresentation(
                <xsl:text>[</xsl:text>
                <xsl:for-each select="Fields/Field[IsPresentation=1]">
                  <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                </xsl:for-each>]
            );
        }
        
        /* синхронна функція для GetPresentation() */
        public string GetPresentationSync() { return Task.Run&lt;string&gt;(async () =&gt; { return await GetPresentation(); }).Result; }
        
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
        public <xsl:value-of select="$DirectoryName"/>_Pointer(object? uid = null) : base(Config.Kernel, "<xsl:value-of select="Table"/>")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public <xsl:value-of select="$DirectoryName"/>_Pointer(UnigueID uid, Dictionary&lt;string, object&gt;? fields = null) : base(Config.Kernel, "<xsl:value-of select="Table"/>")
        {
            base.Init(uid, fields);
        }
        
        public async ValueTask&lt;<xsl:value-of select="$DirectoryName"/>_Objest?&gt; GetDirectoryObject()
        {
            if (this.IsEmpty()) return null;
            <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>ObjestItem = new <xsl:value-of select="$DirectoryName"/>_Objest();
            return await <xsl:value-of select="$DirectoryName"/>ObjestItem.Read(base.UnigueID) ? <xsl:value-of select="$DirectoryName"/>ObjestItem : null;
        }

        public <xsl:value-of select="$DirectoryName"/>_Pointer Copy()
        {
            return new <xsl:value-of select="$DirectoryName"/>_Pointer(base.UnigueID, base.Fields) { Назва = Назва };
        }

        public string Назва { get; set; } = "";

        public async ValueTask&lt;string&gt; GetPresentation()
        {
            return Назва = await base.BasePresentation(
                <xsl:text>[</xsl:text>
                <xsl:for-each select="Fields/Field[IsPresentation=1]">
                  <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                </xsl:for-each>]
            );
        }

        /* синхронна функція для GetPresentation() */
        public string GetPresentationSync() { return Task.Run&lt;string&gt;(async () =&gt; { return await GetPresentation(); }).Result; }

        public async ValueTask SetDeletionLabel(bool label = true)
        {
            <xsl:value-of select="$DirectoryName"/>_Objest? obj = await GetDirectoryObject();
            if (obj != null)
            {
                <xsl:if test="normalize-space(TriggerFunctions/SetDeletionLabel) != ''">
                    await <xsl:value-of select="TriggerFunctions/SetDeletionLabel"/><xsl:text>(obj, label)</xsl:text>;
                </xsl:if>
                await base.BaseDeletionLabel(label);
            }
        }
		
        public <xsl:value-of select="$DirectoryName"/>_Pointer GetEmptyPointer()
        {
            return new <xsl:value-of select="$DirectoryName"/>_Pointer();
        }

        public UuidAndText GetBasis()
        {
            return new UuidAndText(UnigueID.UGuid, <xsl:value-of select="$DirectoryName"/>_Const.POINTER);
        }

        public void Clear()
        {
            Init(new UnigueID(), null);
            Назва = "";
        }
    }
    
    public class <xsl:value-of select="$DirectoryName"/>_Select : DirectorySelect
    {
        public <xsl:value-of select="$DirectoryName"/>_Select() : base(Config.Kernel, "<xsl:value-of select="Table"/>") { }        
        public async ValueTask&lt;bool&gt; Select() { return await base.BaseSelect(); }
        
        public async ValueTask&lt;bool&gt; SelectSingle() { if (await base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new <xsl:value-of select="$DirectoryName"/>_Pointer(base.DirectoryPointerPosition.UnigueID, base.DirectoryPointerPosition.Fields); return true; } else { Current = null; return false; } }

        public <xsl:value-of select="$DirectoryName"/>_Pointer? Current { get; private set; }
        
        public async ValueTask&lt;<xsl:value-of select="$DirectoryName"/>_Pointer&gt; FindByField(string name, object value)
        {
            <xsl:value-of select="$DirectoryName"/>_Pointer itemPointer = new <xsl:value-of select="$DirectoryName"/>_Pointer();
            DirectoryPointer directoryPointer = await base.BaseFindByField(name, value);
            if (!directoryPointer.IsEmpty()) itemPointer.Init(directoryPointer.UnigueID);
            return itemPointer;
        }
        
        public async ValueTask&lt;List&lt;<xsl:value-of select="$DirectoryName"/>_Pointer&gt;&gt; FindListByField(string name, object value, int limit = 0, int offset = 0)
        {
            List&lt;<xsl:value-of select="$DirectoryName"/>_Pointer&gt; directoryPointerList = new List&lt;<xsl:value-of select="$DirectoryName"/>_Pointer&gt;();
            foreach (DirectoryPointer directoryPointer in await base.BaseFindListByField(name, value, limit, offset)) 
                directoryPointerList.Add(new <xsl:value-of select="$DirectoryName"/>_Pointer(directoryPointer.UnigueID));
            return directoryPointerList;
        }
    }
    
      <xsl:for-each select="TabularParts/TablePart"> <!-- TableParts -->
        <xsl:variable name="TablePartName" select="Name"/>
        <xsl:variable name="TablePartFullName" select="concat($DirectoryName, '_', $TablePartName)"/>
    
    public class <xsl:value-of select="$TablePartFullName"/>_TablePart : DirectoryTablePart
    {
        public <xsl:value-of select="$TablePartFullName"/>_TablePart(<xsl:value-of select="$DirectoryName"/>_Objest owner) : base(Config.Kernel, "<xsl:value-of select="Table"/>",
             <xsl:text>[</xsl:text>
             <xsl:for-each select="Fields/Field">
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
             </xsl:for-each>])
        {
            if (owner == null) throw new Exception("owner null");
            Owner = owner;
        }
        <xsl:for-each select="Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>

        public <xsl:value-of select="$DirectoryName"/>_Objest Owner { get; private set; }
        
        public List&lt;Record&gt; Records { get; set; } = [];
        
        public async ValueTask Read()
        {
            Records.Clear();
            await base.BaseRead(Owner.UnigueID);

            foreach (Dictionary&lt;string, object&gt; fieldValue in base.FieldValueList) 
            {
                Record record = new Record()
                {
                    UID = (Guid)fieldValue["uid"],
                    <xsl:for-each select="Fields/Field">
                      <xsl:value-of select="Name"/>
                      <xsl:text> = </xsl:text>
                      <xsl:call-template name="ReadFieldValue">
                        <xsl:with-param name="BaseFieldContainer">fieldValue</xsl:with-param>
                      </xsl:call-template>,
                    </xsl:for-each>
                };
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public async ValueTask Save(bool clear_all_before_save /*= true*/) 
        {
            await base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                await base.BaseDelete(Owner.UnigueID);
            
            foreach (Record record in Records)
            {
                Dictionary&lt;string, object&gt; fieldValue = new Dictionary&lt;string, object&gt;()
                {
                    <xsl:for-each select="Fields/Field">
                        <xsl:text>{"</xsl:text>
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
                        <xsl:text>}</xsl:text>,
                    </xsl:for-each>
                };
                record.UID = await base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
                
            await base.BaseCommitTransaction();
        }
        
        public async ValueTask Delete()
        {
            await base.BaseDelete(Owner.UnigueID);
        }

        public List&lt;Record&gt; Copy()
        {
            List&lt;Record&gt; copyRecords = new List&lt;Record&gt;();
            copyRecords = Records;

            foreach (Record copyRecordItem in copyRecords)
                copyRecordItem.UID = Guid.Empty;

            return copyRecords;
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

namespace <xsl:value-of select="Configuration/NameSpaceGenerationCode"/>.Перелічення
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

        public static <xsl:value-of select="$EnumName"/>? <xsl:value-of select="$EnumName"/>_FindByName(string name)
        {
            switch (name)
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
                case "<xsl:value-of select="$ReturnValue"/>": return <xsl:value-of select="$EnumName"/>.<xsl:value-of select="Name"/>;
                </xsl:for-each>
                default: return null;
            }
        }

        public static List&lt;NameValue&lt;<xsl:value-of select="$EnumName"/>&gt;&gt; <xsl:value-of select="$EnumName"/>_List()
        {
            List&lt;NameValue&lt;<xsl:value-of select="$EnumName"/>&gt;&gt; value = new List&lt;NameValue&lt;<xsl:value-of select="$EnumName"/>&gt;&gt;();
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
            value.Add(new NameValue&lt;<xsl:value-of select="$EnumName"/>&gt;("<xsl:value-of select="$ReturnValue"/>", <xsl:value-of select="$EnumName"/>.<xsl:value-of select="Name"/>));
            </xsl:for-each>
            return value;
        }
        #endregion
    </xsl:for-each>
    }
}

namespace <xsl:value-of select="Configuration/NameSpaceGenerationCode"/>.Документи
{
    <xsl:for-each select="Configuration/Documents/Document">
      <xsl:variable name="DocumentName" select="Name"/>
    #region DOCUMENT "<xsl:value-of select="$DocumentName"/>"
    public static class <xsl:value-of select="$DocumentName"/>_Const
    {
        public const string TABLE = "<xsl:value-of select="Table"/>";
        public const string POINTER = "Документи.<xsl:value-of select="$DocumentName"/>"; /* Повна назва вказівника */
        public const string FULLNAME = "<xsl:value-of select="normalize-space(FullName)"/>"; /* Повна назва об'єкта */
        public const string DELETION_LABEL = "deletion_label"; /* Помітка на видалення true|false */
        public const string SPEND = "spend"; /* Проведений true|false */
        public const string SPEND_DATE = "spend_date"; /* Дата проведення DateTime */
        
        <xsl:for-each select="Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>
    }

    public static class <xsl:value-of select="$DocumentName"/>_Export
    {
        public static async ValueTask ToXmlFile(<xsl:value-of select="$DocumentName"/>_Pointer <xsl:value-of select="$DocumentName"/>, string pathToSave)
        {
            <xsl:value-of select="$DocumentName"/>_Objest? obj = await <xsl:value-of select="$DocumentName"/>.GetDocumentObject(true);
            if (obj == null) return;

            XmlWriter xmlWriter = XmlWriter.Create(pathToSave, new XmlWriterSettings() { Indent = true, Encoding = System.Text.Encoding.UTF8 });
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("root");
            xmlWriter.WriteAttributeString("uid", obj.UnigueID.ToString());
            <xsl:for-each select="Fields/Field">
            xmlWriter.WriteStartElement("<xsl:value-of select="Name"/>");
            xmlWriter.WriteAttributeString("type", "<xsl:value-of select="Type"/>");
            <xsl:choose>
              <xsl:when test="Type = 'pointer'">
                <xsl:variable name="groupPointer" select="substring-before(Pointer, '.')" />
                <xsl:choose>
                  <xsl:when test="$groupPointer = 'Довідники' or $groupPointer = 'Документи'">
                    xmlWriter.WriteAttributeString("pointer", "<xsl:value-of select="Pointer"/>");
                    xmlWriter.WriteAttributeString("uid", obj.<xsl:value-of select="Name"/>.UnigueID.ToString());
                    xmlWriter.WriteString(await obj.<xsl:value-of select="Name"/>.GetPresentation());
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
              <xsl:when test="Type = 'enum'">
                xmlWriter.WriteAttributeString("pointer", "<xsl:value-of select="Pointer"/>");
                xmlWriter.WriteAttributeString("uid", ((int)obj.<xsl:value-of select="Name"/>).ToString());
                xmlWriter.WriteString(obj.<xsl:value-of select="Name"/>.ToString());
              </xsl:when>
              <xsl:when test="Type = 'composite_pointer'">
                xmlWriter.WriteRaw(((UuidAndText)obj.<xsl:value-of select="Name"/>).ToXml());
              </xsl:when>
              <xsl:otherwise>
                xmlWriter.WriteValue(obj.<xsl:value-of select="Name"/>);
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

                    foreach(<xsl:value-of select="$TablePartFullName"/>_TablePart.Record record in obj.<xsl:value-of select="$TablePartName"/>_TablePart.Records)
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
                                xmlWriter.WriteString(await record.<xsl:value-of select="Name"/>.GetPresentation());
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
        public <xsl:value-of select="$DocumentName"/>_Objest() : base(Config.Kernel, "<xsl:value-of select="Table"/>", "<xsl:value-of select="$DocumentName"/>",
             <xsl:text>[</xsl:text>
             <xsl:for-each select="Fields/Field">
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
             </xsl:for-each>])
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
        
        public async ValueTask New()
        {
            BaseNew();
            <xsl:choose>
              <xsl:when test="normalize-space(TriggerFunctions/New) != ''">
                await <xsl:value-of select="TriggerFunctions/New"/><xsl:text>(this)</xsl:text>;
              </xsl:when>
              <xsl:otherwise>
                await ValueTask.FromResult(true);
              </xsl:otherwise>
            </xsl:choose>
        }

        public async ValueTask&lt;bool&gt; Read(UnigueID uid, bool readAllTablePart = false)
        {
            if (await BaseRead(uid))
            {
                <xsl:for-each select="Fields/Field">
                  <xsl:value-of select="Name"/>
                  <xsl:text> = </xsl:text>
                  <xsl:call-template name="ReadFieldValue">
                    <xsl:with-param name="BaseFieldContainer">base.FieldValue</xsl:with-param>
                  </xsl:call-template>;
                </xsl:for-each>
                BaseClear();
                <xsl:if test="count(TabularParts/TablePart) != 0">
                if (readAllTablePart)
                {
                    <xsl:for-each select="TabularParts/TablePart">
                    await <xsl:value-of select="concat(Name, '_TablePart')"/>.Read();</xsl:for-each>
                }
                </xsl:if>
                return true;
            }
            else
                return false;
        }

        /* синхронна функція для Read(UnigueID uid) */
        public bool ReadSync(UnigueID uid, bool readAllTablePart = false) { return Task.Run&lt;bool&gt;(async () =&gt; { return await Read(uid, readAllTablePart); }).Result; }
        
        public async Task&lt;bool&gt; Save()
        {
            <xsl:if test="normalize-space(TriggerFunctions/BeforeSave) != ''">
                await <xsl:value-of select="TriggerFunctions/BeforeSave"/><xsl:text>(this)</xsl:text>;
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
            bool result = await BaseSave();
            
            if (result)
            {
                <xsl:if test="normalize-space(TriggerFunctions/AfterSave) != ''">
                    await <xsl:value-of select="TriggerFunctions/AfterSave"/><xsl:text>(this);</xsl:text>      
                </xsl:if>
                await BaseWriteFullTextSearch(GetBasis(), [<xsl:for-each select="Fields/Field[IsFullTextSearch = '1' and Type = 'string']"><xsl:value-of select="Name"/>, </xsl:for-each>]);
            }

            return result;
        }

        public async ValueTask&lt;bool&gt; SpendTheDocument(DateTime spendDate)
        {
            <xsl:choose>
                <xsl:when test="normalize-space(SpendFunctions/Spend) != ''">
                <xsl:text>bool rezult = await </xsl:text><xsl:value-of select="SpendFunctions/Spend"/><xsl:text>(this)</xsl:text>;
                <xsl:text>await BaseSpend(rezult, spendDate)</xsl:text>;
                <xsl:text>return rezult;</xsl:text>
                </xsl:when>
                <xsl:otherwise>
                <xsl:text>await BaseSpend(false, DateTime.MinValue)</xsl:text>;
                <xsl:text>return false;</xsl:text>
                </xsl:otherwise>
            </xsl:choose>
        }

        /* синхронна функція для SpendTheDocument() */
        public bool SpendTheDocumentSync(DateTime spendDate) { return Task.Run&lt;bool&gt;(async () =&gt; { return await SpendTheDocument(spendDate); }).Result; }

        public async ValueTask ClearSpendTheDocument()
        {
            <xsl:if test="normalize-space(SpendFunctions/ClearSpend) != ''">
                await <xsl:value-of select="SpendFunctions/ClearSpend"/>
              <xsl:text>(this)</xsl:text>;
            </xsl:if>
            <xsl:text>await BaseSpend(false, DateTime.MinValue);</xsl:text>
        }

        /* синхронна функція для ClearSpendTheDocument() */
        public bool ClearSpendTheDocumentSync() { return Task.Run&lt;bool&gt;(async () =&gt; { await ClearSpendTheDocument(); return true; }).Result; } 

        public async ValueTask&lt;<xsl:value-of select="$DocumentName"/>_Objest&gt; Copy(bool copyTableParts = false)
        {
            <xsl:value-of select="$DocumentName"/>_Objest copy = new <xsl:value-of select="$DocumentName"/>_Objest()
            {
                <xsl:for-each select="Fields/Field">
                  <xsl:value-of select="Name"/><xsl:text> = </xsl:text><xsl:value-of select="Name"/>,
                </xsl:for-each>
            };
            <xsl:if test="count(TabularParts/TablePart) != 0">
            if (copyTableParts)
            {
            <xsl:for-each select="TabularParts/TablePart">
                <xsl:variable name="TablePartName" select="concat(Name, '_TablePart')"/>
                //<xsl:value-of select="Name"/> - Таблична частина
                await <xsl:value-of select="$TablePartName"/>.Read();
                copy.<xsl:value-of select="$TablePartName"/>.Records = <xsl:value-of select="$TablePartName"/>.Copy();
            </xsl:for-each>
            }
            </xsl:if>

            await copy.New();
            <xsl:if test="normalize-space(TriggerFunctions/Copying) != ''">
                await <xsl:value-of select="TriggerFunctions/Copying"/><xsl:text>(copy, this);</xsl:text>      
            </xsl:if>
            
            <xsl:choose>
                <xsl:when test="count(TabularParts/TablePart) = 0 and normalize-space(TriggerFunctions/Copying) = ''">
            return await ValueTask.FromResult&lt;<xsl:value-of select="$DocumentName"/>_Objest&gt;(copy);
                </xsl:when>
                <xsl:otherwise>
            return copy;
                </xsl:otherwise>
            </xsl:choose>
        }

        public async ValueTask SetDeletionLabel(bool label = true)
        {
            <xsl:if test="normalize-space(TriggerFunctions/SetDeletionLabel) != ''">
                await <xsl:value-of select="TriggerFunctions/SetDeletionLabel"/><xsl:text>(this, label);</xsl:text>      
            </xsl:if>
            await ClearSpendTheDocument();
            await base.BaseDeletionLabel(label);
        }

        /* синхронна функція для SetDeletionLabel() */
        public bool SetDeletionLabelSync(bool label = true) { return Task.Run&lt;bool&gt;(async () =&gt; { await SetDeletionLabel(label); return true; }).Result; }

        public async ValueTask Delete()
        {
            <xsl:if test="normalize-space(TriggerFunctions/BeforeDelete) != ''">
                await <xsl:value-of select="TriggerFunctions/BeforeDelete"/><xsl:text>(this);</xsl:text>      
            </xsl:if>
            await ClearSpendTheDocument();
            await base.BaseDelete(<xsl:text>new string[] { </xsl:text>
            <xsl:for-each select="TabularParts/TablePart">
              <xsl:if test="position() != 1">
                <xsl:text>, </xsl:text>
              </xsl:if>
              <xsl:text>"</xsl:text><xsl:value-of select="Table"/><xsl:text>"</xsl:text>
            </xsl:for-each> });
        }

        /* синхронна функція для Delete() */
        public bool DeleteSync() { return Task.Run&lt;bool&gt;(async () =&gt; { await Delete(); return true; }).Result; } 
        
        public <xsl:value-of select="$DocumentName"/>_Pointer GetDocumentPointer()
        {
            return new <xsl:value-of select="$DocumentName"/>_Pointer(UnigueID.UGuid);
        }

        public UuidAndText GetBasis()
        {
            return new UuidAndText(UnigueID.UGuid, <xsl:value-of select="$DocumentName"/>_Const.POINTER);
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
        public <xsl:value-of select="$DocumentName"/>_Pointer(object? uid = null) : base(Config.Kernel, "<xsl:value-of select="Table"/>", "<xsl:value-of select="$DocumentName"/>")
        {
            base.Init(new UnigueID(uid), null);
        }
        
        public <xsl:value-of select="$DocumentName"/>_Pointer(UnigueID uid, Dictionary&lt;string, object&gt;? fields = null) : base(Config.Kernel, "<xsl:value-of select="Table"/>", "<xsl:value-of select="$DocumentName"/>")
        {
            base.Init(uid, fields);
        }

        public string Назва { get; set; } = "";

        public async ValueTask&lt;string&gt; GetPresentation()
        {
            return Назва = await base.BasePresentation(
              <xsl:text>[</xsl:text>
              <xsl:for-each select="Fields/Field[IsPresentation=1]">
                <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
              </xsl:for-each>]
            );
        }

        /* синхронна функція для GetPresentation() */
        public string GetPresentationSync() { return Task.Run&lt;string&gt;(async () =&gt; { return await GetPresentation(); }).Result; }

        public async ValueTask&lt;bool&gt; SpendTheDocument(DateTime spendDate)
        {
            <xsl:value-of select="$DocumentName"/>_Objest? obj = await GetDocumentObject();
            return (obj != null ? await obj.SpendTheDocument(spendDate) : false);
        }

        public async ValueTask ClearSpendTheDocument()
        {
            <xsl:value-of select="$DocumentName"/>_Objest? obj = await GetDocumentObject();
            if (obj != null) await obj.ClearSpendTheDocument();
        }

        public async ValueTask SetDeletionLabel(bool label = true)
        {
            <xsl:if test="normalize-space(TriggerFunctions/SetDeletionLabel) != '' or normalize-space(SpendFunctions/ClearSpend) != ''">
                <xsl:value-of select="$DocumentName"/>_Objest? obj = await GetDocumentObject();
                if (obj == null) return;
                <xsl:if test="normalize-space(TriggerFunctions/SetDeletionLabel) != ''">
                    await <xsl:value-of select="TriggerFunctions/SetDeletionLabel"/>
                    <xsl:text>(obj, label)</xsl:text>;
                </xsl:if>
                <xsl:if test="normalize-space(SpendFunctions/ClearSpend) != ''">
                if (label)
                {
                    await <xsl:value-of select="SpendFunctions/ClearSpend"/>
                    <xsl:text>(obj)</xsl:text>;
                    <xsl:text>await BaseSpend(false, DateTime.MinValue)</xsl:text>;
                }
                </xsl:if>
            </xsl:if>
            await base.BaseDeletionLabel(label);
        }

        public <xsl:value-of select="$DocumentName"/>_Pointer Copy()
        {
            return new <xsl:value-of select="$DocumentName"/>_Pointer(base.UnigueID, base.Fields) { Назва = Назва };
        }

        public <xsl:value-of select="$DocumentName"/>_Pointer GetEmptyPointer()
        {
            return new <xsl:value-of select="$DocumentName"/>_Pointer();
        }

        public UuidAndText GetBasis()
        {
            return new UuidAndText(UnigueID.UGuid, <xsl:value-of select="$DocumentName"/>_Const.POINTER);
        }

        public async ValueTask&lt;<xsl:value-of select="$DocumentName"/>_Objest?&gt; GetDocumentObject(bool readAllTablePart = false)
        {
            if (this.IsEmpty()) return null;
            <xsl:value-of select="$DocumentName"/>_Objest <xsl:value-of select="$DocumentName"/>ObjestItem = new <xsl:value-of select="$DocumentName"/>_Objest();
            if (!await <xsl:value-of select="$DocumentName"/>ObjestItem.Read(base.UnigueID, readAllTablePart)) return null;
            <!--<xsl:if test="count(TabularParts/TablePart) != 0">
            if (readAllTablePart)
            {   
                <xsl:for-each select="TabularParts/TablePart">
                await <xsl:value-of select="$DocumentName"/>ObjestItem.<xsl:value-of select="concat(Name, '_TablePart')"/>.Read();</xsl:for-each>
            }
            </xsl:if>-->
            return <xsl:value-of select="$DocumentName"/>ObjestItem;
        }

        public void Clear()
        {
            Init(new UnigueID(), null);
            Назва = "";
        }
    }

    public class <xsl:value-of select="$DocumentName"/>_Select : DocumentSelect
    {		
        public <xsl:value-of select="$DocumentName"/>_Select() : base(Config.Kernel, "<xsl:value-of select="Table"/>") { }
        
        public async ValueTask&lt;bool&gt; Select() { return await base.BaseSelect(); }
        
        public async ValueTask&lt;bool&gt; SelectSingle() { if (await base.BaseSelectSingle()) { MoveNext(); return true; } else { Current = null; return false; } }
        
        public bool MoveNext() { if (MoveToPosition()) { Current = new <xsl:value-of select="$DocumentName"/>_Pointer(base.DocumentPointerPosition.UnigueID, base.DocumentPointerPosition.Fields); return true; } else { Current = null; return false; } }
        
        public <xsl:value-of select="$DocumentName"/>_Pointer? Current { get; private set; }
    }

      <xsl:for-each select="TabularParts/TablePart"> <!-- TableParts -->
        <xsl:variable name="TablePartName" select="Name"/>
        <xsl:variable name="TablePartFullName" select="concat($DocumentName, '_', $TablePartName)"/>
    
    public class <xsl:value-of select="$TablePartFullName"/>_TablePart : DocumentTablePart
    {
        public <xsl:value-of select="$TablePartFullName"/>_TablePart(<xsl:value-of select="$DocumentName"/>_Objest owner) : base(Config.Kernel, "<xsl:value-of select="Table"/>",
             <xsl:text>[</xsl:text>
             <xsl:for-each select="Fields/Field">
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
             </xsl:for-each>])
        {
            if (owner == null) throw new Exception("owner null");
            Owner = owner;
        }
        <xsl:for-each select="Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>

        public <xsl:value-of select="$DocumentName"/>_Objest Owner { get; private set; }
        
        public List&lt;Record&gt; Records { get; set; } = [];
        
        public async ValueTask Read()
        {
            Records.Clear();
            await base.BaseRead(Owner.UnigueID);

            foreach (Dictionary&lt;string, object&gt; fieldValue in base.FieldValueList) 
            {
                Record record = new Record()
                {
                    UID = (Guid)fieldValue["uid"],
                    <xsl:for-each select="Fields/Field">
                      <xsl:value-of select="Name"/>
                      <xsl:text> = </xsl:text>
                      <xsl:call-template name="ReadFieldValue">
                        <xsl:with-param name="BaseFieldContainer">fieldValue</xsl:with-param>
                      </xsl:call-template>,
                    </xsl:for-each>
                };
                Records.Add(record);
            }
            
            base.BaseClear();
        }
        
        public async ValueTask Save(bool clear_all_before_save /*= true*/) 
        {
            await base.BaseBeginTransaction();
                
            if (clear_all_before_save)
                await base.BaseDelete(Owner.UnigueID);

            foreach (Record record in Records)
            {
                Dictionary&lt;string, object&gt; fieldValue = new Dictionary&lt;string, object&gt;()
                {
                    <xsl:for-each select="Fields/Field">
                        <xsl:text>{"</xsl:text>
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
                        <xsl:text>}</xsl:text>,
                    </xsl:for-each>
                };
                record.UID = await base.BaseSave(record.UID, Owner.UnigueID, fieldValue);
            }
            
            await base.BaseCommitTransaction();
        }

        public async ValueTask Delete()
        {
            await base.BaseDelete(Owner.UnigueID);
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

namespace <xsl:value-of select="Configuration/NameSpaceGenerationCode"/>.Журнали
{
    #region Journal
    public class Journal_Select: JournalSelect
    {
        public Journal_Select() : base(Config.Kernel,
             <xsl:text>[</xsl:text><xsl:for-each select="Configuration/Documents/Document"><xsl:text>"</xsl:text><xsl:value-of select="Table"/><xsl:text>", </xsl:text></xsl:for-each>],
			       <xsl:text>[</xsl:text><xsl:for-each select="Configuration/Documents/Document"><xsl:text>"</xsl:text><xsl:value-of select="Name"/><xsl:text>", </xsl:text></xsl:for-each>]) { }

        public async ValueTask&lt;DocumentObject?&gt; GetDocumentObject(bool readAllTablePart = true)
        {
            if (Current == null) return null;
            switch (Current.TypeDocument)
            {
                <xsl:for-each select="Configuration/Documents/Document">
                    <xsl:text>case </xsl:text>"<xsl:value-of select="Name"/>": return await new Документи.<xsl:value-of select="Name"/>_Pointer(Current.UnigueID).GetDocumentObject(readAllTablePart);
                </xsl:for-each>
                default: return null;
            }
        }
    }
    #endregion
<!--
    public class Journal_Document : JournalObject
    {
        public Journal_Document(string documentType, UnigueID uid) : base(Config.Kernel)
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

namespace <xsl:value-of select="Configuration/NameSpaceGenerationCode"/>.РегістриВідомостей
{
    <xsl:for-each select="Configuration/RegistersInformation/RegisterInformation">
       <xsl:variable name="RegisterName" select="Name"/>
    #region REGISTER "<xsl:value-of select="$RegisterName"/>"
    public static class <xsl:value-of select="$RegisterName"/>_Const
    {
        public const string FULLNAME = "<xsl:value-of select="normalize-space(FullName)"/>";
        public const string TABLE = "<xsl:value-of select="Table"/>";
        <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>
    }

    public class <xsl:value-of select="$RegisterName"/>_RecordsSet : RegisterInformationRecordsSet
    {
        public <xsl:value-of select="$RegisterName"/>_RecordsSet() : base(Config.Kernel, "<xsl:value-of select="Table"/>",
             <xsl:text>[</xsl:text>
             <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
             </xsl:for-each>]) { }
		
        public List&lt;Record&gt; Records { get; set; } = [];
        
        public async ValueTask Read()
        {
            Records.Clear();
            await base.BaseRead();
            foreach (Dictionary&lt;string, object&gt; fieldValue in base.FieldValueList) 
            {
                Record record = new Record()
                {
                    UID = (Guid)fieldValue["uid"],
                    Period = DateTime.Parse(fieldValue["period"]?.ToString() ?? DateTime.MinValue.ToString()),
                    Owner = (Guid)fieldValue["owner"],
                    <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                      <xsl:value-of select="Name"/>
                      <xsl:text> = </xsl:text>
                      <xsl:call-template name="ReadFieldValue">
                        <xsl:with-param name="BaseFieldContainer">fieldValue</xsl:with-param>
                      </xsl:call-template>,
                    </xsl:for-each>
                };
                Records.Add(record);
            }
            base.BaseClear();
        }
        
        public async ValueTask Save(DateTime period, Guid owner)
        {
            await base.BaseBeginTransaction();
            await base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary&lt;string, object&gt; fieldValue = new Dictionary&lt;string, object&gt;()
                {
                    <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                        <xsl:text>{"</xsl:text>
                        <xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                        <xsl:if test="Type = 'enum'">
                            <xsl:text>(int)</xsl:text>      
                        </xsl:if>
                        <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>
                        <xsl:if test="Type = 'pointer'">
                        <xsl:text>.UnigueID.UGuid</xsl:text>
                        </xsl:if>
                        <xsl:text>}</xsl:text>,
                    </xsl:for-each>
                };
                record.UID = await base.BaseSave(record.UID, period, owner, fieldValue);
            }
            await base.BaseCommitTransaction();
        }
        
        public async ValueTask Delete(Guid owner)
        {
            await base.BaseDelete(owner);
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
		    public <xsl:value-of select="$RegisterName"/>_Objest() : base(Config.Kernel, "<xsl:value-of select="Table"/>",
             <xsl:text>[</xsl:text>
             <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
             </xsl:for-each>]) 
        {
            <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
              <xsl:value-of select="Name"/>
              <xsl:text> = </xsl:text>
              <xsl:call-template name="DefaultFieldValue" />;
            </xsl:for-each>
        }
        
        public async ValueTask&lt;bool&gt; Read(UnigueID uid)
        {
            if (await BaseRead(uid))
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
        
        public async ValueTask Save()
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
            await BaseSave();
        }

        public <xsl:value-of select="$RegisterName"/>_Objest Copy()
        {
            <xsl:value-of select="$RegisterName"/>_Objest copy = new <xsl:value-of select="$RegisterName"/>_Objest()
            {
                Period = Period, /* Базове поле */
                <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                    <xsl:value-of select="Name"/><xsl:text> = </xsl:text><xsl:value-of select="Name"/>,
                </xsl:for-each>
            };
            copy.New();
            return copy;
        }

        public async ValueTask Delete()
        {
			      await base.BaseDelete();
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

namespace <xsl:value-of select="Configuration/NameSpaceGenerationCode"/>.РегістриНакопичення
{
    public static class VirtualTablesСalculation
    {
        /* Функція повного очищення віртуальних таблиць */
        public static void ClearAll()
        {
            /*  */
        }

        /* Функція для обчислення віртуальних таблиць  */
        public static async ValueTask Execute(DateTime period, string regAccumName)
        {
            if (Config.Kernel == null) return;
            <xsl:variable name="QueryAllCountCalculation" select="count(Configuration/RegistersAccumulation/RegisterAccumulation/QueryBlockList/QueryBlock[FinalCalculation = '0']/Query)"/>
            <xsl:if test="$QueryAllCountCalculation != 0">
            Dictionary&lt;string, object&gt; paramQuery = new Dictionary&lt;string, object&gt;{ { "ПеріодДеньВідбір", period } };

            switch(regAccumName)
            {
            <xsl:for-each select="Configuration/RegistersAccumulation/RegisterAccumulation">
                <xsl:variable name="QueryCount" select="count(QueryBlockList/QueryBlock[FinalCalculation = '0']/Query)"/>
                <xsl:if test="$QueryCount != 0">
                case "<xsl:value-of select="Name"/>":
                {
                    byte transactionID = await Config.Kernel.DataBase.BeginTransaction();
                    <xsl:for-each select="QueryBlockList/QueryBlock[FinalCalculation = '0']">
                    /* QueryBlock: <xsl:value-of select="Name"/> */
                        <xsl:for-each select="Query">
                            <xsl:sort select="@position" data-type="number" order="ascending" />
                    await Config.Kernel.DataBase.ExecuteSQL($@"<xsl:value-of select="normalize-space(.)"/>", paramQuery, transactionID);
                        </xsl:for-each>
                    </xsl:for-each>
                    await Config.Kernel.DataBase.CommitTransaction(transactionID);
                    break;
                }
                </xsl:if>
            </xsl:for-each>
                    default:
                        break;
            }
            </xsl:if>
            <xsl:if test="$QueryAllCountCalculation = 0">
                <xsl:text>await ValueTask.FromResult(true);</xsl:text>
            </xsl:if>
        }

        /* Функція для обчислення підсумкових віртуальних таблиць */
        public static async ValueTask ExecuteFinalCalculation(List&lt;string&gt; regAccumNameList)
        {
            if (Config.Kernel == null) return;
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
                        byte transactionID = await Config.Kernel.DataBase.BeginTransaction();
                        <xsl:for-each select="QueryBlockList/QueryBlock[FinalCalculation = '1']">
                        /* QueryBlock: <xsl:value-of select="Name"/> */
                            <xsl:for-each select="Query">
                                <xsl:sort select="@position" data-type="number" order="ascending" />
                        await Config.Kernel.DataBase.ExecuteSQL($@"<xsl:value-of select="normalize-space(.)"/>", null, transactionID);
                            </xsl:for-each>
                        </xsl:for-each>
                        await Config.Kernel.DataBase.CommitTransaction(transactionID);
                        break;
                    }
                    </xsl:if>
                </xsl:for-each>
                        default:
                            break;
                }
            </xsl:if>
            <xsl:if test="$QueryAllCountCalculation = 0">
                <xsl:text>await ValueTask.FromResult(true);</xsl:text>
            </xsl:if>
        }
    }

    <xsl:for-each select="Configuration/RegistersAccumulation/RegisterAccumulation">
	    <xsl:variable name="Documents" select="../../Documents"/>
      <xsl:variable name="RegisterName" select="Name"/>
    #region REGISTER "<xsl:value-of select="$RegisterName"/>"
    public static class <xsl:value-of select="$RegisterName"/>_Const
    {
        public const string FULLNAME = "<xsl:value-of select="normalize-space(FullName)"/>";
        public const string TABLE = "<xsl:value-of select="Table"/>";
		    public static readonly string[] AllowDocumentSpendTable = [<xsl:for-each select="AllowDocumentSpend/Name">
			  <xsl:variable name="AllowDocumentSpendName" select="."/>
        <xsl:text>"</xsl:text><xsl:value-of select="$Documents/Document[Name = $AllowDocumentSpendName]/Table"/><xsl:text>", </xsl:text></xsl:for-each>];
		    public static readonly string[] AllowDocumentSpendType = [<xsl:for-each select="AllowDocumentSpend/Name"><xsl:text>"</xsl:text><xsl:value-of select="."/><xsl:text>", </xsl:text></xsl:for-each>];
        <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>
    }
	
    public class <xsl:value-of select="$RegisterName"/>_RecordsSet : RegisterAccumulationRecordsSet
    {
        public <xsl:value-of select="$RegisterName"/>_RecordsSet() : base(Config.Kernel, "<xsl:value-of select="Table"/>", "<xsl:value-of select="$RegisterName"/>",
             <xsl:text>[</xsl:text>
             <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
               <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
             </xsl:for-each>]) { }
		
        public List&lt;Record&gt; Records { get; set; } = [];
        
        public async ValueTask Read()
        {
            Records.Clear();
            await base.BaseRead();
            foreach (Dictionary&lt;string, object&gt; fieldValue in base.FieldValueList) 
            {
                Record record = new Record()
                {
                    UID = (Guid)fieldValue["uid"],
                    Period = DateTime.Parse(fieldValue["period"]?.ToString() ?? DateTime.MinValue.ToString()),
                    Income = (bool)fieldValue["income"],
                    Owner = (Guid)fieldValue["owner"],
                    <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                      <xsl:value-of select="Name"/>
                      <xsl:text> = </xsl:text>
                      <xsl:call-template name="ReadFieldValue">
                        <xsl:with-param name="BaseFieldContainer">fieldValue</xsl:with-param>
                      </xsl:call-template>,
                    </xsl:for-each>
                };
                Records.Add(record);
            }
            base.BaseClear();
        }
        
        public async ValueTask Save(DateTime period, Guid owner) 
        {
            await base.BaseBeginTransaction();
            await base.BaseSelectPeriodForOwner(owner, period);
            await base.BaseDelete(owner);
            foreach (Record record in Records)
            {
                record.Period = period;
                record.Owner = owner;
                Dictionary&lt;string, object&gt; fieldValue = new Dictionary&lt;string, object&gt;()
                {
                    <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                        <xsl:text>{"</xsl:text>
                        <xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
                        <xsl:if test="Type = 'enum'">
                            <xsl:text>(int)</xsl:text>      
                        </xsl:if>
                        <xsl:text>record.</xsl:text><xsl:value-of select="Name"/>
                        <xsl:if test="Type = 'pointer'">
                        <xsl:text>.UnigueID.UGuid</xsl:text>
                        </xsl:if>
                        <xsl:text>}</xsl:text>,
                    </xsl:for-each>
                };
                record.UID = await base.BaseSave(record.UID, period, record.Income, owner, fieldValue);
            }
            await base.BaseTrigerAdd(period, owner);
            await base.BaseCommitTransaction();
        }

        public async ValueTask Delete(Guid owner)
        {
            await base.BaseSelectPeriodForOwner(owner);
            await base.BaseDelete(owner);
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
        public <xsl:value-of select="$TablePartFullName"/>_TablePart() : base(Config.Kernel, "<xsl:value-of select="Table"/>",
              <xsl:text>[</xsl:text>
              <xsl:for-each select="Fields/Field">
                <xsl:text>"</xsl:text><xsl:value-of select="NameInTable"/><xsl:text>", </xsl:text>
              </xsl:for-each>]) { }
        
        public const string TABLE = "<xsl:value-of select="Table"/>";
        <xsl:for-each select="Fields/Field">
        public const string <xsl:value-of select="Name"/> = "<xsl:value-of select="NameInTable"/>";</xsl:for-each>
        public List&lt;Record&gt; Records { get; set; } = [];
    
        public async ValueTask Read()
        {
            Records.Clear();
            await base.BaseRead();
            foreach (Dictionary&lt;string, object&gt; fieldValue in base.FieldValueList) 
            {
                Record record = new Record()
                {
                    UID = (Guid)fieldValue["uid"],
                    <xsl:for-each select="Fields/Field">
                      <xsl:value-of select="Name"/>
                      <xsl:text> = </xsl:text>
                      <xsl:call-template name="ReadFieldValue">
                        <xsl:with-param name="BaseFieldContainer">fieldValue</xsl:with-param>
                      </xsl:call-template>,
                    </xsl:for-each>
                };
                Records.Add(record);
            }
            base.BaseClear();
        }
    
        public async ValueTask Save(bool clear_all_before_save /*= true*/) 
        {
            await base.BaseBeginTransaction();
            if (clear_all_before_save) await base.BaseDelete();
            foreach (Record record in Records)
            {
                Dictionary&lt;string, object&gt; fieldValue = new Dictionary&lt;string, object&gt;()
                {
                    <xsl:for-each select="Fields/Field">
                        <xsl:text>{"</xsl:text>
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
                        <xsl:text>}</xsl:text>,
                    </xsl:for-each>
                };
                record.UID = await base.BaseSave(record.UID, fieldValue);
            }
            await base.BaseCommitTransaction();
        }
    
        public async ValueTask Delete()
        {
            await base.BaseDelete();
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