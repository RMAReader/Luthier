using Luthier.CncOperation;
using Luthier.Model.GraphicObjects;
using Luthier.Model.ToolPathSpecification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luthier.Model.Presenter
{
    public class ToolPathManagerPresenter
    {
        private readonly IApplicationDocumentModel model;

        public IApplicationDocumentModel GetModel() => model;

        public BindingList<ToolPathSpecificationBase> ToolPaths { get; set; }

        public ToolPathManagerPresenter(IApplicationDocumentModel model)
        {
            this.model = model;
            LoadToolPaths();
        }

        public void LoadToolPaths()
        {
            ToolPaths = new BindingList<ToolPathSpecificationBase>();
            foreach(ToolPathSpecificationBase specification in model.Objects().Values.Where(x => x is ToolPathSpecificationBase))
            {
                ToolPaths.Add(specification);
            }
        }

        public PocketToolPathPresenter NewPocketToolPath()
        {
            var specification = model.ToolPathFactory().PocketFactory().New();
            ToolPaths.Add(specification);
            return new PocketToolPathPresenter(model, specification);
        }

        public Drawing2DPresenter NewDrawing2dPresenter()
        {
            return new Drawing2DPresenter(model);
        }

        public PocketToolPathPresenter EditPocketToolPath(PocketSpecification specification)
        {
            return new PocketToolPathPresenter(model, specification);
        }

        public void CalculateToolPaths(ToolPathSpecificationBase spec)
        {
            var path = spec.GetCalculator(model).Execute();
            path.SavePathAsCncMachineCode(new CncOperationLanguageVisitorGCode(), @"C:\Users\Richard\Documents\Development\Luthier\TestData\toolpath.txt");
        }
    }
}
