Public Enum BondType As Integer
  [None] = 0
  [Single] = 1
  [Double] = 2
  [Triple] = 3
End Enum

<Serializable(), Xml.Serialization.XmlRoot("link")> _
 Public Structure BondLink
  <Xml.Serialization.XmlText()> _
  Public Atom As Short

  <Xml.Serialization.XmlAttributeAttribute("bond")> _
  Public Bond As BondType

End Structure

<Serializable(), Xml.Serialization.XmlRoot("atom")> _
Public Structure Atom
  'Public bond As Short()
  '<Xml.Serialization.XmlArray(ElementName:="link")> _
  'Public link() As Short
  <Xml.Serialization.XmlArrayItem(ElementName:="link")> _
  Public link As BondLink()

  <Xml.Serialization.XmlAttributeAttribute(AttributeName:="charge")> _
  Public Charge As Short

  <Xml.Serialization.XmlAttributeAttribute(AttributeName:="name")> _
  Public Name As String

  <Xml.Serialization.XmlAttributeAttribute(AttributeName:="x")> _
  Public X As Double

  <Xml.Serialization.XmlAttributeAttribute(AttributeName:="y")> _
  Public Y As Double

  <Xml.Serialization.XmlAttributeAttribute(AttributeName:="z")> _
  Public Z As Double

End Structure

