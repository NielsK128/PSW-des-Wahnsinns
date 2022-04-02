using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Assets.WorldEngine
{
    public class CChemicalReactionRule : CXMLReadWrite
    {
        //INTERNAL ATTRIBUTES
        string _input;         //Eingabeformel: Elementzeichen oder Verbindung. Bspw. "H" oder "H2O".
        string _output;        //Ausgabeformel: Elementzeichen oder Verbindung. Bspw. "H2O" oder "O3".
        double _minKelvin;   //SI-Einheit:  Kelvin
        double _maxKelvin;    //SI-Einheit: Kelvin
        double _minPascal;          //SI-Einheit: Pascal (N/m^2)
        double _maxPascal;            //SI-Einheit: Pascal (N/m^2)

        //PUBLIC PROPERTIES:
        public string input
        {
            get { return _input; }
            set { _input = value; }
        }
        public string output
        {
            get { return _output; }
            set { _output = value; }
        }
        public double minKelvin
        {
            get { return _minKelvin; }
            set { _minKelvin = value; }
        }
        public double maxKelvin
        {
            get { return _maxKelvin; }
            set { _maxKelvin = value; }
        }
        public double minPascal
        {
            get { return _minPascal; }
            set { _minPascal = value; }
        }
        public double maxPascal
        {
            get { return _maxPascal; }
            set { _maxPascal = value; }
        }

        public CChemicalReactionRule()
        {
            input = "";
            output = "";
            minKelvin = 0;
            maxKelvin = -1;
            minPascal = 0;
            maxPascal = -1;
        }

        public void Define(string inSignature, string outSignature,double minTemperature=0,double maxTemperature=-1,double minPressure=0,double maxPressure=-1)
        {
            input = inSignature;
            output = outSignature;
            minKelvin = minTemperature;
            maxKelvin = maxTemperature;
            minPascal = minPressure;
            maxPascal = maxPressure;
        }
        public string Execute(string inSignature, double temperature = 0,  double pressure = 0)
        {
            if (inSignature != input)
                return "";
            if (temperature < minKelvin)
                return "";
            if (maxKelvin != -1 && temperature > maxKelvin)
                return "";
            if (pressure < minPascal)
                return "";
            if (maxPascal != -1 && pressure > maxPascal)
                return "";

            return output;
        }

        public override void Read(XmlNode myNode)
        {
            input = myNode.Attributes["input"].Value;
            output = myNode.Attributes["output"].Value;
            minKelvin = double.Parse(myNode.Attributes["minKelvin"].Value);
            maxKelvin = double.Parse(myNode.Attributes["maxKelvin"].Value);
            minPascal = double.Parse(myNode.Attributes["minPascal"].Value);
            maxPascal = double.Parse(myNode.Attributes["maxPascal"].Value);
        }
        public override void Write(XmlNode myNode)
        {
            XmlAttribute attr;

            attr = myNode.OwnerDocument.CreateAttribute("input");
            myNode.Attributes.Append(attr);
            attr.Value = input;

            attr = myNode.OwnerDocument.CreateAttribute("output");
            myNode.Attributes.Append(attr);
            attr.Value = output;

            attr = myNode.OwnerDocument.CreateAttribute("minKelvin");
            myNode.Attributes.Append(attr);
            attr.Value = minKelvin.ToString();
            attr = myNode.OwnerDocument.CreateAttribute("maxKelvin");
            myNode.Attributes.Append(attr);
            attr.Value = maxKelvin.ToString();

            attr = myNode.OwnerDocument.CreateAttribute("minPascal");
            myNode.Attributes.Append(attr);
            attr.Value = minPascal.ToString();
            attr = myNode.OwnerDocument.CreateAttribute("maxPascal");
            myNode.Attributes.Append(attr);
            attr.Value = maxPascal.ToString();
        }
    }
}
