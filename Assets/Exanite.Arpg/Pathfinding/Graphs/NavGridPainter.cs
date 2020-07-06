using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Exanite.Arpg.Pathfinding.Graphs
{
    public class NavGridPainter : MonoBehaviour
    {
        public NavGrid grid;

        public KeyCode paintKey = KeyCode.Mouse1;

        private Coroutine currentTask;

        private void Update()
        {
            if (grid.Nodes != null)
            {
                if (Input.GetKeyDown(paintKey))
                {
                    currentTask = StartCoroutine(PaintGrid());
                }
                else if (Input.GetKeyUp(paintKey))
                {
                    StopCoroutine(currentTask);

                    currentTask = null;
                }
            }
        }

        private IEnumerator PaintGrid()
        {
            HashSet<Node> paintedNodes = new HashSet<Node>();
            NodeType paintAction = NodeType.Walkable;
            bool firstRun = true;

            while (grid.Nodes != null)
            {
                if (grid.RaycastNode(Camera.main.ScreenPointToRay(Input.mousePosition), out Node node))
                {
                    if (node != null && !paintedNodes.Contains(node))
                    {
                        if (firstRun == true)
                        {
                            switch (node.Type)
                            {
                                case NodeType.NonWalkable:
                                {
                                    paintAction = NodeType.Walkable;
                                    break;
                                }

                                case NodeType.Walkable:
                                {
                                    paintAction = NodeType.NonWalkable;
                                    break;
                                }
                            }

                            firstRun = false;
                        }

                        node.Type = paintAction;

                        paintedNodes.Add(node);
                    }
                }

                yield return null;
            }
        }
    }
}
