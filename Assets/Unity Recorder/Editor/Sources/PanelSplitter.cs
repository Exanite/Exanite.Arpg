using System;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Recorder
{
    class PanelSplitter : VisualElement
    {
        readonly VisualElement m_AffectedElement;

        bool m_Grabbed;
        Vector2 m_GrabbedMousePosition;

        float m_ElementOriginalWidth;

        const float k_SplitterWidth = 5.0f;
        
        [Serializable]
        class Width
        {
            public float value;
        }
        
        Width m_Width;
        
        void SetWidth(float value)
        {
            if (m_Width == null)
                return;
           
            m_Width.value = value;
            m_AffectedElement.style.width = value;

            SavePersistentData();
        }

        public PanelSplitter(VisualElement affectedElement)
        {
            m_AffectedElement = affectedElement;

            style.cursor = UIElementsEditorUtility.CreateDefaultCursorStyle(MouseCursor.ResizeHorizontal);
            style.width = k_SplitterWidth;
            style.minWidth = k_SplitterWidth;
            style.maxWidth = k_SplitterWidth;
            
#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable CS0618 // Type or member is obsolete
            RegisterCallback<MouseDownEvent>(OnMouseDown, Capture.Capture);
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable CS0618 // Type or member is obsolete
            RegisterCallback<MouseMoveEvent>(OnMouseMove, Capture.Capture);
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable CS0618 // Type or member is obsolete
            RegisterCallback<MouseUpEvent>(OnMouseUp, Capture.Capture);
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning restore CS0618 // Type or member is obsolete
        }

        void OnMouseDown(MouseDownEvent evt)
        {
            if (evt.button != (int) MouseButton.LeftMouse)
                return;
            
            if (m_Grabbed)
                return;

#pragma warning disable CS0618 // Type or member is obsolete
            this.TakeMouseCapture();
#pragma warning restore CS0618 // Type or member is obsolete

            m_Grabbed = true;
            m_GrabbedMousePosition = evt.mousePosition;
            m_ElementOriginalWidth = m_AffectedElement.style.width;
            
            evt.StopImmediatePropagation();
        }
        
        void OnMouseMove(MouseMoveEvent evt)
        {
            if (!m_Grabbed)
                return;

            var delta = evt.mousePosition.x - m_GrabbedMousePosition.x;

            var newWidth = Mathf.Max(m_ElementOriginalWidth + delta, m_AffectedElement.style.minWidth);
          
            if (m_AffectedElement.style.maxWidth > 0.0f)
                newWidth = Mathf.Min(newWidth, m_AffectedElement.style.maxWidth);

            SetWidth(newWidth);
        }
        
        void OnMouseUp(MouseUpEvent evt)
        {
            if (evt.button != (int) MouseButton.LeftMouse)
                return;

            if (!m_Grabbed)
                return;

            m_Grabbed = false;
#pragma warning disable CS0618 // Type or member is obsolete
            this.ReleaseMouseCapture();
#pragma warning restore CS0618 // Type or member is obsolete
            
            evt.StopImmediatePropagation();
        }
        
        public override void OnPersistentDataReady()
        {
            base.OnPersistentDataReady();

            var key = GetFullHierarchicalPersistenceKey();

            m_Width = GetOrCreatePersistentData<Width>(m_Width, key);

            if (m_Width.value > 0.0f)
                m_AffectedElement.style.width = m_Width.value;
        }
    }
}