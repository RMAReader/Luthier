using Luthier.Model.GraphicObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luthier.Model.UIForms
{
    public partial class ObjectExplorerForm : Form
    {
        private readonly IApplicationDocumentModel _model;

        public ObjectExplorerForm(IApplicationDocumentModel model)
        {
            InitializeComponent();

            _model = model;

            _model.Model.ModelChangedHandler += OnModelChanged;

            CreateTreeFromModel();
        }

       
        private void OnModelChanged(object sender, ModelChangeEventArgs e)
        {
            if (e.ObjectAdded != null)
            {
                CreateTreeFromModel();
                //modelTreeView.Nodes[0].Nodes[0].Nodes.Add(CreateNode(e.ObjectAdded));
            }
            if (e.ObjectRemoved != null)
            {
                CreateTreeFromModel();
                //modelTreeView.Nodes[0].Nodes[0].Nodes.RemoveByKey(e.ObjectRemoved.Key.ToString());
            }
        }



        private void CreateTreeFromModel()
        {
            modelTreeView.Nodes.Clear();

            var rootNode = new TreeNode();
            rootNode.Text = _model.Model.Name;
            rootNode.Checked = true;
            rootNode.Tag = _model.Model;

            modelTreeView.Nodes.Add(rootNode);

            //build tree recursively from base layer objects
            foreach (var obj in _model.Model.Where(x => x.LayerKey == null))
            {
                rootNode.Nodes.Add(BuildNodesFromBaseLayer(obj));
            }

            modelTreeView.ExpandAll();
        }

        private TreeNode BuildNodesFromBaseLayer(GraphicObjectBase obj)
        {
            var node = CreateNode(obj);

            if(obj is GraphicLayer)
            {
                var layer = obj as GraphicLayer;
                foreach (var childKey in layer.Objects)
                {
                    var childObj = _model.Model[childKey];
                   
                    node.Nodes.Add(BuildNodesFromBaseLayer(childObj));
                }
            }

            return node;
        }


        private TreeNode CreateNode(GraphicObjectBase obj)
        {
            return new TreeNode
            {
                Text = obj.Name,
                Name = obj.Key.ToString(),
                Tag = obj,
                Checked = obj.IsVisible,
                ContextMenuStrip = contextMenuStrip1
            };
        }


        #region "label editing event handlers"


        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mySelectedNode != null && mySelectedNode.Parent != null)
            {
                modelTreeView.SelectedNode = mySelectedNode;
                modelTreeView.LabelEdit = true;
                if (!mySelectedNode.IsEditing)
                {
                    mySelectedNode.BeginEdit();
                }
            }
            else
            {
                MessageBox.Show("No tree node selected or selected node is a root node.\n" +
                   "Editing of root nodes is not allowed.", "Invalid selection");
            }

            contextMenuStrip1.Close();
        }

        private TreeNode mySelectedNode;
        private void modelTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            mySelectedNode = modelTreeView.GetNodeAt(e.X, e.Y);

            SetContextMenuItems(mySelectedNode);
        }

        private void modelTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                if (e.Label.Length > 0)
                {
                    if (e.Label.IndexOfAny(new char[] { '@', '.', ',', '!' }) == -1)
                    {
                        // Stop editing without canceling the label change.
                        e.Node.EndEdit(false);

                        UpdateGraphicObjectIsVisible(e.Node);
                        if (e.Node.Tag is GraphicObjectBase)
                        {
                            ((GraphicObjectBase)e.Node.Tag).Name = e.Label;
                            //((GraphicObjectBase)e.Node.Tag).IsVisible = e.Node.Checked;
                        }
                    }
                    else
                    {
                        /* Cancel the label edit action, inform the user, and 
                           place the node in edit mode again. */
                        e.CancelEdit = true;
                        MessageBox.Show("Invalid tree node label.\n" +
                           "The invalid characters are: '@','.', ',', '!'",
                           "Node Label Edit");
                        e.Node.BeginEdit();
                    }
                }
                else
                {
                    /* Cancel the label edit action, inform the user, and 
                       place the node in edit mode again. */
                    e.CancelEdit = true;
                    MessageBox.Show("Invalid tree node label.\nThe label cannot be blank",
                       "Node Label Edit");
                    e.Node.BeginEdit();
                }
            }
        }



        #endregion


        #region "drag and drop event handlers"

        private void modelTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // Move the dragged node when the left mouse button is used.
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        // Set the target drop effect to the effect 
        // specified in the ItemDrag event handler.
        private void modelTreeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        // Select the node under the mouse pointer to indicate the 
        // expected drop location.
        private void modelTreeView_DragOver(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the mouse position.
            Point targetPoint = modelTreeView.PointToClient(new Point(e.X, e.Y));

            // Select the node at the mouse position.
            modelTreeView.SelectedNode = modelTreeView.GetNodeAt(targetPoint);
        }

        private void modelTreeView_DragDrop(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = modelTreeView.PointToClient(new Point(e.X, e.Y));

            // Retrieve the node at the drop location.
            TreeNode targetNode = modelTreeView.GetNodeAt(targetPoint);

            // Retrieve the node that was dragged.
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            // Confirm that the node at the drop location is not 
            // the dragged node or a descendant of the dragged node.
            if (!draggedNode.Equals(targetNode))// && !ContainsNode(draggedNode, targetNode))
            {
                // If it is a move operation, remove the node from its current 
                // location and add it to the node at the drop location.
                if (e.Effect == DragDropEffects.Move)
                {
                    var draggedObj = draggedNode.Tag as GraphicObjectBase;
                    var targetObj = targetNode.Tag as GraphicLayer;
                    if (targetObj != null && draggedObj != null)
                    {
                        draggedObj.RemoveFromLayer();
                        targetObj.AddToLayer(draggedObj);

                        draggedNode.Remove();
                        targetNode.Nodes.Add(draggedNode);
                    }
                }
               
                targetNode.Expand();
            }
        }


        #endregion



        private void modelTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            UpdateGraphicObjectIsVisible(e.Node);
            foreach (TreeNode node in e.Node.Nodes)
            {
                node.Checked = e.Node.Checked;
                UpdateGraphicObjectIsVisible(node);
            }
            _model.Model.HasChanged = true;
        }

        
        private void UpdateGraphicObjectIsVisible(TreeNode node)
        {
            if (node.Tag is GraphicObjectBase)
            {
                ((GraphicObjectBase)node.Tag).IsVisible = node.Checked;
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mySelectedNode != null)
            {
                var obj = mySelectedNode.Tag as GraphicObjectBase;
                if(obj != null)
                {
                    obj.RemoveFromModel();
                    //_model.Model.Remove(obj);
                }
                mySelectedNode.Remove();

                _model.Model.RaiseModelChangedEvent();
            }
            else
            {
                MessageBox.Show("No tree node selected.");
            }

            contextMenuStrip1.Close();
        }

        private void newLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mySelectedNode != null)
            {
                if (mySelectedNode.Tag is GraphicLayer)
                {
                    GraphicLayer parentLayer = mySelectedNode.Tag as GraphicLayer;

                    GraphicLayer childLayer = new GraphicLayer();
                    parentLayer.AddToLayer(childLayer);

                    _model.Model.Add(childLayer, raiseChangeEvent: true);
                }
                else if(mySelectedNode.Tag is GraphicModel)
                {
                    GraphicLayer childLayer = new GraphicLayer();
                    _model.Model.Add(childLayer, raiseChangeEvent: true);
                }
                else
                {
                    MessageBox.Show("Selected node must be a Layer.");
                }
            }
            else
            {
                MessageBox.Show("No tree node selected.");
            }
        }


        private void SetContextMenuItems(TreeNode node)
        {
            if(node.Tag is GraphicModel)
            {
                this.newLayerToolStripMenuItem.Visible = true;
                this.toolStripMenuItemNewLayerSeparater.Visible = true;

                this.editToolStripMenuItem.Visible = false;
                this.toolStripMenuItemEditSeparater.Visible = false;

                this.deleteToolStripMenuItem.Visible = false;
                this.renameToolStripMenuItem.Visible = true;
            }
            else if (node.Tag is GraphicLayer)
            {
                this.newLayerToolStripMenuItem.Visible = true;
                this.toolStripMenuItemNewLayerSeparater.Visible = true;

                this.editToolStripMenuItem.Visible = false;
                this.toolStripMenuItemEditSeparater.Visible = false;

                this.deleteToolStripMenuItem.Visible = true;
                this.renameToolStripMenuItem.Visible = true;
            }
            else if (node.Tag is GraphicObjectBase)
            {
                this.newLayerToolStripMenuItem.Visible = false;
                this.toolStripMenuItemNewLayerSeparater.Visible = false;

                this.editToolStripMenuItem.Visible = true;
                this.toolStripMenuItemEditSeparater.Visible = true;

                this.deleteToolStripMenuItem.Visible = true;
                this.renameToolStripMenuItem.Visible = true;
            }
        }
    }

  
}
