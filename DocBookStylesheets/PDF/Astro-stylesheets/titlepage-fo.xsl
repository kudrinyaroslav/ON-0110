<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:d="http://docbook.org/ns/docbook" xmlns:exsl="http://exslt.org/common" xmlns:fo="http://www.w3.org/1999/XSL/Format" version="1.0" exclude-result-prefixes="exsl d">

<!-- This stylesheet was created by template/titlepage.xsl-->

<xsl:template name="book.titlepage.recto">
  <xsl:apply-templates mode="book.titlepage.recto.auto.mode" select="d:bookinfo/d:orgname"/>
  <xsl:apply-templates mode="book.titlepage.recto.auto.mode" select="d:info/d:orgname"/>
  <xsl:choose>
    <xsl:when test="d:bookinfo/d:title">
      <xsl:apply-templates mode="book.titlepage.recto.auto.mode" select="d:bookinfo/d:title"/>
    </xsl:when>
    <xsl:when test="d:info/d:title">
      <xsl:apply-templates mode="book.titlepage.recto.auto.mode" select="d:info/d:title"/>
    </xsl:when>
    <xsl:when test="d:title">
      <xsl:apply-templates mode="book.titlepage.recto.auto.mode" select="d:title"/>
    </xsl:when>
  </xsl:choose>

  <xsl:choose>
    <xsl:when test="d:bookinfo/d:subtitle">
      <xsl:apply-templates mode="book.titlepage.recto.auto.mode" select="d:bookinfo/d:subtitle"/>
    </xsl:when>
    <xsl:when test="d:info/d:subtitle">
      <xsl:apply-templates mode="book.titlepage.recto.auto.mode" select="d:info/d:subtitle"/>
    </xsl:when>
    <xsl:when test="d:subtitle">
      <xsl:apply-templates mode="book.titlepage.recto.auto.mode" select="d:subtitle"/>
    </xsl:when>
  </xsl:choose>

  <xsl:apply-templates mode="book.titlepage.recto.auto.mode" select="d:bookinfo/d:date"/>
  <xsl:apply-templates mode="book.titlepage.recto.auto.mode" select="d:info/d:date"/>
  <xsl:apply-templates mode="book.titlepage.recto.auto.mode" select="d:bookinfo/d:releaseinfo"/>
  <xsl:apply-templates mode="book.titlepage.recto.auto.mode" select="d:info/d:releaseinfo"/>
  <xsl:apply-templates mode="book.titlepage.recto.auto.mode" select="d:bookinfo/d:itermset"/>
  <xsl:apply-templates mode="book.titlepage.recto.auto.mode" select="d:info/d:itermset"/>
</xsl:template>

<xsl:template name="book.titlepage.verso">
  <xsl:apply-templates mode="book.titlepage.verso.auto.mode" select="d:bookinfo/d:copyright"/>
  <xsl:apply-templates mode="book.titlepage.verso.auto.mode" select="d:info/d:copyright"/>
  <xsl:apply-templates mode="book.titlepage.verso.auto.mode" select="d:bookinfo/d:legalnotice"/>
  <xsl:apply-templates mode="book.titlepage.verso.auto.mode" select="d:info/d:legalnotice"/>
</xsl:template>

<xsl:template name="book.titlepage.separator"><xsl:call-template name="revhistory.page"/>
</xsl:template>

<xsl:template name="book.titlepage.before.recto">
</xsl:template>

<xsl:template name="book.titlepage.before.verso"><fo:block break-after="page"/>
</xsl:template>

<xsl:template name="book.titlepage">
  <fo:block>
    <xsl:variable name="recto.content">
      <xsl:call-template name="book.titlepage.before.recto"/>
      <xsl:call-template name="book.titlepage.recto"/>
    </xsl:variable>
    <xsl:variable name="recto.elements.count">
      <xsl:choose>
        <xsl:when test="function-available('exsl:node-set')"><xsl:value-of select="count(exsl:node-set($recto.content)/*)"/></xsl:when>
        <xsl:when test="contains(system-property('xsl:vendor'), 'Apache Software Foundation')">
          <!--Xalan quirk--><xsl:value-of select="count(exsl:node-set($recto.content)/*)"/></xsl:when>
        <xsl:otherwise>1</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:if test="(normalize-space($recto.content) != '') or ($recto.elements.count &gt; 0)">
      <fo:block><xsl:copy-of select="$recto.content"/></fo:block>
    </xsl:if>
    <xsl:variable name="verso.content">
      <xsl:call-template name="book.titlepage.before.verso"/>
      <xsl:call-template name="book.titlepage.verso"/>
    </xsl:variable>
    <xsl:variable name="verso.elements.count">
      <xsl:choose>
        <xsl:when test="function-available('exsl:node-set')"><xsl:value-of select="count(exsl:node-set($verso.content)/*)"/></xsl:when>
        <xsl:when test="contains(system-property('xsl:vendor'), 'Apache Software Foundation')">
          <!--Xalan quirk--><xsl:value-of select="count(exsl:node-set($verso.content)/*)"/></xsl:when>
        <xsl:otherwise>1</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:if test="(normalize-space($verso.content) != '') or ($verso.elements.count &gt; 0)">
      <fo:block><xsl:copy-of select="$verso.content"/></fo:block>
    </xsl:if>
    <xsl:call-template name="book.titlepage.separator"/>
  </fo:block>
</xsl:template>

<xsl:template match="*" mode="book.titlepage.recto.mode">
  <!-- if an element isn't found in this mode, -->
  <!-- try the generic titlepage.mode -->
  <xsl:apply-templates select="." mode="titlepage.mode"/>
</xsl:template>

<xsl:template match="*" mode="book.titlepage.verso.mode">
  <!-- if an element isn't found in this mode, -->
  <!-- try the generic titlepage.mode -->
  <xsl:apply-templates select="." mode="titlepage.mode"/>
</xsl:template>

<xsl:template match="d:orgname" mode="book.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="book.titlepage.recto.style" font-size="24pt" font-weight="bold" letter-spacing="0.4pt" font-family="Arial">
<xsl:apply-templates select="." mode="book.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:title" mode="book.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="book.titlepage.recto.style" text-align="center" font-size="24pt" space-before="5pt" font-weight="bold" font-family="{$title.fontset}" letter-spacing="0.4pt">
<xsl:call-template name="division.title">
<xsl:with-param name="node" select="ancestor-or-self::d:book[1]"/>
</xsl:call-template>
</fo:block>
</xsl:template>

<xsl:template match="d:subtitle" mode="book.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="book.titlepage.recto.style" text-align="center" font-size="12pt" space-before="25pt" font-weight="normal" letter-spacing="0.75pt" font-family="Helvetica">
<xsl:apply-templates select="." mode="book.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:date" mode="book.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="book.titlepage.recto.style" text-align="center" font-size="12pt" color="#00B0F0" space-before="9pt" font-weight="normal" letter-spacing="0.75pt">
<xsl:apply-templates select="." mode="book.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:releaseinfo" mode="book.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="book.titlepage.recto.style" font-size="10.5pt" space-before="9pt" font-weight="normal">
<xsl:apply-templates select="." mode="book.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:itermset" mode="book.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="book.titlepage.recto.style">
<xsl:apply-templates select="." mode="book.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:copyright" mode="book.titlepage.verso.auto.mode">
<fo:block xsl:use-attribute-sets="book.titlepage.verso.style" font-size="10.5pt" text-align="center">
<xsl:apply-templates select="." mode="book.titlepage.verso.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:legalnotice" mode="book.titlepage.verso.auto.mode">
<fo:block xsl:use-attribute-sets="book.titlepage.verso.style" text-align="justify" font-family="{$body.fontset}" font-size="10.5pt">
<xsl:apply-templates select="." mode="book.titlepage.verso.mode"/>
</fo:block>
</xsl:template>

<xsl:template name="chapter.titlepage.recto">
  <xsl:choose>
    <xsl:when test="d:chapterinfo/d:title">
      <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:chapterinfo/d:title"/>
    </xsl:when>
    <xsl:when test="d:docinfo/d:title">
      <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:docinfo/d:title"/>
    </xsl:when>
    <xsl:when test="d:info/d:title">
      <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:info/d:title"/>
    </xsl:when>
    <xsl:when test="d:title">
      <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:title"/>
    </xsl:when>
  </xsl:choose>

  <xsl:choose>
    <xsl:when test="d:chapterinfo/d:subtitle">
      <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:chapterinfo/d:subtitle"/>
    </xsl:when>
    <xsl:when test="d:docinfo/d:subtitle">
      <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:docinfo/d:subtitle"/>
    </xsl:when>
    <xsl:when test="d:info/d:subtitle">
      <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:info/d:subtitle"/>
    </xsl:when>
    <xsl:when test="d:subtitle">
      <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:subtitle"/>
    </xsl:when>
  </xsl:choose>

  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:chapterinfo/d:corpauthor"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:docinfo/d:corpauthor"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:info/d:corpauthor"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:chapterinfo/d:authorgroup"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:docinfo/d:authorgroup"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:info/d:authorgroup"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:chapterinfo/d:author"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:docinfo/d:author"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:info/d:author"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:chapterinfo/d:othercredit"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:docinfo/d:othercredit"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:info/d:othercredit"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:chapterinfo/d:releaseinfo"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:docinfo/d:releaseinfo"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:info/d:releaseinfo"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:chapterinfo/d:copyright"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:docinfo/d:copyright"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:info/d:copyright"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:chapterinfo/d:legalnotice"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:docinfo/d:legalnotice"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:info/d:legalnotice"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:chapterinfo/d:pubdate"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:docinfo/d:pubdate"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:info/d:pubdate"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:chapterinfo/d:revision"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:docinfo/d:revision"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:info/d:revision"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:chapterinfo/d:revhistory"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:docinfo/d:revhistory"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:info/d:revhistory"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:chapterinfo/d:abstract"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:docinfo/d:abstract"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:info/d:abstract"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:chapterinfo/d:itermset"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:docinfo/d:itermset"/>
  <xsl:apply-templates mode="chapter.titlepage.recto.auto.mode" select="d:info/d:itermset"/>
</xsl:template>

<xsl:template name="chapter.titlepage.verso">
</xsl:template>

<xsl:template name="chapter.titlepage.separator">
</xsl:template>

<xsl:template name="chapter.titlepage.before.recto">
</xsl:template>

<xsl:template name="chapter.titlepage.before.verso">
</xsl:template>

<xsl:template name="chapter.titlepage">
  <fo:block font-family="{$title.fontset}">
    <xsl:variable name="recto.content">
      <xsl:call-template name="chapter.titlepage.before.recto"/>
      <xsl:call-template name="chapter.titlepage.recto"/>
    </xsl:variable>
    <xsl:variable name="recto.elements.count">
      <xsl:choose>
        <xsl:when test="function-available('exsl:node-set')"><xsl:value-of select="count(exsl:node-set($recto.content)/*)"/></xsl:when>
        <xsl:when test="contains(system-property('xsl:vendor'), 'Apache Software Foundation')">
          <!--Xalan quirk--><xsl:value-of select="count(exsl:node-set($recto.content)/*)"/></xsl:when>
        <xsl:otherwise>1</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:if test="(normalize-space($recto.content) != '') or ($recto.elements.count &gt; 0)">
      <fo:block margin-left="{$title.margin.left}"><xsl:copy-of select="$recto.content"/></fo:block>
    </xsl:if>
    <xsl:variable name="verso.content">
      <xsl:call-template name="chapter.titlepage.before.verso"/>
      <xsl:call-template name="chapter.titlepage.verso"/>
    </xsl:variable>
    <xsl:variable name="verso.elements.count">
      <xsl:choose>
        <xsl:when test="function-available('exsl:node-set')"><xsl:value-of select="count(exsl:node-set($verso.content)/*)"/></xsl:when>
        <xsl:when test="contains(system-property('xsl:vendor'), 'Apache Software Foundation')">
          <!--Xalan quirk--><xsl:value-of select="count(exsl:node-set($verso.content)/*)"/></xsl:when>
        <xsl:otherwise>1</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:if test="(normalize-space($verso.content) != '') or ($verso.elements.count &gt; 0)">
      <fo:block><xsl:copy-of select="$verso.content"/></fo:block>
    </xsl:if>
    <xsl:call-template name="chapter.titlepage.separator"/>
  </fo:block>
</xsl:template>

<xsl:template match="*" mode="chapter.titlepage.recto.mode">
  <!-- if an element isn't found in this mode, -->
  <!-- try the generic titlepage.mode -->
  <xsl:apply-templates select="." mode="titlepage.mode"/>
</xsl:template>

<xsl:template match="*" mode="chapter.titlepage.verso.mode">
  <!-- if an element isn't found in this mode, -->
  <!-- try the generic titlepage.mode -->
  <xsl:apply-templates select="." mode="titlepage.mode"/>
</xsl:template>

<xsl:template match="d:title" mode="chapter.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="chapter.titlepage.recto.style" font-size="18pt" font-weight="bold" font-family="Arial">
<xsl:call-template name="component.title">
<xsl:with-param name="node" select="ancestor-or-self::d:chapter[1]"/>
</xsl:call-template>
</fo:block>
</xsl:template>

<xsl:template match="d:subtitle" mode="chapter.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="chapter.titlepage.recto.style" space-before="0.5em" font-style="italic" font-size="14.4pt" font-weight="bold">
<xsl:apply-templates select="." mode="chapter.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:corpauthor" mode="chapter.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="chapter.titlepage.recto.style" space-before="0.5em" space-after="0.5em" font-size="14.4pt">
<xsl:apply-templates select="." mode="chapter.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:authorgroup" mode="chapter.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="chapter.titlepage.recto.style" space-before="0.5em" space-after="0.5em" font-size="14.4pt">
<xsl:apply-templates select="." mode="chapter.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:author" mode="chapter.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="chapter.titlepage.recto.style" space-before="0.5em" space-after="0.5em" font-size="14.4pt">
<xsl:apply-templates select="." mode="chapter.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:othercredit" mode="chapter.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="chapter.titlepage.recto.style">
<xsl:apply-templates select="." mode="chapter.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:releaseinfo" mode="chapter.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="chapter.titlepage.recto.style">
<xsl:apply-templates select="." mode="chapter.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:copyright" mode="chapter.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="chapter.titlepage.recto.style">
<xsl:apply-templates select="." mode="chapter.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:legalnotice" mode="chapter.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="chapter.titlepage.recto.style">
<xsl:apply-templates select="." mode="chapter.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:pubdate" mode="chapter.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="chapter.titlepage.recto.style">
<xsl:apply-templates select="." mode="chapter.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:revision" mode="chapter.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="chapter.titlepage.recto.style">
<xsl:apply-templates select="." mode="chapter.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:revhistory" mode="chapter.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="chapter.titlepage.recto.style">
<xsl:apply-templates select="." mode="chapter.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:abstract" mode="chapter.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="chapter.titlepage.recto.style">
<xsl:apply-templates select="." mode="chapter.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:itermset" mode="chapter.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="chapter.titlepage.recto.style">
<xsl:apply-templates select="." mode="chapter.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template name="table.of.contents.titlepage.recto">
  <xsl:choose>
    <xsl:when test="d:table.of.contentsinfo/d:title">
      <xsl:apply-templates mode="table.of.contents.titlepage.recto.auto.mode" select="d:table.of.contentsinfo/d:title"/>
    </xsl:when>
    <xsl:when test="d:docinfo/d:title">
      <xsl:apply-templates mode="table.of.contents.titlepage.recto.auto.mode" select="d:docinfo/d:title"/>
    </xsl:when>
    <xsl:when test="d:info/d:title">
      <xsl:apply-templates mode="table.of.contents.titlepage.recto.auto.mode" select="d:info/d:title"/>
    </xsl:when>
    <xsl:when test="d:title">
      <xsl:apply-templates mode="table.of.contents.titlepage.recto.auto.mode" select="d:title"/>
    </xsl:when>
  </xsl:choose>

</xsl:template>

<xsl:template name="table.of.contents.titlepage">
  <fo:block>
    <xsl:variable name="recto.content">
      <xsl:call-template name="table.of.contents.titlepage.before.recto"/>
      <xsl:call-template name="table.of.contents.titlepage.recto"/>
    </xsl:variable>
    <xsl:variable name="recto.elements.count">
      <xsl:choose>
        <xsl:when test="function-available('exsl:node-set')"><xsl:value-of select="count(exsl:node-set($recto.content)/*)"/></xsl:when>
        <xsl:when test="contains(system-property('xsl:vendor'), 'Apache Software Foundation')">
          <!--Xalan quirk--><xsl:value-of select="count(exsl:node-set($recto.content)/*)"/></xsl:when>
        <xsl:otherwise>1</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:if test="(normalize-space($recto.content) != '') or ($recto.elements.count &gt; 0)">
      <fo:block><xsl:copy-of select="$recto.content"/></fo:block>
    </xsl:if>
    <xsl:variable name="verso.content">
      <xsl:call-template name="table.of.contents.titlepage.before.verso"/>
      <xsl:call-template name="table.of.contents.titlepage.verso"/>
    </xsl:variable>
    <xsl:variable name="verso.elements.count">
      <xsl:choose>
        <xsl:when test="function-available('exsl:node-set')"><xsl:value-of select="count(exsl:node-set($verso.content)/*)"/></xsl:when>
        <xsl:when test="contains(system-property('xsl:vendor'), 'Apache Software Foundation')">
          <!--Xalan quirk--><xsl:value-of select="count(exsl:node-set($verso.content)/*)"/></xsl:when>
        <xsl:otherwise>1</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:if test="(normalize-space($verso.content) != '') or ($verso.elements.count &gt; 0)">
      <fo:block><xsl:copy-of select="$verso.content"/></fo:block>
    </xsl:if>
    <xsl:call-template name="table.of.contents.titlepage.separator"/>
  </fo:block>
</xsl:template>

<xsl:template match="*" mode="table.of.contents.titlepage.recto.mode">
  <!-- if an element isn't found in this mode, -->
  <!-- try the generic titlepage.mode -->
  <xsl:apply-templates select="." mode="titlepage.mode"/>
</xsl:template>

<xsl:template match="*" mode="table.of.contents.titlepage.verso.mode">
  <!-- if an element isn't found in this mode, -->
  <!-- try the generic titlepage.mode -->
  <xsl:apply-templates select="." mode="titlepage.mode"/>
</xsl:template>

<xsl:template match="d:title" mode="table.of.contents.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="table.of.contents.titlepage.recto.style" font-size="10.5pt" font-weight="bold">
<xsl:call-template name="gentext">
<xsl:with-param name="key" select="'TableofContents'"/>
</xsl:call-template>
</fo:block>
</xsl:template>

<xsl:template name="appendix.titlepage.recto">
  <xsl:choose>
    <xsl:when test="d:appendixinfo/d:title">
      <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:appendixinfo/d:title"/>
    </xsl:when>
    <xsl:when test="d:docinfo/d:title">
      <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:docinfo/d:title"/>
    </xsl:when>
    <xsl:when test="d:info/d:title">
      <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:info/d:title"/>
    </xsl:when>
    <xsl:when test="d:title">
      <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:title"/>
    </xsl:when>
  </xsl:choose>

  <xsl:choose>
    <xsl:when test="d:appendixinfo/d:subtitle">
      <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:appendixinfo/d:subtitle"/>
    </xsl:when>
    <xsl:when test="d:docinfo/d:subtitle">
      <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:docinfo/d:subtitle"/>
    </xsl:when>
    <xsl:when test="d:info/d:subtitle">
      <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:info/d:subtitle"/>
    </xsl:when>
    <xsl:when test="d:subtitle">
      <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:subtitle"/>
    </xsl:when>
  </xsl:choose>

  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:appendixinfo/d:corpauthor"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:docinfo/d:corpauthor"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:info/d:corpauthor"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:appendixinfo/d:authorgroup"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:docinfo/d:authorgroup"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:info/d:authorgroup"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:appendixinfo/d:author"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:docinfo/d:author"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:info/d:author"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:appendixinfo/d:othercredit"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:docinfo/d:othercredit"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:info/d:othercredit"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:appendixinfo/d:releaseinfo"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:docinfo/d:releaseinfo"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:info/d:releaseinfo"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:appendixinfo/d:copyright"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:docinfo/d:copyright"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:info/d:copyright"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:appendixinfo/d:legalnotice"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:docinfo/d:legalnotice"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:info/d:legalnotice"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:appendixinfo/d:pubdate"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:docinfo/d:pubdate"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:info/d:pubdate"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:appendixinfo/d:revision"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:docinfo/d:revision"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:info/d:revision"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:appendixinfo/d:revhistory"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:docinfo/d:revhistory"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:info/d:revhistory"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:appendixinfo/d:abstract"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:docinfo/d:abstract"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:info/d:abstract"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:appendixinfo/d:itermset"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:docinfo/d:itermset"/>
  <xsl:apply-templates mode="appendix.titlepage.recto.auto.mode" select="d:info/d:itermset"/>
</xsl:template>

<xsl:template name="appendix.titlepage">
  <fo:block>
    <xsl:variable name="recto.content">
      <xsl:call-template name="appendix.titlepage.before.recto"/>
      <xsl:call-template name="appendix.titlepage.recto"/>
    </xsl:variable>
    <xsl:variable name="recto.elements.count">
      <xsl:choose>
        <xsl:when test="function-available('exsl:node-set')"><xsl:value-of select="count(exsl:node-set($recto.content)/*)"/></xsl:when>
        <xsl:when test="contains(system-property('xsl:vendor'), 'Apache Software Foundation')">
          <!--Xalan quirk--><xsl:value-of select="count(exsl:node-set($recto.content)/*)"/></xsl:when>
        <xsl:otherwise>1</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:if test="(normalize-space($recto.content) != '') or ($recto.elements.count &gt; 0)">
      <fo:block><xsl:copy-of select="$recto.content"/></fo:block>
    </xsl:if>
    <xsl:variable name="verso.content">
      <xsl:call-template name="appendix.titlepage.before.verso"/>
      <xsl:call-template name="appendix.titlepage.verso"/>
    </xsl:variable>
    <xsl:variable name="verso.elements.count">
      <xsl:choose>
        <xsl:when test="function-available('exsl:node-set')"><xsl:value-of select="count(exsl:node-set($verso.content)/*)"/></xsl:when>
        <xsl:when test="contains(system-property('xsl:vendor'), 'Apache Software Foundation')">
          <!--Xalan quirk--><xsl:value-of select="count(exsl:node-set($verso.content)/*)"/></xsl:when>
        <xsl:otherwise>1</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:if test="(normalize-space($verso.content) != '') or ($verso.elements.count &gt; 0)">
      <fo:block><xsl:copy-of select="$verso.content"/></fo:block>
    </xsl:if>
    <xsl:call-template name="appendix.titlepage.separator"/>
  </fo:block>
</xsl:template>

<xsl:template match="*" mode="appendix.titlepage.recto.mode">
  <!-- if an element isn't found in this mode, -->
  <!-- try the generic titlepage.mode -->
  <xsl:apply-templates select="." mode="titlepage.mode"/>
</xsl:template>

<xsl:template match="*" mode="appendix.titlepage.verso.mode">
  <!-- if an element isn't found in this mode, -->
  <!-- try the generic titlepage.mode -->
  <xsl:apply-templates select="." mode="titlepage.mode"/>
</xsl:template>

<xsl:template match="d:title" mode="appendix.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="appendix.titlepage.recto.style" margin-left="{$title.margin.left}" font-size="16pt" font-weight="bold" font-family="{$title.fontset}" text-align="center" text-align-last="center">
<xsl:call-template name="component.title">
<xsl:with-param name="node" select="ancestor-or-self::d:appendix[1]"/>
</xsl:call-template>
</fo:block>
</xsl:template>

<xsl:template match="d:subtitle" mode="appendix.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="appendix.titlepage.recto.style" font-family="{$title.fontset}">
<xsl:apply-templates select="." mode="appendix.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:corpauthor" mode="appendix.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="appendix.titlepage.recto.style">
<xsl:apply-templates select="." mode="appendix.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:authorgroup" mode="appendix.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="appendix.titlepage.recto.style">
<xsl:apply-templates select="." mode="appendix.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:author" mode="appendix.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="appendix.titlepage.recto.style">
<xsl:apply-templates select="." mode="appendix.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:othercredit" mode="appendix.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="appendix.titlepage.recto.style">
<xsl:apply-templates select="." mode="appendix.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:releaseinfo" mode="appendix.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="appendix.titlepage.recto.style">
<xsl:apply-templates select="." mode="appendix.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:copyright" mode="appendix.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="appendix.titlepage.recto.style">
<xsl:apply-templates select="." mode="appendix.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:legalnotice" mode="appendix.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="appendix.titlepage.recto.style">
<xsl:apply-templates select="." mode="appendix.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:pubdate" mode="appendix.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="appendix.titlepage.recto.style">
<xsl:apply-templates select="." mode="appendix.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:revision" mode="appendix.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="appendix.titlepage.recto.style">
<xsl:apply-templates select="." mode="appendix.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:revhistory" mode="appendix.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="appendix.titlepage.recto.style">
<xsl:apply-templates select="." mode="appendix.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:abstract" mode="appendix.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="appendix.titlepage.recto.style">
<xsl:apply-templates select="." mode="appendix.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

<xsl:template match="d:itermset" mode="appendix.titlepage.recto.auto.mode">
<fo:block xsl:use-attribute-sets="appendix.titlepage.recto.style">
<xsl:apply-templates select="." mode="appendix.titlepage.recto.mode"/>
</fo:block>
</xsl:template>

</xsl:stylesheet>
