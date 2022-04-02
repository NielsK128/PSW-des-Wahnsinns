using System;
using System.Collections.Generic;
using System.Xml;
using System.Data;
using System.Globalization;

namespace Assets.WorldEngine
{
    public class CXMLReadWrite
    {
        public virtual void Read(XmlNode myNode)
        {
        }
        public virtual void Write(XmlNode myNode)
        {
        }
        public XmlNode CreateSection(XmlNode node, string attributeName)
        {
            XmlNode nodeSection = node.OwnerDocument.CreateElement(attributeName);
            node.AppendChild(nodeSection);
            return nodeSection;
        }
        public bool ReadStringAttribute(XmlNode node, string attributeName, out string value)
        {
            XmlAttribute attr;

            attr = node.Attributes[attributeName];
            if(attr == null)
            {
                value = "";
                return false;
            }
            value = attr.Value;
            return true;
        }
        public void WriteStringAttribute(XmlNode node, string attributeName, string value)
        {
            XmlAttribute attr;

            attr = node.OwnerDocument.CreateAttribute(attributeName);
            node.Attributes.Append(attr);
            attr.Value = value;
        }

        public bool ReadIntAttribute(XmlNode node, string attributeName, out int value)
        {
            XmlAttribute attr;

            attr = node.Attributes[attributeName];
            if(attr==null)
            {
                value = 0;
                return false;
            }
            return int.TryParse(attr.Value, out value);
        }
        public void WriteIntAttribute(XmlNode node,string attributeName,int value)
        {
            XmlAttribute attr;

            attr = node.OwnerDocument.CreateAttribute(attributeName);
            node.Attributes.Append(attr);
            attr.Value = value.ToString();
        }
        public bool ReadDoubleAttribute(XmlNode node, string attributeName, out double value)
        {
            XmlAttribute attr;

            attr = node.Attributes[attributeName];
            if(attr == null)
            {
                value = 0;
                return false;
            }
            return double.TryParse(attr.Value, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out value);
        }
        public void WriteDoubleAttribute(XmlNode node, string attributeName, double value)
        {
            XmlAttribute attr;

            attr = node.OwnerDocument.CreateAttribute(attributeName);
            node.Attributes.Append(attr);
            attr.Value = value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
