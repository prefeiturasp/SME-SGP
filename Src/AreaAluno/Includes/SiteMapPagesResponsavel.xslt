<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" indent="yes" encoding="utf-8" omit-xml-declaration="yes"/>
  <xsl:variable name="shift-width" select="15"/>

  <xsl:template match="/menus">
      <xsl:apply-templates select="sistema"/>
      <div class="clearfix"></div>
  </xsl:template>

  <xsl:template match="sistema">
    <ul class="listaMenu">
      <li class="txtMenu">
      </li>
      <xsl:apply-templates select="menu">
        <xsl:with-param name="depth" select="1"/>
      </xsl:apply-templates>
    </ul>
  </xsl:template>

  <xsl:template match="menu">
    <xsl:param name="depth"/>
    <xsl:if test="$depth>1">
      <li width="{$shift-width}"></li>
    </xsl:if>

    <li class="txtSubMenu">

      <xsl:choose>
        <xsl:when test="position() mod 6 = 1">
          <a class="link verde" >
            <xsl:attribute name="href">
              <xsl:value-of select="@url"/>
            </xsl:attribute>
            <xsl:attribute name="title">
              <xsl:value-of select="@id"/>
            </xsl:attribute>
            <span>
              <xsl:value-of select="@id"/>
            </span>
            <span class="linkHover"></span>
          </a>
        </xsl:when>
        <xsl:when test="position() mod 6 = 2">
          <a class="link roxo" >
            <xsl:attribute name="href">
              <xsl:value-of select="@url"/>
            </xsl:attribute>
            <xsl:attribute name="title">
              <xsl:value-of select="@id"/>
            </xsl:attribute>
            <span>
              <xsl:value-of select="@id"/>
            </span>
            <span class="linkHover"></span>
          </a>
        </xsl:when>
        <xsl:when test="position() mod 6 = 3">
          <a class="link azul" >
            <xsl:attribute name="href">
              <xsl:value-of select="@url"/>
            </xsl:attribute>
            <xsl:attribute name="title">
              <xsl:value-of select="@id"/>
            </xsl:attribute>
            <span>
              <xsl:value-of select="@id"/>
            </span>
            <span class="linkHover"></span>
          </a>
        </xsl:when>
        <xsl:when test="position() mod 6 = 4">
          <a class="link laranja" >
            <xsl:attribute name="href">
              <xsl:value-of select="@url"/>
            </xsl:attribute>
            <xsl:attribute name="title">
              <xsl:value-of select="@id"/>
            </xsl:attribute>
            <span>
              <xsl:value-of select="@id"/>
            </span>
            <span class="linkHover"></span>
          </a>
        </xsl:when>
        <xsl:when test="position() mod 6 = 5">
          <a class="link vermelho" >
            <xsl:attribute name="href">
              <xsl:value-of select="@url"/>
            </xsl:attribute>
            <xsl:attribute name="title">
              <xsl:value-of select="@id"/>
            </xsl:attribute>
            <span>
              <xsl:value-of select="@id"/>
            </span>
            <span class="linkHover"></span>
          </a>
        </xsl:when>
        <xsl:when test="position() mod 6 = 0">
          <a class="link branco" >
            <xsl:attribute name="href">
              <xsl:value-of select="@url"/>
            </xsl:attribute>
            <xsl:attribute name="title">
              <xsl:value-of select="@id"/>
            </xsl:attribute>
            <span>
              <xsl:value-of select="@id"/>
            </span>
            <span class="linkHover"></span>
          </a>
        </xsl:when>
      </xsl:choose>
    </li>
  </xsl:template>
</xsl:stylesheet>
