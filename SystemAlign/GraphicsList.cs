using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Security;

using Microsoft.Win32;

namespace SystemAlign
{
    using DrawList = List<DrawObject>;

    public class GraphicsList : ISerializable
    {
        #region Members

        public DrawList _graphicsList;

        private const string EntryCount = "Count";
        private const string EntryType = "Type";

        #endregion

        #region Constructor
        public GraphicsList()
        {
            _graphicsList = new DrawList();
        }


        public DrawList DrawedList
        {
            get { return _graphicsList; }
            set { _graphicsList = value; }
        }

        #endregion

        #region Serialization Support

        protected GraphicsList(SerializationInfo info, StreamingContext context)
        {
            _graphicsList = new DrawList();

            var n = info.GetInt32(EntryCount);

            for (int i = 0; i < n; i++)
            {
                var typeName = info.GetString(String.Format(CultureInfo.InvariantCulture, "{0}{1}", EntryType, i));

                var drawObject = (DrawObject)Assembly.GetExecutingAssembly().CreateInstance(typeName);

                if (drawObject == null) continue;
                drawObject.LoadFromStream(info, i);

                _graphicsList.Add(drawObject);
            }

        }

        /// <summary>
        /// Save object to serialization stream
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(EntryCount, _graphicsList.Count);

            int i = 0;

            foreach (DrawObject o in _graphicsList)
            {
                info.AddValue(String.Format(CultureInfo.InvariantCulture,"{0}{1}",EntryType, i),o.GetType().FullName);
                o.SaveToStream(info, i);
                i++;
            }
        }

        #endregion

        #region Other functions

        public void Draw(Graphics g)
        {
            int n = _graphicsList.Count;
            DrawObject o;

            // Enumerate list in reverse order to get first
            // object on the top of Z-order.
            for (int i = n - 1; i >= 0; i--)
            {
                o = _graphicsList[i];

                o.Draw(g);

                if (o.Selected == true)
                {
                    o.DrawTracker(g);
                }
            }
        }

        public Rectangle drawingObject;

        public Rectangle DrawingRectangle
        {
            get { return this.drawingObject; }
        }


        /// <summary>
        /// Dump (for debugging)
        /// </summary>
        public void Dump()
        {
            Trace.WriteLine("");

            foreach (DrawObject o in _graphicsList)
            {
                o.Dump();
            }

            if (_graphicsList.Count > 0)
                drawingObject = _graphicsList[0].NowRectangle();
        }

        /// <summary>
        /// Clear all objects in the list
        /// </summary>
        /// <returns>
        /// true if at least one object is deleted
        /// </returns>
        public bool Clear()
        {
            bool result = (_graphicsList.Count > 0);
            _graphicsList.Clear();
            return result;
        }

        /// <summary>
        /// Count and this [nIndex] allow to read all graphics objects
        /// from GraphicsList in the loop.
        /// </summary>
        public int Count
        {
            get
            {
                return _graphicsList.Count;
            }
        }

        public DrawObject this[int index]
        {
            get
            {
                if (index < 0 || index >= _graphicsList.Count)
                    return null;

                return _graphicsList[index];
            }
        }


        /// <summary>
        /// SelectedCount and GetSelectedObject allow to read
        /// selected objects in the loop
        /// </summary>
        public int SelectionCount
        {
            get
            {
                int n = 0;

                foreach (DrawObject o in Selection)
                {
                    n++;
                }

                return n;
            }
        }


        /// <summary>
        /// Returns INumerable object which may be used for enumeration
        /// of selected objects.
        /// 
        /// Note: returning IEnumerable<DrawObject> breaks CLS-compliance
        /// (assembly CLSCompliant = true is removed from AssemblyInfo.cs).
        /// To make this program CLS-compliant, replace 
        /// IEnumerable<DrawObject> with IEnumerable. This requires
        /// casting to object at runtime.
        /// </summary>
        /// <value></value>
        public IEnumerable<DrawObject> Selection
        {
            get
            {
                foreach (DrawObject o in _graphicsList)
                {
                    if (o.Selected)
                    {
                        yield return o;
                    }
                }
            }
        }

        public void Add(DrawObject obj)
        {
            // insert to the top of z-order
            _graphicsList.Insert(0, obj);
        }

        /// <summary>
        /// Insert object to specified place.
        /// Used for Undo.
        /// </summary>
        public void Insert(int index, DrawObject obj)
        {
            if (index >= 0 && index < _graphicsList.Count)
            {
                _graphicsList.Insert(index, obj);
            }
        }

        /// <summary>
        /// Replace object in specified place.
        /// Used for Undo.
        /// </summary>
        public void Replace(int index, DrawObject obj)
        {
            if (index >= 0 && index < _graphicsList.Count)
            {
                _graphicsList.RemoveAt(index);
                _graphicsList.Insert(index, obj);
            }
        }

        /// <summary>
        /// Remove object by index.
        /// Used for Undo.
        /// </summary>
        public void RemoveAt(int index)
        {
            _graphicsList.RemoveAt(index);
        }

        /// <summary>
        /// Delete last added object from the list
        /// (used for Undo operation).
        /// </summary>
        public void DeleteLastAddedObject()
        {
            if (_graphicsList.Count > 0)
            {
                _graphicsList.RemoveAt(0);
            }
        }

        public void SelectInRectangle(Rectangle rectangle)
        {
            UnselectAll();

            foreach (DrawObject o in _graphicsList)
            {
                if (o.IntersectsWith(rectangle))
                    o.Selected = true;
            }

        }

        public void UnselectAll()
        {
            foreach (DrawObject o in _graphicsList)
            {
                o.Selected = false;
            }
        }

        public void SelectAll()
        {
            foreach (DrawObject o in _graphicsList)
            {
                o.Selected = true;
            }
        }

        /// <summary>
        /// Delete selected items
        /// </summary>
        /// <returns>
        /// true if at least one object is deleted
        /// </returns>
        public bool DeleteSelection()
        {
            bool result = false;

            int n = _graphicsList.Count;

            for (int i = n - 1; i >= 0; i--)
            {
                if (((DrawObject)_graphicsList[i]).Selected)
                {
                    _graphicsList.RemoveAt(i);
                    result = true;
                }
            }

            return result;
        }


        /// <summary>
        /// Move selected items to front (beginning of the list)
        /// </summary>
        /// <returns>
        /// true if at least one object is moved
        /// </returns>
        public bool MoveSelectionToFront()
        {
            int n;
            int i;
            DrawList tempList;

            tempList = new DrawList();
            n = _graphicsList.Count;

            // Read source list in reverse order, add every selected item
            // to temporary list and remove it from source list
            for (i = n - 1; i >= 0; i--)
            {
                if ((_graphicsList[i]).Selected)
                {
                    tempList.Add(_graphicsList[i]);
                    _graphicsList.RemoveAt(i);
                }
            }

            // Read temporary list in direct order and insert every item
            // to the beginning of the source list
            n = tempList.Count;

            for (i = 0; i < n; i++)
            {
                _graphicsList.Insert(0, tempList[i]);
            }

            return (n > 0);
        }

        /// <summary>
        /// Move selected items to back (end of the list)
        /// </summary>
        /// <returns>
        /// true if at least one object is moved
        /// </returns>
        public bool MoveSelectionToBack()
        {
            int n;
            int i;
            DrawList tempList;

            tempList = new DrawList();
            n = _graphicsList.Count;

            // Read source list in reverse order, add every selected item
            // to temporary list and remove it from source list
            for (i = n - 1; i >= 0; i--)
            {
                if ((_graphicsList[i]).Selected)
                {
                    tempList.Add(_graphicsList[i]);
                    _graphicsList.RemoveAt(i);
                }
            }

            // Read temporary list in reverse order and add every item
            // to the end of the source list
            n = tempList.Count;

            for (i = n - 1; i >= 0; i--)
            {
                _graphicsList.Add(tempList[i]);
            }

            return (n > 0);
        }

        #endregion

    }
}
