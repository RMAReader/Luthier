using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml.Serialization;

namespace Luthier.Model.GraphicObjects
{
    [Serializable]
    public class GraphicImage : GraphicObjectBase
    {
        [XmlElement]
        public string SourcePath = @"C:\Users\Richard\Documents\Violins\Messiah Stradivarious\Belly profile.bmp";
        //public string SourcePath = @"C:\Users\Richard\Documents\Violins\Titian Stradivarius\XWF~1-1.png";
        [XmlArray]
        public UniqueKey[] pointsKeys = new UniqueKey[3];
        
        private Image image;

        public override double GetDistance(ApplicationDocumentModel model, double x, double y)
        {
            return float.MaxValue;
        }

        public Image GetImage()
        {
            if(image == null)
            {
                try
                {
                    image = Image.FromFile(SourcePath);
                }
                catch(Exception e)
                {
                    
                }
            }
            return image;
        }

    }
}
