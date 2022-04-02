using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Assets.WorldEngine
{
    public class CChemicalEntity : CXMLReadWrite
    {
        //INTERNAL ATTRIBUTES
        string _codeName;
        string _displayName;
        double _creationProbability; //[0,...,1]
        double _mass;

        //PUBLIC PROPERTIES:
        public string codeName
        {
            get { return _codeName; }
            set { _codeName = value; }
        }
        public string displayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
        public double creationProbability
        {
            get { return _creationProbability; }
            set { _creationProbability = Math.Max(0.0, Math.Min(1.0, value)); } //clamp to [0,...,1]
        }
        public double mass
        {
            get { return _mass; }
            set { _mass = Math.Max(0.0, value); } //mass can't be less or equal zero
        }

        public CChemicalEntity()
        {
            _codeName = "unknown";
            _creationProbability = 1;
            _mass = 1;
        }

        //##########################################################
        //XML FILE I/O
        public override void Read(XmlNode myNode)
        {
           codeName = myNode.Attributes["code"].Value;
            displayName = myNode.Attributes["name"].Value;
            creationProbability = double.Parse(myNode.Attributes["probability"].Value);
            mass = double.Parse(myNode.Attributes["mass"].Value);
        }
        public override void Write(XmlNode myNode)
        {
            XmlAttribute attr;
            
            attr = myNode.OwnerDocument.CreateAttribute("code");
            myNode.Attributes.Append(attr);
            attr.Value = codeName;

            attr = myNode.OwnerDocument.CreateAttribute("name");
            myNode.Attributes.Append(attr);
            attr.Value = displayName;

            attr = myNode.OwnerDocument.CreateAttribute("probability");
            myNode.Attributes.Append(attr);
            attr.Value = creationProbability.ToString();

            attr = myNode.OwnerDocument.CreateAttribute("mass");
            myNode.Attributes.Append(attr);
            attr.Value = mass.ToString();

        }
    }
}
