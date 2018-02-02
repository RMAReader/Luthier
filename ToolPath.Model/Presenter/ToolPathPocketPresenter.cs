using Luthier.CncOperation;
using Luthier.CncTool;
using Luthier.Model.GraphicObjects;
using Luthier.Model.MouseController;
using Luthier.Model.ToolPathSpecification;
using Luthier.ToolPath;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luthier.Model.Presenter
{

    

    public class PocketToolPathPresenter
    {

        private readonly IApplicationDocumentModel model;

        private PocketSpecification specification;


        public PolygonSelector MouseController { get; }

        public string Name { get; set; }
        public double CutHeight { get; set; }
        public double SafeHeight { get; set; }
        public double StepLength { get; set; }
        public BaseTool Tool { get; set; }
        public EnumSpindleState SpindleState { get; set; }
        public int SpindleSpeed { get; set; }
        public int FeedRate { get; set; }
        public List<GraphicPolygon2D> BoundaryPolygonKey { get; set; }

        private List<BaseTool> availableTools = new CncToolRepository().GetAllTools();
        public List<BaseTool> AvailableTools { get => availableTools; }
        public List<EnumSpindleState> SpindleStates { get; set; }

        public PocketToolPathPresenter(IApplicationDocumentModel model, PocketSpecification specification)
        {
            this.model = model;
            MouseController = model.MouseControllerFactory().PolygonSelector(20);

            LoadModelSpecification(specification);

        }


        public void LoadModelSpecification(PocketSpecification specification)
        {
            this.specification = specification;

            Name = specification.Name;
            CutHeight = specification.CutHeight;
            SafeHeight = specification.SafeHeight;
            StepLength = specification.StepLength;
            Tool = specification.Tool;
            SpindleSpeed = specification.SpindleSpeed;
            SpindleState = specification.SpindleState;
            FeedRate = specification.FeedRate;
            BoundaryPolygonKey = specification.BoundaryPolygonKey
                                        .Select(x => model.Objects()[x] as GraphicPolygon2D)
                                        .Where(x => x != null)
                                        .ToList();

            MouseController.selectedPolygons = BoundaryPolygonKey;
        }

        public void SaveModelSpecification()
        {
            
            specification.Name = Name;
            specification.CutHeight= CutHeight;
            specification.SafeHeight = SafeHeight;
            specification.StepLength = StepLength;
            specification.Tool = Tool;
            specification.SpindleSpeed = SpindleSpeed;
            specification.SpindleState = SpindleState;
            specification.FeedRate = FeedRate;
            specification.BoundaryPolygonKey = BoundaryPolygonKey.Select(x => x.Key).ToList();
        }

 
        }



    

    }



