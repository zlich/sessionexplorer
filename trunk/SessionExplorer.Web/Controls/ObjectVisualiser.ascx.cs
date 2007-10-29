using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SessionExplorer.Utilities;

namespace SessionExplorer.Web.Controls
{
    public partial class ObjectVisualiser : System.Web.UI.UserControl
    {
        private string targetName;
        private object target;

        /// <summary>
        /// Sets the target.
        /// </summary>
        /// <value>The target.</value>
        public object Target
        {
            set
            {
                target = value;
                PopulateTreeView();
            }
        }

        /// <summary>
        /// Sets the name of the target.
        /// </summary>
        /// <value>The name of the target.</value>
        public string TargetName
        {
            set { targetName = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Populates the tree view.
        /// </summary>
        private void PopulateTreeView()
        {
            ObjectTreeView.Nodes.Clear();
            AttachChild(ObjectTreeView.Nodes, !string.IsNullOrEmpty(targetName) ? targetName : target.GetType().FullName, target);
            ObjectTreeView.CollapseAll();
        }

        /// <summary>
        /// Attaches the children.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="children">The children.</param>
        private void AttachChildren(TreeNode parent, IEnumerable<KeyValuePair<string, object>> children)
        {
            parent.ChildNodes.Clear();
            if (children != null)
                foreach (KeyValuePair<string, object> pair in children)
                    AttachChild(parent.ChildNodes, pair.Key, pair.Value);
            parent.SelectAction = (parent.ChildNodes.Count.Equals(0)) ? TreeNodeSelectAction.None : TreeNodeSelectAction.Expand;
        }

        /// <summary>
        /// Attaches the child.
        /// </summary>
        /// <param name="parentsNodes">The parents nodes.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        private void AttachChild(TreeNodeCollection parentsNodes, string key, object value)
        {
            TreeNode child = new TreeNode();
            if (IsNonSystemObject(value))
            {
                child.Text = (value != null) ? key : string.Format("{0}: {1}", key, value ?? "null");
                AttachChildren(child, new ObjectProperties(value));
                if (IsEnumerable(value))
                    AttachEnumerableChildren(child, (IEnumerable)value);
            }
            else if (IsEnumerable(value))
            {
                child.Text = (value != null) ? key : string.Format("{0}: {1}", key, value ?? "null");
                AttachEnumerableChildren(child, (IEnumerable)value);
            }
            else
            {
                child.Text = string.Format("{0}: {1}", key, value ?? "null");
                child.SelectAction = TreeNodeSelectAction.None;
            }
            parentsNodes.Add(child);
        }

        /// <summary>
        /// Attaches the enumerable children.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="list">The list.</param>
        private void AttachEnumerableChildren(TreeNode parent, IEnumerable list)
        {
            if (list != null)
            {
                int index = -1;
                foreach (object o in list)
                    AttachChild(parent.ChildNodes, string.Format("[{0}]", ++index), o);
                //if (index.Equals(-1))
                //    parent.Text += ": none";
                parent.SelectAction = (parent.ChildNodes.Count.Equals(0)) ? TreeNodeSelectAction.None : TreeNodeSelectAction.Expand;
            }
        }

        /// <summary>
        /// Decides wheteher to show the child properties.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        private static bool IsNonSystemObject(object o)
        {
            return (o != null && !o.GetType().FullName.StartsWith("System."));
        }

        /// <summary>
        /// Determines whether the specified o is enumerable.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns>
        /// 	<c>true</c> if the specified o is enumerable; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsEnumerable(object o)
        {
            IEnumerable enumerable = (o as IEnumerable);
            return (enumerable != null && !o.GetType().FullName.StartsWith("System.String"));
        }
    }
}